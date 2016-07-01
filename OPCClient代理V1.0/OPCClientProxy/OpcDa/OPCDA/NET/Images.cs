namespace OPCDA.NET
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class Images : Form
    {
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private IContainer components;
        public static readonly int Folder = 0;
        public System.Windows.Forms.ImageList ImageList;
        public static Images Instance = new Images();
        public static readonly int Item = 5;
        public static readonly int ItemStateBase = 0x15;
        private ListView lvImages;
        public static readonly int Selected = 1;

        public Images()
        {
            this.InitializeComponent();
            this.ShowAll();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(Images));
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.lvImages = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            base.SuspendLayout();
            this.ImageList.ColorDepth = ColorDepth.Depth24Bit;
            this.ImageList.ImageSize = new Size(0x10, 0x10);
            this.ImageList.ImageStream = (ImageListStreamer) manager.GetObject("ImageList.ImageStream");
            this.ImageList.TransparentColor = Color.Teal;
            this.lvImages.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2, this.columnHeader3 });
            this.lvImages.Name = "lvImages";
            this.lvImages.Scrollable = false;
            this.lvImages.Size = new Size(0x150, 0xb8);
            this.lvImages.SmallImageList = this.ImageList;
            this.lvImages.TabIndex = 0;
            this.lvImages.View = View.SmallIcon;
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 40;
            this.columnHeader2.Text = "";
            this.columnHeader2.Width = 40;
            this.columnHeader3.Text = "";
            this.columnHeader3.Width = 40;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x150, 0xb5);
            base.Controls.AddRange(new Control[] { this.lvImages });
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "Images";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "BrowseTree Image List";
            base.ResumeLayout(false);
        }

        public void ShowAll()
        {
            ListViewItem[] items = new ListViewItem[0x1d];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ListViewItem("  " + i.ToString(), i);
            }
            this.lvImages.SmallImageList = this.ImageList;
            this.lvImages.Items.AddRange(items);
        }
    }
}

