using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SARControlLib
{
	/// <summary>
	/// 图表标题
	/// </summary>
	public partial class SARChartTitleBig : UserControl
	{
		public SARChartTitleBig()
		{
			this.InitializeComponent();
		}
        
        /// <summary>
        /// 图表标题显示的文本
        /// </summary>
        public string Title
        {
            set
            {
                this.txtTitle.Text = value;
            }
            get
            {
                return this.txtTitle.Text;
            }
        }
	}
}