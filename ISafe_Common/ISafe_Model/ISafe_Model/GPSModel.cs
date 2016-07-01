using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_Model
{
    public class GPSModel:PropertyCallBack
    {
        //sampling rate of PPS singal, Hz PPS采样率
        private int _SampleRate;
        public int SampleRate
        {
            get
            {
                return _SampleRate;
            }
            set 
            {
                _SampleRate = value;
                OnPropertyChanged("SampleRate");
            }
        }

        //Size of  time signal block  时间信号块大小
        private int _BlockSize;
        public int BlockSize
        {
            get
            {
                return _BlockSize;
            }
            set 
            {
                _BlockSize = value;
                OnPropertyChanged("BlockSize");
            }
        }

        //maximum time string delay, in ms 最大时间延迟
        private int _MaxTimeStrDelay;
        public int MaxTimeStrDelay
        {
            get 
            {
                return _MaxTimeStrDelay;
            }
            set 
            {
                _MaxTimeStrDelay = value;
                OnPropertyChanged("MaxTimeStrDelay");
            }
        }

        //PPS signal threshold 信号阀值
        private short _ThreshPPS;
        public short ThreshPPS
        {
            get 
            {
                return _ThreshPPS;
            }
            set
            {
                _ThreshPPS = value;
                OnPropertyChanged("ThreshPPS");
            }
        }

        private int _GPS_CH;
        public int GPS_CH
        {
            get 
            {
                return _GPS_CH;
            }
            set 
            {
                _GPS_CH = value;
                OnPropertyChanged("GPS_CH");
            }
        }

        private int _COMM_Index;
        //端口名称
        public int COMM_Index
        {
            get 
            {
                return _COMM_Index;
            }
            set
            {
                _COMM_Index = value;
                OnPropertyChanged("COMM_Index");

                switch (_COMM_Index)
                {
                    case 0:
                        _COMM_Name = "COM1";
                        break;
                    case 1:
                        _COMM_Name = "COM2";
                        break;
                    case 2:
                        _COMM_Name = "COM3";
                        break;
                    case 3:
                        _COMM_Name = "COM4";
                        break;
                    default:
                        _COMM_Name = "COM4";
                        break;
                }
            }
        }

        private string _COMM_Name;
        //端口名称
        public string COMM_Name
        {
            get
            {
                return _COMM_Name;
            }
            set
            {
                _COMM_Name = value;
                OnPropertyChanged("COMM_Name");

                switch (_COMM_Name.ToUpper())
                {
                    case "COM1":
                        _COMM_Index = 0;
                        break;
                    case "COM2":
                        _COMM_Index = 1;
                        break;
                    case "COM3":
                        _COMM_Index = 2;
                        break;
                    case "COM4":
                        _COMM_Index = 3;
                        break;
                    default:
                        _COMM_Index = 4;
                        break;
                }
            }
        }

        private int _BAUD_Rate;
        //波特率
        public int BAUD_Rate
        {
            get
            {
                return _BAUD_Rate;
            }
            set
            {
                _BAUD_Rate = value;
                OnPropertyChanged("BAUD_Rate");

                switch (_BAUD_Rate)
                {
                    case 600:
                        _Rate_Index = 0;
                        break;
                    case 1200:
                        _Rate_Index = 1;
                        break;
                    case 2400:
                        _Rate_Index = 2;
                        break;
                    case 4800:
                        _Rate_Index = 3;
                        break;
                    case 9600:
                        _Rate_Index = 4;
                        break;
                    case 19200:
                        _Rate_Index = 5;
                        break;
                    case 38400:
                        _Rate_Index = 6;
                        break;

                    default:
                        _Rate_Index = 0;
                        break;
                }
            }
        }

        private int _Rate_Index;
        public int Rate_Index 
        {
            get 
            {
                return _Rate_Index;
            }
            set 
            {
                _Rate_Index = value;
                OnPropertyChanged("Rate_Index");

                switch (_Rate_Index)
                {
                    case 0:
                        _BAUD_Rate = 600;
                        break;
                    case 1:
                        _BAUD_Rate = 1200;
                        break;
                    case 2:
                        _BAUD_Rate = 2400;
                        break;
                    case 3:
                        _BAUD_Rate = 4800;
                        break;
                    case 4:
                        _BAUD_Rate = 9600;
                        break;
                    case 5:
                        _BAUD_Rate = 19200;
                        break;
                    case 6:
                        _BAUD_Rate = 38400;
                        break;
                   
                    default:
                        _BAUD_Rate = 300;
                        break;
                }
            }
        }

        private int _DATA_BIT;
        //数据位
        public int DATA_BIT
        {
            get 
            {
                return _DATA_BIT;
            }
            set 
            {
                _DATA_BIT = value;
                OnPropertyChanged("DATA_BIT");
            }
        }

        private string _CHECKOUT;
        //检验，通过考核
        public string CHECKOUT
        {
            get 
            {
                return _CHECKOUT;
            }
            set
            {
                _CHECKOUT = value;
                OnPropertyChanged("CHECKOUT");
            }
        }

        private string _STOPBIT;
        //停止位
        public string STOPBIT
        {
            get 
            {
                return _STOPBIT;
            }
            set 
            {
                _STOPBIT = value;
                OnPropertyChanged("STOPBIT");
            }
        }

       
    }
}
