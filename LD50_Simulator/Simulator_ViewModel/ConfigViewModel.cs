using SimulatorModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Simulator_ViewModel
{
    [XmlRoot("Root")]
    public class ConfigViewModel : PropertyCallBack
    {
        private int _SendFreq;
        /// <summary>
        /// 频率
        /// </summary>
        [XmlAttribute("SendFreq")]
        public int SendFreq 
        {
            get 
            {
                return _SendFreq;
            }
            set 
            {
                _SendFreq = value;
            }
        }

        private int _LeakSpaceMin;
        /// <summary>
        /// 两次泄漏间隔（分钟）
        /// </summary>
        [XmlAttribute("LeakSpaceMin")]
        public int LeakSpaceMin
        {
            get
            {
                return _LeakSpaceMin;
            }
            set
            {
                _LeakSpaceMin = value;
            }
        }

        private DateTime _ForeLeakClickTime = DateTime.MinValue;
        /// <summary>
        /// 上次泄漏点击时间
        /// </summary>
        [XmlIgnore]
        public DateTime ForeLeakClickTime
        {
            get
            {
                return _ForeLeakClickTime;
            }
            set
            {
                _ForeLeakClickTime = value;
            }
        }

        private List<PipeSite> _PipeSites = new List<PipeSite>();
        /// <summary>
        /// 站点信息
        /// </summary>
        [XmlElement("PipeSite")]
        public List<PipeSite> PipeSites
        {
            get
            {
                return _PipeSites;
            }
            set
            {
                _PipeSites = value;
            }
        }

        private bool _IsSelectedWavDate = false;
        /// <summary>
        /// 是否选择外部WAV波形数据作为模拟器的背景压力
        /// </summary>
        [XmlIgnore]
        public bool IsSelectedWavData
        {
            get
            {
                return _IsSelectedWavDate;
            }
            set
            {
                _IsSelectedWavDate = value;
                OnPropertyChanged("IsSelectedWavData");

                if (_IsSelectedWavDate)
                {
                    IsSelectedFlow = false;
                }
                else
                {
                    IsSelectedFlow = true;
                }
            }
        }

        private bool _IsSelectedFlow = true;
        /// <summary>
        /// 是否选择管输量
        /// </summary>
        [XmlIgnore]
        public bool IsSelectedFlow
        {
            get
            {
                return _IsSelectedFlow;
            }
            set
            {
                _IsSelectedFlow = value;
                OnPropertyChanged("IsSelectedFlow");
            }
        }

        private int _SelectedMedium = -1;
        /// <summary>
        /// 输送介质选择 0-汽油 1-柴油 2-混输
        /// </summary>
        [XmlIgnore]
        public int SelectedMedium
        {
            get
            {
                return _SelectedMedium;
            }
            set
            {
                _SelectedMedium = value;
                OnPropertyChanged("SelectedMedium");

                switch (_SelectedMedium)
                {
                    case 0:

                        Selected_PipeFlows = PipeFlowModel_Q;

                        break;
                    case 1:

                        Selected_PipeFlows = PipeFlowModel_C;

                        break;
                    case 2:

                        Selected_PipeFlows = PipeFlowModel_H;

                        break;
                }
            }
        }

        private List<string> _PipeFlowModel_Q = new List<string>();
        /// <summary>
        /// 管道流量模型集合 汽油
        /// </summary>
        [XmlElement("PipeFlowModel_Q")]
        public List<string> PipeFlowModel_Q
        {
            get
            {
                return _PipeFlowModel_Q;
            }
            set
            {
                _PipeFlowModel_Q = value;
                OnPropertyChanged("PipeFlowModel_Q");
            }
        }

        private List<string> _PipeFlowModel_C = new List<string>();
        /// <summary>
        /// 管道流量模型集合 柴油
        /// </summary>
        [XmlElement("PipeFlowModel_C")]
        public List<string> PipeFlowModel_C
        {
            get
            {
                return _PipeFlowModel_C;
            }
            set
            {
                _PipeFlowModel_C = value;
                OnPropertyChanged("PipeFlowModel_C");
            }
        }

        private List<string> _PipeFlowModel_H = new List<string>();
        /// <summary>
        /// 管道流量模型集合 混输
        /// </summary>
        [XmlElement("PipeFlowModel_H")]
        public List<string> PipeFlowModel_H
        {
            get
            {
                return _PipeFlowModel_H;
            }
            set
            {
                _PipeFlowModel_H = value;
                OnPropertyChanged("PipeFlowModel_H");
            }
        }

        private List<string> _Selected_PipeFlows;
        /// <summary>
        /// 管道流量模型集合 选中的某介质下的管输模型集合
        /// </summary>
        [XmlIgnore]
        public List<string> Selected_PipeFlows
        {
            get
            {
                return _Selected_PipeFlows;
            }
            set
            {
                _Selected_PipeFlows = value;
                OnPropertyChanged("Selected_PipeFlows");
            }
        }

        private int _SelectedFlowModel;
        /// <summary>
        /// 选中的管道流量模型
        /// </summary>
        [XmlIgnore]
        public int SelectedFlowModel
        {
            get
            {
                return _SelectedFlowModel;
            }
            set
            {
                _SelectedFlowModel = value;
                OnPropertyChanged("SelectedFlowModel");
            }
        }

        private DelegateCommand _FlowSureCommand = null;
        /// <summary>
        /// 不同管输量不同介质 选择确定按钮
        /// </summary>
        [XmlIgnore]
        public DelegateCommand FlowSureCommand
        {
            get
            {
                if (_FlowSureCommand == null)
                {
                    _FlowSureCommand = new DelegateCommand((obj) =>
                    {

                        if (_SelectedMedium < 0)
                        {
                            MainWindowViewModel.Instance.AddMSG("请选择某种输送介质");
                            return;
                        }

                        if (_SelectedFlowModel < 0)
                        {
                            MainWindowViewModel.Instance.AddMSG("请选择某种管输量");
                            return;
                        }

                        foreach (var pipe in _PipeSites)
                        {
                            lock (pipe.PreSensorManager._LeakPointsLock)
                            {

                                List<double> _SelectedPressureDatas = null;
                                //选中汽油时
                                if (_SelectedMedium == 0)
                                {
                                    _SelectedPressureDatas = pipe.PreSensorManager.MSensorValues_Q;
                                }
                                //选中柴油时
                                else if (_SelectedMedium == 1)
                                {
                                    _SelectedPressureDatas = pipe.PreSensorManager.MSensorValues_C;
                                }
                                else if (_SelectedMedium == 2)
                                {
                                    _SelectedPressureDatas = pipe.PreSensorManager.MSensorValues_H;
                                }

                                if (_SelectedPressureDatas != null && _SelectedPressureDatas.Count > 0)
                                {
                                    lock (pipe.PreSensorManager._LeakPointsLock)
                                    {
                                        pipe.PreSensorManager.LeakPoints.Clear();
                                        pipe.PreSensorManager.NLeakPoints.Clear();
                                        Random rd = new Random();
                                        for (int i = 0; i < 60 * 30 * 5; i++)
                                        {

                                            double getSingleDate = _SelectedPressureDatas[_SelectedFlowModel];// +Convert.ToDouble(rd.Next(Convert.ToInt32(pipe.PreSensorManager.FloatingValue * 1000))) / 1000;
                                            pipe.PreSensorManager.LeakPoints.Enqueue(getSingleDate);
                                            //二级缓存5min数据
                                            if (pipe.PreSensorManager.NLeakPoints.Count < 5 * 60 * 10)
                                            {
                                                pipe.PreSensorManager.NLeakPoints.Add(getSingleDate);
                                            }
                                        }

                                        lock (pipe.PreSensorManager._CurrentPreLock)
                                        {
                                            pipe.PreSensorManager.LekedForeValue = pipe.PreSensorManager.NLeakPoints[pipe.PreSensorManager.NLeakPoints.Count - 1];
                                        }

                                        pipe.PreSensorManager.HasBackData = true;
                                    }

                                }

                            }
                        }
                        MainWindowViewModel.Instance.AddMSG("切换不同管输不同输油介质成功！");
                    });
                }
                return _FlowSureCommand;
            }
        }


        private List<PipeModel> _Pipes = new List<PipeModel>();
        /// <summary>
        /// 管段信息
        /// </summary>
        [XmlElement("Pipe")]
        public List<PipeModel> Pipes
        {
            get
            {
                return _Pipes;
            }
            set
            {
                _Pipes = value;
            }
        }

        private PipeModel _SelectedPipe;
        /// <summary>
        /// 选中的管段
        /// </summary>
        public PipeModel SelectedPipe
        {
            get
            {
                return _SelectedPipe;
            }
            set
            {
                _SelectedPipe = value;
                OnPropertyChanged("SelectedPipe");

                if (_SelectedPipe != null)
                {
                    _ReferPipeSites.Clear();
                    var findSite1 = _PipeSites.FirstOrDefault(para => para.SiteIndex.Equals(_SelectedPipe.PipeSite1Index));
                    if (findSite1 != null)
                    {
                        _ReferPipeSites.Add(findSite1);
                    }

                    var findSite2 = _PipeSites.FirstOrDefault(para => para.SiteIndex.Equals(_SelectedPipe.PipeSite2Index));
                    if (findSite2 != null)
                    {
                        _ReferPipeSites.Add(findSite2);
                    }
                }

            }
        }

        private ObservableCollection<PipeSite> _ReferPipeSites = new ObservableCollection<PipeSite>();
        /// <summary>
        /// 选中的管段两端站点
        /// </summary>
        public ObservableCollection<PipeSite> ReferPipeSites
        {
            get
            {
                return _ReferPipeSites;
            }
            set
            {
                _ReferPipeSites = value;
            }
        }

        private PipeSite _ReferPipeSite;
        /// <summary>
        /// 泄漏参照站点
        /// </summary>
        public PipeSite ReferPipeSite
        {
            get
            {
                return _ReferPipeSite;
            }
            set
            {
                _ReferPipeSite = value;
                OnPropertyChanged("ReferPipeSite");
            }
        }

        private double _ReferLength;
        /// <summary>
        /// 泄漏距离
        /// </summary>
        [XmlIgnore]
        public double ReferLength
        {
            get
            {
                return _ReferLength;
            }
            set
            {
                _ReferLength = value;
                OnPropertyChanged("ReferLength");
            }
        }

        private int _SelectedLeakMode = 0;
        /// <summary>
        /// 泄漏孔径
        /// </summary>
        [XmlIgnore]
        public int SelectedLeakMode
        {
            get
            {
                return _SelectedLeakMode;
            }
            set
            {
                _SelectedLeakMode = value;
                OnPropertyChanged("SelectedLeakMode");

                switch (_SelectedLeakMode)
                {
                    //3mm孔径
                    case 0:

                        SelectedLeakValue = LeakValue1;

                        break;
                    //6mm孔径
                    case 1:

                        SelectedLeakValue = LeakValue2;

                        break;

                    //12mm
                    case 2:

                        SelectedLeakValue = LeakValue3;
                        break;

                    //20mm
                    case 3:

                        SelectedLeakValue = LeakValue4;

                        break;
                }
            }
        }

        private int _LeakTimeMin;
        /// <summary>
        /// 泄漏持续时间 分钟为单位
        /// </summary>
        [XmlIgnore]
        public int LeakTimeMin
        {
            get
            {
                return _LeakTimeMin;
            }
            set
            {
                _LeakTimeMin = value;
                OnPropertyChanged("LeakTimeMin");
            }
        }

        /// <summary>
        /// 泄漏孔径为3mm情况下的压力下降值
        /// </summary>
        [XmlAttribute("LeakValue1")]
        public double LeakValue1
        {
            get;
            set;
        }

        /// <summary>
        /// 泄漏孔径为6mm情况下的压力下降值
        /// </summary>
        [XmlAttribute("LeakValue2")]
        public double LeakValue2
        {
            get;
            set;
        }

        /// <summary>
        /// 泄漏孔径为12mm情况下的压力下降值
        /// </summary>
        [XmlAttribute("LeakValue3")]
        public double LeakValue3
        {
            get;
            set;
        }

        /// <summary>
        /// 泄漏孔径为20mm情况下的压力下降值
        /// </summary>
        [XmlAttribute("LeakValue4")]
        public double LeakValue4
        {
            get;
            set;
        }

        /// <summary>
        /// 选择的泄漏孔径压力下降值
        /// </summary>
        [XmlAttribute("SelectedLeakValue")]
        public double SelectedLeakValue
        {
            get;
            set;
        }

        private DelegateCommand _SendLeakCommand;
        /// <summary>
        /// 发送泄漏信号命令
        /// </summary>
        [XmlIgnore]
        public DelegateCommand SendLeakCommand
        {
            get
            {
                if (_SendLeakCommand == null)
                {
                    _SendLeakCommand = new DelegateCommand((obj) =>
                    {

                        if (DateTime.Compare(ForeLeakClickTime.AddMinutes(LeakSpaceMin), DateTime.Now) > 0)
                        {
                            MainWindowViewModel.Instance.AddMSG(string.Format("{0}:两次模拟泄漏间隔时间太短，请运行一段时间后再模拟发生泄漏！", DateTime.Now.ToString()));
                            return;
                        }
                        else
                        {
                            ForeLeakClickTime = DateTime.Now;
                        }

                        if (_SelectedPipe == null || _LeakTimeMin == 0 || _ReferLength == 0 || _ReferPipeSite == null)
                        {
                            MainWindowViewModel.Instance.AddMSG(string.Format("{0}:请填写完整的泄漏信息！", DateTime.Now.ToString()));
                            return;
                        }
                        else
                        {
                            MainWindowViewModel.Instance.AddMSG(string.Format("{0}:开始模拟产生泄漏！", DateTime.Now.ToString()));

                            foreach (var PipeSiteItem in _ReferPipeSites)
                            {
                                //判断时间是否处于泄漏时间段 防止在泄漏期间再次发生泄漏
                                if (PipeSiteItem.PreSensorManager.LeakStartTime != DateTime.MinValue && PipeSiteItem.PreSensorManager.LeakEndTime != DateTime.MinValue)
                                {
                                    //正处于泄漏期间
                                    if (DateTime.Compare(PipeSiteItem.PreSensorManager.LeakStartTime, DateTime.Now) <= 0 && DateTime.Compare(PipeSiteItem.PreSensorManager.LeakEndTime, DateTime.Now) >= 0)
                                    {
                                        MainWindowViewModel.Instance.AddMSG(string.Format("{0}:{1}正在模拟产生泄漏，此次模拟泄漏无用！", DateTime.Now.ToString(), PipeSiteItem.SiteName));
                                        continue;
                                    }
                                }

                                if (!PipeSiteItem.PreSensorManager.HasBackData)
                                {
                                    MainWindowViewModel.Instance.AddMSG(string.Format("{0}:请载入管段{1}初始正常运行压力数据文件！", DateTime.Now.ToString(), PipeSiteItem.SiteName));
                                    continue;
                                }

                                //泄漏点距离站点距离
                                double _leaklength = 0;

                                //泄漏参照站点
                                if (PipeSiteItem.SiteIndex == _ReferPipeSite.SiteIndex)
                                {
                                    PipeSiteItem.PreSensorManager.LeakStartTime = DateTime.Now.AddSeconds(_ReferLength / _SelectedPipe.Speed);
                                    Console.WriteLine(DateTime.Now.ToString());
                                    Console.WriteLine(PipeSiteItem.PreSensorManager.LeakStartTime.ToString());
                                    Console.WriteLine(_ReferLength / _SelectedPipe.Speed);
                                    PipeSiteItem.PreSensorManager.LeakEndTime = PipeSiteItem.PreSensorManager.LeakStartTime.AddSeconds(_LeakTimeMin * 60);
                                    _leaklength = _ReferLength;
                                }
                                else
                                {
                                    PipeSiteItem.PreSensorManager.LeakStartTime = DateTime.Now.AddSeconds((_SelectedPipe.PipeLength - _ReferLength) / _SelectedPipe.Speed);
                                    Console.WriteLine(DateTime.Now.ToString());
                                    Console.WriteLine(PipeSiteItem.PreSensorManager.LeakStartTime.ToString());
                                    Console.WriteLine((_SelectedPipe.PipeLength - _ReferLength) / _SelectedPipe.Speed);
                                    PipeSiteItem.PreSensorManager.LeakEndTime = PipeSiteItem.PreSensorManager.LeakStartTime.AddSeconds(_LeakTimeMin * 60);
                                    _leaklength = _SelectedPipe.PipeLength - _ReferLength;
                                }

                                //清空下缓存
                                PipeSiteItem.PreSensorManager.MleakPoints.Clear();

                                if (PipeSiteItem.PreSensorManager.NLeakPoints.Count > 0)
                                {
                                    Random rd = new Random();
                                    //生成泄漏信号并保存到缓冲

                                    //站点压力下降度
                                    double _leakpressureValue = SelectedLeakValue * 3 / 4 + SelectedLeakValue * (_SelectedPipe.PipeLength - _leaklength) / (_SelectedPipe.PipeLength * 4);

                                    double a = Convert.ToDouble(SelectedLeakMode + 1);
                                    double lx = 1200 * a / (_LeakTimeMin * 60 * SendFreq + 1200 * (SelectedLeakMode + 1));
                                    for (int i = 1200 * (SelectedLeakMode + 1); i <= _LeakTimeMin * 60 * SendFreq + 1200 * (SelectedLeakMode + 1); i++)
                                    {
                                        double _generateData = ((1200 * a / Convert.ToDouble(i) - lx) / (a / (SelectedLeakMode + 1) - lx)) * _leakpressureValue + PipeSiteItem.PreSensorManager.LekedForeValue - _leakpressureValue;// +Convert.ToDouble(rd.Next(Convert.ToInt32(PipeSiteItem.PreSensorManager.FloatingValue * 1000))) / 1000;
                                        //double _generateData = PipeSiteItem.PreSensorManager.LekedForeValue - Convert.ToDouble(rd.Next(Convert.ToInt32(PipeSiteItem.PreSensorManager.FloatingValue * 1000))) / 1000 - (Convert.ToDouble(i) / (_LeakTimeMin * 60 * 10)) * SelectedLeakValue;
                                        // double _generateData = PipeSiteItem.PreSensorManager.LekedForeValue  - (Convert.ToDouble(i) / (_LeakTimeMin * 60 * 10)) * SelectedLeakValue;
                                        lock (PipeSiteItem.PreSensorManager._MLeakPointsLock)
                                        {
                                            PipeSiteItem.PreSensorManager.MleakPoints.Enqueue(_generateData);
                                        }

                                    }

                                    PipeSiteItem.PreSensorManager.LekedAfrValue = PipeSiteItem.PreSensorManager.LekedForeValue - _leakpressureValue;
                                }

                            }
                        }
                    });
                }
                return _SendLeakCommand;
            }
        }

        private DelegateCommand _StartCommand;
        /// <summary>
        /// 启动模拟器
        /// </summary>
        [XmlIgnore]
        public DelegateCommand StartCommand
        {
            get
            {
                if (_StartCommand == null)
                {
                    _StartCommand = new DelegateCommand((obj) =>
                    {
                        //启动定时器
                        try
                        {
                            //系统是否能启动
                            bool _canstart = false;
                            foreach (var item in _PipeSites)
                            {
                                if (item.PreSensorManager.HasBackData)
                                {
                                    _canstart = true;
                                    break;
                                }
                            }

                            if (_canstart)
                            {
                                if (MainWindowViewModel.Instance._SendDataTimer == null)
                                {
                                    MainWindowViewModel.Instance._SendDataTimer = new System.Timers.Timer(1000);
                                    MainWindowViewModel.Instance._SendDataTimer.Elapsed += MainWindowViewModel.Instance._SendDataTimer_Elapsed;
                                }
                                MainWindowViewModel.Instance._SendDataTimer.Start();
                                MainWindowViewModel.Instance.AddMSG(string.Format("{0}:模拟器开始运行！", DateTime.Now.ToString()));
                            }
                            else
                            {
                                MainWindowViewModel.Instance.AddMSG(string.Format("{0}:模拟器启动失败，请载入初始管道运行压力文件数据！", DateTime.Now.ToString()));
                            }
                        }
                        catch
                        {
                            MainWindowViewModel.Instance.AddMSG(string.Format("{0}:模拟器启动失败！", DateTime.Now.ToString()));
                        }

                    });
                }
                return _StartCommand;
            }
        }

        private DelegateCommand _StopCommand;
        /// <summary>
        /// 停止模拟
        /// </summary>
        [XmlIgnore]
        public DelegateCommand StopCommand
        {
            get
            {
                if (_StopCommand == null)
                {
                    _StopCommand = new DelegateCommand((obj) =>
                    {
                        try
                        {
                            if (MainWindowViewModel.Instance._SendDataTimer != null)
                            {
                                MainWindowViewModel.Instance._SendDataTimer.Stop();
                                MainWindowViewModel.Instance._SendDataTimer = null;

                                //foreach (var pipe in _PipeSites)
                                //{
                                //    pipe.PreSensorManager.NLeakPoints.Clear();
                                //    pipe.PreSensorManager.LeakPoints.Clear();
                                //    pipe.PreSensorManager.
                                //}
                            }

                            MainWindowViewModel.Instance.AddMSG(string.Format("{0}:模拟器停止运行！", DateTime.Now.ToString()));
                        }
                        catch
                        {
                            MainWindowViewModel.Instance.AddMSG(string.Format("{0}:模拟器停止失败！", DateTime.Now.ToString()));
                        }
                    });
                }
                return _StopCommand;
            }
        }


    }
}