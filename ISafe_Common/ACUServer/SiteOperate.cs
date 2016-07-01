using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACUServer
{
    public class SiteOperate
    {
        /// <summary>
        /// 操作对应的SCADA变量ID号
        /// </summary>
        public string PointID
        {
            get;
            set;
        }

        /// <summary>
        /// 站内操作起始时间
        /// </summary>
        public DateTime OperateStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 站内操作影响结束时间
        /// </summary>
        public DateTime OperateEndTime
        {
            get;
            set;
        }

        private List<string> _InFluencePipe = new List<string>();
        /// <summary>
        /// 被影响的管段集合
        /// </summary>
        public List<string> InFluencePipe
        {
            get 
            {
                return _InFluencePipe;
            }
            set 
            {
                _InFluencePipe = value;
            }
        }
    }
}
