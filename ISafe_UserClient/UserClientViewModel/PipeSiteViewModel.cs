using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ISafe_Model;

namespace UserClientViewModel
{
    public class PipeSiteViewModel
    {
        public PipeSiteViewModel()
        {
            _PipeSiteCollection = new ObservableCollection<PipeSiteModel>();
        }

        private ObservableCollection<PipeSiteModel> _PipeSiteCollection;
        /// <summary>
        /// 管段站点集合
        /// </summary>
        public ObservableCollection<PipeSiteModel> PipeSiteCollection
        {
            get
            {
                return _PipeSiteCollection;
            }
            set
            {
                _PipeSiteCollection = value;
            }
        }

        private Dictionary<string, PreSensorModel> _PreSensorCache = new Dictionary<string, PreSensorModel>();
        /// <summary>
        /// 快速找到ID号对应的PressureSensor
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PreSensorModel GetPreSensorByID(string ID)
        {
            PreSensorModel find_pressure = null;

            if (_PreSensorCache.Keys.Contains(ID))
            {
                find_pressure = _PreSensorCache[ID];
            }
            else
            {
                foreach (var site in PipeSiteCollection)
                {
                    foreach (var pressure in site.Children_next)
                    {
                        if ((pressure as PreSensorModel).OPCPointID.Equals(ID))
                        {
                            _PreSensorCache.Add((pressure as PreSensorModel).OPCPointID, pressure as PreSensorModel);
                            find_pressure = pressure as PreSensorModel;
                            break;
                        }
                    }
                }
            }

            return find_pressure;
        }

        private DelegateCommand _SavePipeSites;
        /// <summary>
        /// 保存修改
        /// </summary>
        public DelegateCommand SavePipeSites
        {
            get
            {
                if (_SavePipeSites == null)
                {
                    _SavePipeSites = new DelegateCommand((obj) =>
                    {
                        MainWindowViewModel.Instance.WCFManager.ServerGrapic.PipeSites = null;
                        MainWindowViewModel.Instance.WCFManager.ServerGrapic.PipeSites = new ServiceReference1.PipeSite[_PipeSiteCollection.Count];
                        for (int i = 0; i < PipeSiteCollection.Count; i++)
                        {
                            MainWindowViewModel.Instance.WCFManager.ServerGrapic.PipeSites[i] = ModelExchange.ExchangeToWCF(_PipeSiteCollection[i]);
                        }

                        lock (MainWindowViewModel.Instance.WCFManager._ClientLock)
                        {
                            try
                            {
                                MainWindowViewModel.Instance.WCFManager.Client.SetGraphic(MainWindowViewModel.Instance.WCFManager.ServerGrapic);
                                MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:管道站点参数设置成功！", DateTime.Now.ToString()));
                            }
                            catch
                            {
                                MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:管道站点参数设置失败！", DateTime.Now.ToString()));
                            }

                        }
                    });
                }
                return _SavePipeSites;
            }
        }

        private DelegateCommand _ResetThresh;
        /// <summary>
        /// 重置门限值
        /// </summary>
        public DelegateCommand ResetThresh
        {
            get
            {
                if (_ResetThresh == null)
                {
                    _ResetThresh = new DelegateCommand((obj) =>
                    {
                        PreSensorModel selectedPreSensor = obj as PreSensorModel;
                        if (selectedPreSensor != null)
                        {
                            try
                            {
                                bool result = MainWindowViewModel.Instance.WCFManager.Client.ResetPreSensorThresh(Convert.ToInt32(selectedPreSensor.PreSensorID), selectedPreSensor.ThreshMax, selectedPreSensor.ThreshMin);
                                if (result)
                                {
                                    MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:设置ID号为{1}的压力传感器检测门限成功！", DateTime.Now.ToString(), selectedPreSensor.PreSensorID));
                                }
                                else
                                {
                                    MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:设置ID号为{1}的压力传感器检测门限失败！", DateTime.Now.ToString(), selectedPreSensor.PreSensorID));
                                }
                            }
                            catch
                            {
                                //填写日志文件
                            }
                        }
                    });
                }
                return _ResetThresh;
            }
        }

    }
}
