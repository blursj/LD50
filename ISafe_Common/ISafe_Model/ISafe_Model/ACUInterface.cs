using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_Model
{
    public class ACUInterface : PropertyNode
    {
        private bool _IsSensor1Linked = false;
        /// <summary>
        /// 传感器接口1 是否链接
        /// </summary>
        public bool IsSensor1Linked 
        {
            get 
            {
                return _IsSensor1Linked;
            }
            set 
            {
                _IsSensor1Linked = value;
                OnPropertyChanged("IsSensor1Linked");
            }
        }

        private bool _IsSensor2Linked = false;
        /// <summary>
        /// 传感器接口2 是否链接
        /// </summary>
        public bool IsSensor2Linked
        {
            get
            {
                return _IsSensor2Linked;
            }
            set
            {
                _IsSensor2Linked = value;
                OnPropertyChanged("IsSensor2Linked");
            }
        }

        private bool _IsSensor3Linked = false;
        /// <summary>
        /// 传感器接口3 是否链接
        /// </summary>
        public bool IsSensor3Linked
        {
            get
            {
                return _IsSensor3Linked;
            }
            set
            {
                _IsSensor3Linked = value;
                OnPropertyChanged("IsSensor3Linked");
            }
        }

        private bool _IsSensor4Linked = false;
        /// <summary>
        /// 传感器接口4 是否链接
        /// </summary>
        public bool IsSensor4Linked
        {
            get
            {
                return _IsSensor4Linked;
            }
            set
            {
                _IsSensor4Linked = value;
                OnPropertyChanged("IsSensor4Linked");
            }
        }

        private bool _IsGPSLinked = false;
        /// <summary>
        /// GPS是否链接
        /// </summary>
        public bool IsGPSLinked 
        {
            get 
            {
                return _IsGPSLinked;
            }
            set 
            {
                _IsGPSLinked = value;
                OnPropertyChanged("IsGPSLinked");
            }
        }

    }
}
