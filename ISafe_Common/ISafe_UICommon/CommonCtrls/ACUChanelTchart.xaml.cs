using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ISafe_UICommon.CommonCtrls
{
    /// <summary>
    /// ACUChanelTchart.xaml 的交互逻辑
    /// </summary>
    public partial class ACUChanelTchart : UserControl
    {
        public ACUChanelTchart()
        {
            InitializeComponent();
        }

        private event DeleteEventHandle _DeleteTchart;
        //删除显示控件时触发的事件
        public event DeleteEventHandle DeleteTchart
        {
            add
            {
                _DeleteTchart += value;
            }
            remove
            {
                _DeleteTchart -= value;
            }
        }

        /// <summary>
        /// 所属ACU设备名称
        /// </summary>
        public string ACUName
        {
            get { return (string)GetValue(ACUNameProperty); }
            set { SetValue(ACUNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ACUName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ACUNameProperty =
            DependencyProperty.Register("ACUName", typeof(string), typeof(ACUChanelTchart), new PropertyMetadata(""));


        /// <summary>
        /// 标识符
        /// </summary>
        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ACUIP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(ACUChanelTchart), new PropertyMetadata("",OnKeyChanged));


        private static void OnKeyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ACUChanelTchart).SetTchartTitle();
        }

        private void SetTchartTitle()
        {
            string[] msg = Key.Split('-');
            this.tchart.Title = "传感器阵列" + msg[1] + "-" + "传感器" + msg[2];
        }

        /// <summary>
        /// 接收波形数据
        /// </summary>
        public Dictionary<string, double[]> ChannelDataSource
        {
            get { return (Dictionary<string, double[]>)GetValue(ChannelDataSourceProperty); }
            set { SetValue(ChannelDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChannelDataSourceProperty =
            DependencyProperty.Register("ChannelDataSource", typeof(Dictionary<string, double[]>), typeof(ACUChanelTchart), new PropertyMetadata(null, OnDataChanned));

        private static void OnDataChanned(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ACUChanelTchart).senderData();
        }

        private void senderData() 
        {
            this.tchart.DataSource = ChannelDataSource;
        }

        /// <summary>
        /// 点击删除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (_DeleteTchart != null)
            {
                _DeleteTchart(this,e);
            }
        }

        public delegate void DeleteEventHandle(object sender,EventArgs e);
    }
}
