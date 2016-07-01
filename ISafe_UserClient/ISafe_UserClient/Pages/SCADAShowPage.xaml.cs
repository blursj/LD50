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

namespace ISafe_UserClient
{
    /// <summary>
    /// SCADAShowPage.xaml 的交互逻辑
    /// </summary>
    public partial class SCADAShowPage : Page,iPage
    {
        public SCADAShowPage()
        {
            InitializeComponent();
        }

        public void PageInitial()
        {
            
        }

        public void PageQiut()
        {
            throw new NotImplementedException();
        }
    }
}
