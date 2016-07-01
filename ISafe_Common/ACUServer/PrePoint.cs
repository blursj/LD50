using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACUServer
{
    public class PrePoint
    {
        /// <summary>
        /// 点名称
        /// </summary>
        public string PointName
        {
            get;
            set;
        }

        /// <summary>
        /// 点标识
        /// </summary>
        public string PointID
        {
            get;
            set;
        }

        private List<string> _PointValues = new List<string>();
        /// <summary>
        /// 点值
        /// </summary>
        public List<string> PointValues
        {
            get;
            set;
        }

        /// <summary>
        /// 读取时间秒
        /// </summary>
        public int GetSec
        {
            get;
            set;
        }

        /// <summary>
        /// 读取时间毫秒
        /// </summary>
        public int GetMS
        {
            get;
            set;
        }
    }
}
