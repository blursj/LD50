using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{
    [XmlRoot]
    public class PressureLeak
    {
        /// <summary>
        /// 是否为误报
        /// </summary>
        [XmlIgnore]
        public bool IsErrorLeak
        {
            get;
            set;
        }

        /// <summary>
        /// 站点索引号
        /// </summary>
        [XmlAttribute("SiteIndex")]
        public int SiteIndex
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
        /// 压力传感器索引号
        /// </summary>
         [XmlAttribute("PressuresnsorIndex")]
        public int PressuresnsorIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 泄漏发生时刻
        /// </summary>
        [XmlAttribute("LeakTime")]
        public DateTime LeakTime
        {
            get;
            set;
        }

        /// <summary>
        /// 泄漏发生时刻的秒部分
        /// </summary>
        [XmlAttribute("s")]
        public int s
        {
            get;
            set;
        }

        /// <summary>
        /// 泄漏发生的毫秒部分
        /// </summary>
        [XmlAttribute("ms")]
        public int ms
        {
            get;
            set;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("泄漏站点名称：" + SiteName);
            sb.AppendLine("传感器索引号：" + PressuresnsorIndex);
            sb.AppendLine("泄漏时间：" + LeakTime.ToString());
            sb.AppendLine();

            return sb.ToString();
        }

    }
}
