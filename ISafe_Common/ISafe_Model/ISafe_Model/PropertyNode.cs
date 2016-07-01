using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ISafe_Model
{
    public enum NodeType 
    {
        Site =0,
        ACUDevice = 1,
        PreSensor = 2,
        ACUGroup = 3,
        ACUSensor = 4
    }

    public class PropertyNode : PropertyCallBack
    {
        private bool _IsLink;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsLink 
        {
            get 
            {
                return _IsLink;
            }
            set 
            {
                _IsLink = value;
                OnPropertyChanged("IsLink");
            }
        }

        private NodeType _DataType = new NodeType();
        /// <summary>
        /// 数据类型
        /// </summary>
        public NodeType DataType
        {
            get
            {
                return _DataType;
            }
            set
            {
                _DataType = value;
                OnPropertyChanged("DataType");
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("_Name");
            }
        }

        private string _ImagePath;
        /// <summary>
        /// 表示图标
        /// </summary>
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        private PropertyNode _Parent;
        /// <summary>
        /// 父节点
        /// </summary>
        public PropertyNode Parent 
        {
            get 
            {
                return _Parent;
            }
            set 
            {
                _Parent = value;
            }
        }

        private string _NodeKey;
        /// <summary>
        /// 节点ID
        /// </summary>
        public string NodeKey 
        {
            get 
            {
                return _NodeKey;
            }
            set 
            {
                _NodeKey = value;
            }
        }

        private ObservableCollection<PropertyNode> _Children = new ObservableCollection<PropertyNode>();
        /// <summary>
        /// 孩子节点集合
        /// </summary>
        public ObservableCollection<PropertyNode> Children 
        {
            get 
            {
                return _Children;
            }
            set 
            {
                _Children = value;
            }
        }

        private ObservableCollection<PropertyNode> _Children_next = new ObservableCollection<PropertyNode>();
        /// <summary>
        /// 孩子节点集合
        /// </summary>
        public ObservableCollection<PropertyNode> Children_next
        {
            get
            {
                return _Children_next;
            }
            set
            {
                _Children_next = value;
            }
        }
    }
}
