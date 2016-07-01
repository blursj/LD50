using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{
    [XmlRoot]
    public class OPCPxoryModel
    {
        private string _GUID;
        /// <summary>
        /// 唯一标识符
        /// </summary>
        [XmlAttribute("GUID")]
        public string GUID
        {
            get
            {
                return _GUID;
            }
            set
            {
                _GUID = value;
            }
        }

        private string _Sign;
        /// <summary>
        /// 代理标记
        /// </summary>
        [XmlAttribute("Sign")]
        public string Sign
        {
            get
            {
                return _Sign;
            }
            set
            {
                _Sign = value;
            }
        }

        private bool _IsLinked;
        /// <summary>
        /// 是否连接
        /// </summary>
        [XmlIgnore]
        public bool IsLinked
        {
            get
            {
                return _IsLinked;
            }
            set
            {
                _IsLinked = value;
            }
        }

        private string _Description;
        /// <summary>
        /// 代理描述
        /// </summary>
        [XmlAttribute("Description")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
    }
}
