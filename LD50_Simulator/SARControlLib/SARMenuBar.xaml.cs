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
	/// SARMenuBar.xaml 的交互逻辑
	/// </summary>
	public partial class SARMenuBar : UserControl
	{
		public SARMenuBar()
		{
			this.InitializeComponent();
           
		}

        private int currentSelectedIndex = 0;

        /// <summary>
        /// 获取当前选中的菜单项目，从0开始数
        /// </summary>
        public int SARCurrentSelectedIndex
        {
            get { return currentSelectedIndex; }
            set
            {

            }
        }

        /// <summary>
        /// 菜单被单击了的事件
        /// </summary>
        public event MenuClickHandler SARMenuClick;

        private void mouse1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (currentSelectedIndex == 0)
            {
                return;
            }
           
        }

        private void mouse1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (currentSelectedIndex == 0)
            {
                return;
            }
          
        }

        private void mouse1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SARCurrentSelectedIndex = 0;
            if (SARMenuClick != null)
            {
                SARMenuClick.Invoke(0);
            }
        }

        private void mouse2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (currentSelectedIndex == 1)
            {
                return;
            }
        }

        private void mouse2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (currentSelectedIndex == 1)
            {
                return;
            }
        }

        private void mouse2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SARCurrentSelectedIndex = 1;
            if (SARMenuClick != null)
            {
                SARMenuClick.Invoke(1);
            }
        }

        private void mouse3_MouseEnter(object sender, MouseEventArgs e)
        {
            if (currentSelectedIndex == 2)
            {
                return;
            }
        }

        private void mouse3_MouseLeave(object sender, MouseEventArgs e)
        {
            if (currentSelectedIndex == 2)
            {
                return;
            }
        }

        private void mouse3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SARCurrentSelectedIndex = 2;
            if (SARMenuClick != null)
            {
                SARMenuClick.Invoke(2);
            }
        }       
	}

    public delegate void MenuClickHandler(int menuItemIndex);
}