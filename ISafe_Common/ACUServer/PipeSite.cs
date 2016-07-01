using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{
    [XmlRoot]
    public class PipeSite
    {
        /// <summary>
        /// 站点索引号
        /// </summary>
        [XmlAttribute("SiteIndex")]
        public string SiteIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [XmlAttribute("SiteName")]
        public string SiteName
        {
            get;
            set;
        }

        /// <summary>
        /// 负压波的采样频率
        /// </summary>
        [XmlAttribute("PressureReq")]
        public int PressureReq
        {
            get;
            set;
        }

        /// <summary>
        /// 数据处理块容量
        /// </summary>
        [XmlAttribute("SamplePerBlock")]
        public int SamplePerBlock
        {
            get;
            set;
        }

        /// <summary>
        /// 数据处理块容量
        /// </summary>
        [XmlAttribute("DataBufferSize")]
        public int DataBufferSize
        {
            get;
            set;
        }

        private List<PressureSensor> _PreSensors = new List<PressureSensor>();
        /// <summary>
        /// 站点安装的压力传感器列表
        /// </summary>
        [XmlElement("PressureSensor")]
        public List<PressureSensor> PreSensors
        {
            get
            {
                return _PreSensors;
            }
            set
            {
                _PreSensors = value;
            }
        }
      

    }
}
