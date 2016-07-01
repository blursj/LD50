namespace OPCDA.NET
{
    using System;
    using System.Windows.Forms;

    public class tlNodeInfo
    {
        public ListViewItem[] itemNames;
        public string path;

        public tlNodeInfo(string pName, int len)
        {
            this.path = pName;
            this.itemNames = new ListViewItem[len];
        }
    }
}

