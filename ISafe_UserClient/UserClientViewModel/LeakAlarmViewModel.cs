using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISafe_Model;

namespace UserClientViewModel
{
    public class LeakAlarmViewModel : PropertyCallBack
    {
        private bool _IsSure;
        /// <summary>
        /// 是否确定
        /// </summary>
        public bool IsSure 
        {
            get 
            {
                return _IsSure;
            }
            set 
            {
                _IsSure = value;
                OnPropertyChanged("IsSure");
            }
        }

        private string _LeakPipe;
        /// <summary>
        /// 泄漏发生管段
        /// </summary>
        public string LeakPipe
        {
            get
            {
                return _LeakPipe;
            }
            set
            {
                _LeakPipe = value;
                OnPropertyChanged("LeakPipe");
            }
        }

        private string _LeakTime;
        /// <summary>
        /// 泄漏发生时刻
        /// </summary>
        public string LeakTime 
        {
            get 
            {
                return _LeakTime;
            }
            set 
            {
                _LeakTime = value;
                OnPropertyChanged("LeakTime");

            }
        }

        private string _LeakType;
        /// <summary>
        /// 报警类型
        /// </summary>
        public string LeakType 
        {
            get 
            {
                return _LeakType;
            }
            set 
            {
                _LeakType = value;
                OnPropertyChanged("LeakType");
            }
        }

        private string _LeakMSG;
        /// <summary>
        /// 泄漏信息
        /// </summary>
        public string LeakMSG 
        {
            get 
            {
                return _LeakMSG;
            }
            set 
            {
                _LeakMSG = value;
                OnPropertyChanged("LeakMSG");
            }
        }

        private DelegateCommand _SureCommand;
        /// <summary>
        /// 确定泄漏
        /// </summary>
        public DelegateCommand SureCommand 
        {
            get 
            {
                if (_SureCommand == null)
                {
                    _SureCommand = new DelegateCommand((obj) => 
                    {
                        IsSure = false;
                    });
                }
                return _SureCommand;
            }
        }

    }
}
