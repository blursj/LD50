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
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            this.cav.PreviewMouseLeftButtonDown += cav_PreviewMouseLeftButtonDown;
            this.cav.MouseMove += cav_MouseMove;
            this.cav.MouseRightButtonDown += cav_MouseRightButtonDown;
        }

        void cav_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isPaint)
            {
                this.cav.Children.RemoveAt(this.cav.Children.Count - 1);
                _PipeLines.Add(pipe);
                pipe = new PipeLine() { Points = new List<Point>() };
            }
        }
        
        private Line line;
        private PipeLine pipe = new PipeLine() { Points = new List<Point>() };
        void cav_MouseMove(object sender, MouseEventArgs e)
        {
            if (line!=null)
            {
                line.X2 = e.GetPosition(cav as IInputElement).X;
                line.Y2 = e.GetPosition(cav as IInputElement).Y;
            }
        }
        
        void cav_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (cav.Children.Count >= 1&&isPaint)
            {
                var p = e.GetPosition(cav as IInputElement);
                pipe.Points.Add(p);
                line = new Line();
                line.X1=line.X2 = p.X;
                line.Y1=line.Y2 = p.Y;
                line.Stroke = Brushes.Red;
                line.StrokeThickness = 2;
                Console.WriteLine(string.Format("{0},{1}",p.X,p.Y));
                cav.Children.Add(line);                
            }            
        }
        public List<PipeLine> _PipeLines = new List<PipeLine>();
        /// <summary>
        /// 获取管线信息
        /// </summary>
        public List<PipeLine> PipeLines
        {
            get
            {
                return _PipeLines;
            }
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData("FileDrop") as string[];

            foreach (var item in files)
            {
                string extFile = System.IO.Path.GetExtension(item);
                if (extFile.Equals(".jpg") || extFile.Equals(".bit") || extFile.Equals(".png") || extFile.Equals(".jpeg"))
                {
                    cav.Children.Clear();
                    _PipeLines.Clear();
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(item, UriKind.Absolute));
                    cav.Children.Add(img);
                }
            }
        }

        private bool isPaint = false;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!isPaint)
            {
                (sender as Button).Content = "取消";
                isPaint = true;
            }
            else
            {
                (sender as Button).Content = "绘制";
                isPaint = false;
            }
        }                
    }

    /// <summary>
    /// 管线
    /// </summary>
    public class PipeLine
    {
        private string _Length = "50";
        /// <summary>
        /// 管线长度:公里
        /// </summary>
        public string Length
        {
            get
            {
                return _Length;
            }
            set
            {
                _Length = value;
            }
        }

        /// <summary>
        /// 管线转折点
        /// </summary>
        public List<Point> Points
        {
            get;
            set;
        }
    }

}
