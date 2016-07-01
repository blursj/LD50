using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ISafe_Model
{
    public class PipeSiteModel:PropertyNode
    {
        private string _SiteIndex;
        /// <summary>
        /// 站点索引号
        /// </summary>
        public string SiteIndex
        {
            get 
            {
                return _SiteIndex;
            }
            set 
            {
                _SiteIndex = value;
            }
        }

        private int _PressureReq;
        /// <summary>
        /// 负压波的采样频率
        /// </summary>
        public int PressureReq
        {
            get 
            {
                return _PressureReq;
            }
            set
            {
                _PressureReq = value;
                OnPropertyChanged("PressureReq");
            }
        }

        private int _SamplePerBlock;
        /// <summary>
        /// 数据处理块容量
        /// </summary>
        public int SamplePerBlock
        {
            get 
            {
                return _SamplePerBlock;
            }
            set 
            {
                _SamplePerBlock = value;
                OnPropertyChanged("SamplePerBlock");
            }
        }

        public int _DataBufferSize;
        /// <summary>
        /// 数据处理块容量
        /// </summary>
        public int DataBufferSize
        {
            get
            {
                return _DataBufferSize;
            }
            set 
            {
                _DataBufferSize = value;
                OnPropertyChanged("DataBufferSize");
            }
        }

        private DelegateCommand _AddPreSensor;
        /// <summary>
        /// 添加压力变送器
        /// </summary>
        public DelegateCommand AddPreSensor
        {
            get
            {
                if (_AddPreSensor == null)
                {
                    _AddPreSensor = new DelegateCommand((obj) =>
                    {
                        PreSensorModel newPreSensor = new PreSensorModel();
                        int newIndex = 0;
                        var find_sensor = Children_next.FirstOrDefault(para => (para as PreSensorModel).PreSensorID.Equals(newIndex.ToString()));
                        while (find_sensor != null)
                        {
                            newIndex++;
                            find_sensor = Children_next.FirstOrDefault(para => (para as PreSensorModel).PreSensorID.Equals(newIndex.ToString()));
                        }
                        newPreSensor.PreSensorID = newIndex.ToString();
                        newPreSensor.Name = "传感器" + newPreSensor.PreSensorID;
                        newPreSensor.IsLink = false;
                        newPreSensor.NodeKey = newPreSensor.PreSensorID;
                        newPreSensor.Parent = this;
                        
                        Children_next.Add(newPreSensor);
                    });
                }
                return _AddPreSensor;
            }
        }
    }
}
