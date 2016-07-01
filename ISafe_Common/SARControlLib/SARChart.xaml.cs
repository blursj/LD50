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
using Steema.TeeChart.WPF;
using System.Linq;
using Steema.TeeChart.WPF.Styles;
using System.Text.RegularExpressions;

namespace SARControlLib
{
    /// <summary>
    /// 图表控件
    /// </summary>
    public partial class SARChart : UserControl
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

        public SARChart()
        {
            this.InitializeComponent();

            //由于使用了TeeChart，所以本控件只作为一个背景了
            this.Loaded += new RoutedEventHandler(SARChart_Loaded);
            this.teechart.Panel.Color = Colors.Blue;
        }

        private void SARChart_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsInitial)
            {
                lines = new Dictionary<string, FastLine>();
                line_signs = new Dictionary<string, int>();

                this.teechart.Chart.Axes.Left.Minimum = 0;
                this.teechart.Chart.Axes.Left.Maximum = 100;
                this.teechart.Chart.Axes.Left.Automatic = false;

                this.teechart.Aspect.View3D = false;
                this.teechart.Axes.Bottom.Labels.Visible = false;
                this.teechart.Legend.Visible = true;
                this.teechart.Legend.CheckBoxes = true;

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
                    findcolor = Colors.Sienna;
                    break;
                case 4:
                    findcolor = Colors.BurlyWood;
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
                    findcolor = Colors.Chocolate;
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
            DependencyProperty.Register("MaxPointNum", typeof(int), typeof(SARChart), new PropertyMetadata(500));

        /// <summary>
        /// 对外提供的接口函数（将接收的数据生成波形）
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="datasource"></param>
        public void IFunc_senderWaveData(string key, double[] datasource)
        {
            WaveWorkFunc(key, datasource);
            this.MaxPeak.Text = this.teechart.Chart.Axes.Left.MaxYValue.ToString();
            this.MinPeak.Text = this.teechart.Chart.Axes.Left.MinYValue.ToString();
        }

        /// <summary>
        /// 波形工作函数
        /// </summary>
        /// <param name="LineNum"></param>
        /// <param name="senderdata"></param>
        private void WaveWorkFunc(string key, double[] senderdata)
        {
            if (senderdata == null)
            {
                return;
            }

            if (!IsInitial)
            {
                return;
            }

            FastLine line = null;

            if (!lines.Keys.Contains(key))
            {
                line = new FastLine(this.teechart.Chart);
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
                line.Repaint();
            }
            line.Add(senderdata);

        }

        //private void drawCoordinate()
        //{
        //    Line X = new Line();
        //    X.X1 = leftbottom.X;
        //    X.Y1 = leftbottom.Y;
        //    X.X2 = rightbottom.X + 5;
        //    X.Y2 = rightbottom.Y;
        //    X.Stroke = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x27));
        //    X.StrokeThickness = 2;
        //    this.gridData.Children.Add(X);

        //    Line Y = new Line();
        //    Y.X1 = leftbottom.X;
        //    Y.Y1 = leftbottom.Y;
        //    Y.X2 = lefttop.X;
        //    Y.Y2 = lefttop.Y - 5;
        //    Y.Stroke = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x27));
        //    Y.StrokeThickness = 2;
        //    this.gridData.Children.Add(Y);
        //}

        //#region 坐标
        //private Point leftbottom
        //{
        //    get
        //    {
        //        return new Point(40, this.ActualHeight - 40);
        //    }
        //}

        //private Point rightbottom
        //{
        //    get
        //    {
        //        return new Point(this.ActualWidth - 20, this.ActualHeight - 40);
        //    }
        //}

        //private Point lefttop
        //{
        //    get
        //    {
        //        return new Point(40, 40);
        //    }
        //}

        //private Point righttop
        //{
        //    get
        //    {
        //        return new Point(this.ActualWidth - 20, 40);
        //    }
        //}

        //#endregion



        #region 公共接口

        /// <summary>
        /// 图标标题
        /// </summary>
        public string SARChartTitle
        {
            set
            {
                this.chartTitle.Title = value;
                this.teechart.Header.Text = "";
            }
            get
            {
                return this.chartTitle.Title;
            }
        }

       

        //设置是否显示标题
        private bool isChartShowTitle = true;

        /// <summary>
        /// 设置或者获取是否显示标题
        /// </summary>
        public bool SARShowTitle
        {
            get
            {
                return isChartShowTitle;
            }
            set
            {
                isChartShowTitle = value;
                if (!isChartShowTitle)
                {
                    this.chartTitle.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.chartTitle.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// 返回内部封装的TeeChart
        /// </summary>
        public TChart SARTeeChart
        {
            get
            {
                return this.teechart;
            }
        }

        /// <summary>
        /// 鼠标单击事件
        /// </summary>
        public event MouseButtonEventHandler SARTeeChartDoubleClick;
        private void teechart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SARTeeChartDoubleClick != null)
            {
                SARTeeChartDoubleClick.Invoke(this, e);
            }
        }

        ///// <summary>
        ///// 画X方向上的刻度
        ///// </summary>
        ///// <param name="xInterval">刻度间隔</param>
        //public void SARDrawXMark(double xInterval)
        //{
        //    if (xStart == Double.NaN || xEnd == Double.NaN)
        //    {
        //        throw new Exception("请先设置xStart和xEnd属性");
        //    }
        //    double myInterval = (rightbottom.X - leftbottom.X) / (xEnd - xStart) * xInterval;
        //    double current = leftbottom.X + myInterval;
        //    while (current <= rightbottom.X)
        //    {
        //        Line mark = new Line();
        //        mark.X1 = current;
        //        mark.Y1 = leftbottom.Y;
        //        mark.X2 = current;
        //        mark.Y2 = leftbottom.Y + 5;
        //        mark.Stroke = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x27));
        //        mark.StrokeThickness = 1;
        //        this.gridData.Children.Add(mark);
        //        current += myInterval;
        //    }
        //}

        ///// <summary>
        ///// 画Y方向上的刻度
        ///// </summary>
        ///// <param name="xInterval">刻度间隔</param>
        //public void SARDrawYMark(double yInterval)
        //{
        //    if (yStart == Double.NaN || yEnd == Double.NaN)
        //    {
        //        throw new Exception("请先设置yStart和yEnd属性");
        //    }
        //    double myInterval = (leftbottom.Y - lefttop.Y) / (yEnd - yStart) * yInterval;
        //    double current = leftbottom.Y - myInterval;
        //    while (current >= lefttop.Y)
        //    {
        //        Line mark = new Line();
        //        mark.X1 = leftbottom.X;
        //        mark.Y1 = current;
        //        mark.X2 = leftbottom.X - 5;
        //        mark.Y2 = current;
        //        mark.Stroke = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x27));
        //        mark.StrokeThickness = 1;
        //        this.gridData.Children.Add(mark);
        //        current -= myInterval;
        //    }
        //}

        ///// <summary>
        ///// 画X轴上的刻度值（文本）
        ///// </summary>
        ///// <param name="xInterval">间隔，尽量与刻度间隔一直或为刻度间隔的整数倍</param>
        ///// <param name="format">字体格式，可为""</param>
        //public void SARDrawXText(double xInterval, string format)
        //{
        //    if (xStart == Double.NaN || xEnd == Double.NaN)
        //    {
        //        throw new Exception("请先设置xStart和xEnd属性");
        //    }
        //    double myInterval = (rightbottom.X - leftbottom.X) / (xEnd - xStart) * xInterval;
        //    double current = leftbottom.X + myInterval;
        //    double currentValue = xStart + xInterval;
        //    while (current < rightbottom.X)
        //    {
        //        //先画文本
        //        TextBlock text = new TextBlock();
        //        text.Text = currentValue.ToString(format);
        //        text.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
        //        text.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        //        text.Margin = new Thickness(current - 3 * text.Text.Length, 0, 0, 20);
        //        text.Foreground = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x27));
        //        this.gridData.Children.Add(text);

        //        //再画方格线
        //        Line mark = new Line();
        //        mark.X1 = current;
        //        mark.Y1 = leftbottom.Y;
        //        mark.X2 = current;
        //        mark.Y2 = lefttop.Y;
        //        mark.Stroke = new SolidColorBrush(Color.FromRgb(0x8e, 0xb3, 0xbd));
        //        mark.StrokeThickness = 1;
        //        this.gridData.Children.Add(mark);

        //        current += myInterval;
        //        currentValue += xInterval;
        //    }
        //}

        ///// <summary>
        ///// 画Y方向上的刻度值（文本）
        ///// </summary>
        ///// <param name="yInterval">间隔，尽量与刻度间隔一直或为刻度间隔的整数倍</param>
        ///// <param name="format">字体格式，可为""</param>
        //public void SARDrawYText(double yInterval, string format)
        //{
        //    if (yStart == Double.NaN || yEnd == Double.NaN)
        //    {
        //        throw new Exception("请先设置yStart和yEnd属性");
        //    }
        //    double myInterval = (leftbottom.Y - lefttop.Y) / (yEnd - yStart) * yInterval;
        //    double current = leftbottom.Y - myInterval;
        //    double currentValue = yStart + yInterval;
        //    while (current > lefttop.Y)
        //    {
        //        TextBlock text = new TextBlock();
        //        text.Text = currentValue.ToString(format);
        //        text.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        //        text.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        //        text.Margin = new Thickness(0, current - 7, righttop.X - 13, 0);
        //        text.Foreground = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x27));
        //        this.gridData.Children.Add(text);

        //        Line mark = new Line();
        //        mark.X1 = leftbottom.X;
        //        mark.Y1 = current;
        //        mark.X2 = rightbottom.X;
        //        mark.Y2 = current;
        //        mark.Stroke = new SolidColorBrush(Color.FromRgb(0x8e, 0xb3, 0xbd));
        //        mark.StrokeThickness = 1;
        //        this.gridData.Children.Add(mark);

        //        current -= myInterval;
        //        currentValue += yInterval;
        //    }
        //}

        ///// <summary>
        ///// 画直线
        ///// </summary>
        ///// <param name="x1"></param>
        ///// <param name="y1"></param>
        ///// <param name="x2"></param>
        ///// <param name="y2"></param>
        ///// <param name="width">宽度</param>
        ///// <param name="color">颜色</param>
        //public void SARDrawLine(double x1, double y1, double x2, double y2, double width, Color color)
        //{
        //    if (xStart == Double.NaN || xEnd == Double.NaN || yStart == Double.NaN || yEnd == Double.NaN)
        //    {
        //        throw new Exception("请先设置xStart、xEnd、yStart和yEnd属性");
        //    }

        //    Line line = new Line();
        //    line.X1 = (rightbottom.X - leftbottom.X) / (xEnd - xStart) * (x1 - xStart) + leftbottom.X;
        //    line.Y1 = leftbottom.Y - ((leftbottom.Y - lefttop.Y) / (yEnd - yStart) * (y1 - yStart));
        //    line.X2 = (rightbottom.X - leftbottom.X) / (xEnd - xStart) * (x2 - xStart) + leftbottom.X;
        //    line.Y2 = leftbottom.Y - ((leftbottom.Y - lefttop.Y) / (yEnd - yStart) * (y2 - yStart));
        //    line.Stroke = new SolidColorBrush(color);
        //    line.StrokeThickness = width;
        //    this.gridData.Children.Add(line);
        //}

        ///// <summary>
        ///// 画一个小圆点
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <param name="color">颜色</param>
        //public void SARDrawDataPoint(double x, double y, Color color)
        //{
        //    if (xStart == Double.NaN || xEnd == Double.NaN || yStart == Double.NaN || yEnd == Double.NaN)
        //    {
        //        throw new Exception("请先设置xStart、xEnd、yStart和yEnd属性");
        //    }

        //    ChartPoint point = new ChartPoint();
        //    point.ChartPointColor = color;

        //    point.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        //    point.VerticalAlignment = System.Windows.VerticalAlignment.Top;

        //    double left = (rightbottom.X - leftbottom.X) / (xEnd - xStart) * (x - xStart) + leftbottom.X;
        //    double top = leftbottom.Y - ((leftbottom.Y - lefttop.Y) / (yEnd - yStart) * (y - yStart));
        //    point.Margin = new Thickness(left - 6, top - 6, 0, 0);

        //    this.gridData.Children.Add(point);
        //}

        ///// <summary>
        ///// 在图中画一个小文本作为注释
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <param name="text"></param>
        //public void SARDrawComment(double x, double y, string text)
        //{
        //    TextBlock textblock = new TextBlock();
        //    textblock.Text = text;
        //    textblock.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        //    textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        //    double left = (rightbottom.X - leftbottom.X) / (xEnd - xStart) * (x - xStart) + leftbottom.X;
        //    double top = leftbottom.Y - ((leftbottom.Y - lefttop.Y) / (yEnd - yStart) * (y - yStart));
        //    textblock.Margin = new Thickness(left, top, 0, 0);
        //    textblock.Foreground = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x27));
        //    this.gridData.Children.Add(textblock);
        //}

        /// <summary>
        /// 获取另存为的图片，格式为png格式的
        /// </summary>
        /// <returns></returns>
        public byte[] getImage()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.all.ActualWidth, (int)this.all.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
            bmp.Render(this.all);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(ms);
            ms.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// y轴最小值调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void y_min_TextChanged_1(object sender, TextChangedEventArgs e)
        {
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
                        if (this.teechart != null)
                        {
                            this.teechart.Chart.Axes.Left.Minimum = Convert.ToDouble(this.MinY.Text);
                        }

                    }

                }
                catch
                {
                    //填写日志文件
                }

            }
        }

        /// <summary>
        /// y轴最大值调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void y_max_TextChanged_1(object sender, TextChangedEventArgs e)
        {
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
                        if (this.teechart != null)
                        {
                            this.teechart.Chart.Axes.Left.Maximum = Convert.ToDouble(this.MaxY.Text);
                        }

                    }

                }
                catch
                {
                    //填写日志文件
                }

            }
        }       

        ///// <summary>
        ///// 清除所有数据
        ///// </summary>
        //public void SARClear()
        //{
        //    this.gridData.Children.Clear();
        //}

        #endregion

    }
}