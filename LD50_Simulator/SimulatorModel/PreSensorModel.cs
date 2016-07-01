using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace SimulatorModel
{
    [XmlRoot]
    public class PreSensorModel : PropertyCallBack
    {
        private string _FilePath;
        /// <summary>
        /// 波形源文件路径
        /// </summary>
        [XmlIgnore]
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                _FilePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        private int _ReadTimeMin;
        /// <summary>
        /// 文件读取时间
        /// </summary>
        [XmlAttribute("ReadTimeMin")]
        public int ReadTimeMin
        {
            get
            {
                return _ReadTimeMin;
            }
            set
            {
                _ReadTimeMin = value;
                OnPropertyChanged("ReadTimeMin");
            }
        }

        private bool _HasBackData;
        /// <summary>
        /// 是否载入初始管道数据（背景噪音）
        /// </summary>
        [XmlIgnore]
        public bool HasBackData 
        {
            get 
            {
                return _HasBackData;
            }
            set 
            {
                _HasBackData = value;
            }
        }

        /// <summary>
        /// 压力信号标志
        /// </summary>
        [XmlAttribute("PreSign")]
        public string PreSign
        {
            get;
            set;
        }

        private double _FloatingValue;
        /// <summary>
        /// 浮动值
        /// </summary>
        [XmlAttribute("FloatingValue")]
        public double FloatingValue
        {
            get
            {
                return _FloatingValue;
            }
            set
            {
                _FloatingValue = value;
                OnPropertyChanged("FloatingValue");
            }
        }

        private DateTime _LeakStartTime = DateTime.MinValue;
        /// <summary>
        /// 泄漏起始时间
        /// </summary>
        [XmlIgnore]
        public DateTime LeakStartTime
        {
            get 
            {
                return _LeakStartTime;
            }
            set 
            {
                _LeakStartTime = value;
            }
        }

        private DateTime _LeakEndTime = DateTime.MinValue;
        /// <summary>
        /// 泄漏结束时间
        /// </summary>
        [XmlIgnore]
        public DateTime LeakEndTime
        {
            get 
            {
                return _LeakEndTime;
            }
            set 
            {
                _LeakEndTime = value;
            }
        }

        /// <summary>
        /// 泄漏前值
        /// </summary>
        [XmlIgnore]
        public double LekedForeValue
        {
            get;
            set;
        }

        /// <summary>
        /// 泄漏后值
        /// </summary>
        [XmlIgnore]
        public double LekedAfrValue
        {
            get;
            set;
        }

        private bool _HasHestoryLeak = false;
        /// <summary>
        /// 是否发生历史泄漏
        /// </summary>
        [XmlIgnore]
        public bool HasHestoryLeak 
        {
            get
            {
                return _HasHestoryLeak;
            }
            set 
            {
                _HasHestoryLeak = value;
            }
        }

        private List<double> _NLeakPoints = new List<double>();
        /// <summary>
        /// 外部源数据缓存 二级缓存
        /// </summary>
        [XmlIgnore]
        public List<double> NLeakPoints
        {
            get
            {
                return _NLeakPoints;
            }
            set
            {
                _NLeakPoints = value;
            }
        }

        private Queue<double> _LeakPoints = new Queue<double>();
        /// <summary>
        /// 外部源数据缓存 一级缓存
        /// </summary>
        [XmlIgnore]
        public Queue<double> LeakPoints
        {
            get
            {
                return _LeakPoints;
            }
            set
            {
                _LeakPoints = value;
            }
        }

        private Queue<double> _MLeakPoints = new Queue<double>();
        /// <summary>
        /// 模拟泄漏信号缓存
        /// </summary>
        [XmlIgnore]
        public Queue<double> MleakPoints 
        {
            get 
            {
                return _MLeakPoints;
            }
            set 
            {
                _MLeakPoints = value;
            }
        }

        private List<double> _MSensorValues_Q = new List<double>();
        /// <summary>
        /// 完全软件模拟情况下，站点在不同管输量情况下的初始压力值 介质——汽油
        /// </summary>
        [XmlElement("SensorValue_Q")]
        public List<double> MSensorValues_Q 
        {
            get 
            {
                return _MSensorValues_Q;
            }
            set
            {
                _MSensorValues_Q = value;
            }
        }

        private List<double> _MSensorValues_C = new List<double>();
        /// <summary>
        /// 完全软件模拟情况下，站点在不同管输量情况下的初始压力值 介质——柴油
        /// </summary>
        [XmlElement("SensorValue_C")]
        public List<double> MSensorValues_C
        {
            get
            {
                return _MSensorValues_C;
            }
            set
            {
                _MSensorValues_C = value;
            }
        }

        private List<double> _MSensorValues_H = new List<double>();
        /// <summary>
        /// 完全软件模拟情况下，站点在不同管输量情况下的初始压力值 介质——混输
        /// </summary>
        [XmlElement("SensorValue_H")]
        public List<double> MSensorValues_H
        {
            get
            {
                return _MSensorValues_H;
            }
            set
            {
                _MSensorValues_H = value;
            }
        }

        private double _SelectedSensorValue;
        /// <summary>
        /// 选中的站点管道初始值
        /// </summary>
        [XmlIgnore]
        public double SeledctedSensorValue 
        {
            get 
            {
                return _SelectedSensorValue;
            }
            set 
            {
                _SelectedSensorValue = value;
                OnPropertyChanged("SeledctedSensorValue");
            }
        }

        

        /// <summary>
        /// _MLeakPoints对象锁
        /// </summary>
        public object _MLeakPointsLock = new object();

        /// <summary>
        /// LeakPoints操作锁
        /// </summary>
        public object _LeakPointsLock = new object();

        /// <summary>
        /// LekedForeValue 操作锁
        /// </summary>
        public object _CurrentPreLock = new object(); 

        private DelegateCommand _LoadFile = null;
        /// <summary>
        /// 载入压力文件数据
        /// </summary>
        [XmlIgnore]
        public DelegateCommand LoadFile
        {
            get
            {
                if (_LoadFile == null)
                {
                    _LoadFile = new DelegateCommand((obj) =>
                    {
                        PipeSite selectedPipe = obj as PipeSite;
                        if (selectedPipe != null)
                        {
                            //倒入外部压力文件数据
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.InitialDirectory = "d:\\";//注意这里写路径时要用c:\\而不是c:\
                            openFileDialog.Filter = "波形文件|*.wav";
                            openFileDialog.RestoreDirectory = true;
                            openFileDialog.FilterIndex = 1;
                            if (openFileDialog.ShowDialog() == true)
                            {
                                if (openFileDialog.FileName != null)
                                {
                                    FilePath = openFileDialog.FileName;

                                    ThreadPool.QueueUserWorkItem((_obj) =>
                                    {
                                        Wav wav = new Wav();

                                        try
                                        {
                                            using (System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                            {
                                                var oo = wav.GetWavInfo(fs);
                                                fs.Seek(0, System.IO.SeekOrigin.Begin);
                                                var bytes = new byte[46];
                                                //读取头信息
                                                fs.Read(bytes, 0, 46);

                                                //采集频率为10Hz
                                                int _ReadDataLength = Math.Min(2 * selectedPipe.PreSensorManager._ReadTimeMin * 60 * 10, Convert.ToInt32(fs.Length));
                                                byte[] wavedate = new byte[_ReadDataLength];
                                                //读取有效数据

                                                fs.Read(wavedate, 0, _ReadDataLength);
                                                lock (_LeakPointsLock)
                                                {
                                                    _LeakPoints.Clear();
                                                    _NLeakPoints.Clear();
                                                    for (int i = 0; i < (wavedate.Length / 2); i++)
                                                    {
                                                        byte[] singledata = new byte[2];
                                                        singledata[0] = wavedate[i * 2];
                                                        singledata[1] = wavedate[i * 2 + 1];

                                                        double getSingleDate = Convert.ToDouble(BitConverter.ToInt16(singledata, 0)) / 1000;
                                                        _LeakPoints.Enqueue(getSingleDate);
                                                        //二级缓存5min数据
                                                        if (_NLeakPoints.Count < 5 * 60 * 10)
                                                        {
                                                            _NLeakPoints.Add(getSingleDate);
                                                        }
                                                    }

                                                }

                                                lock (_CurrentPreLock)
                                                {
                                                    LekedForeValue = _NLeakPoints[_NLeakPoints.Count - 1];
                                                }

                                                _HasBackData = true;


                                                //数据读取完之后
                                                fs.Close();
                                            }
                                        }
                                        catch
                                        {
                                            //填写日志文件信息
                                        }

                                    }, null);


                                }
                            }
                        }


                    });
                }
                return _LoadFile;
            }
        }
    }
}
