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
	/// SARMaxMin.xaml 的交互逻辑
	/// </summary>
	public partial class SARMaxMin : UserControl
	{
		public SARMaxMin()
		{
			this.InitializeComponent();
		}

        /// <summary>
        /// 最大最小化关闭按钮被单击事件
        /// </summary>
        public event MouseButtonEventHandler SARMinClick;

        public event MouseButtonEventHandler SARCloseClick;

        private void mouse1_MouseEnter(object sender, MouseEventArgs e)
        {
            highlight1.Opacity = 1;

        }

        private void mouse1_MouseLeave(object sender, MouseEventArgs e)
        {
            highlight1.Opacity = 0;

        }

        private void mouse1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SARMinClick != null)
            {
                SARMinClick.Invoke(this, e); 
            }

        }

        private void mouse3_MouseEnter(object sender, MouseEventArgs e)
        {
            highlight3.Opacity = 1;
        }

        private void mouse3_MouseLeave(object sender, MouseEventArgs e)
        {
            highlight3.Opacity = 0;
        }

        private void mouse3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SARCloseClick != null)
            {
                SARCloseClick.Invoke(this, e);
            }

        }
	}
}