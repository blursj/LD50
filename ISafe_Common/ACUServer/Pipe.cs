using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{
    [XmlRoot]
    public class Pipe
    {
        /// <summary>
        /// 传播时间门限
        /// </summary>
        [XmlAttribute("TimeThreshold")]
        public double TimeThreshold
        {
            get;
            set;
        }

        private int _PipeIndex;
        [XmlAttribute("Index")]
        public int PipeIndex
        {
            get
            {
                return _PipeIndex;
            }
            set
            {
                _PipeIndex = value;
            }
        }

        private string _PipeName;
        [XmlAttribute("Name")]
        public string PipeName
        {
            get
            {
                return _PipeName;
            }
            set
            {
                _PipeName = value;
            }
        }

        private double _Speed0;
        /// <summary>
        /// 介质传播速度 向上游传播速度
        /// </summary>
        [XmlAttribute("Speed0")]
        public double Speed0
        {
            get
            {
                return _Speed0;
            }
            set
            {
                _Speed0 = value;
            }
        }

        private double _Speed1;
        /// <summary>
        /// 介质传播速度 向下游传播速度
        /// </summary>
        [XmlAttribute("Speed1")]
        public double Speed1
        {
            get
            {
                return _Speed1;
            }
            set
            {
                _Speed1 = value;
            }
        }

        private double _PipeLength;
        /// <summary>
        /// 管段长度
        /// </summary>
        [XmlAttribute("Length")]
        public double PipeLength
        {
            get
            {
                return _PipeLength;
            }
            set
            {
                _PipeLength = value;
            }
        }

        private List<Segment> _Segments = new List<Segment>();
        [XmlElement("Segment")]
        public List<Segment> Segments
        {
            get
            {
                return _Segments;
            }
            set
            {
                _Segments = value;
            }
        }
    }
 
    [XmlRoot]
    public class Segment
    {
        [XmlAttribute]        
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute]
        public double X1
        {
            get;
            set;
        }
        [XmlAttribute]
        public double X2
        {
            get;
            set;
        }
        [XmlAttribute]
        public double Y1
        {
            get;
            set;
        }
        [XmlAttribute]
        public double Y2
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Stroke
        {
            get;
            set;
        }
    }
}


