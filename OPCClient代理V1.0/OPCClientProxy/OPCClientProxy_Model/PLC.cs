using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OPCClientProxy_Model
{
    /// <summary>
    /// OPC点类型
    /// </summary>
    public enum PointType 
    {
        real = 0,
        Ave = 1,
        Year = 2,
        month = 3,
        day = 4,
        hour = 5,
        min = 6,
        sec = 7,
        ms = 8,
        PC_heartbeat=9,
        TimerSure = 10,
        Year_out = 11,
        month_out = 12,
        day_out = 13,
        hour_out = 14,
        min_out = 15,
        sec_out = 16,
        ms_out = 17,
        m_heartbeat = 18,
        other
    }

    [XmlRoot]
    public class PLC
    {
        /// <summary>
        /// PLC HeartBean 锁
        /// </summary>
        public object _PLC_HBLock = new object();

        public List<string> _HeartBean;
        /// <summary>
        /// 心跳标记
        /// </summary>
        public List<string> HeartBean
        {
            get 
            {
                return _HeartBean;
            }
            set 
            {
                _HeartBean = value;           
            }
        }

        /// <summary>
        /// PLC采集单元压力传感器连接状态
        /// </summary>
        public bool PLCPreLinked
        {
            get;
            set;
        }

        /// <summary>
        /// PLC标志
        /// </summary>
        [XmlAttribute("PLCSign")]
        public string PLCSign
        {
            get;
            set;
        }

        private List<OPCPoint> _PLC_Points = new List<OPCPoint>();
        /// <summary>
        /// PLC单元包含的点数据
        /// </summary>
        [XmlElement("OPCPoint")]
        public List<OPCPoint> PLC_Points
        {
            get 
            {
                return _PLC_Points;
            }
            set 
            {
                _PLC_Points = value;
            }
        }

        private List<OPCPoint> _PLC_OutPutPoints = new List<OPCPoint>();
        /// <summary>
        /// 输出点列表
        /// </summary>
        [XmlElement("OutPutPoint")]
        public List<OPCPoint> PLC_OutPutPoints 
        {
            get 
            {
                return _PLC_OutPutPoints;
            }
            set 
            {
                _PLC_OutPutPoints = value;
            }
        }

        //private OPCPoint _PC_heartbeat = new OPCPoint();
        ///// <summary>
        ///// OPCClient与OPCServer心跳 下传时
        ///// </summary>
        //[XmlElement("PC_heartbeat")]
        //public OPCPoint PC_heartbeat
        //{
        //    get
        //    {
        //        return _PC_heartbeat;
        //    }
        //    set
        //    {
        //        _PC_heartbeat = value;
        //    }
        //}

        //private OPCPoint _Stamp_heartbeat = new OPCPoint();
        ///// <summary>
        /////  OPCClient与OPCServer心跳 上传时
        ///// </summary>
        //[XmlElement("Stamp_heartbeat")]
        //public OPCPoint Stamp_heartbeat 
        //{
        //    get 
        //    {
        //        return _Stamp_heartbeat;
        //    }
        //    set 
        //    {
        //        _Stamp_heartbeat = value;
        //    }
        //}

        //private OPCPoint _C_time_Button = new OPCPoint();
        ///// <summary>
        ///// //bit0=1  下发确认；bit0=0  下发取消；
        ///// </summary>
        //[XmlElement("TimerSure")]
        //public OPCPoint C_time_Button 
        //{
        //    get 
        //    {
        //        return _C_time_Button;
        //    }
        //    set 
        //    {
        //        _C_time_Button = value;
        //    }
        //}

        //private OPCPoint _PC_Year = new OPCPoint();
        ///// <summary>
        ///// 下传校正时间年部分
        ///// </summary>
        //[XmlElement("SetYear")]
        //public OPCPoint PC_Year 
        //{
        //    get 
        //    {
        //        return _PC_Year;
        //    }
        //    set 
        //    {
        //        _PC_Year = value;
        //    }
        //}

        //private OPCPoint _PC_Month = new OPCPoint();
        ///// <summary>
        ///// 下传校正时间月部分
        ///// </summary>
        //[XmlElement("SetMonth")]
        //public OPCPoint PC_Month
        //{
        //    get
        //    {
        //        return _PC_Month;
        //    }
        //    set
        //    {
        //        _PC_Month = value;
        //    }
        //}

        //private OPCPoint _PC_Day = new OPCPoint();
        ///// <summary>
        ///// 下传校正时间天部分
        ///// </summary>
        //[XmlElement("SetDay")]
        //public OPCPoint PC_Day
        //{
        //    get
        //    {
        //        return _PC_Day;
        //    }
        //    set
        //    {
        //        _PC_Day = value;
        //    }
        //}

        //private OPCPoint _PC_Hour = new OPCPoint();
        ///// <summary>
        ///// 下传校正时间时部分
        ///// </summary>
        //[XmlElement("SetHour")]
        //public OPCPoint PC_Hour
        //{
        //    get
        //    {
        //        return _PC_Hour;
        //    }
        //    set
        //    {
        //        _PC_Hour = value;
        //    }
        //}

        //private OPCPoint _PC_Min = new OPCPoint();
        ///// <summary>
        ///// 下传校正时间分部分
        ///// </summary>
        //[XmlElement("SetMin")]
        //public OPCPoint PC_Min
        //{
        //    get
        //    {
        //        return _PC_Min;
        //    }
        //    set
        //    {
        //        _PC_Min = value;
        //    }
        //}

        //private OPCPoint _PC_Sec = new OPCPoint();
        ///// <summary>
        ///// 下传校正时间秒部分
        ///// </summary>
        //[XmlElement("SetSec")]
        //public OPCPoint PC_Sec
        //{
        //    get
        //    {
        //        return _PC_Sec;
        //    }
        //    set
        //    {
        //        _PC_Sec = value;
        //    }
        //}

        //private OPCPoint _PC_MS = new OPCPoint();
        ///// <summary>
        ///// 下传校正时间毫秒部分
        ///// </summary>
        //[XmlElement("SetMS")]
        //public OPCPoint PC_MS
        //{
        //    get
        //    {
        //        return _PC_MS;
        //    }
        //    set
        //    {
        //        _PC_MS = value;
        //    }
        //}


        //private OPCPoint _Stamp_Year = new OPCPoint();
        ///// <summary>
        ///// 上传校正后的时间年部分
        ///// </summary>
        //[XmlElement("GetYear")]
        //public OPCPoint Stamp_Year
        //{
        //    get
        //    {
        //        return _Stamp_Year;
        //    }
        //    set
        //    {
        //        _Stamp_Year = value;
        //    }
        //}

        //private OPCPoint _Stamp_Month = new OPCPoint();
        ///// <summary>
        ///// 上传校正后的时间月部分
        ///// </summary>
        //[XmlElement("GetMonth")]
        //public OPCPoint Stamp_Month
        //{
        //    get
        //    {
        //        return _Stamp_Month;
        //    }
        //    set
        //    {
        //        _Stamp_Month = value;
        //    }
        //}

        //private OPCPoint _Stamp_Day = new OPCPoint();
        ///// <summary>
        ///// 上传校正后的时间天部分
        ///// </summary>
        // [XmlElement("GetDay")]
        //public OPCPoint Stamp_Day
        //{
        //    get
        //    {
        //        return _Stamp_Day;
        //    }
        //    set
        //    {
        //        _Stamp_Day = value;
        //    }
        //}

        //private OPCPoint _Stamp_Hour = new OPCPoint();
        ///// <summary>
        ///// 上传校正后的时间时部分
        ///// </summary>
        //[XmlElement("GetHour")]
        //public OPCPoint Stamp_Hour
        //{
        //    get
        //    {
        //        return _Stamp_Hour;
        //    }
        //    set
        //    {
        //        _Stamp_Hour = value;
        //    }
        //}

        //private OPCPoint _Stamp_Min = new OPCPoint();
        ///// <summary>
        ///// 上传校正后的时间分部分
        ///// </summary>
        //[XmlElement("GetMin")]
        //public OPCPoint Stamp_Min
        //{
        //    get
        //    {
        //        return _Stamp_Min;
        //    }
        //    set
        //    {
        //        _Stamp_Min = value;
        //    }
        //}

        //private OPCPoint _Stamp_Sec = new OPCPoint();
        ///// <summary>
        ///// 上传校正后的时间秒部分
        ///// </summary>
        //[XmlElement("GetSec")]
        //public OPCPoint Stamp_Sec
        //{
        //    get
        //    {
        //        return _Stamp_Sec;
        //    }
        //    set
        //    {
        //        _Stamp_Sec = value;
        //    }
        //}

        //private OPCPoint _Stamp_MS = new OPCPoint();
        ///// <summary>
        ///// 上传校正后的时间毫秒部分
        ///// </summary>
        //[XmlElement("GetMS")]
        //public OPCPoint Stamp_MS
        //{
        //    get
        //    {
        //        return _Stamp_MS;
        //    }
        //    set
        //    {
        //        _Stamp_MS = value;
        //    }
        //}

        //private OPCPoint _RealPreSensorValue = new OPCPoint();
        ///// <summary>
        ///// 压力实时值
        ///// </summary>
        //[XmlElement("RealValue")]
        //public OPCPoint RealPreSensorValue 
        //{
        //    get 
        //    {
        //        return _RealPreSensorValue;
        //    }
        //    set 
        //    {
        //        _RealPreSensorValue = value;
        //    }
        //}

        //private OPCPoint _AvePreSensorValue = new OPCPoint();
        ///// <summary>
        ///// 压力平均值
        ///// </summary>
        //[XmlElement("AveValue")]
        //public OPCPoint AvePreSensorValue 
        //{
        //    get 
        //    {
        //        return _AvePreSensorValue;
        //    }
        //    set 
        //    {
        //        _AvePreSensorValue = value;
        //    }
        //}
    }
}
