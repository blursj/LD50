using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_Model
{
    public class SensorModel:PropertyNode
    {
        private SensorPair _BelongSensorPair;
        /// <summary>
        /// 所属传感器对
        /// </summary>
        public SensorPair BelongSensorPair 
        {
            get 
            {
                return _BelongSensorPair;
            }
            set 
            {
                _BelongSensorPair = value;
                OnPropertyChanged("BelongSensorPair");
            }
        }

        private int _SensorIndex;
        /// <summary>
        /// 传感器索引号
        /// </summary>
        public int SensorIndex
        {
            get 
            {
                return _SensorIndex;
            }
            set 
            {
                _SensorIndex = value;
                OnPropertyChanged("SensorIndex");
            }
        }

        private bool _IsMainSensor;
        /// <summary>
        /// 是否为主传感器
        /// </summary>
        public bool IsMainSensor 
        {
            get 
            {
                return _IsMainSensor;
            }
            set 
            {
                _IsMainSensor = value;
                OnPropertyChanged("IsMainSensor");
            }
        }

        private ChannelGroupModel _LinkChannelGroup;
        /// <summary>
        /// 传感器链接的通道组
        /// </summary>
        public ChannelGroupModel LinkChannelGroup 
        {
            get 
            {
                return _LinkChannelGroup;
            }
            set 
            {
                _LinkChannelGroup = value;
                OnPropertyChanged("LinkChannelGroup");
            }
        }

        private DelegateCommand _DelSensor;
        /// <summary>
        /// 将该传感器删除
        /// </summary>
        public DelegateCommand DelSensor 
        {
            get 
            {
                if (_DelSensor == null)
                {
                    _DelSensor = new DelegateCommand((obj) => 
                    {
                        SensorModel sensor = obj as SensorModel;

                        if (sensor != null)
                        {
                            this._BelongSensorPair.Children.Remove(sensor);
                            this._BelongSensorPair.BlongACU.AllSensors.Remove(sensor);
                        }
                    });
                }

                return _DelSensor;
            }
        }
    }
}

