using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{
    [XmlRoot]
    public class SCADAPoint
    {
        /// <summary>
        /// 变量名
        /// </summary>
        [XmlAttribute("PointName")]
        public string PointName
        {
            get;
            set;
        }

        /// <summary>
        /// 变量ID
        /// </summary>
        [XmlAttribute("PointID")]
        public string PointID
        {
            get;
            set;
        }

        /// <summary>
        /// 点值
        /// </summary>
        [XmlIgnore]
        public string Value
        {
            get;
            set;
        }

        [XmlAttribute("Capacity")]
        /// <summary>
        /// 值容量
        /// </summary>
        public int Capacity
        {
            get;
            set;
        }

        private List<string> _InfluencePipes = new List<string>();
        /// <summary>
        /// 影响的站点集合
        /// </summary>
        [XmlElement("InfluencePipe")]
        public List<string> InfluencePipes 
        {
            get 
            {
                return _InfluencePipes;
            }
            set 
            {
                _InfluencePipes = value;
            }
        }

       
        /// <summary>
        /// 影响持续秒
        /// </summary>
        [XmlAttribute("DelaySecond")]
        public int DelaySecond
        {
            get;
            set;
        }

        [XmlAttribute("UnChangeRage")]
        public double UnChangeRage
        {
            get;
            set;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        [XmlAttribute("DataType")]
        public int DataType
        {
            get;
            set;
        }

        private  bool _IsFirstRead = true;
        /// <summary>
        /// 判断是否为第一次读取
        /// </summary>
        [XmlIgnore]
        public bool IsFirstRead 
        {
            get 
            {
                return _IsFirstRead;
            }
            set 
            {
                _IsFirstRead = value;
            }
        }
    }
}
