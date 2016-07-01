using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;
using UserClientViewModel;

namespace ISafe_UserClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PageControl.Instance.LD50DataShowPage.PageInitial();
            this.frame.Content = PageControl.Instance.LD50DataShowPage;

            this.DataContext = MainWindowViewModel.Instance.LeakAlarmManager;
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            MainWindowViewModel.Instance.OnClose();
            PageControl.Instance.LD50DataShowPage.PageQiut();
        }

        private void SARMaxMin_SARMinClick(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void SARMaxMin_SARCloseClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 页面菜单选择事件
        /// </summary>
        /// <param name="menuItemIndex"></param>
        private void SARMenuBar_SARMenuClick(int menuItemIndex)
        {
            switch (menuItemIndex)
            {
                case 0:
                    PageControl.Instance.ISafeSetPage.PageInitial();
                    this.frame.Content = PageControl.Instance.ISafeSetPage;
                    PageControl.Instance.CurrentPage = PageControl.Instance.ISafeSetPage;
                    break;
                case 1:
                    PageControl.Instance.LD50DataShowPage.PageInitial();
                    this.frame.Content = PageControl.Instance.LD50DataShowPage;
                    PageControl.Instance.CurrentPage = PageControl.Instance.LD50DataShowPage;
                    break;
                case 2:
                    PageControl.Instance.DolDataShowPage.PageInitial();
                    this.frame.Content = PageControl.Instance.DolDataShowPage;
                    PageControl.Instance.CurrentPage = PageControl.Instance.DolDataShowPage;
                    break;
            }
        }

        ///// <summary>
        ///// 打开流量监控显示页面
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Button_Click_4(object sender, RoutedEventArgs e)
        //{
        //    PageControl.Instance.SCADADataShowPage.PageInitial();
        //    this.frame.Content = PageControl.Instance.SCADADataShowPage;
        //}
    }
}
