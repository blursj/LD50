using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_Model
{
    public class SCADAPoint : PropertyCallBack
    {
        private string _PointName;
        /// <summary>
        /// 变量名
        /// </summary>
        public string PointName
        {
            get 
            {
                return _PointName;
            }
            set 
            {
                _PointName = value;
                OnPropertyChanged("PointName");
            }
        }

        private string _PointID;
        /// <summary>
        /// 变量ID
        /// </summary>
        public string PointID
        {
            get 
            {
                return _PointID;
            }
            set
            {
                _PointID = value;
                OnPropertyChanged("PointID");
            }
        }

        private string _Value;
        /// <summary>
        /// 点值
        /// </summary>
        public string Value
        {
            get 
            {
                return _Value;
            }
            set 
            {
                _Value = value;
                OnPropertyChanged("Value");
            }
        }

        private int _Capacity;
        /// <summary>
        /// 值容量
        /// </summary>
        public int Capacity
        {
            get 
            {
                return _Capacity;
            }
            set 
            {
                _Capacity = value;
                OnPropertyChanged("Capacity");
            }
        }
    }
}
