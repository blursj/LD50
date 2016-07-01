using ISafe_Model;
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
    /// DOLPHINShowPage.xaml 的交互逻辑
    /// </summary>
    public partial class DOLPHINShowPage : Page, iPage
    {
        public DOLPHINShowPage()
        {
            InitializeComponent();
        }

        public void PageInitial()
        {
           
            this.DataContext = null;
            this.DataContext = MainWindowViewModel.Instance.DolShowManager;

            this.datagrid1.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
        }

        public void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }


        public void PageQiut()
        {
            throw new NotImplementedException();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string pointID = btn.Tag.ToString();
            if (pointID != null)
            {
                MainWindowViewModel.Instance.WCFManager.SetDolPointValue(pointID, true);
            }
        }

      


    }
}
