using Simulator_ViewModel;
using SimulatorModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace LD50_Simulator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.InitialConfig();

            this.DataContext = MainWindowViewModel.Instance;

            MainWindowViewModel.Instance.SendDataToTchart += new MainWindowViewModel.SendDataToTchartHandler(SendDataToTchartFunc);

            MainWindowViewModel.Instance.SendMSGToUIEvent += new MainWindowViewModel.SendMSGToUIHandler(ShowMSG);

            MainWindowViewModel.Instance.ConfigManager.ForeLeakClickTime = DateTime.Now;
        }

        /// <summary>
        /// 显示系统运行信息
        /// </summary>
        /// <param name="MSG"></param>
        private void ShowMSG(string MSG) 
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if(this.MSGCtr.Children.Count > 15)
                {
                    this.MSGCtr.Children.RemoveAt(0);
                }

                TextBlock text = new TextBlock();
                text.Foreground = new SolidColorBrush(Colors.White);
                text.FontSize = 14.5;
                text.Margin = new Thickness(2, 1, 0, 0);
                text.Text = MSG;

                this.MSGCtr.Children.Add(text);
            })); 
        }

        /// <summary>
        /// 将数据给到TChart进行曲线显示
        /// </summary>
        /// <param name="SendDatas"></param>
        private void SendDataToTchartFunc(Dictionary<string, List<double>> SendDatas)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                foreach (var item in SendDatas)
                {
                    MyChart.IFunc_senderWaveData(item.Key, item.Value.ToArray<double>());
                }
            }));

        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SARMaxMin_SARMinClick_1(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SARMaxMin_SARCloseClick_1(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }


    }
}
