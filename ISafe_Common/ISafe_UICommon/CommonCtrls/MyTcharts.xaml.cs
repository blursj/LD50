/*
* Copyright (c) 2013,北京昆仑卓越信息技术有限公司
* All rights reserved.
* 
* 文件名称：WaveControl
* 文件标识：见配置管理计划书
* 摘    要：用户控件描述
* 
* 当前版本：1.0
* 作    者：刘建伟
* 完成日期：$date$
*
* 取代版本：
* 原 作 者：
* 完成日期：
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Steema.TeeChart.WPF;
using Steema.TeeChart.WPF.Styles;

namespace ISafe_UICommon.CommonCtrls
{
    /// <summary>
    /// WaveControl.xaml 的交互逻辑
    /// </summary>
    public partial class MyTcharts : UserControl
    {
        public MyTcharts()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 记录tchart集合
        /// </summary>
        private List<MyTchart> tchars;

        
        private List<CheckBox> checkBoxList;


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MyTcharts), new PropertyMetadata(""));



        /// <summary>
        /// 波形控件个数
        /// </summary>
        public int WaveNum
        {
            get { return (int)GetValue(WaveNumProperty); }
            set { SetValue(WaveNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaveNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaveNumProperty =
            DependencyProperty.Register("WaveNum", typeof(int), typeof(MyTcharts), new PropertyMetadata(0, OnWaveNumChanged));

        /// <summary>
        /// 波形控件数大小发生改变后的处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWaveNumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as MyTcharts).GenerateTcharts();
        }

        /// <summary>
        /// 每个波形能显示的最大点数
        /// </summary>
        public int MaxPointNums
        {
            get { return (int)GetValue(MaxPointNumsProperty); }
            set { SetValue(MaxPointNumsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxPointNums.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxPointNumsProperty =
            DependencyProperty.Register("MaxPointNums", typeof(int), typeof(MyTcharts), new PropertyMetadata(0, OnPointNumChanged));


        private static void OnPointNumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as MyTcharts).SetMaxPointNum();
        }

        private void SetMaxPointNum()
        {
            while (tchars.Count == 0)
            {
                //do nothing
            }

            foreach (var item in tchars)
            {
                item.MaxPointNum = MaxPointNums;
            }
        }

        /// <summary>
        /// 接收波形数据
        /// </summary>
        public Dictionary<string, double[]> DataSource
        {
            get { return (Dictionary<string, double[]>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(Dictionary<string, double[]>), typeof(MyTcharts), new PropertyMetadata(null, OnDataChanged));


        private static void OnDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //显示波形
            (sender as MyTcharts).GenerateWaves();
        }


        /// <summary>
        /// 显示波形
        /// </summary>
        private void GenerateWaves()
        {
            if (DataSource == null || DataSource.Count == 0)
            {
                return;
            }

            int index = 0;
            foreach (var item in DataSource)
            {
                if (!(bool)checkBoxList[index].IsChecked)
                {
                    index++;
                    continue;
                }

                Dictionary<string, double[]> singleData = new Dictionary<string, double[]>();
                singleData.Add(item.Key, item.Value);
                tchars[index].DataSource = singleData;

                index++;
            }
        }

        /// <summary>
        /// 生成Tchart控件试图
        /// </summary>
        private void GenerateTcharts()
        {
            if (tchars == null)
            {
                tchars = new List<MyTchart>();
            }
            else
            {
                tchars.Clear();
            }

            if (checkBoxList == null)
            {
                checkBoxList = new List<CheckBox>();
            }
            else
            {
                checkBoxList.Clear();
            }

            this.Wavepanel.Children.Clear();
            this.signPanel.Children.Clear();

            //生成TChart控件和Checkbox控件
            for (int i = 0; i < WaveNum; i++)
            {
                MyTchart tchart = new MyTchart();

                tchart.Margin = new Thickness(5, 0, 5, 5);
                tchart.Title = "通道" + i;
                tchart.MaxPointNum = MaxPointNums;

                tchars.Add(tchart);
                this.Wavepanel.Children.Add(tchart);


                CheckBox checkbox = new CheckBox();
                checkbox.Content = "通道" + i;

                checkbox.Margin = new Thickness(5, 0, 0, 0);
                this.signPanel.Children.Add(checkbox);

                checkBoxList.Add(checkbox);
            }
        }
    }
}
