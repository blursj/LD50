using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{
    [XmlRoot]
    public class PressureSensor
    {
        private object _MinValueLock = new object();

        /// <summary>
        /// 压力传感器索引号
        /// </summary>
        [XmlAttribute("PreSensorID")]
        public string PreSensorID
        {
            get;
            set;
        }

        /// <summary>
        /// OPC点对应的ID
        /// </summary>
        [XmlAttribute("OPCPointID")]
        public string OPCPointID
        {
            get;
            set;
        }

        /// <summary>
        /// 压力传感器安装站点索引号
        /// </summary>
        [XmlAttribute("BelongSiteID")]
        public string BelongSiteID
        {
            get;
            set;
        }

        /// <summary>
        /// 压力传感器对应的采集数据
        /// </summary>
        [XmlIgnore]
        public float[] PressureValue
        {
            get;
            set;
        }

        /// <summary>
        /// 压力传感器是否连接
        /// </summary>
        [XmlIgnore]
        public bool IsLinked
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可以向客户端发送数据的标志
        /// </summary>
        [XmlAttribute("AllSendToClient")]
        public bool AllSendToClient
        {
            get;
            set;
        }

        /// <summary>
        /// 是否保存压力数据
        /// </summary>
        [XmlAttribute("IsSaveData")]
        public bool IsSaveData
        {
            get;
            set;
        }

        /// <summary>
        /// 最大检测门限值
        /// </summary>
        [XmlAttribute("ThreshMax")]
        public float ThreshMax
        {
            get;
            set;
        }

        /// <summary>
        /// 最小检测门限值
        /// </summary>
        [XmlAttribute("ThreshMin")]
        public float ThreshMin
        {
            get;
            set;
        }

        private DateTime _MinMaskValueTime = DateTime.MinValue;
        /// <summary>
        /// MASK值最小时对应的时间
        /// </summary>
        [XmlIgnore]
        public DateTime MinMaskValueTime
        {
            get
            {
                return _MinMaskValueTime;
            }
            set
            {
                _MinMaskValueTime = value;
            }
        }

        private float _MinMaskValue = 0f;
        /// <summary>
        /// MASK最小值
        /// </summary>
        [XmlIgnore]
        public float MinMaskValue
        {
            get
            {
                return _MinMaskValue;
            }
            set
            {
                _MinMaskValue = value;
            }
        }

        /// <summary>
        /// 设置最小MASK值
        /// </summary>
        /// <param name="currentTime"></param>
        /// <param name="currentValue"></param>
        public void SetMinMaskValue(DateTime currentTime, float currentValue)
        {
            if (currentTime == null)
            {
                return;
            }

            if (currentValue < _MinMaskValue)
            {
                lock (_MinValueLock)
                {
                    _MinMaskValue = currentValue;
                    _MinMaskValueTime = currentTime;
                }
            }
        }

        /// <summary>
        /// 恢复初始设置
        /// </summary>
        public void ClearMinMaskValue()
        {
            lock (_MinValueLock)
            {
                _MinMaskValue = 0f;
                _MinMaskValueTime = DateTime.MinValue;
            }
        }

        private List<pipesegment> _MonitorPipeSegments = new List<pipesegment>();
        /// <summary>
        /// 监控管段
        /// </summary>
        [XmlElement("MonitorPipeSegment")]
        public List<pipesegment> MonitorPipeSegments
        {
            get
            {
                return _MonitorPipeSegments;
            }
            set
            {
                _MonitorPipeSegments = value;
            }
        }
    }

    [XmlRoot]
    public class pipesegment
    {
        /// <summary>
        /// 监控管段索引号
        /// </summary>
        [XmlAttribute("SegmentIndex")]
        public string SegmentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 监控管段处于监控站点方位 0-上游 1-下游
        /// </summary>
        [XmlAttribute("SegmentLocation")]
        public int SegmentLocation
        {
            get;
            set;
        }

        /// <summary>
        /// 监控管段名称
        /// </summary>
        [XmlIgnore]
        public string SegmentName
        {
            get;
            set;
        }
    }
}
