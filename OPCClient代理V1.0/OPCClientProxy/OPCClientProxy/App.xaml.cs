using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using OPCClientProxy_ViewModel;
using System.Threading;

namespace OPCClientProxy
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            MainWindowViewModel.Instance.WindowDispatcher = this.Dispatcher;

            //系统初始化
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                MainWindowViewModel.Instance.OPCServersVMInstance.Initial();
            }, null);

            mainwindow.DataContext = MainWindowViewModel.Instance;
            mainwindow.Show();

            //TestWindow tst = new TestWindow();
            //tst.DataContext = MainWindowViewModel.Instance;
            //tst.Show();
        }
    }
}
