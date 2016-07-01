using Steema.TeeChart.WPF.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ISafe_UICommon.CommonCtrls
{
    /// <summary>
    /// MyTchart.xaml 的交互逻辑
    /// </summary>
    public partial class MyTchart : UserControl
    {
        /// 记录线条集合
        /// </summary>
        private Dictionary<string, FastLine> lines;

        /// <summary>
        /// 记录线条的下标
        /// </summary>
        private Dictionary<string, int> line_signs;

        /// <summary>
        /// 保证控件只初始化一遍
        /// </summary>
        private bool IsInitial = false;

        //控件标题
        public string Title
        {
            get
            {
                return this.uctrlTchart.Chart.Header.Text;
            }
            set
            {
                this.uctrlTchart.Chart.Header.Text = value;
            }
        }



        public MyTchart()
        {
            InitializeComponent();

            this.Loaded += SingleWaveShowCtrl_Loaded;
        }

        //控件初始化
        void SingleWaveShowCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.uctrlTchart.Chart.Series.Clear();
            // this.uctrlTchart.Chart.Series.RemoveAllSeries();

            if (!IsInitial)
            {
                lines = new Dictionary<string, FastLine>();
                line_signs = new Dictionary<string, int>();

                this.uctrlTchart.Chart.Axes.Left.Minimum = 0;
                this.uctrlTchart.Chart.Axes.Left.Maximum = 100;
                this.uctrlTchart.Chart.Axes.Left.Automatic = false;

                this.uctrlTchart.Aspect.View3D = false;
                this.uctrlTchart.Axes.Bottom.Labels.Visible = false;
                this.uctrlTchart.Legend.Visible = true;
                this.uctrlTchart.Legend.CheckBoxes = true;

                IsInitial = true;
            }

        }

        #region //获取颜色

        private Color GetColor(int sign)
        {
            Color findcolor = Colors.Green;
            switch (sign)
            {
                case 0:
                    findcolor = Colors.Green;
                    break;
                case 1:
                    findcolor = Colors.Red;
                    break;
                case 2:
                    findcolor = Colors.Blue;
                    break;
                case 3:
                    findcolor = Colors.Yellow;
                    break;
                case 4:
                    findcolor = Colors.Gold;
                    break;
                case 5:
                    findcolor = Colors.YellowGreen;
                    break;
                case 6:
                    findcolor = Colors.Tomato;
                    break;
                case 7:
                    findcolor = Colors.Navy;
                    break;
                case 8:
                    findcolor = Colors.Maroon;
                    break;
                case 9:
                    findcolor = Colors.RosyBrown;
                    break;
                case 10:
                    findcolor = Colors.Purple;
                    break;

                default:
                    findcolor = Colors.Green;
                    break;
            }

            return findcolor;
        }

        #endregion


        //Tchart控件可显示最大点数
        public int MaxPointNum
        {
            get { return (int)GetValue(MaxPointNumProperty); }
            set { SetValue(MaxPointNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxPointNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxPointNumProperty =
            DependencyProperty.Register("MaxPointNum", typeof(int), typeof(MyTchart), new PropertyMetadata(1000));



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
            DependencyProperty.Register("DataSource", typeof(Dictionary<string, double[]>), typeof(MyTchart), new PropertyMetadata(null, OnDataChanged));


        private static void OnDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        /// <summary>
        /// 对外提供的接口函数（将接收的数据生成波形）
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="datasource"></param>
        public void IFunc_senderWaveData(string key, double[] datasource)
        {
            WaveWorkFunc(key,datasource);
            this.MaxPeak.Text = this.uctrlTchart.Chart.Axes.Left.MaxYValue.ToString();
            this.MinPeak.Text = this.uctrlTchart.Chart.Axes.Left.MinYValue.ToString();
        }

        /// <summary>
        /// 删除波形控件一条线
        /// </summary>
        /// <param name="sign"></param>
        public void DeleteWaveLine(string key)
        {
            if (lines.Keys.Contains(key))
            {
                //找到元素索引号

                //在缓冲中清除该线条
                lines.Remove(key);

                this.uctrlTchart.Chart.Series[line_signs[key]].Clear();
                this.uctrlTchart.Chart.Series[line_signs[key]].Repaint();
                this.uctrlTchart.Chart.Series.RemoveAt(line_signs[key]);


                line_signs.Remove(key);

                int index = 0;

                for (int i = 0; i < line_signs.Count; i++)
                {
                    if (line_signs.Values.ElementAt(i) == index)
                    {
                        index++;
                        continue;
                    }
                    else
                    {
                        line_signs[line_signs.Keys.ElementAt(i)] = index;
                    }
                    index++;
                }
            }
        }

      

        /// <summary>
        /// 波形工作函数
        /// </summary>
        /// <param name="LineNum"></param>
        /// <param name="senderdata"></param>
        private void WaveWorkFunc(string key,double[] senderdata)
        {
            if (senderdata == null)
            {
                return;
            }

            FastLine line = null;

            if (!lines.Keys.Contains(key))
            {
                line = new FastLine(this.uctrlTchart.Chart);
                line_signs.Add(key, lines.Count);
                line.Title = key;
                line.Visible = true;
                line.Color = GetColor(line_signs[key]);

                lines.Add(key, line);


            }
            else
            {
                line = lines[key];
            }

            int pointNums = line.Count + senderdata.Length;

            if (pointNums > MaxPointNum)
            {
                int deletePNum = Math.Min(line.Count, pointNums - MaxPointNum);

                line.Delete(0, deletePNum, true);
                //lines[i].RefreshSeries();
                line.Repaint();
            }
            line.Add(senderdata);

        }

        //最大刻度值发生改变时发生的事件
        private void MaxY_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            // string pattern = @"^([1-9]\d*)|(-[1-9]\d*)|0$";
            string pattern = @"^(-?\d+)(\.\d+)?$";
            Regex re = new Regex(pattern);

            if (this.MaxY == null || this.MinY == null)
            {
                return;
            }
            if (this.MaxY.Text == "" || this.MinY.Text == "")
            {
                return;
            }

            if (re.IsMatch(this.MaxY.Text) && re.IsMatch(this.MinY.Text))
            {
                try
                {
                    if (Convert.ToDouble(this.MaxY.Text) > Convert.ToDouble(this.MinY.Text))
                    {
                        if (this.uctrlTchart != null)
                        {
                            this.uctrlTchart.Chart.Axes.Left.Maximum = Convert.ToDouble(this.MaxY.Text);
                        }

                    }
                }
                catch { }

            }
        }

        //最小刻度值发生改变时发生的事件
        private void MinY_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //string pattern = @"^([1-9]\d*)|(-[1-9]\d*)|0$";
            string pattern = @"^(-?\d+)(\.\d+)?$";
            Regex re = new Regex(pattern);
            if (this.MaxY == null || this.MinY == null)
            {
                return;
            }
            if (this.MaxY.Text == "" || this.MinY.Text == "")
            {
                return;
            }
            if (re.IsMatch(this.MaxY.Text) && re.IsMatch(this.MinY.Text))
            {
                try
                {
                    if (Convert.ToDouble(this.MaxY.Text) > Convert.ToDouble(this.MinY.Text))
                    {
                        if (this.uctrlTchart != null)
                        {
                            this.uctrlTchart.Chart.Axes.Left.Minimum = Convert.ToDouble(this.MinY.Text);
                        }

                    }

                }
                catch
                {

                }

            }
        }

        //图例说明是否可见发生改变时发生的事件
        private void LegendZooe_Checked_1(object sender, RoutedEventArgs e)
        {
            if (this.uctrlTchart != null)
            {
                this.uctrlTchart.Legend.Visible = (bool)this.LegendZooe.IsChecked;
            }

        }

        private void LegendZooe_Unchecked_1(object sender, RoutedEventArgs e)
        {
            if (this.uctrlTchart != null)
            {
                this.uctrlTchart.Legend.Visible = (bool)this.LegendZooe.IsChecked;
            }
        }
    }
}
