using ISafe_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Reflection;

namespace UserClientViewModel
{
    public class WCFViewModel : IDisposable
    {
        public delegate void SorDataCallBackHandler(int datasign, string Key, double[] Datasource);
        /// <summary>
        /// 数据传入界面显示委托
        /// </summary>
        public SorDataCallBackHandler SorDataCallBackEvent
        {
            get;
            set;
        }

        /// <summary>
        /// 连接的服务器IP地址
        /// </summary>
        private string _LinkServerIP;

        /// <summary>
        /// 心跳线程
        /// </summary>
        private Thread _HeartThread;

        /// <summary>
        /// 心跳线程使能标志
        /// </summary>
        private bool _HeartThreadSign = false;

        /// <summary>
        ///  WCF客户端访问锁
        /// </summary>
        public object _ClientLock = new object();

        /// <summary>
        /// UI主线程
        /// </summary>
        public Dispatcher WindowDispatcher
        {
            get;
            set;
        }

        public WCFViewModel()
        {
            _HeartThread = new Thread(HeartWorkFunc);
        }

        private ServiceReference1.UserClient _Client;
        /// <summary>
        /// WCF客户端
        /// </summary>
        public ServiceReference1.UserClient Client
        {
            get
            {
                return _Client;
            }
            set
            {
                _Client = value;
            }
        }

        /// <summary>
        /// 记录从服务器获取的配置信息
        /// </summary>
        public ServiceReference1.Graphic ServerGrapic
        {
            get;
            set;
        }

        /// <summary>
        /// 登录服务器
        /// </summary>
        /// <returns></returns>
        public bool LoginServer(string serverIP)
        {
            _LinkServerIP = serverIP;
            try
            {
                lock (_ClientLock)
                {
                    bool result = LinkServer();
                    if (result)
                    {
                        _HeartThreadSign = true;
                        _HeartThread.Start();
                    }

                    return result;
                }

            }
            catch
            {
                return false;
                //写日志
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <returns></returns>
        private bool LinkServer()
        {
            try
            {
                _Client = null;
                WCFCallBack callback = new WCFCallBack();
                InstanceContext con = new InstanceContext(callback);
                _Client = new ServiceReference1.UserClient(con, new NetTcpBinding(SecurityMode.None, false), new EndpointAddress(string.Format(@"net.tcp://{0}:9695/acuservice", _LinkServerIP)));
                _Client.Open();
                _Client.Login();

                ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
                {
                    var source = _Client.GetGraphic();

                    if (WindowDispatcher != null)
                    {
                        WindowDispatcher.Invoke(new Action(() =>
                        {
                            InitialDevicesAndPipes(source);

                        }));
                    }
                }));
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 心跳工作函数
        /// </summary>
        private void HeartWorkFunc()
        {
            while (_HeartThreadSign)
            {
                try
                {
                    lock (_ClientLock)
                    {
                        _Client.BeatHeart();
                    }
                }
                catch
                {
                    //重新连接
                    LinkServer();
                    //填写日志文件
                }
                //5m发送心跳包
                Thread.Sleep(5000);
            }

        }

        /// <summary>
        /// ACU设备信息、管道信息、压力变送器初始化
        /// </summary>
        public void InitialDevicesAndPipes(ServiceReference1.Graphic Graphic)
        {
            try
            {
                if (Graphic != null)
                {

                    //将服务器端传过来的配置信息先保存在内存
                    ServerGrapic = Graphic;

                    if (Graphic.Pipes != null)
                    {
                        MainWindowViewModel.Instance.PipesManager.Pipes.Clear();
                        MainWindowViewModel.Instance.PipesManager.TotalPipes.Clear();
                        //将所有管道信息保存在UI视图集合中
                        foreach (var pipe in Graphic.Pipes)
                        {
                            MainWindowViewModel.Instance.PipesManager.Pipes.Add(ModelExchange.ExchangedToWPF(pipe));
                            MainWindowViewModel.Instance.PipesManager.TotalPipes.Add(ModelExchange.ExchangedToWPF(pipe));
                        }
                    }
                    if (Graphic.CombinPipes != null)
                    {
                        MainWindowViewModel.Instance.PipesManager.CombinePipes.Clear();
                        foreach (var combinepipe in Graphic.CombinPipes)
                        {
                            MainWindowViewModel.Instance.PipesManager.CombinePipes.Add(ModelExchange.ExchangedToWPF(combinepipe));
                            MainWindowViewModel.Instance.PipesManager.TotalPipes.Add(ModelExchange.ExchangedToWPF(combinepipe));
                        }
                    }
                    if (Graphic.PipeSites != null)
                    {
                        MainWindowViewModel.Instance.PipeSitesManager.PipeSiteCollection.Clear();
                        foreach (var pipesite in Graphic.PipeSites)
                        {
                            MainWindowViewModel.Instance.PipeSitesManager.PipeSiteCollection.Add(ModelExchange.ExchangeToWPF(pipesite));
                        }

                        MainWindowViewModel.Instance.LD50ShowManager.LD50Nodes.Clear();

                    }
                    if (Graphic.OPCProxys != null)
                    {
                        MainWindowViewModel.Instance.OPCProxysManager.OPCProxysCollection.Clear();
                        foreach (var opcproxy in Graphic.OPCProxys)
                        {
                            MainWindowViewModel.Instance.OPCProxysManager.OPCProxysCollection.Add(ModelExchange.ExchangeToWPF(opcproxy));
                        }
                    }

                }
            }
            catch
            {
                //填写日志信息
            }
        }

        /// <summary>
        /// 系统退出时
        /// </summary>
        public void OnClose()
        {
            _HeartThreadSign = false;
        }

        public void SetDolPointValue(string ID, object value)
        {
            try
            {
                _Client.SetDolPointValue(ID, value);
            }
            catch
            {
                //填写日志信息
            }

        }

        public void Dispose()
        {
            OnClose();
        }
    }


    public class WCFCallBack : ServiceReference1.IUserCallback
    {

        public void GraphicChanged(ServiceReference1.Graphic gra)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 服务器回调定位信息
        /// </summary>
        /// <param name="loca"></param>
        public void LocationCallBack(ServiceReference1.Location loca)
        {
            if (loca != null)
            {
                if (loca.LocateDetectedType == ServiceReference1.DetectedType.pressure)
                {
                    MainWindowViewModel.Instance.WCFManager.WindowDispatcher.Invoke(new Action(() =>
                    {
                        MainWindowViewModel.Instance.LD50ShowManager.Locations.Add(loca);
                        MainWindowViewModel.Instance.LeakAlarmManager.IsSure = true;
                        MainWindowViewModel.Instance.LeakAlarmManager.LeakPipe = string.Format("{0}-{1}",loca.StartSiteName,loca.EndSiteName);
                        MainWindowViewModel.Instance.LeakAlarmManager.LeakTime = loca.LocateGetTime;
                        MainWindowViewModel.Instance.LeakAlarmManager.LeakType = "负压波";
                        MainWindowViewModel.Instance.LeakAlarmManager.LeakMSG = string.Format("距离泄漏管段{0}{1}米", loca.StartSiteName,loca.LocateLength);
                    }));

                }
            }
        }

        /// <summary>
        /// 代理连接状态回调
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="islink"></param>
        public void OPCProxyLinkCallBack(string Guid, bool islink)
        {
            var find_opcproxy = MainWindowViewModel.Instance.OPCProxysManager.OPCProxysCollection.FirstOrDefault(para => para.GUID.Equals(Guid));
            if (find_opcproxy != null)
            {
                find_opcproxy.IsLinked = islink;
            }
        }

        /// <summary>
        /// 负压波泄漏信息
        /// </summary>
        /// <param name="leak"></param>
        public void PreLeakCallBack(ServiceReference1.PressureLeak leak)
        {
            if (leak != null)
            {
                MainWindowViewModel.Instance.WCFManager.WindowDispatcher.Invoke(new Action(() =>
                {
                    MainWindowViewModel.Instance.LD50ShowManager.Leaks.Add(leak);

                }));

            }
        }

        /// <summary>
        /// 服务器端回调的压力数据
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Datasource"></param>
        public void PressureDateCallBack(string Key, double[] Datasource)
        {
            if (Key != null && Datasource != null)
            {
                if (MainWindowViewModel.Instance.WCFManager.SorDataCallBackEvent != null)
                {
                    MainWindowViewModel.Instance.WCFManager.SorDataCallBackEvent.Invoke(0, Key, Datasource);
                }
            }
        }

        /// <summary>
        /// 服务器回调过来的PLC传感器连接状态
        /// </summary>
        /// <param name="PLCSign"></param>
        /// <param name="link"></param>
        public void PLCPreLinkingCallBack(Dictionary<string, bool> PreSensorLinks)
        {
            MainWindowViewModel.Instance.WCFManager.WindowDispatcher.Invoke(new Action(() =>
            {
                foreach (var item in PreSensorLinks)
                {
                    var finditem = MainWindowViewModel.Instance.PipeSitesManager.GetPreSensorByID(item.Key);
                    if (finditem != null)
                    {
                        finditem.IsLink = item.Value;
                    }
                }
            }));

        }

        /// <summary>
        /// 服务器回调DOLPHIN定位信息
        /// </summary>
        /// <param name="location"></param>
        public void DolLocationCallBack(ServiceReference1.DolLocation location)
        {
            MainWindowViewModel.Instance.WCFManager.WindowDispatcher.Invoke(new Action(() =>
            {
                if (location != null)
                {
                    MainWindowViewModel.Instance.DolShowManager.DolLoactions.Add(location);

                    MainWindowViewModel.Instance.LeakAlarmManager.IsSure = true;
                    MainWindowViewModel.Instance.LeakAlarmManager.LeakPipe = location.PipeName;
                    MainWindowViewModel.Instance.LeakAlarmManager.LeakTime = location.ShowTime;
                    MainWindowViewModel.Instance.LeakAlarmManager.LeakType = "次声波";
                    MainWindowViewModel.Instance.LeakAlarmManager.LeakMSG = string.Format("距离泄漏管段起始站点{0}米", location.Pos);
                }

            }));
        }

        /// <summary>
        /// 负压波Mask波形输出
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="MaskData"></param>
        public void PreMaskCallBack(string Key, double[] MaskData)
        {
            if (Key != null && MaskData != null)
            {
                if (MainWindowViewModel.Instance.WCFManager.SorDataCallBackEvent != null)
                {
                    MainWindowViewModel.Instance.WCFManager.SorDataCallBackEvent.Invoke(1, Key, MaskData);
                }
            }
        }

        /// <summary>
        /// 负压波监测门限输出
        /// </summary>、
        /// <param name="Key"></param>
        /// <param name="ThreshData"></param>
        public void PreThreshCallBack(string Key, double[] ThreshData)
        {
            if (Key != null && ThreshData != null)
            {
                if (MainWindowViewModel.Instance.WCFManager.SorDataCallBackEvent != null)
                {
                    MainWindowViewModel.Instance.WCFManager.SorDataCallBackEvent.Invoke(2, Key, ThreshData);
                }
            }
        }

        /// <summary>
        /// DOLPHIN显示数据
        /// </summary>
        /// <param name="gra"></param>
        public void CallBackDOLPHINData(ServiceReference1.DolPoint[] DolPoints)
        {
            if (DolPoints == null)
            {
                return;
            }
            MainWindowViewModel.Instance.WCFManager.WindowDispatcher.Invoke(new Action(() =>
            {
                Type type = MainWindowViewModel.Instance.DolShowManager.DOLPHINMSGManager.GetType();
                PropertyInfo[] propertys = type.GetProperties();

                foreach (var property in propertys)
                {
                    //找到名称一致的点
                    var findpoint = DolPoints.FirstOrDefault(para => para.PointID.Replace(".", "").ToLower().Equals(property.Name.ToLower()));
                    if (findpoint != null)
                    {
                        property.SetValue(MainWindowViewModel.Instance.DolShowManager.DOLPHINMSGManager, findpoint.PointValue, null);
                    }
                }
            }));
        }
    }
}
