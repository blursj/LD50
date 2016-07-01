using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Threading;


namespace Simulator_ViewModel
{
    public class MainWindowViewModel
    {
        /// <summary>
        /// UI主线程
        /// </summary>
        public Dispatcher WindowDispatcher
        {
            get;
            set;
        }

        /// <summary>
        /// 向服务器发送数据的定时器
        /// </summary>
        public Timer _SendDataTimer;

        /// <summary>
        /// 配置文件存储路径
        /// </summary>
        private string _path = AppDomain.CurrentDomain.BaseDirectory + "Simulator.config";

        /// <summary>
        /// 文件锁
        /// </summary>
        private object _ConfigLock = new object();

        string _FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Time.dat");

        StreamWriter _writer;

        private MainWindowViewModel()
        {
            _ConfigManager = new ConfigViewModel();
            _RunningMSG = new ObservableCollection<string>();
            _WCFManager = new WCFManagerViewModel();

            _writer = new StreamWriter(_FilePath, true);
        }

        private static MainWindowViewModel _Instance = new MainWindowViewModel();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static MainWindowViewModel Instance
        {
            get
            {
                return _Instance;
            }
        }

        private ConfigViewModel _ConfigManager;
        public ConfigViewModel ConfigManager
        {
            get
            {
                return _ConfigManager;
            }
            set
            {
                _ConfigManager = value;
            }
        }

        private WCFManagerViewModel _WCFManager;
        public WCFManagerViewModel WCFManager
        {
            get
            {
                return _WCFManager;
            }
            set
            {
                _WCFManager = value;
            }
        }

        private ObservableCollection<string> _RunningMSG;
        /// <summary>
        /// 系统运行信息
        /// </summary>
        public ObservableCollection<string> RunningMSG
        {
            get
            {
                return _RunningMSG;
            }
            set
            {
                _RunningMSG = value;
            }
        }



        /// <summary>
        /// 添加运行信息
        /// </summary>
        /// <param name="msg"></param>
        public void AddMSG(string msg)
        {
            if (msg == null)
            {
                return;
            }
            else
            {
                if (SendMSGToUIEvent != null)
                {
                    SendMSGToUIEvent.Invoke(msg);
                }
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public void InitialConfig()
        {
            lock (_ConfigLock)
            {
                if (File.Exists(_path))
                {
                    if (_ConfigManager.PipeSites.Count == 0)
                    {
                        try
                        {
                            using (FileStream fStream = new FileStream(_path, FileMode.Open))
                            {
                                XmlSerializer deserial = new XmlSerializer(typeof(ConfigViewModel));
                                _ConfigManager = (ConfigViewModel)deserial.Deserialize(fStream);
                                fStream.Close();
                            }
                        }

                        catch
                        {
                            //填写日志文件
                        }
                    }
                }
            }

            // 定时器 向WCF服务器发送模拟信号
            //_SendDataTimer = new Timer(1000);
            //_SendDataTimer.Elapsed += _SendDataTimer_Elapsed;

            //连接到WCF服务器
            System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                MainWindowViewModel.Instance.WCFManager.LinkWCFServer();
            }, null);

        }

        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _SendDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<ServiceReference1.PrePoint> _SendDatas = new List<ServiceReference1.PrePoint>();
            foreach (var pipeSiteItem in MainWindowViewModel.Instance.ConfigManager.PipeSites)
            {
                if (pipeSiteItem.PreSensorManager.NLeakPoints.Count < MainWindowViewModel.Instance.ConfigManager.SendFreq)
                {
                    continue;
                }

                ServiceReference1.PrePoint sendData = new ServiceReference1.PrePoint();
                sendData.PointValues = new List<string>();

                sendData.PointID = pipeSiteItem.PreSensorManager.PreSign;
                sendData.PointName = pipeSiteItem.SiteName;
                sendData.GetMS = DateTime.Now.Millisecond;
                sendData.GetSec = ConvertDateTimeInt(DateTime.Now);

                bool _IsHasLeak = false;
                //判断时间是否处于泄漏时间段
                if (pipeSiteItem.PreSensorManager.LeakStartTime != DateTime.MinValue && pipeSiteItem.PreSensorManager.LeakEndTime != DateTime.MinValue)
                {
                    //正处于泄漏期间
                    if (DateTime.Compare(pipeSiteItem.PreSensorManager.LeakStartTime, DateTime.Now) <= 0 && DateTime.Compare(pipeSiteItem.PreSensorManager.LeakEndTime, DateTime.Now) >= 0)
                    {
                        _IsHasLeak = true;
                    }
                }

                if (_IsHasLeak)
                {
                    lock (pipeSiteItem.PreSensorManager._MLeakPointsLock)
                    {
                        if (pipeSiteItem.PreSensorManager.MleakPoints.Count >= MainWindowViewModel.Instance.ConfigManager.SendFreq)
                        {
                            for (int i = 0; i < MainWindowViewModel.Instance.ConfigManager.SendFreq; i++)
                            {
                                sendData.PointValues.Add(pipeSiteItem.PreSensorManager.MleakPoints.Dequeue().ToString("0.0000"));
                            }
                        }
                    }
                }
                else
                {

                    //判断泄漏时间已过
                    if (pipeSiteItem.PreSensorManager.LeakEndTime != DateTime.MinValue)
                    {
                        if (DateTime.Compare(pipeSiteItem.PreSensorManager.LeakEndTime, DateTime.Now) < 0)
                        {
                            if (!pipeSiteItem.PreSensorManager.HasHestoryLeak)
                            {
                                pipeSiteItem.PreSensorManager.HasHestoryLeak = true;
                            }

                            pipeSiteItem.PreSensorManager.LekedForeValue = pipeSiteItem.PreSensorManager.LekedAfrValue;
                        }
                    }

                    if (pipeSiteItem.PreSensorManager.HasHestoryLeak)
                    {
                        Random rd = new Random();
                        for (int i = 0; i < MainWindowViewModel.Instance.ConfigManager.SendFreq; i++)
                        {
                            //sendData.PointValues.Add((pipeSiteItem.PreSensorManager.LekedForeValue - Convert.ToDouble(rd.Next(Convert.ToInt32(pipeSiteItem.PreSensorManager.FloatingValue * 1000))) / 1000).ToString("0.0000"));
                            sendData.PointValues.Add(pipeSiteItem.PreSensorManager.LekedForeValue.ToString("0.0000"));
                        }
                    }
                    else
                    {
                        if (pipeSiteItem.PreSensorManager.LeakPoints.Count >= MainWindowViewModel.Instance.ConfigManager.SendFreq)
                        {
                            lock (pipeSiteItem.PreSensorManager._LeakPointsLock)
                            {
                                for (int i = 0; i < MainWindowViewModel.Instance.ConfigManager.SendFreq; i++)
                                {
                                    sendData.PointValues.Add(pipeSiteItem.PreSensorManager.LeakPoints.Dequeue().ToString("0.0000"));
                                }
                            }
                        }
                        else
                        {
                            Random rd = new Random();
                            for (int i = 0; i < MainWindowViewModel.Instance.ConfigManager.SendFreq; i++)
                            {
                                sendData.PointValues.Add(pipeSiteItem.PreSensorManager.NLeakPoints[rd.Next(pipeSiteItem.PreSensorManager.NLeakPoints.Count - 1)].ToString("0.0000"));
                            }

                        }
                    }

                }

                _SendDatas.Add(sendData);

            }

            //向WCF服务器发送压力数据
            if (MainWindowViewModel.Instance.WCFManager.IsOnline)
            {
                try
                {
                    if (MainWindowViewModel.Instance.WCFManager.IsOnline)
                    {
                        MainWindowViewModel.Instance.WCFManager.Client.SendPLCData("OPCProxy_Simulator", _SendDatas);
                    }
                }
                catch
                {
                    AddMSG(string.Format("{0}:向服务器发送模拟数据失败！",DateTime.Now.ToString()));
                    //填写日志文件
                }

            }


            //向TChart发送显示数据
            Dictionary<string, List<double>> _SendDataCollection = new Dictionary<string, List<double>>();
            foreach (var item in _SendDatas)
            {
                List<double> data = new List<double>();
                foreach (var pointvalue in item.PointValues)
                {
                    data.Add(Convert.ToDouble(pointvalue));
                }
                _SendDataCollection.Add(item.PointName, data);

                _writer.Write(string.Format("{0} value:{1} s:{2} ms{3} ", item.PointName, item.PointValues[0], item.GetSec, item.GetMS));
            }

            _writer.WriteLine();
            _writer.Flush();

            if (SendDataToTchart != null)
            {
                SendDataToTchart.Invoke(_SendDataCollection);
            }

        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        public void Save()
        {
            lock (_ConfigLock)
            {
                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }

                using (FileStream fStream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    try
                    {
                        XmlSerializer xm = new XmlSerializer(typeof(ConfigViewModel));
                        xm.Serialize(fStream, _ConfigManager);
                        fStream.Close();
                    }
                    catch { }

                }
            }
        }

        /// <summary>
        /// 将DateTime转化为UTC时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt32((time - startTime).TotalSeconds);
            return intResult;
        }

        /// <summary>
        /// 向UI界面发送显示数据的委托类型
        /// </summary>
        /// <param name="SendData"></param>
        public delegate void SendDataToTchartHandler(Dictionary<string, List<double>> SendData);

        /// <summary>
        /// 向服务器传输数据委托
        /// </summary>
        public SendDataToTchartHandler SendDataToTchart;

        /// <summary>
        /// 向UI界面发送运行信息的委托类型
        /// </summary>
        /// <param name="MSG"></param>
        public delegate void SendMSGToUIHandler(string MSG);

        /// <summary>
        /// 向UI界面发送运行信息的委托
        /// </summary>
        public SendMSGToUIHandler SendMSGToUIEvent;

    }
}
