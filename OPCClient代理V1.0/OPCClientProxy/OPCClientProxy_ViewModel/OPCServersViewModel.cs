using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using OPCClientProxy_Model;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Timers;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using LogHelper;

namespace OPCClientProxy_ViewModel
{
    public class OPCServersViewModel : PropertyCallBack
    {
        /// <summary>
        /// _SendPoinsThread 线程退出锁
        /// </summary>
        private bool _SendDataSign = false;
        /// <summary>
        /// 向服务器发送点数据的线程
        /// </summary>
        public Thread _SendPoinsThread;

        /// <summary>
        /// OPC点信息回调
        /// </summary>
        private DataChange _OPCDataCallBack;

        /// <summary>
        /// 上一次读取的点数据缓存，数据补齐时使用
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> _PointValueSign = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 需要发送的数据缓存，发送给服务器进行负压波计算
        /// </summary>
        private Dictionary<string, Queue<PreSensorPoint>> _SendPreDataCache = new Dictionary<string, Queue<PreSensorPoint>>();

        /// <summary>
        /// 心跳写机制，向OPC服务器发送心跳包
        /// </summary>
        private System.Timers.Timer _PCHearbeat;

        private int _PCHearbeat_Sign = 0;

        public OPCServersViewModel()
        {
            _OPCServersManager = new ObservableCollection<OPCServerModel>();
        }

        private string _HostName = string.Empty;
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string HostName
        {
            get
            {
                return _HostName;
            }
            set
            {
                _HostName = value;
                OnPropertyChanged("HostName");
            }
        }

        private string _ServerIP = string.Empty;
        /// <summary>
        /// 服务器设备IP
        /// </summary>
        public string ServerIP
        {
            get
            {
                return _ServerIP;
            }
            set
            {
                _ServerIP = value;
                OnPropertyChanged("ServerIP");
            }
        }

        private ObservableCollection<OPCServerModel> _OPCServersManager;
        /// <summary>
        /// OPCServer列表信息
        /// </summary>
        public ObservableCollection<OPCServerModel> OPCServersManager
        {
            get
            {
                return _OPCServersManager;
            }
            set
            {
                _OPCServersManager = value;
            }
        }

        /// <summary>
        /// UserSelectedOPCServer 操作锁
        /// </summary>
        private object _UserOPCServerLock = new object();

        private OPCServerModel _UserSelectedOPCServer;
        /// <summary>
        /// 选中连接的OPCServers
        /// </summary>
        public OPCServerModel UserSelectedOPCServer
        {
            get
            {
                return _UserSelectedOPCServer;
            }
            set
            {
                _UserSelectedOPCServer = value;
                OnPropertyChanged("UserSelectedOPCServer");
            }
        }

        /// <summary>
        /// 连接到配置的OPCserver服务器
        /// </summary>
        public bool LinkServer_FUNC()
        {
            bool result = false;
            if (MainWindowViewModel.Instance.ConfigSetManager.LinkOPCServerName != null)
            {
                var find_opcserver = MainWindowViewModel.Instance.OPCServersVMInstance.OPCServersManager.FirstOrDefault(para => para.OPCServerName.Equals(MainWindowViewModel.Instance.ConfigSetManager.LinkOPCServerName));
                if (find_opcserver != null)
                {
                    if (MainWindowViewModel.Instance.OPCProxy.JudgeLink() == 0)
                    {
                        //请先退出某个OPCServer连接
                        MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                        {
                            MainWindowViewModel.Instance.AddRunningMSG("已经连接到某个OPCServer，请确认退出！");
                            MyLog.Log.Info(string.Format("{0}:已经连接到某个OPCServer，请确认退出！", DateTime.Now.ToString()));
                        }));
                        return result;
                    }

                    if (find_opcserver.IsLinked)
                    {
                        return result;
                    }
                    if (MainWindowViewModel.Instance.OPCProxy.Conn2Server(find_opcserver.OPCServerName, ServerIP, MainWindowViewModel.Instance.ProxySetViewModel.Requrecy) == 0)//100ms刷新一次opc数据，可以自行设置该处刷新时间。
                    {
                        find_opcserver.IsLinked = true;
                        result = true;
                        MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                        {
                            lock (_UserOPCServerLock)
                            {
                                UserSelectedOPCServer = find_opcserver;
                            }

                            MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}:成功连接到{1}OPC服务器。", DateTime.Now.ToString(), find_opcserver.OPCServerName));
                            MyLog.Log.Info(string.Format(string.Format("{0}:成功连接到{1}OPC服务器。", DateTime.Now.ToString(), find_opcserver.OPCServerName)));
                        }));

                    }
                    else
                    {
                        find_opcserver.IsLinked = false;
                        MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                        {
                            MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}:未连接到{1}OPC服务器。", DateTime.Now.ToString(), find_opcserver.OPCServerName));
                            MyLog.Log.Info(string.Format(string.Format("{0}:未连接到{1}OPC服务器。", DateTime.Now.ToString(), find_opcserver.OPCServerName)));
                        }));
                    }
                }
            }

            return result;


        }

        /// <summary>
        /// 获取OPCServer点变量列表
        /// </summary>
        public void GetItemList_FUNC()
        {
            ThreadPool.QueueUserWorkItem((work_obj) =>
            {

                MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                {
                    MainWindowViewModel.Instance.AddRunningMSG("开始读取OPCServer服务器点变量列表信息请稍候......");
                }));

                if (MainWindowViewModel.Instance.OPCProxy.GetItemList() == 0)
                {
                    System.Windows.Forms.TreeNode[] rootNodes = MainWindowViewModel.Instance.OPCProxy.ItemTree.Root();
                    MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                    {
                        MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}:已读取完{1}服务器点列表信息。", DateTime.Now.ToString(), UserSelectedOPCServer.OPCServerName));
                        foreach (var item in rootNodes)
                        {
                            ListNode node = new ListNode();
                            node.NodeName = item.Text;
                            node.NodeID = item.Tag.ToString();

                            var find_node = UserSelectedOPCServer.OPCServerListNode.Children.FirstOrDefault(para => para.NodeName.Equals(node.NodeName));
                            if (find_node == null)
                            {
                                MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                                {

                                    UserSelectedOPCServer.OPCServerListNode.Children.Add(node);
                                    if (item.Nodes.Count > 0)
                                    {
                                        GenerateNodeTree(item.Nodes, node.Children);
                                        node.HasChild = true;
                                    }
                                    else
                                    {
                                        node.HasChild = false;
                                    }
                                }));
                            }
                        }
                    }));

                }

            });
          
        }

        /// <summary>
        ///  断开OPC_Server连接
        /// </summary>
        public void CloseLinkServer_FUNC()
        {
            ThreadPool.QueueUserWorkItem((work_obj) =>
            {
                try
                {
                    if (MainWindowViewModel.Instance.OPCProxy.DisConn() == 0)
                    {                      
                        if (_PCHearbeat != null)
                        {
                            _PCHearbeat.Stop();
                            _PCHearbeat.Close();
                            _PCHearbeat = null;
                        }

                        //线程退出标志false
                        _SendDataSign = false;
                    }
                }
                catch { }

            });
        }

        /// <summary>
        /// 获取本地OPC服务器
        /// </summary>
        public bool GetOPCServerList_FUNC()
        {
            bool result = false;
            try
            {
                string[] serverList = MainWindowViewModel.Instance.OPCProxy.GetSerList(this._HostName);
                if (serverList != null)
                {
                    if (serverList.Length == 0)
                    {
                        MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                        {
                            MainWindowViewModel.Instance.AddRunningMSG("获取不到本地OPCServer信息");
                            MyLog.Log.Info(string.Format("{0}:获取本地OPC服务列表为空，请确保安装了OPC服务", DateTime.Now.ToString()));
                        }));
                        return result;
                    }
                    foreach (string turn in serverList)
                    {
                        OPCServerModel OPCSERVER = new OPCServerModel();
                        OPCSERVER.OPCServerName = turn;
                        OPCSERVER.IsLinked = false;

                        MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                        {
                            _OPCServersManager.Add(OPCSERVER);
                        }));
                    }

                    result = true;

                    MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                    {
                        MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}:{1}", DateTime.Now.ToString(), "获取本机OPC服务器列表成功！"));
                        MyLog.Log.Info(string.Format("{0}:获取本地OPC服务列表成功", DateTime.Now.ToString()));
                    }));

                }

            }
            catch (Exception err)
            {
                MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                {
                    MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}:{1}", DateTime.Now.ToString(), "获取本机OPC服务器列表时失败！"));
                    MyLog.Log.Info(string.Format("{0}:获取本地OPC服务列表失败", DateTime.Now.ToString()));
                }));
            }

            return result;
        }

        /// <summary>
        /// 回调点数据
        /// </summary>
        public void CallBackNodes_FUNC()
        {

            MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
            {
                MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                {
                    MainWindowViewModel.Instance.AddRunningMSG(string.Format("开始向OPC请求数据"));
                }));

                #region LD50代理
                if (MainWindowViewModel.Instance.ConfigSetManager.ProxySign.Equals("LD50"))
                {
                    _PCHearbeat = null;
                    _PCHearbeat = new System.Timers.Timer(1000);
                    _PCHearbeat.Elapsed += new ElapsedEventHandler(_PCHearbeat_Elapsed);
                    _PCHearbeat.Start();

                    _SendPoinsThread = new Thread(SendPointFunc);
                    _SendPoinsThread.IsBackground = true;
                    _SendPoinsThread.Start();
                    _SendDataSign = true;


                    foreach (var plc in MainWindowViewModel.Instance.ConfigSetManager.PLCConllection)
                    {
                        foreach (var point in plc.PLC_OutPutPoints)
                        {
                            ListNode node = new ListNode();
                            node.NodeID = point.PointID;
                            node.NodeName = point.PointID.Replace(plc.PLCSign, "");
                            node.IsAdd = false;
                            UserSelectedOPCServer.SelectedNodes.Add(node);
                        }
                    }

                    try
                    {
                        if (UserSelectedOPCServer.SelectedNodes.Count > 0)
                        {
                            for (int i = 0; i < UserSelectedOPCServer.SelectedNodes.Count; i++)
                            {
                                if (!UserSelectedOPCServer.SelectedNodes[i].IsAdd)
                                {
                                    MainWindowViewModel.Instance.OPCProxy.Add2RefrGroup(UserSelectedOPCServer.SelectedNodes[i].NodeID);
                                    UserSelectedOPCServer.SelectedNodes[i].IsAdd = true;
                                }

                            }
                        }


                    }
                    catch { }
                }

                #endregion

                #region DOLPHIN服务器代理
                else if (MainWindowViewModel.Instance.ConfigSetManager.ProxySign.Equals("DOLPHIN"))
                {
                    foreach (var item in MainWindowViewModel.Instance.ConfigSetManager.DolPointCollection)
                    {
                        ListNode node = new ListNode();
                        node.NodeID = item.PointID;
                        node.NodeName = item.PointName;
                        node.IsAdd = true;
                        UserSelectedOPCServer.SelectedNodes.Add(node);
                    }

                    try
                    {
                        for (int i = 0; i < UserSelectedOPCServer.SelectedNodes.Count; i++)
                        {
                            if (UserSelectedOPCServer.SelectedNodes[i].IsAdd)
                            {
                                MainWindowViewModel.Instance.OPCProxy.Add2RefrGroup(UserSelectedOPCServer.SelectedNodes[i].NodeID);
                                UserSelectedOPCServer.SelectedNodes[i].IsAdd = true;
                            }
                        }
                    }
                    catch { }
                }
                #endregion

            }));

        }

        /// <summary>
        /// 向OPC服务器发送心跳包，用于和OPC服务器保持长连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _PCHearbeat_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_PCHearbeat_Sign >= 60)
                {
                    _PCHearbeat_Sign = 0;
                }

                foreach (var plc in MainWindowViewModel.Instance.ConfigSetManager.PLCConllection)
                {
                    MainWindowViewModel.Instance.OPCProxy.Write(MainWindowViewModel.Instance.ConfigSetManager.GetPointByType(plc, PointType.PC_heartbeat).PointID, _PCHearbeat_Sign);

                }

                _PCHearbeat_Sign++;
            }
            catch
            {
                MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                {
                    MainWindowViewModel.Instance.SystemRunningMSG.Add(string.Format("{0}:下传心跳失败！", DateTime.Now.ToString()));
                }));

            }
        }

        private DelegateCommand _AddNodeCommand;
        /// <summary>
        /// 添加要读取的点
        /// </summary>
        public DelegateCommand AddNodeCommand
        {
            get
            {
                if (_AddNodeCommand == null)
                {
                    _AddNodeCommand = new DelegateCommand((obj) =>
                    {
                        ListNode selectedNode = obj as ListNode;
                        if (selectedNode != null)
                        {
                            var find_node = UserSelectedOPCServer.SelectedNodes.FirstOrDefault(para => para.NodeID.Equals(selectedNode.NodeID));
                            if (find_node == null)
                            {
                                UserSelectedOPCServer.SelectedNodes.Add(selectedNode);
                                selectedNode.IsAdd = false;
                            }

                        }
                    });

                }
                return _AddNodeCommand;
            }
        }

        private DelegateCommand _RemoveOneNode;
        /// <summary>
        /// 删除某个点
        /// </summary>
        public DelegateCommand RemoveOneNode
        {
            get
            {
                if (_RemoveOneNode == null)
                {
                    _RemoveOneNode = new DelegateCommand((obj) =>
                    {
                        ListNode selectedNode = obj as ListNode;
                        if (selectedNode != null)
                        {
                            try
                            {
                                if (selectedNode.IsAdd)
                                {
                                    MainWindowViewModel.Instance.OPCProxy.Remove2RefrGroup(selectedNode.NodeID);
                                    selectedNode.IsAdd = false;
                                }
                            }
                            catch
                            {
                                //填写日志文件
                            }
                            UserSelectedOPCServer.SelectedNodes.Remove(selectedNode);
                        }
                    });
                }
                return _RemoveOneNode;
            }
        }

        private DelegateCommand _CallBackNodes;
        /// <summary>
        /// 回调点数据
        /// </summary>
        public DelegateCommand CallBackNodes
        {
            get
            {
                if (_CallBackNodes == null)
                {
                    _CallBackNodes = new DelegateCommand((obj) =>
                    {
                        try
                        {
                            if (UserSelectedOPCServer.SelectedNodes.Count > 0)
                            {
                                for (int i = 0; i < UserSelectedOPCServer.SelectedNodes.Count; i++)
                                {
                                    if (!UserSelectedOPCServer.SelectedNodes[i].IsAdd)
                                    {
                                        MainWindowViewModel.Instance.OPCProxy.Add2RefrGroup(UserSelectedOPCServer.SelectedNodes[i].NodeID);
                                    }

                                }
                            }

                        }
                        catch { }

                    });
                }
                return _CallBackNodes;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initial()
        {
            string ip = "127.0.0.1";
            //通过IP来获取计算机名称，可用在局域网内
            IPHostEntry ipHostEntry = Dns.GetHostEntry(ip);
            string strHostName = ipHostEntry.HostName.ToString();

            if (ip != null && ip != "")
            {
                ServerIP = ip;
            }

            if (strHostName != null && strHostName != "")
            {
                HostName = strHostName;
            }

            if (_OPCDataCallBack == null)
            {
                _OPCDataCallBack = new DataChange(DataChangeEvent);
            }
            MainWindowViewModel.Instance.OPCProxy.DtChange = _OPCDataCallBack;

            try
            {
                string address = AppDomain.CurrentDomain.BaseDirectory + "ConfigSetConfig.xml";

                if (File.Exists(address))
                {
                    using (FileStream fStream = new FileStream(address, FileMode.Open))
                    {
                        XmlSerializer deserial = new XmlSerializer(typeof(ConfigSetViewModel));
                        MainWindowViewModel.Instance.ConfigSetManager = (ConfigSetViewModel)deserial.Deserialize(fStream);
                        fStream.Close();

                        MainWindowViewModel.Instance.ProxySetViewModel.GUID = MainWindowViewModel.Instance.ConfigSetManager.GUID;
                        MainWindowViewModel.Instance.ProxySetViewModel.Requrecy = MainWindowViewModel.Instance.ConfigSetManager.Requrecy;
                        MainWindowViewModel.Instance.ProxySetViewModel.WCFServerIP = MainWindowViewModel.Instance.ConfigSetManager.WCFServerIP;
                        MainWindowViewModel.Instance.ProxySetViewModel.OPCTimeSetRequrecy = MainWindowViewModel.Instance.ConfigSetManager.OPCTimeSetRequrecy;
                        MainWindowViewModel.Instance.ConfigSetManager.GeneratePreIDCollection();
                    }

                    MainWindowViewModel.Instance.SystemRunningMSG.Add("读取OPC代理配置文件成功！");
                    MyLog.Log.Info(string.Format("{0}:读取ConfigSetConfig.xml配置文件成功！", DateTime.Now.ToString()));


                    //主动连接到WCF服务器
                    MainWindowViewModel.Instance.ProxySetViewModel.LinkWCFServer();

                    //主动读取本地OPCServer服务器
                    bool getOPCServerList = GetOPCServerList_FUNC();

                    if (getOPCServerList)
                    {
                        //主动连接到配置的OPCServer服务器
                        bool linkresult = LinkServer_FUNC();
                        if (linkresult)
                        {
                            CallBackNodes_FUNC();
                            GetItemList_FUNC();
                        }
                    }

                }
                else
                {
                    MainWindowViewModel.Instance.SystemRunningMSG.Add("请检查OPC代理配置文件是否存在！");
                    MyLog.Log.Info(string.Format("{0}:读取配置文件时找不到路径", DateTime.Now.ToString()));
                }

            }
            catch
            {
                MainWindowViewModel.Instance.SystemRunningMSG.Add("读取OPC代理配置文件失败！");
            }
        }

        /// <summary>
        /// 向服务器发送数据的线程工作函数
        /// </summary>
        private void SendPointFunc()
        {
            while (true)
            {
                List<ServiceReference1.PrePoint> _SendDatas = new List<ServiceReference1.PrePoint>();
                foreach (var item in MainWindowViewModel.Instance.ConfigSetManager.PrePoint)
                {
                    if (_SendPreDataCache.Keys.Contains(item))
                    {
                        lock (_SendPreDataCache[item])
                        {
                            if (_SendPreDataCache[item].Count >= (1000 / MainWindowViewModel.Instance.ConfigSetManager.Requrecy))
                            {
                                ServiceReference1.PrePoint sendData = new ServiceReference1.PrePoint();
                                sendData.PointValues = new List<string>();

                                for (int i = 0; i < (1000 / MainWindowViewModel.Instance.ConfigSetManager.Requrecy); i++)
                                {
                                    var GetPreSensorPoint = _SendPreDataCache[item].Dequeue();
                                    //第一个数据
                                    if (i == 0)
                                    {
                                        sendData.PointID = GetPreSensorPoint.ID;
                                        sendData.PointName = GetPreSensorPoint.Name;
                                        sendData.GetMS = Convert.ToInt32(GetPreSensorPoint.ReadMs);
                                        sendData.GetSec = Convert.ToInt32(GetPreSensorPoint.ReadS);
                                        sendData.PointValues.Add(GetPreSensorPoint.ReadValue);
                                    }
                                    else
                                    {
                                        sendData.PointValues.Add(GetPreSensorPoint.ReadValue);
                                    }
                                }

                                _SendDatas.Add(sendData);
                            }
                        }
                    }
                }

                try
                {
                    //向WCF服务器发送压力数据
                    if (MainWindowViewModel.Instance.ProxySetViewModel.IsOnline)
                    {
                        if (_SendDatas.Count > 0)
                        {
                            MainWindowViewModel.Instance.ProxySetViewModel.Client.SendPLCData(MainWindowViewModel.Instance.ProxySetViewModel.GUID, _SendDatas);
                        }

                    }
                }
                catch
                {
                    MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                    {
                        MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}：发送传感器数据失败！", DateTime.Now.ToString()));
                    }));

                }
            }
        }

        /// <summary>
        /// 接收数据、保存缓冲
        /// </summary>
        /// <param name="receivepoint"></param>
        private void ReceiveAndSendPreData(PreSensorPoint receivepoint)
        {
            if (_SendPreDataCache.Keys.Contains(receivepoint.ID))
            {
                lock (_SendPreDataCache[receivepoint.ID])
                {
                    _SendPreDataCache[receivepoint.ID].Enqueue(receivepoint);
                }
            }
            else
            {
                Queue<PreSensorPoint> PreQueue = new Queue<PreSensorPoint>();
                PreQueue.Enqueue(receivepoint);
                _SendPreDataCache.Add(receivepoint.ID, PreQueue);
            }
        }

        private List<PreSensorPoint> Complete(string ForeMS, string ForeS, string ForeValue, PreSensorPoint lastPoint)
        {
            List<PreSensorPoint> list = null;

            //前后数据时间戳间隔
            int ms_Interval = 0;
            if (Convert.ToInt32(lastPoint.ReadMs) >= Convert.ToInt32(ForeMS))
            {
                ms_Interval = (Convert.ToInt32(lastPoint.ReadMs) - Convert.ToInt32(ForeMS)) / 100;
            }
            else
            {
                ms_Interval = ((Convert.ToInt32(lastPoint.ReadMs) + 1000) - Convert.ToInt32(ForeMS)) / 100;
            }

            if ((ms_Interval - 1) > 0)
            {
                list = new List<PreSensorPoint>();
                for (int sign = 0; sign < (ms_Interval - 1); sign++)
                {
                    PreSensorPoint newPrePoint = new PreSensorPoint();
                    newPrePoint.ID = lastPoint.ID;
                    newPrePoint.Name = lastPoint.Name;
                    if ((Convert.ToInt32(ForeMS) + (sign + 1) * 100) == 1000)
                    {
                        newPrePoint.ReadMs = "0";

                        newPrePoint.ReadS = (Convert.ToInt32(ForeS) + 1).ToString();

                    }
                    else
                    {
                        newPrePoint.ReadMs = (Convert.ToInt32(ForeMS) + (sign + 1) * 100).ToString();
                        newPrePoint.ReadS = ForeS;
                    }

                    newPrePoint.ReadValue = ((Convert.ToDouble(ForeValue) + Convert.ToDouble(lastPoint.ReadValue)) / 2).ToString();
                    list.Add(newPrePoint);
                }
            }

            return list;
        }

        private void DataChangeEvent(READRESULT[] readRes, string[] itemsID)
        {
            MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
            {
                for (int i = 0; i < itemsID.Length; i++)
                {
                    var find_node = UserSelectedOPCServer.SelectedNodes.FirstOrDefault(para => para.NodeID.Equals(itemsID[i]));
                    if (find_node != null)
                    {
                        find_node.NodeValue = readRes[i].readValue;

                    }
                }
            }));

            #region 负压波数据传输
            /*
             * 每次回调时有可能丢失某路压力数据 进行线性差值（前后数据平均值） 补齐数据 S
             */
            if (MainWindowViewModel.Instance.ConfigSetManager.ProxySign.Equals("LD50"))
            {
                //将所有读取的点进行分类，按照PLC-Sign 以及数据点Sign来区分
                Dictionary<string, Dictionary<string, OPCPoint>> _ReadPoint_Distinguish = new Dictionary<string, Dictionary<string, OPCPoint>>();
                for (int i = 0; i < itemsID.Length; i++)
                {
                    OPCPoint find_opcpoint = MainWindowViewModel.Instance.ConfigSetManager.GetOPCPointbyItemID(itemsID[i]);
                    find_opcpoint.ReadValue = readRes[i].readValue;

                    if (_ReadPoint_Distinguish.Keys.Contains(find_opcpoint.BlongPLCSign))
                    {
                        _ReadPoint_Distinguish[find_opcpoint.BlongPLCSign].Add(find_opcpoint.PointSign, find_opcpoint);
                    }
                    else
                    {
                        Dictionary<string, OPCPoint> itemkeg_value = new Dictionary<string, OPCPoint>();
                        itemkeg_value.Add(find_opcpoint.PointSign, find_opcpoint);
                        _ReadPoint_Distinguish.Add(find_opcpoint.BlongPLCSign, itemkeg_value);
                    }
                }

                //将点区分完之后进行分别处理 按PLC-SIGN
                foreach (var item in _ReadPoint_Distinguish)
                {
                    var find_plc = MainWindowViewModel.Instance.ConfigSetManager.GetPLCByID(item.Key);
                    if (find_plc != null)
                    {
                        if (item.Value.Count == (find_plc.PLC_OutPutPoints.Count))
                        {
                            //如果为完整数据 录入到_PointValueSign作为差值补齐基础数据
                            if (!item.Value["AveValue"].ReadValue.Equals("0"))
                            {
                                PreSensorPoint newPrePoint = new PreSensorPoint();
                                newPrePoint.ReadMs = item.Value["GetMS"].ReadValue;
                                newPrePoint.ReadS = item.Value["GetSec"].ReadValue;
                                newPrePoint.ReadValue = item.Value["AveValue"].ReadValue;
                                newPrePoint.ID = item.Value["AveValue"].PointID;
                                newPrePoint.Name = item.Value["AveValue"].PointName;

                                if (_PointValueSign.Keys.Contains(item.Key))
                                {
                                    //进行线性差值补齐
                                    var CompletePrePoint = Complete(_PointValueSign[item.Key]["GetMS"], _PointValueSign[item.Key]["GetSec"], _PointValueSign[item.Key]["AveValue"], newPrePoint);
                                    if (CompletePrePoint != null)
                                    {
                                        foreach (var point in CompletePrePoint)
                                        {
                                            ReceiveAndSendPreData(point);
                                        }

                                    }

                                    //更新基础值
                                    foreach (var opcpoint in item.Value)
                                    {
                                        _PointValueSign[item.Key][opcpoint.Key] = opcpoint.Value.ReadValue;
                                    }
                                }
                                else
                                {
                                    Dictionary<string, string> chosePoint_Collection = new Dictionary<string, string>();
                                    foreach (var chosePoint in item.Value)
                                    {
                                        chosePoint_Collection.Add(chosePoint.Key, chosePoint.Value.ReadValue);
                                    }
                                    _PointValueSign.Add(item.Key, chosePoint_Collection);
                                }

                                ReceiveAndSendPreData(newPrePoint);
                            }
                        }
                        else
                        {
                            if (_PointValueSign.Keys.Contains(item.Key))
                            {
                                if (item.Value.Keys.Contains("GetMS"))
                                {
                                    //将数据写入到缓存
                                    PreSensorPoint newPrePoint = new PreSensorPoint();
                                    newPrePoint.ReadMs = item.Value["GetMS"].ReadValue;

                                    if (item.Value.Keys.Contains("GetSec"))
                                    {
                                        newPrePoint.ReadS = item.Value["GetSec"].ReadValue;
                                    }
                                    else
                                    {
                                        newPrePoint.ReadS = _PointValueSign[item.Key]["GetSec"];
                                    }

                                    var find_opcpoint = MainWindowViewModel.Instance.ConfigSetManager.GetOPCPointbyItemSign(item.Key, "AveValue");
                                    newPrePoint.ID = find_opcpoint.PointID;
                                    newPrePoint.Name = find_opcpoint.PointName;
                                    if (item.Value.Keys.Contains("AveValue"))
                                    {
                                        newPrePoint.ReadValue = item.Value["AveValue"].ReadValue;

                                    }
                                    else
                                    {
                                        newPrePoint.ReadValue = _PointValueSign[item.Key]["AveValue"];
                                    }

                                    //进行线性差值补齐
                                    var CompletePrePoint = Complete(_PointValueSign[item.Key]["GetMS"], _PointValueSign[item.Key]["GetSec"], _PointValueSign[item.Key]["AveValue"], newPrePoint);

                                    if (CompletePrePoint != null)
                                    {
                                        foreach (var point in CompletePrePoint)
                                        {
                                            ReceiveAndSendPreData(point);
                                        }

                                    }

                                    //更新基础数据
                                    _PointValueSign[item.Key]["GetMS"] = item.Value["GetMS"].ReadValue;
                                    if (item.Value.Keys.Contains("GetSec"))
                                    {
                                        _PointValueSign[item.Key]["GetSec"] = item.Value["GetSec"].ReadValue;
                                    }
                                    if (item.Value.Keys.Contains("AveValue"))
                                    {
                                        _PointValueSign[item.Key]["AveValue"] = item.Value["AveValue"].ReadValue;

                                    }
                                    ReceiveAndSendPreData(newPrePoint);
                                }
                            }
                        }

                    }
                }

            }
            #endregion

            #region DOLPHIN服务器
            else if (MainWindowViewModel.Instance.ConfigSetManager.ProxySign.Equals("DOLPHIN"))
            {
                //向服务器发送采集点数据
                ThreadPool.QueueUserWorkItem((work_obj) =>
                {
                    List<ServiceReference1.DolPoint> Dolpoints = new List<ServiceReference1.DolPoint>();

                    for (int i = 0; i < UserSelectedOPCServer.SelectedNodes.Count; i++)
                    {
                        ServiceReference1.DolPoint point = new ServiceReference1.DolPoint();
                        point.PointID = UserSelectedOPCServer.SelectedNodes[i].NodeID;
                        point.PointName = UserSelectedOPCServer.SelectedNodes[i].NodeName;

                        point.PointValue = UserSelectedOPCServer.SelectedNodes[i].NodeValue;
                        Dolpoints.Add(point);
                    }

                    lock (MainWindowViewModel.Instance.ProxySetViewModel._ClientObj)
                    {
                        try
                        {
                            if (MainWindowViewModel.Instance.ProxySetViewModel.Client != null && MainWindowViewModel.Instance.ProxySetViewModel.IsOnline)
                            {
                                MainWindowViewModel.Instance.ProxySetViewModel.Client.SendDolPhinData(MainWindowViewModel.Instance.ProxySetViewModel.GUID, Dolpoints);
                            }
                        }
                        catch
                        {
                            MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                            {
                                MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}:向服务器发送DOLPHIN数据失败，请检查网络连接状态！", DateTime.Now.ToString()));
                            }));

                        }

                    }

                    //清空DOLPHIN定位信息 等待下一条定位信息
                    var leakcountPoint = UserSelectedOPCServer.SelectedNodes.FirstOrDefault(para => para.NodeID.Equals("leak.count"));
                    if (leakcountPoint != null && Convert.ToInt32(leakcountPoint.NodeValue) > 0)
                    {
                        if (Convert.ToInt32(leakcountPoint.NodeValue) > 0)
                        {
                            var clearpoint = UserSelectedOPCServer.SelectedNodes.FirstOrDefault(para => para.NodeID.Equals("leak.clear"));
                            if (clearpoint != null)
                            {
                                MainWindowViewModel.Instance.OPCProxy.Write(clearpoint.NodeID, true);
                            }
                        }

                    }


                });
            }
            #endregion

        }

        private void GenerateNodeTree(System.Windows.Forms.TreeNodeCollection NodeSource, ObservableCollection<ListNode> OPCListNode)
        {
            if (NodeSource != null)
            {
                foreach (var item in NodeSource)
                {
                    ListNode node = new ListNode();
                    node.NodeName = (item as System.Windows.Forms.TreeNode).Text;
                    node.NodeID = (item as System.Windows.Forms.TreeNode).Tag.ToString();

                    OPCListNode.Add(node);
                    if ((item as System.Windows.Forms.TreeNode).Nodes.Count > 0)
                    {
                        GenerateNodeTree((item as System.Windows.Forms.TreeNode).Nodes, node.Children);
                        node.HasChild = true;
                    }
                    else
                    {
                        node.HasChild = false;
                    }
                }
            }
        }

    }
}
