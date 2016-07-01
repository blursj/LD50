using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SimulatorModel
{
    [XmlRoot]
    public class PipeSite:PropertyCallBack
    {
        private int _SiteIndex;
        [XmlAttribute("SiteIndex")]
        public int SiteIndex 
        {
            get 
            {
                return _SiteIndex;
            }
            set 
            {
                _SiteIndex = value;
                OnPropertyChanged("SiteIndex");
            }
        }

        private string _SiteName;
        /// <summary>
        /// 站点名称
        /// </summary>
        [XmlAttribute("SiteName")]
        public string SiteName 
        {
            get 
            {
                return _SiteName;
            }
            set 
            {
                _SiteName = value;
                OnPropertyChanged("SiteName");
            }
        }

        /// <summary>
        /// 压力信号参数配置
        /// </summary>
        [XmlElement(ElementName = "PreSensorModel")]
        public PreSensorModel PreSensorManager
        {
            get;
            set;
        }
        
    }
}
