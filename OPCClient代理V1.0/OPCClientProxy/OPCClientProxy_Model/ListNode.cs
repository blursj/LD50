using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace OPCClientProxy_Model
{
    public class ListNode:PropertyCallBack
    {
        public ListNode() 
        {
            _Children = new ObservableCollection<ListNode>();
        }

        private bool _HasChild;
        /// <summary>
        /// 是否包含孩子节点
        /// </summary>
        public bool HasChild
        {
            get
            {
                return _HasChild;
            }
            set
            {
                _HasChild = value;
                OnPropertyChanged("HasChild");
            }
        }

        private string _NodeName;
        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName 
        {
            get 
            {
                return _NodeName;
            }
            set 
            {
                _NodeName = value;
                OnPropertyChanged("NodeName");
            }
        }

        private string _NodeID;
        /// <summary>
        /// 节点ID
        /// </summary>
        public string NodeID 
        {
            get 
            {
                return _NodeID;
            }
            set 
            {
                _NodeID = value;
                OnPropertyChanged("NodeID");
            }
        }

        private string _NodeValue;
        /// <summary>
        /// 节点值
        /// </summary>
        public string NodeValue 
        {
            get 
            {
                return _NodeValue;
            }
            set 
            {
                _NodeValue = value;
                OnPropertyChanged("NodeValue");
            }
        }

        private bool _IsAdd;
        /// <summary>
        /// 该节点是否添加
        /// </summary>
        public bool IsAdd 
        {
            get 
            {
                return _IsAdd;
            }
            set 
            {
                _IsAdd = value;
                OnPropertyChanged("IsAdd");
            }
        }

        private ObservableCollection<ListNode> _Children;
        /// <summary>
        /// 孩子节点
        /// </summary>
        public ObservableCollection<ListNode> Children 
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
    }
}
