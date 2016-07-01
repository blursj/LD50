using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_UserClient
{
    public class PageControl
    {
        private PageControl()
        {
            _ISafeSetPage = new ISafeConfigSetPage();
            _LD50DataShowPage = new LD50ShowPage();
            _LD100DataShowPage = new LD100ShowPage();
            _SCADADataShowPage = new SCADAShowPage();
            _DolDataShowPage = new DOLPHINShowPage();
        }

        private static PageControl _Instance = new PageControl();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static PageControl Instance 
        {
            get 
            {
                return _Instance;
            }
        }

        /// <summary>
        /// 当前显示页面表
        /// </summary>
        public iPage CurrentPage
        {
            get;
            set;
        }

        private iPage _ISafeSetPage;
        /// <summary>
        /// ISafe系统启动与配置页面
        /// </summary>
        public iPage ISafeSetPage 
        {
            get 
            {
                return _ISafeSetPage;
            }
            set 
            {
                _ISafeSetPage = value;
            }
        }

        private iPage _LD50DataShowPage;
        /// <summary>
        /// LD50负压波系统数据显示页面
        /// </summary>
        public iPage LD50DataShowPage 
        {
            get 
            {
                return _LD50DataShowPage;
            }
            set 
            {
                _LD50DataShowPage = value;
            }
        }

        private iPage _LD100DataShowPage;
        /// <summary>
        /// LD100次声波系统数据显示页面
        /// </summary>
        public iPage LD100DataShowPage 
        {
            get
            {
                return _LD100DataShowPage;
            }
            set
            {
                _LD100DataShowPage = value;
            }
        }

        private iPage _SCADADataShowPage;
        /// <summary>
        /// 第三发SCADA系统数据显示
        /// </summary>
        public iPage SCADADataShowPage 
        {
            get 
            {
                return _SCADADataShowPage;
            }
            set 
            {
                _SCADADataShowPage = value;
            }
        }

        private iPage _DolDataShowPage;
        /// <summary>
        /// DOLPHIN数据显示页面
        /// </summary>
        public iPage DolDataShowPage 
        {
            get 
            {
                return _DolDataShowPage;
            }
            set 
            {
                _DolDataShowPage = value;
            }
        }
    }
}
