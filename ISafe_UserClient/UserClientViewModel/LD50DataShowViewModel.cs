using ISafe_Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UserClientViewModel.ServiceReference1;

namespace UserClientViewModel
{
    public class LD50DataShowViewModel
    {
        private ObservableCollection<PressureLeak> _Leaks = new ObservableCollection<PressureLeak>();
        /// <summary>
        /// 泄漏信息
        /// </summary>
        public ObservableCollection<PressureLeak> Leaks
        {
            get
            {
                return _Leaks;
            }
            set
            {
                _Leaks = value;
            }
        }

        private ObservableCollection<Location> _Locations = new ObservableCollection<Location>();
        /// <summary>
        /// 定位信息
        /// </summary>
        public ObservableCollection<Location> Locations
        {
            get
            {
                return _Locations;
            }
            set
            {
                _Locations = value;
            }
        }

        private ObservableCollection<PropertyNode> _LD50Nodes = new ObservableCollection<PropertyNode>();
        public ObservableCollection<PropertyNode> LD50Nodes 
        {
            get 
            {
                return _LD50Nodes;
            }
            set 
            {
                _LD50Nodes = value;
            }
        }
    }
}
