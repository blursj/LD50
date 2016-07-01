using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace OPCClientProxy_Model
{
    public class OPCServerModel:PropertyCallBack
    {
        public OPCServerModel() 
        {
            _OPCServerListNode = new ListNode();
            _SelectedNodes = new ObservableCollection<ListNode>();
        }

        

        private string _OPCServerName;
        /// <summary>
        /// OPC_Server名称
        /// </summary>
        public string OPCServerName
        {
            get
            {
                return _OPCServerName;
            }
            set 
            {
                _OPCServerName = value;
                OnPropertyChanged("OPCServerName");
            }
        }

        private bool _IsLinked;
        /// <summary>
        /// OPC服务器是否连接
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

        private ListNode _OPCServerListNode;
        /// <summary>
        /// 节点列表
        /// </summary>
        public ListNode OPCServerListNode 
        {
            get 
            {
                return _OPCServerListNode;
            }
            set 
            {
                _OPCServerListNode = value;
                OnPropertyChanged("OPCServerListNode");
            }
        }

        private ObservableCollection<ListNode> _SelectedNodes;
        /// <summary>
        /// 选中的要读取的点
        /// </summary>
        public ObservableCollection<ListNode> SelectedNodes 
        {
            get 
            {
                return _SelectedNodes;
            }
            set 
            {
                _SelectedNodes = value;
            }
        }

    }
}
