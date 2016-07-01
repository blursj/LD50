using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCClientProxy_Model;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using LogHelper;


namespace OPCClientProxy_ViewModel
{
    public class OPCClientProxySetViewModel : PropertyCallBack
    {
        /// <summary>
        /// 心跳包线程退出标志
        /// </summary>
        private bool _HeartSign = false;

        private bool _IsOnLine = false;
        /// <summary>
        /// 服务器是否连接标志
        /// </summary>
        public bool IsOnline
        {
            get
            {
                return _IsOnLine;
            }
            set
            {
                _IsOnLine = value;
            }
        }

        /// <summary>
        /// Client使用锁
        /// </summary>
        public object _ClientObj = new object();

     
        private string _GUID;
        /// <summary>
        /// 代理唯一标示符
        /// </summary>
        public string GUID
        {
            get
            {
                return _GUID;
            }
            set
            {
                _GUID = value;
                OnPropertyChanged("GUID");
            }
        }

        private int _Requrecy;
        /// <summary>
        /// 采样频率
        /// </summary>
        public int Requrecy
        {
            get
            {
                return _Requrecy;
            }
            set
            {
                _Requrecy = value;
                OnPropertyChanged("Requrecy");
            }
        }

        private int _OPCTimeSetRequrecy;
        /// <summary>
        /// OPC采样时间校准
        /// </summary>
        public int OPCTimeSetRequrecy
        {
            get
            {
                return _OPCTimeSetRequrecy;
            }
            set
            {
                _OPCTimeSetRequrecy = value;
                OnPropertyChanged("OPCTimeSetRequrecy");
            }
        }

        private string _WCFServerIP;
        /// <summary>
        /// WCF通信服务器IP地址
        /// </summary>
        public string WCFServerIP
        {
            get
            {
                return _WCFServerIP;
            }
            set
            {
                _WCFServerIP = value;
                OnPropertyChanged("WCFServerIP");
            }
        }

        private ServiceReference1.OPCClientProxyClient _Client;
        /// <summary>
        /// WCF服务客户端
        /// </summary>
        public ServiceReference1.OPCClientProxyClient Client
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
        /// 连接到WCF服务器
        /// </summary>
        public void LinkWCFServer()
        {
            LoginWCFServer();

            Thread thread = new Thread(new ThreadStart(() =>
            {
                while (_HeartSign)
                {
                    if (_Client == null)
                    {
                        LoginWCFServer();
                    }
                    else
                    {
                        try
                        {
                            //发送心跳包
                            _Client.BeatHeart();
                        }
                        catch
                        {
                            MyLog.Log.Info(string.Format("{0}:与服务器失去连接...", DateTime.Now.ToString()));
                            lock (_ClientObj)
                            {
                                _Client = null;
                            }
                            _IsOnLine = false;
                        }
                    }
                    Thread.Sleep(5000);
                }

            }));

            _HeartSign = true;
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 退出关闭函数
        /// </summary>
        public void OnClose() 
        {
            //心跳线程退出
            _HeartSign = false;
        }


        /// <summary>
        /// 服务器连接函数
        /// </summary>
        private void LoginWCFServer()
        {
            try
            {
                lock (_ClientObj)
                {
                    MyLog.Log.Info(string.Format("{0}:尝试与服务器建立连接...", DateTime.Now.ToString()));

                    WCFCallBack callback = new WCFCallBack();
                    InstanceContext con = new InstanceContext(callback);

                    _Client = null;

                    _Client = new ServiceReference1.OPCClientProxyClient(con);
                    _Client.Open();

                    if (_Client.Login(MainWindowViewModel.Instance.ProxySetViewModel.GUID))
                    {
                        MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                        {
                            MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}:连接到WCF服务器", DateTime.Now.ToString()));
                            MyLog.Log.Info(string.Format("{0}:与服务器建立连接成功", DateTime.Now.ToString()));
                        }));

                    }
                    _IsOnLine = true;
                }


            }
            catch
            {
                MainWindowViewModel.Instance.WindowDispatcher.Invoke(new Action(() =>
                {
                    _IsOnLine = false; 
                    MainWindowViewModel.Instance.AddRunningMSG(string.Format("{0}:连接WCF服务器失败，可能原因是WCF服务器IP地址错误/网络已断开连接/网络配置错误", DateTime.Now.ToString()));
                    MyLog.Log.Info(string.Format("{0}:尝试连接到服务器失败！", DateTime.Now.ToString()));
                }));

            }

        }

    }

  
    public class WCFCallBack : ServiceReference1.IOPCClientProxyCallback
    {
        /// <summary>
        /// 重新设置DOL系统变量点值
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetDolPointValue(string pointID, object value)
        {
            bool result = false;

            try
            {
                MainWindowViewModel.Instance.OPCProxy.Write(pointID, value);
                result = true;
            }
            catch 
            {
                return result;
            }


            return result;
        }
    }

}
