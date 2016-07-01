using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ISafe_Model;

namespace UserClientViewModel
{
    public class OPCProxyViewModel
    {
        public OPCProxyViewModel() 
        {
            _OPCProxysCollection = new ObservableCollection<OPCProxyModel>();
        }

        private ObservableCollection<OPCProxyModel> _OPCProxysCollection;
        /// <summary>
        /// opc代理管理
        /// </summary>
        public ObservableCollection<OPCProxyModel> OPCProxysCollection 
        {
            get 
            {
                return _OPCProxysCollection;
            }
            set 
            {
                _OPCProxysCollection = value;
            }
        }

        private DelegateCommand _AddOPCProxy;
        /// <summary>
        /// 增加一个代理
        /// </summary>
        public DelegateCommand AddOPCProxy 
        {
            get 
            {
                if (_AddOPCProxy == null)
                {
                    _AddOPCProxy = new DelegateCommand((obj) => 
                    {
                        OPCProxyModel opcmodel = new OPCProxyModel();
                        OPCProxysCollection.Add(opcmodel);
                    });
                }
                return _AddOPCProxy;
            }
        }

        private DelegateCommand _RemoveOPCProxy;
        /// <summary>
        /// 删除一个代理
        /// </summary>
        public DelegateCommand RemoveOPCProxy
        {
            get 
            {
                if (_RemoveOPCProxy == null)
                {
                    _RemoveOPCProxy = new DelegateCommand((obj) => 
                    {
                        OPCProxyModel selectedModel = obj as OPCProxyModel;
                        if (selectedModel != null)
                        {
                            OPCProxysCollection.Remove(selectedModel);
                        }
                    });
                }
                return _RemoveOPCProxy;
            }
        }

        private DelegateCommand _SaveOPCProxy;
        /// <summary>
        /// 保存修改
        /// </summary>
        public DelegateCommand SaveOPCProxy
        {
            get 
            {
                if (_SaveOPCProxy == null)
                {
                    _SaveOPCProxy = new DelegateCommand((obj) => 
                    {
                        List<ServiceReference1.OPCPxoryModel> WCFOPCProxys = new List<ServiceReference1.OPCPxoryModel>();
                        for (int i = 0; i < OPCProxysCollection.Count; i++)
                        {
                            WCFOPCProxys.Add(ModelExchange.ExchangeToWCF(OPCProxysCollection[i]));
                        }
                        if (MainWindowViewModel.Instance.WCFManager.ServerGrapic != null)
                        {
                            MainWindowViewModel.Instance.WCFManager.ServerGrapic.OPCProxys = WCFOPCProxys.ToArray<ServiceReference1.OPCPxoryModel>();
                        }
                        lock (MainWindowViewModel.Instance.WCFManager._ClientLock)
                        {
                            MainWindowViewModel.Instance.WCFManager.Client.SetGraphic(MainWindowViewModel.Instance.WCFManager.ServerGrapic);
                        }
                    });
                }
                return _SaveOPCProxy;
            }
        }
    }
}
