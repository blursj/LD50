using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ISafe_Model
{
    public class ChannelGroupModel:PropertyCallBack
    {
        public ChannelGroupModel() 
        {
            _Channels = new ObservableCollection<ChannelModel>();
        }

        private int _GroupIndex;
        /// <summary>
        /// 通道组索引号
        /// </summary>
        public int GroupIndex 
        {
            get 
            {
                return _GroupIndex;
            }
            set 
            {
                _GroupIndex = value;
                OnPropertyChanged("GroupIndex");
            }
        }

        private ObservableCollection<ChannelModel> _Channels;
        public ObservableCollection<ChannelModel> Channels
        {
            get 
            {
                return _Channels;
            }
            set 
            {
                _Channels = value;
            }
        }
    }
}
