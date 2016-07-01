using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ACUServer
{
    /// <summary>
    /// 泄漏定位信息
    /// </summary>
    public class Location
    {
        /// <summary>
        /// 泄漏字符信息
        /// </summary>
        public string MGS
        {
            get;
            set;
        }

        /// <summary>
        /// 泄漏管段索引号
        /// </summary>
        public string PipeIndex
        {
            get;
            set;
        }


        /// <summary>
        /// 泄漏定位位置 相对于起始ACU站点
        /// </summary>
        public double LocateLength
        {
            get;
            set;
        }

        /// <summary>
        /// 起始站点索引号
        /// </summary>
        public string StartSiteIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 起始站点名称
        /// </summary>
        public string StartSiteName
        {
            get;
            set;
        }

        /// <summary>
        /// 终止站点索引号
        /// </summary>
        public string EndSiteIndex
        {
            get;
            set;
        }

        /// <summary>
        ///终止站点名称
        /// </summary>
        public string EndSiteName
        {
            get;
            set;
        }

        /// <summary>
        /// 定位时间
        /// </summary>
        public string LocateGetTime
        {
            get;
            set;
        }

        /// <summary>
        /// 以字符串的方式返回定位信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("泄漏定位产生时间：" + LocateGetTime.ToString());
            sb.AppendLine("泄漏定位信息：" + MGS);
            sb.AppendLine();
            return sb.ToString();
        }

        private DetectedType _LocateDetectedType;
        /// <summary>
        /// 检测方法
        /// </summary>
        public DetectedType LocateDetectedType
        {
            get 
            {
                return _LocateDetectedType;
            }
            set 
            {
                _LocateDetectedType = value;
                switch (_LocateDetectedType)
                {
                    case  DetectedType.infrasound:
                        _DetectedTypestring = "次声波";
                        break;
                    case DetectedType.pressure:
                        _DetectedTypestring = "负压波";
                        break;
                    case DetectedType.SCADA:
                        _DetectedTypestring = "SCADA";
                        break;  
                    default:
                        break;
                }
            }
        }

        private string _DetectedTypestring;
        /// <summary>
        /// 检测方法名称
        /// </summary>
        public string DetectedTypestring
        {
            get 
            {
                return _DetectedTypestring;
            }
            set
            {
                _DetectedTypestring = value;
            }
        }
    }

    /// <summary>
    /// 检测方式
    /// </summary>
    public enum DetectedType 
    {
        infrasound = 0, //次声波
        pressure = 1,   //负压波
        SCADA = 2       //第三方SCADA
    }
}
