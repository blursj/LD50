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
	/// 单选框
	/// </summary>
	public partial class RadioButton : UserControl
	{
		public RadioButton()
		{
			this.InitializeComponent();
		}

        /// <summary>
        /// 设置或者获取互斥的组名
        /// </summary>
        public string SARGroupName
        {
            set
            {
                this.intRadioBtn.GroupName = value;
            }
            get
            {
                return this.intRadioBtn.GroupName;
            }
        }

        /// <summary>
        /// 设置或者获取当前是否被选中
        /// </summary>
        public bool? SARIsChecked
        {
            set
            {
                this.intRadioBtn.IsChecked = value;
                if (value == true)
                {
                    this.gridSelected.Visibility = System.Windows.Visibility.Visible;
                    this.gridUnselected.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    this.gridSelected.Visibility = System.Windows.Visibility.Hidden;
                    this.gridUnselected.Visibility = System.Windows.Visibility.Visible;
                }
            }
            get
            {
                return this.intRadioBtn.IsChecked;
            }
        }

        /// <summary>
        /// 设置或者获取当前Radiobutton的文字
        /// </summary>
        public string SARText
        {
            set
            {
                this.txtLabel.Text = value;
            }
            get
            {
                return this.txtLabel.Text;
            }
        }

        public Brush SARColor
        {
            set
            {
                this.txtLabel.Foreground = value;
            }
            get
            {
                return this.txtLabel.Foreground;
            }
        }

        /// <summary>
        /// 该单选框被选中事件
        /// </summary>
        public event MouseButtonEventHandler SAR_RadioBtnChecked;


        private void gridMouseCapture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.intRadioBtn.IsChecked = true;            
        }

        private void intRadioBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            this.gridSelected.Visibility = System.Windows.Visibility.Hidden;
            this.gridUnselected.Visibility = System.Windows.Visibility.Visible;
        }

        private void intRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            this.gridSelected.Visibility = System.Windows.Visibility.Visible;
            this.gridUnselected.Visibility = System.Windows.Visibility.Hidden;

            if (SAR_RadioBtnChecked != null)
            {
                SAR_RadioBtnChecked.Invoke(this, null);
            }
        }
	}
}