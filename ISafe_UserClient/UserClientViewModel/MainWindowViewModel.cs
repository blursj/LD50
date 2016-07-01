using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClientViewModel;
using ISafe_Model;
using System.Collections.ObjectModel;

namespace UserClientViewModel
{
    public class MainWindowViewModel:PropertyCallBack
    {
        private MainWindowViewModel()
        {
            _WCFManager = new WCFViewModel();
            _PipesManager = new PipesViewModel();
            _PCISetingMes = new ObservableCollection<string>();
            _OPCProxysManager = new OPCProxyViewModel();
            _PipeSitesManager = new PipeSiteViewModel();
            _LD50ShowManager = new LD50DataShowViewModel();
            _ConfigSetManager = new ConfigSetViewModel();
            _DolShowManager = new DolShowViewModel();
            _LeakAlarmManager = new LeakAlarmViewModel();
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

        private WCFViewModel _WCFManager;
        /// <summary>
        /// 
        /// </summary>
        public WCFViewModel WCFManager 
        {
            get 
            {
                return _WCFManager;
            }
            set 
            {
                _WCFManager = value;
                OnPropertyChanged("WCFManager");
            }
        }

        private PipesViewModel _PipesManager;
        /// <summary>
        /// 管段管理
        /// </summary>
        public PipesViewModel PipesManager
        {
            get
            {
                return _PipesManager;
            }
            set
            {
                _PipesManager = value;
                OnPropertyChanged("PipesManager");
            }
        }

        private PipeSiteViewModel _PipeSitesManager;
        /// <summary>
        /// 管道站点管理
        /// </summary>
        public PipeSiteViewModel PipeSitesManager 
        {
            get
            {
                return _PipeSitesManager;
            }
            set 
            {
                _PipeSitesManager = value;
            }
        }

        private ObservableCollection<string> _PCISetingMes;
        /// <summary>
        /// ACU板卡配置信息
        /// </summary>
        public ObservableCollection<string> PCISetingMes
        {
            get
            {
                return _PCISetingMes;
            }
            set
            {
                _PCISetingMes = value;
            }
        }

        private OPCProxyViewModel _OPCProxysManager;
        /// <summary>
        /// OPC代理管理类
        /// </summary>
        public OPCProxyViewModel OPCProxysManager 
        {
            get 
            {
                return _OPCProxysManager;
            }
            set 
            {
                _OPCProxysManager = value;
            }
        }

        private LD50DataShowViewModel _LD50ShowManager;
        /// <summary>
        /// LD50显示管理
        /// </summary>
        public LD50DataShowViewModel LD50ShowManager 
        {
            get 
            {
                return _LD50ShowManager;
            }
            set
            {
                _LD50ShowManager = value;
            }
        }

        private ConfigSetViewModel _ConfigSetManager;
        /// <summary>
        /// 配置文件管理类
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

        private DolShowViewModel _DolShowManager;
        /// <summary>
        /// DOLPHIN数据显示类
        /// </summary>
        public DolShowViewModel DolShowManager 
        {
            get 
            {
                return _DolShowManager;
            }
            set 
            {
                _DolShowManager = value;
            }
        }

        private LeakAlarmViewModel _LeakAlarmManager;
        public LeakAlarmViewModel LeakAlarmManager 
        {
            get 
            {
                return _LeakAlarmManager;
            }
            set 
            {
                _LeakAlarmManager = value;
            }
        }

        private ObservableCollection<string> _RuningMSG = new ObservableCollection<string>();
        /// <summary>
        /// 运行操作信息
        /// </summary>
        public ObservableCollection<string> RuningMSG 
        {
            get 
            {
                return _RuningMSG;
            }
            set
            {
                _RuningMSG = value;
            }
        }

        public void OnClose() 
        {
            WCFManager.OnClose();
        }
    }
}
