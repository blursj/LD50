using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ISafe_Model
{
    public class PCI8018Model:PropertyCallBack
    {
        public PCI8018Model() 
        {
            _ChannelGroups = new ObservableCollection<ChannelGroupModel>();
        }

        private int _DiluteSign = 10;
        /// <summary>
        /// 采集参数稀释度 默认为10
        /// </summary>
        public int DiluteSign
        {
            get
            {
                return _DiluteSign;
            }
            set
            {
                _DiluteSign = value;
                OnPropertyChanged("DiluteSign");
            }
        }

        private int _ReadStep = 1;
        /// <summary>
        /// 读取时间间隔 默认1秒
        /// </summary>
        public int ReadStep
        {
            get
            {
                return _ReadStep;
            }
            set
            {
                _ReadStep = value;
                OnPropertyChanged("ReadStep");
            }
        }

        private int _ChannelNums = 16;
        /// <summary>
        /// 每块采集卡上的通道数，默认为16
        /// </summary>
        public int ChanelNums 
        {
            get 
            {
                return _ChannelNums;
            }
            set 
            {
                _ChannelNums = value;
                OnPropertyChanged("ChanelNums");
            }
        }

        /// <summary>
        /// 通道集合
        /// </summary>
        public ObservableCollection<ChannelModel> Channels 
        {
            get 
            {
                var _Channels = new ObservableCollection<ChannelModel>();

                foreach (var Group in _ChannelGroups)
                {
                    foreach (var channel in Group.Channels)
                    {
                        _Channels.Add(channel);
                    }
                }

                return _Channels;
            }
           
        }

        private ObservableCollection<ChannelGroupModel> _ChannelGroups;
        public ObservableCollection<ChannelGroupModel> ChannelGroups 
        {
            get 
            {
                return _ChannelGroups;
            }
            set 
            {
                _ChannelGroups = value;
                OnPropertyChanged("ChannelGroups");
            }
        }

        private Int32 _Frequency = 1000;
        /// <summary>
        /// 采集频率, 单位为Hz, [3, 80000],默认1000
        /// </summary>
        public Int32 Frequency 
        {
            get 
            {
                return _Frequency;
            }
            set 
            {
                _Frequency = value;
                OnPropertyChanged("Frequency");
            }
        }

        private Int32 _TriggerMode = 0;
        /// <summary>
        /// 触发模式选择 0-软件内触发 1-软件外触发
        /// </summary>
        public Int32 TriggerMode 
        {
            get 
            {
                return _TriggerMode;
            }
            set 
            {
                _TriggerMode = value;
                OnPropertyChanged("_TriggerMode");
            }
        }

        private Int32 _TriggerSource = 0;
        /// <summary>
        /// 触发源选择 0-外部ATR触发源 1-外部DTR触发源
        /// </summary>
        public Int32 TriggerSource 
        {
            get 
            {
                return _TriggerSource;
            }
            set 
            {
                _TriggerSource = value;
                OnPropertyChanged("TriggerSource");
            }
        }

        private Int32 _TriggerType = 0;
        /// <summary>
        /// 触发类型选择(边沿触发0/脉冲触发1)
        /// </summary>
        public Int32 TriggerType 
        {
            get 
            {
                return _TriggerType;
            }
            set 
            {
                _TriggerType = value;
                OnPropertyChanged("TriggerType");
            }
        }

        private Int32 _TriggerDir = 0;
        /// <summary>
        /// 触发方向选择  负向触发(低脉冲/下降沿触发)-0 正向触发(高脉冲/上升沿触发)-1 正负向触发(高/低脉冲或上升/下降沿触发)-2
        /// </summary>
        public Int32 TriggerDir 
        {
            get 
            {
                return _TriggerDir;
            }
            set 
            {
                _TriggerDir = value;
                OnPropertyChanged("TriggerDir");
            }
        }

        private Int32 _TrigLevelVolt =0;
        /// <summary>
        ///触发电平(0--10000mV)
        /// </summary>
        public Int32 TrigLevelVolt 
        {
            get 
            {
                return _TrigLevelVolt;
            }
            set 
            {
                _TrigLevelVolt = value;
                OnPropertyChanged("TrigLevelVolt");
            }
        }

        private Int32 _TrigWindow = 40;
        /// <summary>
        ///触发灵敏窗[1, 65535], 单位50纳秒
        /// </summary>
        public Int32 TrigWindow 
        {
            get 
            {
                return _TrigWindow;
            }
            set
            {
                _TrigWindow = value;
                OnPropertyChanged("TrigWindow");
            }
        }

        private Int32 _ClockSource = 0;
        /// <summary>
        ///时钟源选择(内0/外时钟源1)
        /// </summary>
        public Int32 ClockSource 
        {
            get 
            {
                return _ClockSource;
            }
            set 
            {
                _ClockSource = value;
                OnPropertyChanged("ClockSource");
            }
        }

        private bool _bClockOutput = true;
        /// <summary>
        ///允许时钟输出到CLKOUT,=TRUE:允许时钟输出, =FALSE:禁止时钟输出
        /// </summary>
        public bool bClockOutput 
        {
            get 
            {
                return _bClockOutput;
            }
            set 
            {
                _bClockOutput = value;
                OnPropertyChanged("bClockOutput");
            }
        }

    }
}
