using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.ObjectModel;

namespace OPCClientProxy_ViewModel
{
    public class MainWindowViewModel
    {
        private string _path = AppDomain.CurrentDomain.BaseDirectory + "server.config";

        private MainWindowViewModel()
        {
            _OPCProxy = new OPCCommViewModel();
            _OPCServersVMInstance = new OPCServersViewModel();
            _SystemRunningMSG = new ObservableCollection<string>();
            _ProxySetViewModel = new OPCClientProxySetViewModel();
            _ConfigSetManager = new ConfigSetViewModel();
        }

        public System.Windows.Threading.Dispatcher WindowDispatcher
        {
            get;
            set;
        }

        private static MainWindowViewModel _Instance = new MainWindowViewModel();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static MainWindowViewModel Instance
        {
            get
            {
                return _Instance;
            }
        }

        private OPCCommViewModel _OPCProxy;
        public OPCCommViewModel OPCProxy
        {
            get
            {
                return _OPCProxy;
            }
            set
            {
                _OPCProxy = value;
            }
        }

        private OPCServersViewModel _OPCServersVMInstance;
        public OPCServersViewModel OPCServersVMInstance
        {
            get
            {
                return _OPCServersVMInstance;
            }
            set
            {
                _OPCServersVMInstance = value;
            }
        }

        private OPCClientProxySetViewModel _ProxySetViewModel;
        public OPCClientProxySetViewModel ProxySetViewModel 
        {
            get 
            {
                return _ProxySetViewModel;
            }
            set 
            {
                _ProxySetViewModel = value;
            }
        }

        private ObservableCollection<string> _SystemRunningMSG;
        /// <summary>
        /// 系统运行消息
        /// </summary>
        public ObservableCollection<string> SystemRunningMSG 
        {
            get 
            {
                return _SystemRunningMSG;
            }
            set 
            {
                _SystemRunningMSG = value;
            }
        }

        private ConfigSetViewModel _ConfigSetManager = null;
        /// <summary>
        /// PLC配置管理
        /// </summary>
        public ConfigSetViewModel ConfigSetManager 
        {
            get 
            {
                return _ConfigSetManager;
            }
            set 
            {
                _ConfigSetManager = value;
            }
        }


        /// <summary>
        /// 添加系统消息
        /// </summary>
        /// <param name="MSG"></param>
        public void AddRunningMSG(string MSG) 
        {
            if (SystemRunningMSG.Count >= 50)
            {
                SystemRunningMSG.RemoveAt(0);
            }

            SystemRunningMSG.Add(MSG);
        }


    }
}
