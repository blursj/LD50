using OPCClientProxy_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OPCClientProxy_ViewModel
{
    [XmlRoot("ConfigSet")]
    public class ConfigSetViewModel
    {
        /// <summary>
        /// 变量点缓存，用来根据ID号快速查找点
        /// </summary>
        private Dictionary<string, OPCPoint> _OPCPointDictionary = new Dictionary<string, OPCPoint>();

        private List<string> _PrePoint = new List<string>();
        public List<string> PrePoint
        {
            get
            {
                return _PrePoint;
            }
        }

        private string _GUID;
        /// <summary>
        /// 代理唯一标示符
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

        private string _ProxySign;
        /// <summary>
        /// 代理标志
        /// </summary>
        [XmlAttribute("ProxySign")]
        public string ProxySign
        {
            get
            {
                return _ProxySign;
            }
            set
            {
                _ProxySign = value;
            }
        }

        private int _Requrecy;
        /// <summary>
        /// 采样频率
        /// </summary>
        [XmlAttribute("Requrecy")]
        public int Requrecy
        {
            get
            {
                return _Requrecy;
            }
            set
            {
                _Requrecy = value;
            }
        }

        private int _OPCTimeSetRequrecy;
        /// <summary>
        /// OPC采样时间校准频率
        /// </summary>
        [XmlAttribute("OPCTimeSetRequrecy")]
        public int OPCTimeSetRequrecy
        {
            get
            {
                return _OPCTimeSetRequrecy;
            }
            set
            {
                _OPCTimeSetRequrecy = value;
            }
        }

        private string _WCFServerIP;
        /// <summary>
        /// WCF通信服务器IP地址
        /// </summary>
        [XmlAttribute("WCFServerIP")]
        public string WCFServerIP
        {
            get
            {
                return _WCFServerIP;
            }
            set
            {
                _WCFServerIP = value;
            }
        }

        private string _LinkOPCServerName;
        /// <summary>
        /// 连接到的OPCserver服务器名称
        /// </summary>
        [XmlAttribute("LinkOPCServerName")]
        public string LinkOPCServerName
        {
            get
            {
                return _LinkOPCServerName;
            }
            set
            {
                _LinkOPCServerName = value;
            }
        }

        private List<PLC> _PLCConllection = new List<PLC>();
        /// <summary>
        /// PLC
        /// </summary>
        [XmlElement("PLC")]
        public List<PLC> PLCConllection
        {
            get
            {
                return _PLCConllection;
            }
            set
            {
                _PLCConllection = value;
            }
        }

        private List<OPCPoint> _DolPointCollection = new List<OPCPoint>();
        /// <summary>
        /// DOLPHIN服务器读取点
        /// </summary>
        [XmlElement("DOLPHINOutPutPoint")]
        public List<OPCPoint> DolPointCollection
        {
            get
            {
                return _DolPointCollection;
            }
            set
            {
                _DolPointCollection = value;
            }
        }

        /// <summary>
        /// 根据类型查找点
        /// </summary>
        /// <param name="plcmodel"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public OPCPoint GetPointByType(PLC plcmodel, PointType type)
        {
            OPCPoint find_opcpoint = null;
            try
            {
                switch (type)
                {
                    case PointType.Year:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("SetYear"));
                        break;
                    case PointType.month:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("SetMonth"));
                        break;
                    case PointType.day:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("SetDay"));
                        break;
                    case PointType.hour:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("SetHour"));
                        break;
                    case PointType.min:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("SetMin"));
                        break;
                    case PointType.sec:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("SetSec"));
                        break;
                    case PointType.ms:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("SetMS"));
                        break;
                    case PointType.TimerSure:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("TimerSure"));
                        break;
                    case PointType.Year_out:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("GetYear"));
                        break;
                    case PointType.month_out:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("GetMonth"));
                        break;
                    case PointType.day_out:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("GetDay"));
                        break;
                    case PointType.hour_out:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("GetHour"));
                        break;
                    case PointType.min_out:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("GetMin"));
                        break;
                    case PointType.sec_out:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("GetSec"));
                        break;
                    case PointType.ms_out:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("GetMS"));
                        break;
                    case PointType.real:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("RealValue"));
                        break;
                    case PointType.Ave:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("AveValue"));
                        break;
                    case PointType.PC_heartbeat:
                        find_opcpoint = plcmodel.PLC_Points.FirstOrDefault(para => para.PointSign.Equals("PC_heartbeat"));
                        break;
                    default:
                        return find_opcpoint;
                }
                return find_opcpoint;
            }
            catch
            {
                return find_opcpoint;
            }

        }

        /// <summary>
        /// 根据ID号找对应的PLC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PLC GetPLCByID(string id)
        {
            return PLCConllection.FirstOrDefault(para => para.PLCSign.Equals(id));
        }

        /// <summary>
        /// 根据ID号找对应的变量点 从输出点集合中搜索
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public OPCPoint GetOPCPointbyItemID(string itemid)
        {
            OPCPoint find_OPCPoint = null;
            if (_OPCPointDictionary.Keys.Contains(itemid))
            {
                find_OPCPoint = _OPCPointDictionary[itemid];
            }
            else
            {
                foreach (var plc in PLCConllection)
                {
                    if (itemid.Contains(plc.PLCSign))
                    {
                        foreach (var point in plc.PLC_OutPutPoints)
                        {
                            if (point.PointID.Equals(itemid))
                            {
                                find_OPCPoint = point;
                                _OPCPointDictionary.Add(itemid, point);
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            return find_OPCPoint;
        }

        /// <summary>
        /// 传感器值列表
        /// </summary>
        public void GeneratePreIDCollection()
        {
            foreach (var plc in PLCConllection)
            {
                foreach (var point in plc.PLC_OutPutPoints)
                {
                    if (point.PointSign.Equals("AveValue"))
                    {
                        _PrePoint.Add(point.PointID);
                        break;
                    }
                }
            }
        }

        public OPCPoint GetOPCPointbyItemSign(string PLCid, string OPCPointSign)
        {
            OPCPoint find_opcpoint = null;
            var find_plc = GetPLCByID(PLCid);
            if (find_plc != null)
            {
                if (_OPCPointDictionary.Keys.Contains(PLCid + "-" + OPCPointSign))
                {
                    find_opcpoint = _OPCPointDictionary[PLCid + "-" + OPCPointSign];
                }
                else
                {
                    foreach (var point in find_plc.PLC_OutPutPoints)
                    {
                        if (point.PointSign.Equals(OPCPointSign))
                        {
                            find_opcpoint = point;
                            _OPCPointDictionary.Add(PLCid + "-" + OPCPointSign, point);
                            break;
                        }
                    }
                }
            }

            return find_opcpoint;
        }

        public PointType JudgePointType(PLC belongPLC, string pointid)
        {
            OPCPoint find_point = null;
            foreach (var point in belongPLC.PLC_Points)
            {
                if (point.PointID.Equals(pointid))
                {
                    find_point = point;
                    break;
                }
            }

            if (find_point != null)
            {
                switch (find_point.PointSign)
                {
                    case "GetYear":
                        return PointType.Year;
                    case "GetMonth":
                        return PointType.month;
                    case "GetDay":
                        return PointType.day;
                    case "GetHour":
                        return PointType.hour;
                    case "GetMin":
                        return PointType.min;
                    case "GetSec":
                        return PointType.sec;
                    case "GetMS":
                        return PointType.ms;
                    case "AveValue":
                        return PointType.Ave;

                    default:
                        return PointType.other;
                }
            }
            else
            {
                return PointType.other;
            }
        }

    }
}
