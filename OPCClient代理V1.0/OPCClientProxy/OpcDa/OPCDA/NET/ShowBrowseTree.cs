namespace OPCDA.NET
{
    using OPC;
    using System;
    using System.Windows.Forms;

    public class ShowBrowseTree
    {
        private BrowseTree srvTree;
        private TreeView treeTV;

        public ShowBrowseTree(OpcServer srv, TreeView tvTree)
        {
            this.treeTV = tvTree;
            this.srvTree = new BrowseTree(srv, tvTree);
            this.treeTV.Nodes.Clear();
            this.treeTV.ImageList = this.srvTree.ImageList;
        }

        public void Dispose()
        {
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

        public int Show()
        {
            this.treeTV.Nodes.Clear();
            int hresultcode = this.srvTree.CreateTree();
            if (HRESULTS.Succeeded(hresultcode))
            {
                this.treeTV.Nodes.AddRange(this.srvTree.Root());
                return 0;
            }
            return hresultcode;
        }

        public void ShowImageList()
        {
            this.srvTree.ShowImageList();
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
                return this.treeTV.ImageList;
            }
            set
            {
                this.treeTV.ImageList = value;
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

        public TreeNode[] ItemTree
        {
            get
            {
                return this.srvTree.Root();
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

