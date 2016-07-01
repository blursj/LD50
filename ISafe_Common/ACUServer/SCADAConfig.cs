using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{
    public class SCADAConfig
    {
        /// <summary>
        /// SCADA系统服务器连接IP地址
        /// </summary>
        [XmlAttribute("SCADAServerIP")]
        public string SCADAServerIP
        {
            get;
            set;
        }

        /// <summary>
        /// 登录名称
        /// </summary>
        [XmlAttribute("UserName")]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        [XmlAttribute("UserPwd")]
        public string UserPwd
        {
            get;
            set;
        }

        /// <summary>
        /// 读取模式
        /// </summary>
        [XmlAttribute("Mode")]
        public uint Mode
        {
            get;
            set;
        }

        /// <summary>
        /// 读取频率 
        /// </summary>
        [XmlAttribute("ReadReq")]
        public int ReadReq
        {
            get;
            set;
        }

        private List<SCADASite> _SCADASites = new List<SCADASite>();
        /// <summary>
        /// SCADA站点集合
        /// </summary>
        [XmlElement("SCADASite")]
        public List<SCADASite> SCADASites
        {
            get
            {
                return _SCADASites;
            }
            set
            {
                _SCADASites = value;
            }
        }
    }

    [XmlRoot]
    public class SCADASite
    {
        /// <summary>
        /// SCADA站点名称
        /// </summary>
        [XmlAttribute("SCADASiteName")]
        public string SCADASiteName
        {
            get;
            set;
        }

        /// <summary>
        /// SCADA站点ID号
        /// </summary>
        [XmlAttribute("SCADASiteID")]
        public string SCADASiteID
        {
            get;
            set;
        }

        private List<SCADAPoint> _points = new List<SCADAPoint>();
        /// <summary>
        /// SCADA点集合
        /// </summary>
        [XmlElement("SCADAPoint")]
        public List<SCADAPoint> Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }
      
    }
}
