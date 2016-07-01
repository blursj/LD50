namespace OPCDA.NET
{
    using OPC;
    using OPCDA;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class BrowseTree
    {
        public bool DoNotSort;
        private VarEnum dtFilter;
        private bool handlerInstalled;
        private bool Hierarchical;
        public int ImageIndexBranch;
        public int ImageIndexBranchSelected;
        public int ImageIndexItem;
        public int ItemCodeIndex;
        private int[] ItemPropAccRights;
        public string NameFilter;
        private bool OneLevelBrowseMode;
        internal OpcServer Srv;
        private TreeNode[] SrvItems;
        internal bool treeListMode;
        private TreeView tvBrItems;

        public BrowseTree(OpcServer srv)
        {
            this.ItemCodeIndex = 2;
            this.ImageIndexItem = Images.ItemStateBase;
            this.ImageIndexBranch = Images.Folder;
            this.ImageIndexBranchSelected = Images.Selected;
            this.OneLevelBrowseMode = false;
            this.dtFilter = VarEnum.VT_EMPTY;
            this.NameFilter = "";
            this.Hierarchical = true;
            this.tvBrItems = null;
            this.handlerInstalled = false;
            this.ItemPropAccRights = new int[] { 5 };
            this.treeListMode = false;
            this.Srv = srv;
            try
            {
                if (this.Srv.QueryOrganization() == OPCNAMESPACETYPE.OPC_NS_FLAT)
                {
                    this.Hierarchical = false;
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        public BrowseTree(OpcServer srv, TreeView trview)
        {
            this.ItemCodeIndex = 2;
            this.ImageIndexItem = Images.ItemStateBase;
            this.ImageIndexBranch = Images.Folder;
            this.ImageIndexBranchSelected = Images.Selected;
            this.OneLevelBrowseMode = false;
            this.dtFilter = VarEnum.VT_EMPTY;
            this.NameFilter = "";
            this.Hierarchical = true;
            this.tvBrItems = null;
            this.handlerInstalled = false;
            this.ItemPropAccRights = new int[] { 5 };
            this.treeListMode = false;
            this.Srv = srv;
            this.tvBrItems = trview;
            this.OneLevelBrowseMode = true;
            try
            {
                if (this.Srv.QueryOrganization() == OPCNAMESPACETYPE.OPC_NS_FLAT)
                {
                    this.Hierarchical = false;
                }
                else
                {
                    this.tvBrItems.MouseDown += new MouseEventHandler(this.trview_MouseDown);
                    this.handlerInstalled = true;
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        public int Browse(out TreeNode[] tree)
        {
            return this.Browse("", out tree);
        }

        public int Browse(string startBranch, out TreeNode[] tree)
        {
            return this.Browse(startBranch, this.OneLevelBrowseMode, out tree);
        }

        public int Browse(bool OneLevelOnly, bool IncludeItems, out TreeNode[] tree)
        {
            int result;
            tree = null;
            ArrayList nameArr = null;
            ArrayList list2 = null;
            int count = 0;
            int num2 = 0;
            if (this.Hierarchical)
            {
                try
                {
                    result = this.Srv.BrowseCurrentBranch(OPCBROWSETYPE.OPC_BRANCH, this.NameFilter, this.dtFilter, OPCACCESSRIGHTS.OPC_UNKNOWN, out nameArr);
                }
                catch (OPCException exception)
                {
                    result = exception.Result;
                }
                if ((result != 0) || (nameArr == null))
                {
                    count = 0;
                }
                else
                {
                    count = nameArr.Count;
                    if (!this.DoNotSort)
                    {
                        nameArr.Sort();
                    }
                }
            }
            if (IncludeItems)
            {
                try
                {
                    result = this.Srv.BrowseCurrentBranch(OPCBROWSETYPE.OPC_LEAF, this.NameFilter, this.dtFilter, OPCACCESSRIGHTS.OPC_UNKNOWN, out list2);
                }
                catch (OPCException exception2)
                {
                    result = exception2.Result;
                }
                if ((result != 0) || (list2 == null))
                {
                    num2 = 0;
                }
                else
                {
                    num2 = list2.Count;
                    if (!this.DoNotSort)
                    {
                        list2.Sort();
                    }
                }
            }
            if ((count + num2) == 0)
            {
                tree = null;
                return 0;
            }
            tree = new TreeNode[count + num2];
            for (int i = 0; i < count; i++)
            {
                if (OneLevelOnly)
                {
                    tree[i] = new TreeNode(nameArr[i].ToString(), 0, 1, new TreeNode[0]);
                    tree[i].Tag = this.Srv.GetItemID(nameArr[i].ToString());
                }
                else
                {
                    TreeNode[] nodeArray = null;
                    try
                    {
                        result = this.Srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, nameArr[i].ToString());
                    }
                    catch (OPCException exception3)
                    {
                        result = exception3.Result;
                    }
                    if (result == 0)
                    {
                        this.Browse(OneLevelOnly, IncludeItems, out nodeArray);
                        try
                        {
                            result = this.Srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, "");
                        }
                        catch (OPCException exception4)
                        {
                            result = exception4.Result;
                        }
                    }
                    if (nodeArray == null)
                    {
                        tree[i] = new TreeNode(nameArr[i].ToString(), 0, 1);
                    }
                    else
                    {
                        tree[i] = new TreeNode(nameArr[i].ToString(), 0, 1, nodeArray);
                    }
                    tree[i].Tag = this.Srv.GetItemID(nameArr[i].ToString());
                }
            }
            if (IncludeItems)
            {
                for (int j = 0; j < num2; j++)
                {
                    int num5;
                    string itemID = this.Srv.GetItemID(list2[j].ToString());
                    try
                    {
                        ItemPropertyData[] dataArray;
                        if (HRESULTS.Succeeded(this.Srv.GetItemProperties(itemID, this.ItemPropAccRights, out dataArray)) && HRESULTS.Succeeded(dataArray[0].Error))
                        {
                            num5 = Convert.ToInt32(dataArray[0].Data);
                        }
                        else
                        {
                            num5 = 0;
                        }
                    }
                    catch
                    {
                        num5 = 0;
                    }
                    tree[j + count] = new TreeNode(list2[j].ToString(), this.ImageIndexItem + num5, (this.ImageIndexItem + 4) + num5);
                    tree[j + count].ForeColor = Color.Blue;
                    tree[j + count].Tag = itemID;
                }
            }
            return 0;
        }

        private int Browse(string startBranch, bool OneLevelOnly, out TreeNode[] tree)
        {
            tree = null;
            if (this.Hierarchical)
            {
                int num;
                try
                {
                    num = this.Srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_TO, startBranch);
                }
                catch
                {
                    SERVERSTATUS serverstatus;
                    this.Srv.GetStatus(out serverstatus);
                    if (serverstatus.szVendorInfo.StartsWith("KEP"))
                    {
                        num = 0;
                    }
                    else
                    {
                        num = -2147467259;
                    }
                }
                if (num != 0)
                {
                    if ((num != -2147024809) && (num != -2147467259))
                    {
                        return num;
                    }
                    while (this.Srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, "") == 0)
                    {
                    }
                    if (startBranch != "")
                    {
                        char[] separator = new char[] { '.' };
                        foreach (string str in startBranch.Split(separator))
                        {
                            try
                            {
                                num = this.Srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, str);
                                if (HRESULTS.Failed(num))
                                {
                                    return num;
                                }
                            }
                            catch
                            {
                                return -2147467259;
                            }
                        }
                    }
                }
            }
            return this.Browse(OneLevelOnly, true, out tree);
        }

        internal void BuildTreeList(TreeNode nd)
        {
            int len = 0;
            foreach (TreeNode node in nd.Nodes)
            {
                if (this.isItem(node))
                {
                    len++;
                }
            }
            tlNodeInfo info = new tlNodeInfo((string) nd.Tag, len);
            nd.Tag = info;
            int index = 0;
            ArrayList list = new ArrayList();
            foreach (TreeNode node2 in nd.Nodes)
            {
                if (this.isItem(node2))
                {
                    info.itemNames[index] = new ListViewItem();
                    info.itemNames[index].Text = node2.Text;
                    info.itemNames[index].ImageIndex = node2.ImageIndex;
                    info.itemNames[index].Tag = node2.Tag.ToString();
                    index++;
                    list.Add(node2);
                }
            }
            foreach (TreeNode node3 in list)
            {
                nd.Nodes.Remove(node3);
            }
        }

        public TreeNode ConvertToTreeList(string rootName)
        {
            TreeNode tree = new TreeNode(rootName, this.ImageIndexBranch, this.ImageIndexBranchSelected, this.SrvItems);
            tree.Tag = "";
            TreeNode[] children = new TreeNode[0];
            TreeNode treeList = new TreeNode(rootName, this.ImageIndexBranch, this.ImageIndexBranchSelected, children);
            this.TreeToTreeListNode(tree, treeList);
            return treeList;
        }

        public int CreateTree()
        {
            if (this.Hierarchical)
            {
                return this.Browse("", out this.SrvItems);
            }
            return this.Browse(true, true, out this.SrvItems);
        }

        public void Dispose()
        {
            if ((this.tvBrItems != null) && this.handlerInstalled)
            {
                try
                {
                    this.tvBrItems.MouseDown -= new MouseEventHandler(this.trview_MouseDown);
                }
                catch
                {
                }
                this.handlerInstalled = false;
            }
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

        private void reinstallHandler()
        {
            if ((this.tvBrItems != null) && !this.handlerInstalled)
            {
                this.tvBrItems.MouseDown += new MouseEventHandler(this.trview_MouseDown);
                this.handlerInstalled = true;
            }
        }

        public TreeNode[] Root()
        {
            return this.SrvItems;
        }

        public void ShowImageList()
        {
            new Images().ShowDialog();
        }

        private TreeNode TreeToTreeListNode(TreeNode tree, TreeNode treeList)
        {
            int len = 0;
            foreach (TreeNode node in tree.Nodes)
            {
                if (this.isItem(node))
                {
                    len++;
                }
            }
            tlNodeInfo info = new tlNodeInfo(tree.Tag.ToString(), len);
            treeList.Tag = info;
            int index = 0;
            foreach (TreeNode node2 in tree.Nodes)
            {
                if (this.isItem(node2))
                {
                    info.itemNames[index] = new ListViewItem();
                    info.itemNames[index].Text = node2.Text;
                    info.itemNames[index].ImageIndex = node2.ImageIndex;
                    info.itemNames[index].Tag = node2.Tag.ToString();
                    index++;
                }
                else
                {
                    TreeNode[] children = new TreeNode[0];
                    TreeNode node3 = new TreeNode(node2.Text, this.ImageIndexBranch, this.ImageIndexBranchSelected, children);
                    node3.Tag = node2.Tag;
                    treeList.Nodes.Add(node3);
                    this.TreeToTreeListNode(node2, node3);
                }
            }
            return treeList;
        }

        private void trview_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode nodeAt = this.tvBrItems.GetNodeAt(e.X, e.Y);
            if (((nodeAt != null) && (this.isBranch(nodeAt) && (nodeAt.Nodes.Count == 0))) && ((nodeAt.Tag.GetType() != typeof(tlNodeInfo)) || (((tlNodeInfo) nodeAt.Tag).itemNames.Length <= 0)))
            {
                string tag;
                if (nodeAt.Tag.GetType() == typeof(string))
                {
                    tag = (string) nodeAt.Tag;
                }
                else
                {
                    tag = ((tlNodeInfo) nodeAt.Tag).path;
                    nodeAt.Tag = tag;
                }
                if (this.Srv.ServerName != "")
                {
                    TreeNode[] nodeArray;
                    if ((HRESULTS.Succeeded(this.Browse(tag, true, out nodeArray)) && (nodeArray != null)) && (nodeArray.Length > 0))
                    {
                        nodeAt.Nodes.AddRange(nodeArray);
                        if (this.treeListMode)
                        {
                            this.BuildTreeList(nodeAt);
                        }
                    }
                    this.tvBrItems.SelectedNode = null;
                    this.tvBrItems.SelectedNode = nodeAt;
                }
            }
        }

        public bool BrowseModeOneLevel
        {
            get
            {
                return this.OneLevelBrowseMode;
            }
            set
            {
                this.OneLevelBrowseMode = value;
                if (this.OneLevelBrowseMode)
                {
                    this.reinstallHandler();
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        public System.Type DataTypeFilter
        {
            get
            {
                return types.ConvertToSystemType(this.dtFilter);
            }
            set
            {
                this.dtFilter = types.ConvertToVarType(value);
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return Images.Instance.ImageList;
            }
        }
    }
}

