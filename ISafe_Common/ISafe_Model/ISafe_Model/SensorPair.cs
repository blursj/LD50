using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ISafe_Model
{
    public class SensorPair:PropertyNode
    {
        public SensorPair() 
        {
            Children = new ObservableCollection<PropertyNode>();
        }

        private ACUDevice _BlongACU;
        /// <summary>
        /// 传感器组所属ACU
        /// </summary>
        public ACUDevice BlongACU 
        {
            get 
            {
                return _BlongACU;
            }
            set
            {
                _BlongACU = value;
                OnPropertyChanged("BlongACU");
            }
        }

        private int _SensorPairIndex;
        /// <summary>
        /// 传感器对索引号
        /// </summary>
        public int SensorPairIndex 
        {
            get 
            {
                return _SensorPairIndex;
            }
            set
            {
                _SensorPairIndex = value;
                OnPropertyChanged("SensorPairIndex");
            }
        }

        private double _Offset;
        /// <summary>
        /// 偏移量
        /// </summary>
        public double Offset
        {
            get 
            {
                return _Offset;
            }
            set 
            {
                _Offset = value;
                OnPropertyChanged("Offset");
            }
        }

        private double _Space;
        /// <summary>
        /// 传感器间隔
        /// </summary>
        public double Space 
        {
            get 
            {
                return _Space;
            }
            set 
            {
                _Space = value;
                OnPropertyChanged("Space");
            }
        }

        private PipeModel _LinkPipe;
        /// <summary>
        /// 传感器阵列链接的管道
        /// </summary>
        public PipeModel LinkPipe
        {
            get
            {
                return _LinkPipe;
            }
            set
            {
                _LinkPipe = value;
                OnPropertyChanged("LinkPipe");
            }
        }

        private float _mininum_threshold;
        /// <summary>
        /// 最低阀值 服务器端输入 关键设置
        /// </summary>
        public float Minninum_Threshold
        {
            get
            {
                return _mininum_threshold;
            }
            set
            {
                _mininum_threshold = value;
                OnPropertyChanged("Minninum_Threshold");
            }
        }

        private float _threshold_multiplier;  //
        /// <summary>
        /// 阀值增益 同上 输入
        /// </summary>
        public float Threshold_Multiplier
        {
            get
            {
                return _threshold_multiplier;
            }
            set
            {
                _threshold_multiplier = value;
                OnPropertyChanged("Threshold_Multiplier");
            }
        }

        private float _a_to_b_signal_delay;   //
        /// <summary>
        /// Sensor a到Sensor b的信号延迟 输入
        /// </summary>
        public float A_to_B_Signal_Delay
        {
            get
            {
                return _a_to_b_signal_delay;
            }
            set
            {
                _a_to_b_signal_delay = value;
                OnPropertyChanged("A_to_B_Signal_Delay");
            }
        }

        private double _Gain1;
        /// 第一级增益
        /// </summary>
        public double Gain1
        {
            get 
            {
                return _Gain1;
            }
            set 
            {
                _Gain1 = value;
                OnPropertyChanged("Gain1");
            }
        }

        private double _Gain2;
        /// <summary>
        /// 第二级增益
        /// </summary>
        public double Gain2
        {
            get 
            {
                return _Gain2;
            }
            set 
            {
                _Gain2 = value;
                OnPropertyChanged("Gain2");
            }
        }

        private DelegateCommand _DelSensorPair;
        /// <summary>
        /// 删除一个传感器组
        /// </summary>
        public DelegateCommand DelSensorPair
        {
            get
            {
                if (_DelSensorPair == null)
                {
                    _DelSensorPair = new DelegateCommand((obj) =>
                    {
                        SensorPair selectedPair = obj as SensorPair;

                        if (selectedPair != null)
                        {
                            foreach (var item in selectedPair.Children)
                            {
                               selectedPair.BlongACU.AllSensors.Remove(item);
                            }
                            selectedPair.BlongACU.Children.Remove(selectedPair);
                        }
                    });

                }
                return _DelSensorPair;
            }
        }

        private DelegateCommand _AddSensor;
        /// <summary>
        /// 在传感器组中添加一个传感器
        /// </summary>
        public DelegateCommand AddSensor 
        {
            get 
            {
                if (_AddSensor == null)
                {
                    _AddSensor = new DelegateCommand((obj) =>
                    {
                        SensorModel sensor = new SensorModel();
                        sensor.BelongSensorPair = this;

                        this.Children.Add(sensor);
                        this.BlongACU.AllSensors.Add(sensor);
                    });
                }

                return _AddSensor;
            }
        }

    }
}
