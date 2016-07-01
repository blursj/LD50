using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SimulatorModel
{
    [XmlRoot]
    public class PipeModel : PropertyCallBack
    {
        private string _PipeName;
        [XmlAttribute("PipeName")]
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

        private int _PipeIndex;
        [XmlAttribute("PipeIndex")]
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

        private double _PipeLength;
        [XmlAttribute("PipeLength")]
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

        private double _Speed;
        [XmlAttribute("Speed")]
        public double Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                _Speed = value;
            }
        }

        private int _PipeSite1Index;
        [XmlAttribute("PipeSite1Index")]
        public int PipeSite1Index 
        {
            get 
            {
                return _PipeSite1Index;
            }
            set 
            {
                _PipeSite1Index = value;
            }
        }

        private int _PipeSite2Index;
        [XmlAttribute("PipeSite2Index")]
        public int PipeSite2Index
        {
            get
            {
                return _PipeSite2Index;
            }
            set
            {
                _PipeSite2Index = value;
            }
        }

    }
}
