using OPCClientProxy_ViewModel;
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
using System.Windows.Shapes;

namespace OPCClientProxy
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_1]MINT1004",txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_1]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_1]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_1]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_1]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_1]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_1]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_1]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_2]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_2]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_2]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_2]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_2]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_2]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_2]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_2]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_3]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_3]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_3]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_3]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_3]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_3]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_3]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_3]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_4]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_5]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_5]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_5]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_5]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_5]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_5]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_5]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_5]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_6]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_6]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_6]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_6]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_6]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_6]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_6]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_6]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_7]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_7]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_7]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_7]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_7]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_7]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_7]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_7]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_8]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_8]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_8]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_8]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_8]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_8]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_8]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_8]MINT1032", 1);

            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_9]MINT1004", txt1.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_9]MINT1008", txt2.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_9]MINT1012", txt3.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_9]MINT1016", txt4.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_9]MINT1020", txt5.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_9]MINT1024", txt6.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_9]MINT1028", txt7.Text);
            MainWindowViewModel.Instance.OPCProxy.Write("S7:[S7_Connection_9]MINT1032", 1);

        }
    }
}
