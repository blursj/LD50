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
using System.Windows.Shapes;
using System.Xml.Serialization;
using UserClientViewModel;

namespace ISafe_UserClient
{
    /// <summary>
    /// LoginServerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginServerWindow : Window
    {
        public LoginServerWindow()
        {
            InitializeComponent();
            this.Loaded += LoginServerWindow_Loaded;
        }

        void LoginServerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginViewModel.LoginInstance.Initial();
            this.DataContext = LoginViewModel.LoginInstance;
            MainWindowViewModel.Instance.WCFManager.WindowDispatcher = this.Dispatcher;
        }

        /// <summary>
        /// 登录服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (LoginViewModel.LoginInstance.LoginServer())
            {
                MainWindow mainwindow = new MainWindow();
                mainwindow.DataContext = MainWindowViewModel.Instance;
                mainwindow.Show();

                this.Close();
            }
        }
    }
}
