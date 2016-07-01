using LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Timers;
using ISafe_Algorithm;

namespace ACUServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class OPCClientProxyServer : IOPCClientProxy
    {
        /// <summary>
        /// 对应的OPCClient代理客户端
        /// </summary>
        private OPCClientProxy _OPCClient = null;

        /// <summary>
        /// 记录心跳函数的更新时间
        /// </summary>
        private DateTime _BeatHeartUpdateTime = DateTime.MinValue;

        public OPCClientProxyServer()
        {
            OperationContext.Current.Channel.Closed += Channel_Closed;
        }

        /// <summary>
        /// 通信通道断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Channel_Closed(object sender, EventArgs e)
        {
            if (_OPCClient != null)
            {
                try
                {
                    //向所有展示端发送该代理已断开
                    foreach (var user in UserClientManager.Instance.Clients)
                    {
                        user.Context.GetCallbackChannel<IUserCallBack>().OPCProxyLinkCallBack(_OPCClient.OPCModel.GUID, false);
                    }

                    OPCClientProxyManager.Instance.OPCClientProxyCollection.Remove(_OPCClient);

                    MyLog.Log.Info(string.Format("标识符为{0}的OPCClient代理于服务器时间:{1}断开连接", _OPCClient.OPCModel.GUID, DateTime.Now.ToString()));

                    //向客户端发送PLC传感器连接连接状态

                    if (_OPCClient.OPCModel.GUID.Equals("OPCProxy_LD50"))
                    {
                        Dictionary<string, bool> _PresensorLinks = new Dictionary<string, bool>();
                        foreach (var pressure in XmlHelper.PreSensorCache)
                        {

                            pressure.Value.IsLinked = false;
                            _PresensorLinks.Add(pressure.Value.OPCPointID, pressure.Value.IsLinked);

                        }
                        for (int i = 0; i < UserClientManager.Instance.Clients.Count; i++)
                        {
                            UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().PLCPreLinkingCallBack(_PresensorLinks);
                        }
                    }



                }
                catch
                {
                    //添加日志文件信息
                }
            }
        }

        /// <summary>
        /// 监听OPCClient 登陆
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public bool Login(string GUID)
        {
            if (GUID == null && GUID == "")
            {
                return false;
            }

            MyLog.Log.Info(string.Format("{0}:接收到GUID为{1}的OPCClient的登陆请求！", DateTime.Now.ToString(), GUID));

            var find_opcclientproxy = OPCClientProxyManager.Instance.GetOPCClientProxyByGUID(GUID);

            //排除重复添加
            if (find_opcclientproxy == null)
            {
                //在库中查找对应的OPCClient
                var find_opcproxy = XmlHelper.Graphic.OPCProxys.FirstOrDefault(para => para.GUID.Equals(GUID));
                if (find_opcproxy != null)
                {
                    OPCClientProxy _Proxy = new OPCClientProxy();
                    _Proxy.OPCModel = find_opcproxy;
                    _Proxy.OPCModel.IsLinked = true;
                    _Proxy.OPCModel.GUID = GUID;
                    _Proxy.Context = OperationContext.Current;
                    OPCClientProxyManager.Instance.OPCClientProxyCollection.Add(_Proxy);
                    _OPCClient = _Proxy;

                    //向所有展示端发送该代理已连接上
                    try
                    {
                        foreach (var user in UserClientManager.Instance.Clients)
                        {
                            user.Context.GetCallbackChannel<IUserCallBack>().OPCProxyLinkCallBack(GUID, true);
                        }
                    }
                    catch
                    {
                        MyLog.Log.Info(string.Format("{0}:GUID为{1}的OPCClient成功登陆到服务器！", DateTime.Now.ToString(), GUID));
                        //填写日志文件信息
                    }


                    //负压波系统处理内核初始化
                    if (_Proxy.OPCModel.Sign.Equals("LD50") || _Proxy.OPCModel.Sign.Equals("Simulator"))
                    {
                        if (!LD50AlarmsManager.Instance.IsLD50Initialized)
                        {
                            //初始化LD50系统
                            int intitialResult = LD50AlarmsManager.Instance.InitialLD50();
                            if (intitialResult == 1)
                            {
                                MyLog.Log.Info("负压波系统内核初始化成功！");
                                LD50AlarmsManager.Instance.IsLD50Initialized = true;
                            }
                            else 
                            {
                                LD50AlarmsManager.Instance.IsLD50Initialized = false;
                                MyLog.Log.Info("负压波系统内核初始化失败！");
                                
                            }
                        }

                    }

                    MyLog.Log.Info(string.Format("标识符为{0}的OPCClient代理于服务器时间:{1}连接至服务器", GUID, DateTime.Now.ToString()));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 心跳工作函数
        /// </summary>
        public void BeatHeart()
        {
            _BeatHeartUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 接收OPC代理采集过来的压力数据
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="points"></param>
        public void SendPLCData(string GUID, List<PrePoint> Prepoints)
        {
            #region LD50负压波系统
            if (GUID.Equals("OPCProxy_LD50") || GUID.Equals("OPCProxy_Simulator"))
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((obj) =>
                {
                    List<PrePoint> _PreSonrDatas = obj as List<PrePoint>;

                    if (_PreSonrDatas != null && XmlHelper.IsIntial)
                    {
                        foreach (var presonrData in _PreSonrDatas)
                        {
                            //检测泄漏并保存波形文件
                            var LEAK = LD50AlarmsManager.Instance.JudgeLeak(presonrData);

                            if (LEAK != null)
                            {

                                try
                                {
                                    //保存泄漏信息
                                    FileManager.Instance.SaveAlarmToFile(presonrData.PointName + "-leak", LEAK.ToString(),"AlarmData");

                                    //向客户端发送该负压波的泄漏信息
                                    for (int i = 0; i < UserClientManager.Instance.Clients.Count; i++)
                                    {
                                        UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().PreLeakCallBack(LEAK);
                                    }

                                    var PreLocation = LD50AlarmsManager.Instance.GenaratePreLocation(LEAK);
                                    if (PreLocation != null)
                                    {
                                        FileManager.Instance.SaveAlarmToFile(presonrData.PointName + "-loaction", PreLocation.ToString(), "AlarmData");

                                        //向客户端发送该负压波的定位信息
                                        for (int i = 0; i < UserClientManager.Instance.Clients.Count; i++)
                                        {
                                            UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().LocationCallBack(PreLocation);
                                        }
                                    }
                                }
                                catch
                                {
                                    MyLog.Log.Error(string.Format("{0}向客户端发送报警信息失败！", DateTime.Now.ToString()));
                                }
                            }
                        }

                    }

                }), Prepoints);
            }
            #endregion


        }

        /// <summary>
        /// 接收DOLPHIN数据
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="DolPoints"></param>
        public void SendDolPhinData(string GUID, List<DolPoint> DolPoints)
        {
            #region DOLPHIN智能音波检漏系统定位数据
            if (GUID.Equals("OPCProxy_DOLPHIN"))
            {
                if (DolPoints.Count > 0)
                {
                    var leakcountPoint = DolPoints.FirstOrDefault(para => para.PointID.Equals("leak.count"));
                    if (leakcountPoint != null && Convert.ToInt32(leakcountPoint.PointValue) > 0)
                    {
                        #region DOLPHIN定位报警信息处理
                        DolLocation dolphin_location = new DolLocation();
                        Type type = dolphin_location.GetType();
                        PropertyInfo[] propertys = type.GetProperties();

                        foreach (var property in propertys)
                        {
                            //找到名称一致的点
                            var findpoint = DolPoints.FirstOrDefault(para => para.PointName.ToLower().Equals(property.Name.ToLower()));
                            if (findpoint != null)
                            {
                                property.SetValue(dolphin_location, findpoint.PointValue, null);
                            }
                        }

                        try
                        {
                            if (Convert.ToInt32(dolphin_location.Pipe) != -1)
                            {
                                //向客户端发送DOLPHIN定位信息
                                for (int i = 0; i < UserClientManager.Instance.Clients.Count; i++)
                                {
                                    UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().DolLocationCallBack(dolphin_location);
                                }

                                //将DOLPHIN定位信息保存到本地文件 以后要保存到数据库
                                string path = AppDomain.CurrentDomain.BaseDirectory + "DolphinLeak.dat";
                                if (File.Exists(path))
                                {
                                    StreamWriter writer = new StreamWriter(path, true);
                                    foreach (var property in propertys)
                                    {
                                        string leakMsg = property.Name + ":" + property.GetValue(dolphin_location, null);
                                        writer.WriteLine(leakMsg);
                                    }
                                    writer.WriteLine("******************************************");
                                    writer.Flush();
                                    writer.Close();
                                    writer = null;

                                }
                            }
                        }
                        catch
                        {
                            MyLog.Log.Error("接收和处理DOLPHIN信息失败!");
                        }
                        #endregion
                    }

                    #region DOLPHIN显示信息获取处理

                    //更新缓存
                    UserClientManager.Instance.DolPointsCache = DolPoints;

                    //向所有连接的展示端发送配置信息已发生改变
                    try
                    {
                        for (int USERClientNum = 0; USERClientNum < UserClientManager.Instance.Clients.Count; USERClientNum++)
                        {
                            UserClientManager.Instance.Clients[USERClientNum].Context.GetCallbackChannel<IUserCallBack>().CallBackDOLPHINData(DolPoints);
                        }
                    }
                    catch
                    {
                        //填写日志文件信息
                    }

                    #endregion
                }

            }
            #endregion
        }

    }
}
