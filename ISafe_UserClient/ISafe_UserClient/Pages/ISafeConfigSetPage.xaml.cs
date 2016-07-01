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
using UserClientViewModel;

namespace ISafe_UserClient
{
    /// <summary>
    /// ISafeConfigSetPage.xaml 的交互逻辑
    /// </summary>
    public partial class ISafeConfigSetPage : Page,iPage
    {
        public ISafeConfigSetPage()
        {
            InitializeComponent();
        }

        public void PageInitial()
        {
            this.DataContext = MainWindowViewModel.Instance;
        }

        public void PageQiut()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// panel页面显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch(btn.Tag.ToString())
            {
                case "0":
                    this.panel1.Visibility = Visibility.Visible;
                    //this.panel2.Visibility = Visibility.Hidden;
                    this.panel3.Visibility = Visibility.Hidden;
                    break;
                case "1":
                    this.panel1.Visibility = Visibility.Hidden;
                    //this.panel2.Visibility = Visibility.Visible;
                    this.panel3.Visibility = Visibility.Hidden;
                    break;
                case "2":
                    this.panel1.Visibility = Visibility.Hidden;
                    //this.panel2.Visibility = Visibility.Hidden;
                    this.panel3.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
