using Steema.TeeChart.WPF.Styles;
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
    /// AccumulateTchart.xaml 的交互逻辑
    /// </summary>
    public partial class AccumulateTchart : UserControl
    {
        public AccumulateTchart()
        {
            InitializeComponent();

            _GridCollection = new Dictionary<string, Grid>();
            this.tchart.Title = "波形重叠显示";
        }

        private Dictionary<string, Grid> _GridCollection;


        private event DeleteEventHandle _DeletedTchart;
        //删除显示控件时触发的事件
        public event DeleteEventHandle DeletedTchart
        {
            add
            {
                _DeletedTchart += value;
            }
            remove
            {
                _DeletedTchart -= value;
            }
        }

        /// <summary>
        /// 拖拽添加一条波形线
        /// </summary>
        /// <param name="ACUDeviceName"></param>
        /// <param name="ChannelNum"></param>
        public void DropDataGerateChart(string key,string Name)
        {
            if (_GridCollection.Keys.Contains(key))
            {
                return;
            }

            Grid grid = new Grid();
            //grid.RowDefinitions = new RowDefinitionCollection();
            RowDefinition row1 = new RowDefinition();
            row1.Height = GridLength.Auto;

            RowDefinition row2 = new RowDefinition();
            row2.Height = GridLength.Auto;

            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);

            TextBlock textblock1 = new TextBlock();
            textblock1.Text = key;
            textblock1.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(textblock1);
            Grid.SetRow(textblock1, 0);

            Image image = new Image();
            image.Height = 30;
            image.Width = 30;
            string path = "/ISafe_UICommon;component/Images/delete.png";
            image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            image.Tag = key;
            image.MouseLeftButtonDown += image_MouseLeftButtonDown;
            grid.Children.Add(image);
            Grid.SetRow(image, 0);

            TextBlock textblock2 = new TextBlock();
            textblock2.Text = Name;
            textblock2.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(textblock2);
            Grid.SetRow(textblock2, 1);

            grid.Margin = new Thickness(5, 0, 5, 0);
            this.panel.Children.Add(grid);

            _GridCollection.Add(key, grid);
        }

        /// <summary>
        /// 将数据
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="senderdata"></param>
        public void SendDataToLine(string key,double[] senderdata)
        {
            this.tchart.IFunc_senderWaveData(key,senderdata);
        }

        /// <summary>
        /// 清除显示线条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;

            if (image.Tag != null)
            {
                string key = image.Tag.ToString();
                if (key == null)
                {
                    return;
                }
                //在波形显示控件中删除该线条
                this.tchart.DeleteWaveLine(key);

                if (_GridCollection.Keys.Contains(key))
                {
                    this.panel.Children.Remove(_GridCollection[key]);
                    _GridCollection.Remove(key);
                }

                if (_DeletedTchart != null)
                {
                    _DeletedTchart.Invoke(key);
                }
            }
        }

        public delegate void DeleteEventHandle(string key);
    }
}
