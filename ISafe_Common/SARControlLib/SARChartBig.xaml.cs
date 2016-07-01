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

namespace SARControlLib
{
    /// <summary>
    /// 图表控件
    /// </summary>
    public partial class SARChartBig : UserControl
    {
        public SARChartBig()
        {
             
            this.InitializeComponent();

            //由于使用了TeeChart，所以本控件只作为一个背景了
            //this.Loaded += new RoutedEventHandler(SARChart_Loaded);
            
        }

        //private void SARChart_Loaded(object sender, RoutedEventArgs e)
        //{
        //    drawCoordinate();
        //}

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
        public string SARChartTitleBig
        {
            set
            {
                this.chartTitleBig.Title = value;
            }
            get
            {
                return this.chartTitleBig.Title;
            }
        }

        //private double xStart = Double.NaN;

        ///// <summary>
        ///// X轴的起始值
        ///// </summary>
        //public double SARXStart
        //{
        //    get { return xStart; }
        //    set { xStart = value; }
        //}
        //private double xEnd = Double.NaN;

        ///// <summary>
        ///// X轴的结束值
        ///// </summary>
        //public double SARXEnd
        //{
        //    get { return xEnd; }
        //    set { xEnd = value; }
        //}
        //private double yStart = Double.NaN;

        ///// <summary>
        ///// Y轴的起始值
        ///// </summary>
        //public double SARYStart
        //{
        //    get { return yStart; }
        //    set { yStart = value; }
        //}
        //private double yEnd = Double.NaN;

        ///// <summary>
        ///// Y轴的结束值
        ///// </summary>
        //public double SARYEnd
        //{
        //    get { return yEnd; }
        //    set { yEnd = value; }
        //}

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
                    this.chartTitleBig.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.chartTitleBig.Visibility = Visibility.Visible;
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