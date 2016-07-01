using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_Model
{
    public class OPCProxyModel:PropertyCallBack
    {
        private string _GUID;
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string GUID
        {
            get
            {
                return _GUID;
            }
            set
            {
                _GUID = value;
                OnPropertyChanged("GUID");
            }
        }

        private string _Sign;
        /// <summary>
        /// 代理标记
        /// </summary>
        public string Sign
        {
            get
            {
                return _Sign;
            }
            set
            {
                _Sign = value;
                OnPropertyChanged("Sign");
            }
        }

        private bool _IsLinked;
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsLinked
        {
            get
            {
                return _IsLinked;
            }
            set
            {
                _IsLinked = value;
                OnPropertyChanged("IsLinked");
            }
        }

        private string _Description;
        /// <summary>
        /// 代理描述
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }
    }
}
