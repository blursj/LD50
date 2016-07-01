using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ISafe_Model
{
    public class ACUDevice : PropertyNode
    {
        public ACUDevice()
        {
            Children = new ObservableCollection<PropertyNode>();
            _AllSensors = new ObservableCollection<PropertyNode>();
            _PCIModel = new PCI8018Model();
            _GPSData = new GPSModel();
            _DeviceLinks = new ACUInterface();
        }

        private PCI8018Model _PCIModel;
        /// <summary>
        ///  板卡配置信息
        /// </summary>
        public PCI8018Model PCIModel 
        {
            get 
            {
                return _PCIModel;
            }
            set 
            {
                _PCIModel = value;
                OnPropertyChanged("PCIModel");
            }
        }

        private GPSModel _GPSData;
        /// <summary>
        /// GPS配置信息
        /// </summary>
        public GPSModel GPSData 
        {
            get 
            {
                return _GPSData;
            }
            set 
            {
                _GPSData = value;
                OnPropertyChanged("GPSData");
            }
        }

        private ACUState _ACUState = ACUState.stop;
        /// <summary>
        /// 设备运行状态
        /// </summary>
        public ACUState ACUWorkState
        {
            get
            {
                return _ACUState;
            }
            set
            {
                _ACUState = value;
                OnPropertyChanged("ACUWorkState");
            }
        }

        private bool _IsSelected = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private string _Index;
        /// <summary>
        /// 索引号
        /// </summary>
        public string Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
                OnPropertyChanged("Index");
            }
        }

        private string _IP;
        /// <summary>
        /// ACU设备地址
        /// </summary>
        public string IP
        {
            get
            {
                return _IP;
            }
            set
            {
                _IP = value;
                OnPropertyChanged("IP");
            }
        }

        private int _MEDIUM;
        /// <summary>
        /// 管道传输介质类型 0-气体 1-液体
        /// </summary>
        public int MEDIUM
        {
            get 
            {
                return _MEDIUM;
            }
            set 
            {
                _MEDIUM = value;
                OnPropertyChanged("MEDIUM");
            }
        }

        private float _muting_zone;   //
        /// <summary>
        /// 静音区 输入 站内干扰屏蔽设置 盲区设置
        /// </summary>
        public float Muting_Zone
        {
            get
            {
                return _muting_zone;
            }
            set
            {
                _muting_zone = value;
                OnPropertyChanged("Muting_Zone");
            }
        }

        private float _site_location;     //
        /// <summary>
        /// 站点位置，站点在整个管道的位置 输入
        /// </summary>
        public float Site_Location
        {
            get
            {
                return _site_location;
            }
            set
            {
                _site_location = value;
                OnPropertyChanged("Site_Location");
            }
        }

        private ACUInterface _DeviceLinks;
        /// <summary>
        /// 传感器设备、GPS设备接入状况
        /// </summary>
        public ACUInterface DeviceLinks 
        {
            get 
            {
                return _DeviceLinks;
            }
            set 
            {
                _DeviceLinks = value;
                OnPropertyChanged("DeviceLinks");
            }
        }

        private ObservableCollection<PropertyNode> _AllSensors;
        /// <summary>
        /// 传感器集合
        /// </summary>
        public ObservableCollection<PropertyNode> AllSensors
        {
            get
            {
                //ObservableCollection<PropertyNode> _AllSensor = new ObservableCollection<PropertyNode>();

                //for (int i = 0; i < this.Children.Count;i++)
                //{
                //    for (int j = 0; j < this.Children[i].Children.Count;j++)
                //    {
                //        _AllSensor.Add(this.Children[i].Children[j]);
                //    }
                //}

                return _AllSensors;
            }

        }



        private DelegateCommand _AddSensorPair;
        /// <summary>
        /// 添加一个传感器组
        /// </summary>
        public DelegateCommand AddSensorPair
        {
            get
            {
                if (_AddSensorPair == null)
                {
                    _AddSensorPair = new DelegateCommand((obj) =>
                    {
                        SensorPair sensorpair = new SensorPair();
                        sensorpair.BlongACU = this;

                        this.Children.Add(sensorpair);
                    });
                }

                return _AddSensorPair;
            }
        }

    }
}
