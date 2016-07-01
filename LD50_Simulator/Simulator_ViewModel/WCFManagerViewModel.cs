using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Timers;

namespace Simulator_ViewModel
{
    public class WCFManagerViewModel
    {
        /// <summary>
        /// 主动连接到服务器的定时器
        /// </summary>
        private Timer _LoginWCFServerTimer;

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
        /// 断开连接
        /// </summary>
        public void OnClose() 
        {
            if (_LoginWCFServerTimer != null)
            {
                _LoginWCFServerTimer.Stop();
                _LoginWCFServerTimer = null;
            }
        }

        /// <summary>
        /// 连接到WCF服务器
        /// </summary>
        public void LinkWCFServer()
        {
            LoginWCFServer();
            _LoginWCFServerTimer = new Timer(5000);
            _LoginWCFServerTimer.Elapsed += _LoginWCFServerTimer_Elapsed;
            _LoginWCFServerTimer.Start();
        }

        /// <summary>
        /// 向服务器发送心跳包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _LoginWCFServerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_Client == null)
            {
                LoginWCFServer();
            }
            else
            {
                try
                {
                    _Client.BeatHeart();
                }
                catch
                {
                    MainWindowViewModel.Instance.AddMSG(string.Format("{0}:连接不到iSafe服务器，请检查网络连接状态！", DateTime.Now.ToString()));
                    _Client = null;
                    _IsOnLine = false;
                }
            }
        }

        /// <summary>
        /// 服务器连接函数
        /// </summary>
        private void LoginWCFServer()
        {
            try
            {
                WCFCallBack callback = new WCFCallBack();
                InstanceContext con = new InstanceContext(callback);

                _Client = null;
                _Client = new ServiceReference1.OPCClientProxyClient(con);
                _Client.Open();

                if (_Client.Login("OPCProxy_Simulator"))
                {
                    //连接上后所做的处理
                    _IsOnLine = true;
                    MainWindowViewModel.Instance.AddMSG(string.Format("{0}:连接到iSafe服务器！", DateTime.Now.ToString()));
                }
            }
            catch
            {
                //填写日志文件信息
            }

        }
    }

    public class WCFCallBack : ServiceReference1.IOPCClientProxyCallback
    {
        public void CallBack1()
        {
            throw new NotImplementedException();
        }
    }

}
