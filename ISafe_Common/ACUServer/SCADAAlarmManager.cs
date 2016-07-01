using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using ISafe_Algorithm;
using LogHelper;

namespace ACUServer
{
    public class SCADAAlarmManager
    {
        private SCADAAlarmManager() { }

        private bool _pingSign = true;

        private bool _ReadSign = true;

        private bool _ClearSign = true;

        /// <summary>
        /// ping SCADA服务器线程
        /// </summary>
        private Thread _PingSCADAServerThread = null;

        /// <summary>
        /// SCADA服务器句柄号
        /// </summary>
        private IntPtr _handle = IntPtr.Zero;

        /// <summary>
        /// 读取SCADA点变量线程
        /// </summary>
        private Thread _ReadSCADAPointsThread = null;

        /// <summary>
        /// 默认10分钟清理一次
        /// </summary>
        private int waitSecond = 10 * 60 * 1000;
        /// <summary>
        /// 定期清理_SiteOperateCollection过时操作
        /// </summary>
        private Thread _ClearOverTimeOPThread = null;

        /// <summary>
        /// 站内操作集合
        /// </summary>
        private List<SiteOperate> _SiteOperateCollection = new List<SiteOperate>();

        /// <summary>
        /// 站内操作集合操作锁
        /// </summary>
        public object _SOLock = new object();

        private static SCADAAlarmManager _Instance = new SCADAAlarmManager();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static SCADAAlarmManager Instance
        {
            get
            {
                return _Instance;
            }
        }

        //是否连接到SCADA服务器
        private bool _IsConnectSCADAServer = false;
        /// <summary>
        /// 是否连接到SCADA服务器的标志
        /// </summary>
        public bool IsConnectSCADAServer
        {
            get
            {
                return _IsConnectSCADAServer;
            }
            set
            {
                _IsConnectSCADAServer = value;
            }
        }

        /// <summary>
        /// 启动SCADA点数据读取
        /// </summary>
        public void StartSCADAPointRead()
        {
            MyLog.Log.Info(string.Format("{0}:启动ping服务器线程、读取SCADA数据线程、误报时间段清除线程！", DateTime.Now.ToString()));

            _PingSCADAServerThread = new Thread(PingSCADAServerFUNC);
            _PingSCADAServerThread.IsBackground = true;
            _PingSCADAServerThread.Start();

            _ReadSCADAPointsThread = new Thread(ReadSCADAPointFUNC);
            _ReadSCADAPointsThread.IsBackground = true;
            _ReadSCADAPointsThread.Start();

            _ClearOverTimeOPThread = new Thread(ClearOverTimeSOFunc);
            _ClearOverTimeOPThread.Start();
            _ClearOverTimeOPThread.IsBackground = true;
        }

        /// <summary>
        /// 停止SCADA点数据读取
        /// </summary>
        public void CloseSCADAPointRead()
        {
            _pingSign = false;
            _ReadSign = false;
            _ClearSign = false;
        }

        /// <summary>
        /// 数据读取工作函数 读取频率为1s
        /// </summary>
        private void ReadSCADAPointFUNC()
        {
            while (_ReadSign)
            {
                if (_IsConnectSCADAServer)
                {
                    try
                    {
                        //循环次数太多 会不会影响读取性能？
                        foreach (var scadasite in XmlHelper.Graphic.SCADAConfigManager.SCADASites)
                        {
                            foreach (var scadapoint in scadasite.Points)
                            {
                                if (_handle != IntPtr.Zero)
                                {
                                    StringBuilder result = new StringBuilder(scadapoint.Capacity);

                                    //读取tag值,value将写进vsl中，读取此数据需要时间，但是不能确定需要多少时间如果时间较长则会出现延迟问题
                                    bool flag = CTAPI.ctTagRead(_handle, scadapoint.PointID, result, scadapoint.Capacity);
                                    if (flag)
                                    {
                                        if (scadapoint.IsFirstRead)
                                        {
                                            scadapoint.Value = result.ToString();
                                            scadapoint.IsFirstRead = false;
                                        }
                                        else
                                        {
                                            GenerateSiteOperate(scadapoint, result.ToString());
                                        }

                                        /***************将SCADA数据写入到文件中******/
                                        string pattern = @"^(-?\d+)(\.\d+)?$";
                                        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(pattern);
                                        if (re.IsMatch(result.ToString()))
                                        {
                                            Int16 value = Convert.ToInt16(Math.Max(Int16.MinValue, Math.Min(Int16.MaxValue, Convert.ToDouble(result.ToString()) * 100)));
                                            FileManager.Instance.SaveDateToWaveFile(scadasite.SCADASiteName + "-" + scadapoint.PointName, new Int16[] { value }, "SCADAData");
                                        }
                                      
                                    }
                                  
                                }

                            }
                        }
                    }
                    catch
                    {
                        MyLog.Log.Error(string.Format("{0}:读取SCADA点数据失败！", DateTime.Now.ToString()));
                    }

                }
                Thread.Sleep(XmlHelper.Graphic.SCADAConfigManager.ReadReq);
            }
        }

        /// <summary>
        /// 判断是否ping通服务器 频率为5s
        /// </summary>
        private void PingSCADAServerFUNC()
        {
            while (_pingSign)
            {
                Ping _PING = new Ping();
                PingReply pingReply = _PING.Send(XmlHelper.Graphic.SCADAConfigManager.SCADAServerIP);
                if (pingReply != null)
                {
                    //如果能Ping通
                    if (pingReply.Status == IPStatus.Success)
                    {
                        if (_handle == IntPtr.Zero)
                        {
                            _handle = CTAPI.ctOpen(XmlHelper.Graphic.SCADAConfigManager.SCADAServerIP, XmlHelper.Graphic.SCADAConfigManager.UserName, XmlHelper.Graphic.SCADAConfigManager.UserPwd, XmlHelper.Graphic.SCADAConfigManager.Mode);
                            if (_handle == IntPtr.Zero)
                            {
                                MyLog.Log.Info(string.Format("{0}:连接到SCADA服务器失败！", DateTime.Now.ToString()));
                                _IsConnectSCADAServer = false;
                                uint dwStatus = CTAPI.GetLastError(); // get error
                            }
                            else
                            {
                                MyLog.Log.Info(string.Format("{0}:连接到SCADA服务器成功！", DateTime.Now.ToString()));
                                _IsConnectSCADAServer = true;
                            }
                        }
                    }
                    else //如果ping不通则断开连接
                    {
                        try
                        {
                            if (_handle != IntPtr.Zero)
                            {
                                CTAPI.ctClose(_handle);
                            }
                        }
                        catch 
                        {
                            MyLog.Log.Error(string.Format("{0}:关闭SCADA连接出现位置问题！", DateTime.Now.ToString()));
                        }

                        _handle = IntPtr.Zero;
                        _IsConnectSCADAServer = false;
                    }
                }
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 定期清理站内操作缓存工作函数
        /// </summary>
        private void ClearOverTimeSOFunc()
        {
            while (_ClearSign)
            {
                lock (_SOLock)
                {
                    for (int i = 0; i < _SiteOperateCollection.Count; i++)
                    {
                        //结束时间早于现时间
                        if (DateTime.Compare(_SiteOperateCollection[i].OperateEndTime, DateTime.Now) < 0)
                        {
                            _SiteOperateCollection.RemoveAt(i);
                        }
                    }
                }
                Thread.Sleep(waitSecond);
            }
        }

        /// <summary>
        /// 产生一条站内操作
        /// </summary>
        /// <param name="point"></param>
        private void GenerateSiteOperate(SCADAPoint point, string readvalue)
        {
            if (point != null)
            {
                switch (point.DataType)
                {
                    case 0:

                        if (point.Value != null)
                        {
                            if (!point.Value.Equals(readvalue))
                            {
                                bool _canAddSiteOperate = false;
                                /******如果之前的站内操作还在继续受影响，则不添加此次站内操作*/
                                var find_SiteOperate = _SiteOperateCollection.FirstOrDefault(para => para.PointID.Equals(point.PointID));
                                if (find_SiteOperate != null)
                                {
                                    if (DateTime.Compare(find_SiteOperate.OperateStartTime, DateTime.Now) <= 0 && DateTime.Compare(find_SiteOperate.OperateEndTime, DateTime.Now) >= 0)
                                    {
                                        _canAddSiteOperate = true;
                                    }
                                }

                                if (!_canAddSiteOperate)
                                {
                                    SiteOperate newSiteOperate = new SiteOperate();
                                    newSiteOperate.OperateStartTime = DateTime.Now;
                                    newSiteOperate.OperateEndTime = DateTime.Now.AddSeconds(point.DelaySecond);
                                    newSiteOperate.PointID = point.PointID;
                                    foreach (var item in point.InfluencePipes)
                                    {
                                        newSiteOperate.InFluencePipe.Add(item);
                                    }

                                    lock (_SOLock)
                                    {
                                        _SiteOperateCollection.Add(newSiteOperate);
                                    }
                                }



                            }
                        }
                        point.Value = readvalue;

                        break;

                    case 2:

                        if (point.Value != null)
                        {
                            bool _canAddSiteOperate = false;
                            var find_SiteOperate = _SiteOperateCollection.FirstOrDefault(para => para.PointID.Equals(point.PointID));
                            if (find_SiteOperate != null)
                            {
                                if (DateTime.Compare(find_SiteOperate.OperateStartTime, DateTime.Now) <= 0 && DateTime.Compare(find_SiteOperate.OperateEndTime, DateTime.Now) >= 0)
                                {
                                    _canAddSiteOperate = true;
                                }
                            }
                            if (_canAddSiteOperate)
                            {
                                double interval = Math.Abs(Convert.ToDouble(point.Value) - Convert.ToDouble(readvalue));
                                if (interval >= point.UnChangeRage)
                                {
                                    SiteOperate newSiteOperate = new SiteOperate();
                                    newSiteOperate.OperateStartTime = DateTime.Now;
                                    newSiteOperate.OperateEndTime = DateTime.Now.AddSeconds(point.DelaySecond);
                                    newSiteOperate.PointID = point.PointID;
                                    foreach (var item in point.InfluencePipes)
                                    {
                                        newSiteOperate.InFluencePipe.Add(item);
                                    }

                                    lock (_SOLock)
                                    {
                                        _SiteOperateCollection.Add(newSiteOperate);
                                    }
                                }
                            }

                        }


                        point.Value = readvalue;
                        break;
                }
            }
        }

        /// <summary>
        /// 判断是否为误报
        /// </summary>
        /// <returns></returns>
        public bool JudgeErrorLeak(DateTime leaktime, string leakPipeIndex)
        {
            bool result = false;
            lock (_SOLock)
            {
                for (int i = 0; i < _SiteOperateCollection.Count; i++)
                {
                    if (_SiteOperateCollection[i].InFluencePipe.Contains(leakPipeIndex))
                    {
                        //落在时间区间内
                        if (DateTime.Compare(_SiteOperateCollection[i].OperateStartTime, leaktime) <= 0 && DateTime.Compare(_SiteOperateCollection[i].OperateEndTime, leaktime) >= 0)
                        {
                            result = true;
                            break;
                        }
                    }

                }
            }

            return result;
        }

    }
}
