/*
* 
* 文件名称：XmlHelper.cs
* 文件标识：见配置管理计划书
* 摘    要：简要描述本文件的内容
* 
* 当前版本：1.0
* 作    者：吴士杰
* 完成日期：2015/4/8 10:54:24
*
* 取代版本：
* 原作者  ：
* 完成日期：
*/

using LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{
    /// <Summary>
    /// 作者:吴士杰
    /// 创建时间:2015/4/8 10:54:24
    /// <Summary>
    public static class XmlHelper
    {
        private static string _path = AppDomain.CurrentDomain.BaseDirectory + "server.config";
        private static Graphic _Graphic = null;
        private static object _obj = new object();

        /// <summary>
        /// 系统是否初始化
        /// </summary>
        public static bool IsIntial
        {
            get;
            set;
        }

        /// <summary>
        /// 获取配置文件对象
        /// </summary>
        public static Graphic Graphic
        {
            get
            {
                return _Graphic;
            }
        }

        /// <summary>
        /// 读取系统配置文件
        /// </summary>
        public static void ReadGraphic()
        {
            lock (_obj)
            {
                if (File.Exists(_path))
                {
                    if (_Graphic == null)
                    {
                        try
                        {
                            using (FileStream fStream = new FileStream(_path, FileMode.Open))
                            {
                                XmlSerializer deserial = new XmlSerializer(typeof(Graphic));
                                _Graphic = (Graphic)deserial.Deserialize(fStream);
                                fStream.Close();
                            }

                            MyLog.Log.Info(string.Format("读取系统配置信息成功！"));
                        }

                        catch
                        {
                            MyLog.Log.Info(string.Format("读取系统配置信息失败！"));
                            //添加日志文件
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 保存整个xml文件
        /// </summary>
        /// <param name="gra"></param>
        public static void Save(Graphic gra)
        {
            lock (_obj)
            {
                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }

                _Graphic = gra;

                using (FileStream fStream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    XmlSerializer xm = new XmlSerializer(typeof(Graphic));
                    xm.Serialize(fStream, gra);
                    fStream.Close();
                }
            }
        }

        /// <summary>
        /// 根据ID号查找Site
        /// </summary>
        /// <param name="PipeSiteID"></param>
        /// <returns></returns>
        public static PipeSite GetPipeSite(string PipeSiteID)
        {
            PipeSite findPipeSiteID = null;

            foreach (var pipesite in Graphic.PipeSites)
            {

                if (pipesite.SiteIndex.Equals(PipeSiteID))
                {
                    findPipeSiteID = pipesite;
                    break;
                }
            }

            return findPipeSiteID;
        }

        /// <summary>
        /// 根据索引号快速查找PressureSensor
        /// </summary>
        /// <param name="SensorIndex"></param>
        /// <returns></returns>
        public static PressureSensor GetPreSensor(string SensorIndex)
        {
            PressureSensor findSensor = null;
            if (SensorIndex == null)
            {
                return findSensor;
            }

            try
            {
                foreach (var pipesite in Graphic.PipeSites)
                {
                    var find_Sensor = pipesite.PreSensors.FirstOrDefault(para => para.PreSensorID.Equals(SensorIndex));
                    if (find_Sensor != null)
                    {
                        findSensor = find_Sensor;
                        break;
                    }
                }
            }
            catch
            {
                //填写日志信息
            }

            return findSensor;
        }

        /// <summary>
        /// 根据管段ID号找到对应的管段
        /// </summary>
        /// <param name="PipeIndex"></param>
        /// <returns></returns>
        public static Pipe GetPipeByIndex(string PipeIndex)
        {
            Pipe pipe = null;

            pipe = XmlHelper.Graphic.Pipes.FirstOrDefault(para => para.PipeIndex.ToString().Equals(PipeIndex));
            if (pipe != null)
            {
                return pipe;
            }
            else
            {
                pipe = XmlHelper.Graphic.CombinPipes.FirstOrDefault(para => para.PipeIndex.ToString().Equals(PipeIndex));
                if (pipe != null)
                {
                    return pipe;
                }
            }

            return pipe;
        }

        private static Dictionary<string, PressureSensor> _PreSensorsCache = new Dictionary<string, PressureSensor>();
        /// <summary>
        /// 压力传感器缓存，用来快速查找传感器 Key为OPCID
        /// </summary>
        public static Dictionary<string, PressureSensor> PreSensorCache
        {
            get
            {
                return _PreSensorsCache;
            }
        }

        /// <summary>
        /// 填充_PreSensorsCache Key为OPCID
        /// </summary>
        public static void InitialPreSensorCache()
        {
            foreach (var Pipesite in XmlHelper.Graphic.PipeSites)
            {
                foreach (var Pressure in Pipesite.PreSensors)
                {
                    if (!_PreSensorsCache.Keys.Contains(Pressure.OPCPointID))
                    {
                        _PreSensorsCache.Add(Pressure.OPCPointID, Pressure);
                    }
                }
            }
        }

        /// <summary>
        /// 快速查找PressureSensor
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static PressureSensor GetPressureSensorByID(string ID)
        {
            PressureSensor find_PreSensor = null;
            if (_PreSensorsCache.Keys.Contains(ID))
            {
                find_PreSensor = _PreSensorsCache[ID];
            }

            return find_PreSensor;
        }

    }
}
