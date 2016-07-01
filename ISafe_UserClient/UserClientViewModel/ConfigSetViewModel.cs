using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UserClientViewModel
{
    [XmlRoot("ConfigSet")]
    public class ConfigSetViewModel
    {
        [XmlAttribute("WCFServerIP")]
        public string WCFServerIP
        {
            get;
            set;
        }

        private List<ShowSite> _ShowSiteCollection = new List<ShowSite>();
        [XmlElement("ShowSite")]
        public List<ShowSite> ShowSiteCollection 
        {
            get 
            {
                return _ShowSiteCollection;
            }
            set
            {
                _ShowSiteCollection = value;
            }
        }
    }

     [XmlRoot]
    public class ShowSite 
    {
         [XmlAttribute("SiteName")]
         public string SiteName
         {
             get;
             set;
         }

         [XmlAttribute("SiteKey")]
         public string SiteKey
         {
             get;
             set;
         }
    }
}
