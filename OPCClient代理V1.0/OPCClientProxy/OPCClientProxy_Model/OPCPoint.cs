using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OPCClientProxy_Model
{
    [XmlRoot]
    public class OPCPoint
    {
        /// <summary>
        /// 点标记
        /// </summary>
        [XmlAttribute("PointSign")]
        public string PointSign
        {
            get;
            set;
        }

        /// <summary>
        /// 点名称
        /// </summary>
        [XmlAttribute("PointName")]
        public string PointName
        {
            get;
            set;
        }

        /// <summary>
        /// 点标识
        /// </summary>
        [XmlAttribute("PointID")]
        public string PointID
        {
            get;
            set;
        }

        [XmlAttribute("BlongPLCSign")]
        public string BlongPLCSign
        {
            get;
            set;
        }

        [XmlIgnore]
        public string ReadValue
        {
            get;
            set;
        }
    }
}
