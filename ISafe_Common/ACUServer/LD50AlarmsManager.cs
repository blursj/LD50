using ISafe_Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LogHelper;

namespace ACUServer
{
    public class LD50AlarmsManager : IDisposable
    {
        private const int _MaxItems = 200;
        private const int _MaxCheckItems = 50;

        private bool _IsLD50Initialized = false;
        /// <summary>
        /// 判断LD50系统是否初始化
        /// </summary>
        public bool IsLD50Initialized
        {
            get
            {
                return _IsLD50Initialized;
            }
            set
            {
                _IsLD50Initialized = value;
            }
        }

        private LD50AlarmsManager()
        {
            _PressureLeakConllection = new List<PressureLeak>();
            _PressureLocation = new List<Location>();
        }

        private static LD50AlarmsManager _Instance = new LD50AlarmsManager();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static LD50AlarmsManager Instance
        {
            get
            {
                return _Instance;
            }
        }

        private List<PressureLeak> _PressureLeakConllection;
        /// <summary>
        /// 负压波泄漏集合
        /// </summary>
        public List<PressureLeak> PressureLeakConllection
        {
            get
            {
                return _PressureLeakConllection;
            }
            set
            {
                _PressureLeakConllection = value;
            }
        }

        private List<Location> _PressureLocation;
        /// <summary>
        /// 负压波报警信息
        /// </summary>
        public List<Location> PressureLocation
        {
            get
            {
                return _PressureLocation;
            }
            set
            {
                _PressureLocation = value;
            }

        }



        /// <summary>
        /// 初始化LD50系统
        /// </summary>
        /// <returns></returns>
        public int InitialLD50()
        {
            int result = 1;

            //传感器总数
            int AllPreSensorsCount = 0;

            //压力数据采集频率
            int PressureReq = 0;

            //算法处理容量
            int SamplePerBlock = 0;

            //算法处理容量
            int DataBufferSize = 0;

            foreach (var site in XmlHelper.Graphic.PipeSites)
            {
                AllPreSensorsCount += site.PreSensors.Count;
                PressureReq = site.PressureReq;

                SamplePerBlock = site.SamplePerBlock;
                DataBufferSize = site.DataBufferSize;
            }

            try
            {
                int Algorithresult = Algorithm.InitPressureProcessor(AllPreSensorsCount, 1, PressureReq, SamplePerBlock, DataBufferSize, 0f, 0f);
                if (Algorithresult == 0)
                {
                    return Algorithresult;
                }
                MyLog.Log.Info(string.Format("{0}:调用InitPressureProcessor算法接口时成功", DateTime.Now.ToString()));
            }
            catch
            {
                result = 0;
                MyLog.Log.Error(string.Format("{0}:调用InitPressureProcessor算法接口时失败", DateTime.Now.ToString()));
            }

            try
            {
                foreach (var site in XmlHelper.Graphic.PipeSites)
                {

                    foreach (var pressure in site.PreSensors)
                    {
                        int updateresult = Algorithm.UpdatePressureThresh(Convert.ToInt32(pressure.PreSensorID), pressure.ThreshMax, pressure.ThreshMin);
                        if (updateresult == 0)
                        {
                            return updateresult;
                        }
                    }
                }
            }
            catch
            {
                result = 0;
                MyLog.Log.Error(string.Format("{0}:调用UpdatePressureThresh算法接口时失败", DateTime.Now.ToString()));
            }

            return result;
        }

        /// <summary>
        /// 根据压力传感器传输过来的压力值进行判断
        /// </summary>
        /// <returns></returns>
        public PressureLeak JudgeLeak(PrePoint sitePreData)
        {
            PressureLeak leak = null;

            if (sitePreData != null)
            {
                //先找到对应的压力传感器

                PressureSensor find_PressureSensor = XmlHelper.GetPressureSensorByID(sitePreData.PointID);

                //如果在站点集合中没有找到该压力传感器
                if (find_PressureSensor == null)
                {
                    return leak;
                }
                else
                {
                    if (sitePreData.PointValues == null)
                    {
                        return leak;
                    }

                    if (sitePreData.PointValues.Count != 10)
                    {
                        MyLog.Log.Info(string.Format("{0}:收到id号为{1}的传感器数据个数为{2}！", DateTime.Now.ToString(), find_PressureSensor.OPCPointID, sitePreData.PointValues.Count));
                    }

                    //判断数据是否能给到算法
                    bool canSendDataToAl = true; ;
                    string pattern = @"^(-?\d+)(\.\d+)?$";
                    System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(pattern);
                    for (int i = 0; i < sitePreData.PointValues.Count; i++)
                    {
                        if (!re.IsMatch(sitePreData.PointValues[i]))
                        {
                            canSendDataToAl = false;
                            break;
                        }
                    }

                    if (!canSendDataToAl)
                    {
                        MyLog.Log.Error(string.Format("{0}:id号为{1}的传感器数据异常，有不是实数的数据！", DateTime.Now.ToString(), find_PressureSensor.OPCPointID));
                        return leak;
                    }

                    //判断数据是否有效
                    bool isDataeffective = true;
                    for (int i = 0; i < sitePreData.PointValues.Count; i++)
                    {
                        if (Convert.ToDouble(sitePreData.PointValues[i]) == 9999 || Convert.ToDouble(sitePreData.PointValues[i]) > 100)
                        {
                            isDataeffective = false;
                            break;
                        }
                    }

                    if (isDataeffective)
                    {
                        find_PressureSensor.IsLinked = true;
                        try
                        {
                            Dictionary<string, bool> _GPSLink = new Dictionary<string, bool>();
                            _GPSLink.Add(find_PressureSensor.OPCPointID,true);
                            for (int i = 0; i < UserClientManager.Instance.Clients.Count; i++)
                            {
                                UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().PLCPreLinkingCallBack(_GPSLink);
                            }
                        }
                        catch
                        {
                            MyLog.Log.Error(string.Format("{0}:向客户端发送id号为{1}传感器状态失败！", DateTime.Now.ToString(), find_PressureSensor.OPCPointID));
                            //填写日志文件
                        }
                    }
                    else 
                    {
                        find_PressureSensor.IsLinked = false;
                        try
                        {
                            Dictionary<string, bool> _GPSLink = new Dictionary<string, bool>();
                            _GPSLink.Add(find_PressureSensor.OPCPointID, false);
                            for (int i = 0; i < UserClientManager.Instance.Clients.Count; i++)
                            {
                                UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().PLCPreLinkingCallBack(_GPSLink);
                            }
                        }
                        catch
                        {
                            MyLog.Log.Error(string.Format("{0}:向客户端发送id号为{1}传感器状态失败！", DateTime.Now.ToString(), find_PressureSensor.OPCPointID));
                            //填写日志文件
                        }
                        MyLog.Log.Error(string.Format("{0}:id号为{1}的传感器数为非连接状态下数据，传感器断开连接！", DateTime.Now.ToString(), find_PressureSensor.OPCPointID));
                        return leak;
                    }

                    //保存压力数据 *1000 WAVE图形显示小数点
                    //**************************************************************************
                    if (find_PressureSensor.IsSaveData)
                    {
                        Int16[] singlePreDate = new short[sitePreData.PointValues.Count];
                        for (int i = 0; i < sitePreData.PointValues.Count; i++)
                        {
                            singlePreDate[i] = Convert.ToInt16( Math.Max(Int16.MinValue,Math.Min(Int16.MaxValue, Convert.ToDouble(sitePreData.PointValues[i]) * 1000)));
                          
                        }

                        FileManager.Instance.SaveDateToWaveFile(sitePreData.PointName, singlePreDate, "PressureData");
                    }

                    //**************************************************************************


                    if (sitePreData.PointValues != null && sitePreData.PointValues.Count > 0)
                    {
                       
                            //向客户端发送的压力数据
                            double[] SendData = new double[sitePreData.PointValues.Count];

                            //负压波算法处理
                            float[] GetingData = new float[sitePreData.PointValues.Count];
                            for (int i = 0; i < sitePreData.PointValues.Count; i++)
                            {
                                GetingData[i] = Convert.ToSingle(sitePreData.PointValues[i]);
                                SendData[i] = Convert.ToDouble(sitePreData.PointValues[i]);
                            }

                            try 
                            {
                                //向客户端发送该数据
                                if (find_PressureSensor.AllSendToClient)
                                {
                                    for (int i = 0; i < UserClientManager.Instance.Clients.Count; i++)
                                    {
                                        UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().PressureDateCallBack(sitePreData.PointName + "-" + find_PressureSensor.PreSensorID, SendData);
                                    }
                                }
                            }
                            catch
                            {
                                MyLog.Log.Error(string.Format("{0}:向客户端发送负压波数据时失败，客户端不存在！", DateTime.Now.ToString()));
                            }
                           
                            if (LD50AlarmsManager.Instance.IsLD50Initialized)
                            {
                                //算法Mask检测值输出
                                float pDetValue = 0f;
                                //算法检测门限值值输出
                                float pDetThresh = 0f;
                                //数据块长度
                                int length = 10;
                                var find_pipesite = XmlHelper.Graphic.PipeSites.FirstOrDefault(para => para.SiteIndex.Equals(find_PressureSensor.BelongSiteID));
                                if (find_pipesite != null)
                                {
                                    length = find_pipesite.PressureReq;
                                }

                                int result_Process = 0;
                                try
                                {
                                    //负压波泄漏监测
                                    result_Process = Algorithm.ProcessPressure(Convert.ToInt32(find_PressureSensor.PreSensorID), ref GetingData[0], sitePreData.GetSec, sitePreData.GetMS, length, ref pDetValue, ref pDetThresh);
                                    find_PressureSensor.SetMinMaskValue(DateTime.Now, pDetValue);
                                }
                                catch
                                {
                                    MyLog.Log.Error(string.Format("{0}:调用ProcessPressure时出错，传感ID号为{1}！", DateTime.Now.ToString(), find_PressureSensor.OPCPointID));
                                    return leak;
                                }

                                Int16 getMaskValue = Convert.ToInt16(Math.Max(Int16.MinValue ,Math.Min(Int16.MaxValue, pDetValue * 1000)));

                                Int16 getThreshValue = Convert.ToInt16(Math.Max(Int16.MinValue, Math.Min(Int16.MaxValue, pDetThresh * 1000))); 

                                //保存负压波Mask波形
                                FileManager.Instance.SaveDateToWaveFile(sitePreData.PointName + "-" + "Mask", new Int16[] { getMaskValue }, "PressureData");

                                //保存负压波算法输出检测门限值
                                FileManager.Instance.SaveDateToWaveFile(sitePreData.PointName + "-" + "Thresh", new Int16[] { getThreshValue }, "PressureData");

                                //向客户端发送算法输出信息
                                try
                                {
                                    if (find_PressureSensor.AllSendToClient)
                                    {
                                        for (int i = 0; i < UserClientManager.Instance.Clients.Count; i++)
                                        {
                                            UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().PreMaskCallBack(sitePreData.PointName + "-" + find_PressureSensor.PreSensorID, new double[] { pDetValue });

                                            UserClientManager.Instance.Clients[i].Context.GetCallbackChannel<IUserCallBack>().PreThreshCallBack(sitePreData.PointName + "-" + find_PressureSensor.PreSensorID, new double[] { pDetThresh });
                                        }
                                    }
                                }
                                catch
                                {
                                    MyLog.Log.Error(string.Format("{0}:向客户端发送传感器数据时失败，传感ID号为{1}！", DateTime.Now.ToString(), find_PressureSensor.OPCPointID));
                                    //填写日志文件信息
                                }


                                //如果监测到泄漏
                                if (result_Process == 1)
                                {
                                    int pLeakStatus = 0;			//LEAK: ==1, leak detected; ==0, no leak.
                                    int pLeakTimeS = 0;             //TS
                                    int pLeakTimeMs = 0;			//TMS
                                    float pLeakDetectValue = 0f;		//Leak detection value
                                    float pLeakDetectThresh = 0f;

                                    int leak_result = -1;

                                    try 
                                    {
                                        leak_result = Algorithm.GetPressureDetectRlt(Convert.ToInt32(find_PressureSensor.PreSensorID), ref pLeakStatus, ref pLeakTimeS, ref pLeakTimeMs, ref pLeakDetectValue, ref pLeakDetectThresh);
                                    }
                                    catch 
                                    {
                                        MyLog.Log.Error(string.Format("{0}:调用GetPressureDetectRlt时出错，传感ID号为{1}！", DateTime.Now.ToString(), find_PressureSensor.OPCPointID));
                                        return leak;
                                    }
                                   
                                    if (leak_result == 1 && pLeakStatus == 1)
                                    {
                                        //判断是否为误报
                                        bool _iserrorleak = SCADAAlarmManager.Instance.JudgeErrorLeak(DateTime.Now, find_PressureSensor.PreSensorID);
                                        if (!_iserrorleak)
                                        {
                                            if (find_PressureSensor.MinMaskValueTime != DateTime.MinValue)
                                            {
                                                leak = new PressureLeak();
                                                leak.ms = pLeakTimeMs;
                                                leak.s = pLeakTimeS;
                                                leak.PressuresnsorIndex = Convert.ToInt32(find_PressureSensor.PreSensorID);
                                                leak.LeakTime = find_PressureSensor.MinMaskValueTime;
                                                find_PressureSensor.ClearMinMaskValue();
                                                leak.SiteIndex = Convert.ToInt32(find_pipesite.SiteIndex);
                                                leak.SiteName = find_pipesite.SiteName;
                                                leak.IsErrorLeak = _iserrorleak;

                                                if (_PressureLeakConllection.Count > _MaxItems)
                                                {
                                                    _PressureLeakConllection.RemoveAt(0);
                                                }
                                                _PressureLeakConllection.Add(leak);
                                            }

                                        }

                                    }

                                }
                            }
                       

                    }

                }
            }

            return leak;
        }

        /// <summary>
        /// 依据泄漏信息产生定位信息
        /// </summary>
        /// <returns></returns>
        public Location GenaratePreLocation(PressureLeak leak)
        {
            Location Prelocation = null;
            if (leak == null)
            {
                return Prelocation;
            }
            else
            {
                if (_PressureLeakConllection.Count > 0)
                {
                    int CheckItemNums = Math.Min(_MaxCheckItems, _PressureLeakConllection.Count);
                    for (int i = _PressureLeakConllection.Count - 1; i >= _PressureLeakConllection.Count - CheckItemNums; i--)
                    {
                        if (!_PressureLeakConllection[i].PressuresnsorIndex.Equals(leak.PressuresnsorIndex))
                        {
                            var find_pressure1 = XmlHelper.GetPreSensor(leak.PressuresnsorIndex.ToString());
                            var find_pressure2 = XmlHelper.GetPreSensor(_PressureLeakConllection[i].PressuresnsorIndex.ToString());

                            if (find_pressure1 == null || find_pressure2 == null)
                            {
                                return Prelocation;
                            }
                            else
                            {
                                //找到共享管段
                                pipesegment find_pipesegment1 = null;
                                pipesegment find_pipesegment2 = null;
                                foreach (var item in find_pressure1.MonitorPipeSegments)
                                {
                                    var findpipe = find_pressure2.MonitorPipeSegments.FirstOrDefault(para => para.SegmentIndex.Equals(item.SegmentIndex));
                                    if (findpipe != null)
                                    {
                                        find_pipesegment1 = item;
                                        find_pipesegment2 = findpipe;
                                        break;
                                    }
                                }

                                //如果找到共享管段
                                if (find_pipesegment1 != null && find_pipesegment2 != null)
                                {
                                    var Find_Pipe = XmlHelper.GetPipeByIndex(find_pipesegment1.SegmentIndex);

                                    if (Find_Pipe != null)
                                    {
                                        //double time_dif = (Convert.ToDouble(leak.s) + Convert.ToDouble(leak.ms) / 1000) - (Convert.ToDouble(_PressureLeakConllection[i].s) + Convert.ToDouble(_PressureLeakConllection[i].ms) / 1000);
                                        double time_dif = (leak.LeakTime - _PressureLeakConllection[i].LeakTime).TotalSeconds;
                                        if (Math.Abs(time_dif) < Find_Pipe.TimeThreshold)
                                        {
                                            //速度校正
                                            double speed_leak1 = 0;
                                            double speed_leak2 = 0;
                                            if (find_pipesegment1.SegmentLocation == 0)
                                            {
                                                speed_leak1 = Find_Pipe.Speed0;
                                            }
                                            else
                                            {
                                                speed_leak1 = Find_Pipe.Speed1;
                                            }

                                            if (find_pipesegment2.SegmentLocation == 0)
                                            {
                                                speed_leak2 = Find_Pipe.Speed0;
                                            }

                                            else
                                            {
                                                speed_leak2 = Find_Pipe.Speed1;
                                            }
                                            if (speed_leak2 != 0 && speed_leak1 != 0)
                                            {
                                                Prelocation = new Location();
                                                double relative_distance = Math.Round((time_dif * speed_leak1 * speed_leak2 + Find_Pipe.PipeLength * speed_leak1) / (speed_leak1 + speed_leak2), 2);

                                                var pipesite1 = XmlHelper.Graphic.PipeSites.FirstOrDefault(para => para.SiteIndex.Equals(find_pressure1.BelongSiteID));
                                                var pipesite2 = XmlHelper.Graphic.PipeSites.FirstOrDefault(para => para.SiteIndex.Equals(find_pressure2.BelongSiteID));
                                                if (pipesite1 != null && pipesite2 != null)
                                                {
                                                    Prelocation.MGS = string.Format("{0}站到{1}站发生泄漏，距离{0}站点{2}m处,请注意！", pipesite1.SiteName, pipesite2.SiteName, relative_distance);
                                                }

                                                Prelocation.StartSiteName = pipesite1.SiteName;
                                                Prelocation.StartSiteIndex = pipesite1.SiteIndex;
                                                Prelocation.EndSiteIndex = pipesite2.SiteIndex;
                                                Prelocation.EndSiteName = pipesite2.SiteName;
                                                Prelocation.LocateLength = relative_distance;
                                                Prelocation.PipeIndex = Find_Pipe.PipeIndex.ToString();
                                                Prelocation.LocateGetTime = DateTime.Now.ToString();
                                                Prelocation.LocateDetectedType = DetectedType.pressure;

                                                if (_PressureLocation.Count > _MaxItems)
                                                {
                                                    _PressureLocation.RemoveAt(0);
                                                }
                                                _PressureLocation.Add(Prelocation);
                                            }


                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            return Prelocation;
        }

        /// <summary>
        /// 系统资源退出
        /// </summary>
        public void Dispose()
        {
            try
            {
                Algorithm.ReleasePressureProcessor();
            }
            catch
            {
                LogHelper.MyLog.Log.Error("负压波系统退出失败！");
            }

        }
    }
}
