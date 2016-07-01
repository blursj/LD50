using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ISafe_Model
{
    public class PreSensorModel : PropertyNode
    {
        private string _PreSensorID;
        /// <summary>
        /// 压力传感器索引号
        /// </summary>
        public string PreSensorID
        {
            get
            {
                return _PreSensorID;
            }
            set
            {
                _PreSensorID = value;
                OnPropertyChanged("PreSensorID");
            }
        }

        private string _OPCPointID;
        /// <summary>
        /// OPC点对应的ID
        /// </summary>
        public string OPCPointID
        {
            get
            {
                return _OPCPointID;
            }
            set
            {
                _OPCPointID = value;
                OnPropertyChanged("OPCPointID");
            }
        }

        /// <summary>
        /// 是否可以向客户端发送数据的标志
        /// </summary>
        public bool AllSendToClient
        {
            get;
            set;
        }

        /// <summary>
        /// 是否保存压力数据
        /// </summary>
        public bool IsSaveData
        {
            get;
            set;
        }

        private float _ThreshMax;
        /// <summary>
        /// 最大检测门限值
        /// </summary>
        public float ThreshMax
        {
            get
            {
                return _ThreshMax;
            }
            set
            {
                _ThreshMax = value;
                OnPropertyChanged("ThreshMax");
            }
        }

        private float _ThreshMin;
        /// <summary>
        /// 最小检测门限值
        /// </summary>
        public float ThreshMin
        {
            get
            {
                return _ThreshMin;
            }
            set
            {
                _ThreshMin = value;
                OnPropertyChanged("ThreshMin");
            }
        }

        private ObservableCollection<SegmentPipe> _SegmentPipes = new ObservableCollection<SegmentPipe>();
        /// <summary>
        /// 监控管段列表
        /// </summary>
        public ObservableCollection<SegmentPipe> SegmentPipes
        {
            get
            {
                return _SegmentPipes;
            }
            set
            {
                _SegmentPipes = value;
            }
        }

        private DelegateCommand _AddSegmentPipe;
        /// <summary>
        /// 添加一条监控管段
        /// </summary>
        public DelegateCommand AddSegmentPipe
        {
            get
            {
                if (_AddSegmentPipe == null)
                {
                    _AddSegmentPipe = new DelegateCommand((obj) =>
                    {
                        SegmentPipe newSegmentPipe = new SegmentPipe();
                        newSegmentPipe.BelongPreSensor = this;
                        SegmentPipes.Add(newSegmentPipe);
                    });
                }
                return _AddSegmentPipe;
            }
        }


        private DelegateCommand _RemovePreSensor;
        /// <summary>
        /// 删除压力变送器
        /// </summary>
        public DelegateCommand RemovePreSensor
        {
            get
            {
                if (_RemovePreSensor == null)
                {
                    _RemovePreSensor = new DelegateCommand((obj) =>
                    {
                        PreSensorModel selectedPreSensor = obj as PreSensorModel;
                        if (selectedPreSensor != null)
                        {
                            if (selectedPreSensor.Parent != null)
                            {
                                selectedPreSensor.Parent.Children_next.Remove(selectedPreSensor);
                            }

                        }
                    });
                }
                return _RemovePreSensor;
            }
        }

    }

    public class SegmentPipe : PropertyCallBack
    {
        private PreSensorModel _BelongPreSensor;
        public PreSensorModel BelongPreSensor
        {
            get
            {
                return _BelongPreSensor;
            }
            set
            {
                _BelongPreSensor = value;
            }
        }

        private PipeModel _MoniterPipe;
        /// <summary>
        ///监控管段
        /// </summary>
        public PipeModel MoniterPipe
        {
            get
            {
                return _MoniterPipe;
            }
            set
            {
                _MoniterPipe = value;
                OnPropertyChanged("MoniterPipe");
            }
        }

        private int _SegmentLocation;
        /// <summary>
        /// 监控管段处于监控站点方位 0-上游 1-下游
        /// </summary>
        public int SegmentLocation
        {
            get
            {
                return _SegmentLocation;
            }
            set
            {
                _SegmentLocation = value;
                OnPropertyChanged("SegmentLocation");
            }
        }


        private DelegateCommand _RemoveSegmentPipe;
        /// <summary>
        /// 删除一条监控管段
        /// </summary>
        public DelegateCommand RemoveSegmentPipe
        {
            get
            {
                if (_RemoveSegmentPipe == null)
                {
                    _RemoveSegmentPipe = new DelegateCommand((obj) =>
                    {
                        SegmentPipe selectedPipe = obj as SegmentPipe;
                        if (selectedPipe != null)
                        {
                            if (BelongPreSensor != null)
                            {
                                BelongPreSensor.SegmentPipes.Remove(selectedPipe);
                            }

                        }
                    });
                }
                return _RemoveSegmentPipe;
            }
        }

        
    }
}
