/*
* 
* 文件名称：Graphic.cs
* 文件标识：见配置管理计划书
* 摘    要：简要描述本文件的内容
* 
* 当前版本：1.0
* 作    者：吴士杰
* 完成日期：2015/4/8 9:37:22
*
* 取代版本：
* 原作者  ：
* 完成日期：
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{

    [XmlRoot("root")]
    public class Graphic
    {
        private List<Pipe> _Pipes = new List<Pipe>();
        /// <summary>
        /// 原始管道信息
        /// </summary>
        [XmlElement("PIPE")]
        public List<Pipe> Pipes
        {
            get
            {
                return _Pipes;
            }
            set
            {
                _Pipes = value;
            }
        }

        private List<Pipe> _CombinPipes = new List<Pipe>();
        /// <summary>
        /// 原始管道组合管道信息
        /// </summary>
        [XmlElement("CombinPipes")]
        public List<Pipe> CombinPipes
        {
            get
            {
                return _CombinPipes;
            }
            set
            {
                _CombinPipes = value;
            }
        }

        private List<PipeSite> _PipeSites = new List<PipeSite>();
        /// <summary>
        /// 管道站点集合
        /// </summary>
        [XmlElement("PipeSite")]
        public List<PipeSite> PipeSites
        {
            get
            {
                return _PipeSites;
            }
            set
            {
                _PipeSites = value;
            }
        }

        private List<OPCPxoryModel> _OPCProxys = new List<OPCPxoryModel>();
        /// <summary>
        /// OPC代理集合
        /// </summary>
        [XmlElement("OPCPxoryModel")]
        public List<OPCPxoryModel> OPCProxys
        {
            get
            {
                return _OPCProxys;
            }
            set
            {
                _OPCProxys = value;
            }
        }

        /// <summary>
        /// SCADA信息读取配置项
        /// </summary>
        [XmlElement(ElementName = "SCADAConfig")]
        public SCADAConfig SCADAConfigManager
        {
            get;
            set;
        }

    }
}
