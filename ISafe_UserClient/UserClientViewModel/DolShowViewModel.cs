using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ISafe_Model;

namespace UserClientViewModel
{
    public class DolShowViewModel
    {
        private DOLPHINMSG _DOLPHINMSGManager = new DOLPHINMSG();
        public DOLPHINMSG DOLPHINMSGManager 
        {
            get 
            {
                return _DOLPHINMSGManager;
            }
            set 
            {
                _DOLPHINMSGManager = value;
            }
        }

        private ObservableCollection<ServiceReference1.DolLocation> _DolLocations = new ObservableCollection<ServiceReference1.DolLocation>();
        /// <summary>
        /// DOLPHIN服务定位信息缓存
        /// </summary>
        public ObservableCollection<ServiceReference1.DolLocation> DolLoactions 
        {
            get 
            {
                return _DolLocations;
            }
            set 
            {
                _DolLocations = value;
            }
        }

    }
}
