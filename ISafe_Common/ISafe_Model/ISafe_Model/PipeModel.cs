using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_Model
{
    public class PipeModel : PropertyCallBack
    {
        private double _TimeThreshold;
        /// <summary>
        /// 管段传播时间阀值
        /// </summary>
        public double TimeThreshold
        {
            get
            {
                return _TimeThreshold;
            }
            set
            {
                _TimeThreshold = value;
                OnPropertyChanged("TimeThreshold");
            }
        }

        private int _PipeIndex;
        public int PipeIndex
        {
            get
            {
                return _PipeIndex;
            }
            set
            {
                _PipeIndex = value;
                OnPropertyChanged("PipeIndex");
            }
        }

        private string _PipeName;
        public string PipeName
        {
            get
            {
                return _PipeName;
            }
            set
            {
                _PipeName = value;
                OnPropertyChanged("PipeName");
            }
        }

        private double _Speed0;
        /// <summary>
        /// 介质传播速度
        /// </summary>
        public double Speed0
        {
            get
            {
                return _Speed0;
            }
            set
            {
                _Speed0 = value;
                OnPropertyChanged("Speed0");
            }
        }

        private double _Speed1;
        /// <summary>
        /// 介质传播速度
        /// </summary>
        public double Speed1
        {
            get
            {
                return _Speed1;
            }
            set
            {
                _Speed1 = value;
                OnPropertyChanged("Speed1");
            }
        }

        private double _PipeLength;
        /// <summary>
        /// 管段长度
        /// </summary>
        public double PipeLength
        {
            get
            {
                return _PipeLength;
            }
            set
            {
                _PipeLength = value;
                OnPropertyChanged("PipeLength");
            }
        }

        private List<Segment> _Segments = new List<Segment>();
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

    public class Segment
    {
        public string Name
        {
            get;
            set;
        }

        public double X1
        {
            get;
            set;
        }

        public double X2
        {
            get;
            set;
        }

        public double Y1
        {
            get;
            set;
        }

        public double Y2
        {
            get;
            set;
        }

        public string Stroke
        {
            get;
            set;
        }

    }
}
