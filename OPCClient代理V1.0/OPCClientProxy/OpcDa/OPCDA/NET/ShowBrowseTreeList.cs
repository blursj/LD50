namespace OPCDA.NET
{
    using OPC;
    using System;
    using System.Windows.Forms;

    public class ShowBrowseTreeList
    {
        private TreeView branchesTV;
        private ColumnHeader columnHeader1;
        private ListView itemsLV;
        private BrowseTree srvTree;

        public ShowBrowseTreeList(OpcServer srv, TreeView tvBranches, ListView lvItems)
        {
            this.branchesTV = tvBranches;
            this.itemsLV = lvItems;
            this.srvTree = new BrowseTree(srv, tvBranches);
            this.srvTree.treeListMode = true;
            tvBranches.AfterSelect += new TreeViewEventHandler(this.tvBranches_AfterSelect);
            lvItems.View = View.SmallIcon;
            this.columnHeader1 = new ColumnHeader();
            lvItems.Columns.AddRange(new ColumnHeader[] { this.columnHeader1 });
            this.columnHeader1.Width = lvItems.Size.Width;
            tvBranches.ImageList = this.srvTree.ImageList;
            lvItems.SmallImageList = this.srvTree.ImageList;
        }

        public void Dispose()
        {
            this.branchesTV.AfterSelect -= new TreeViewEventHandler(this.tvBranches_AfterSelect);
            this.srvTree.Dispose();
            this.srvTree = null;
        }

        public bool isBranch(TreeNode node)
        {
            return (node.ImageIndex < this.ItemCodeIndex);
        }

        public bool isItem(TreeNode node)
        {
            return (node.ImageIndex >= this.ItemCodeIndex);
        }

        public string ItemName(TreeNode node)
        {
            return node.Tag.ToString();
        }

        public int Show(string rootName)
        {
            this.branchesTV.Nodes.Clear();
            this.itemsLV.Items.Clear();
            int hresultcode = this.srvTree.CreateTree();
            if (HRESULTS.Succeeded(hresultcode))
            {
                TreeNode node = this.srvTree.ConvertToTreeList(rootName);
                this.branchesTV.Nodes.Add(node);
                this.branchesTV.Nodes[0].Expand();
                this.branchesTV.SelectedNode = this.branchesTV.Nodes[0];
                return 0;
            }
            if (this.srvTree.Srv.myErrorsAsExecptions)
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public void ShowImageList()
        {
            this.srvTree.ShowImageList();
        }

        private void tvBranches_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                if (this.srvTree.isBranch(node) && (node.Tag.GetType() == typeof(string)))
                {
                    this.srvTree.BuildTreeList(node);
                }
                tlNodeInfo tag = (tlNodeInfo) node.Tag;
                this.itemsLV.Items.Clear();
                this.itemsLV.Items.AddRange(tag.itemNames);
            }
        }

        public bool BrowseModeOneLevel
        {
            get
            {
                return this.srvTree.BrowseModeOneLevel;
            }
            set
            {
                this.srvTree.BrowseModeOneLevel = value;
            }
        }

        public System.Type DataTypeFilter
        {
            get
            {
                return this.srvTree.DataTypeFilter;
            }
            set
            {
                this.srvTree.DataTypeFilter = value;
            }
        }

        public bool DoNotSort
        {
            get
            {
                return this.srvTree.DoNotSort;
            }
            set
            {
                this.srvTree.DoNotSort = value;
            }
        }

        public int ImageIndexBranch
        {
            get
            {
                return this.srvTree.ImageIndexBranch;
            }
            set
            {
                this.srvTree.ImageIndexBranch = value;
            }
        }

        public int ImageIndexBranchSelected
        {
            get
            {
                return this.srvTree.ImageIndexBranchSelected;
            }
            set
            {
                this.srvTree.ImageIndexBranchSelected = value;
            }
        }

        public int ImageIndexItem
        {
            get
            {
                return this.srvTree.ImageIndexItem;
            }
            set
            {
                this.srvTree.ImageIndexItem = value;
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this.branchesTV.ImageList;
            }
            set
            {
                this.branchesTV.ImageList = value;
                this.itemsLV.SmallImageList = value;
            }
        }

        public int ItemCodeIndex
        {
            get
            {
                return this.srvTree.ItemCodeIndex;
            }
            set
            {
                this.srvTree.ItemCodeIndex = value;
            }
        }

        public string NameFilter
        {
            get
            {
                return this.srvTree.NameFilter;
            }
            set
            {
                this.srvTree.NameFilter = value;
            }
        }
    }
}

