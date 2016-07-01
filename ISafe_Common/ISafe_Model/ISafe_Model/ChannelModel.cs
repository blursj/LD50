using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ISafe_Model
{
    public class ChannelModel:PropertyNode
    {
        public ChannelModel() 
        {
            Children = new ObservableCollection<PropertyNode>();
        }

        private string _PCIChannelName;
        /// <summary>
        /// PCI通道名称
        /// </summary>
        public string PCIChannelName
        {
            get
            {
                return _PCIChannelName;
            }
            set
            {
                _PCIChannelName = value;
                OnPropertyChanged("PCIChannelName");
            }
        }

        private ACUDevice _BelongACU;
        /// <summary>
        /// 通道所属的ACU
        /// </summary>
        public ACUDevice BelongACU 
        {
            get 
            {
                return _BelongACU;
            }
            set 
            {
                _BelongACU = value;
                OnPropertyChanged("BelongACU");
            }
        }

        private bool _ChannelSign = true;
        /// <summary>
        /// 通道是否有效
        /// </summary>
        public bool ChannelSign 
        {
            get 
            {
                return _ChannelSign;
            }
            set 
            {
                _ChannelSign = value;
                OnPropertyChanged("ChannelSign");
            }
        }

        private int _ChannelNum;
        /// <summary>
        /// 通道号
        /// </summary>
        public int ChannelNum 
        {
            get
            {
                return _ChannelNum;
            }
            set 
            {
                _ChannelNum = value;
                OnPropertyChanged("_ChannelNum");
            }
        }

        private int _ChannelGains = 0;
        /// <summary>
        /// 通道增益
        /// </summary>
        public int ChannelGains 
        {
            get 
            {
                return _ChannelGains;
            }
            set 
            {
                _ChannelGains = value;
                OnPropertyChanged("ChannelGains");
            }
        }

        private double[] _GetingData;
        /// <summary>
        /// 获取到的瞬时缓存
        /// </summary>
        public double[] GetingData 
        {
            get 
            {
                return _GetingData;
            }
            set 
            {
                _GetingData = value;
                OnPropertyChanged("GetingData");
            }
        }

        private int _BelongGroupIndex;
        /// <summary>
        /// 通道所属组索引号
        /// </summary>
        public int BelongGroupIndex
        {
            get
            {
                return _BelongGroupIndex;
            }
            set
            {
                _BelongGroupIndex = value;
                OnPropertyChanged("BelongGroupIndex");
            }
        }

        private ChannelTypeEnum _ChannelType;
        /// <summary>
        /// 通道类型 
        /// </summary>
        public ChannelTypeEnum ChannelType
        {
            get
            {
                return _ChannelType;
            }
            set 
            {
                _ChannelType = value;
                OnPropertyChanged("ChannelType");
            }
        }



    }
}
