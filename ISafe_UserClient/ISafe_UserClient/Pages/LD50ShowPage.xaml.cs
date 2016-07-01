using ISafe_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// LD50ShowPage.xaml 的交互逻辑
    /// </summary>
    public partial class LD50ShowPage : Page, iPage
    {
        private System.Timers.Timer _ShowWaveTimer;

        private object _WaveDataCache1lock = new object();

        private object _WaveDataCache2lock = new object();

        private Dictionary<string, List<double>> _WaveDataCache1;

        private Dictionary<string, List<double>> _WaveDataCache2;

        public LD50ShowPage()
        {
            InitializeComponent();
        }

        public void PageInitial()
        {
            this.DataContext = MainWindowViewModel.Instance;
            MainWindowViewModel.Instance.WCFManager.SorDataCallBackEvent += WCFManager_SorDataCallBackEvent;

            _WaveDataCache1 = new Dictionary<string, List<double>>();
            _WaveDataCache2 = new Dictionary<string, List<double>>();
            _ShowWaveTimer = new System.Timers.Timer(2000);
            _ShowWaveTimer.Start();

            _ShowWaveTimer.Elapsed += _ShowWaveTimer_Elapsed;
        }



        /// <summary>
        /// 显示波形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _ShowWaveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
           {
               lock (_WaveDataCache1lock)
               {
                   foreach (var item in _WaveDataCache1)
                   {
                       this.Pre_Chart.IFunc_senderWaveData(item.Key, item.Value.ToArray<double>());
                   }

                   _WaveDataCache1.Clear();
               }

               lock (_WaveDataCache2lock)
               {
                   foreach (var item in _WaveDataCache2)
                   {
                       this.Mask_Chart.IFunc_senderWaveData(item.Key, item.Value.ToArray<double>());
                   }

                   _WaveDataCache2.Clear();
               }
           }));

        }

        /// <summary>
        /// 压力数据回调
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Datasource"></param>
        void WCFManager_SorDataCallBackEvent(int datasign, string Key, double[] Datasource)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                switch (datasign)
                {
                    //压力原始波形
                    case 0:

                        lock (_WaveDataCache1lock)
                        {

                            if (_WaveDataCache1.Keys.Contains(Key))
                            {
                                foreach (var item in Datasource)
                                {
                                    _WaveDataCache1[Key].Add(item);
                                }

                            }
                            else
                            {
                                _WaveDataCache1.Add(Key, new List<double>(Datasource));
                            }
                        }
                        // this.Pre_Chart.IFunc_senderWaveData(Key, Datasource);
                        break;
                    //Mask波形
                    case 1:
                        lock (_WaveDataCache2lock)
                        {

                            if (_WaveDataCache2.Keys.Contains(Key))
                            {
                                foreach (var item in Datasource)
                                {
                                    _WaveDataCache2[Key].Add(item);
                                }

                            }
                            else
                            {
                                _WaveDataCache2.Add(Key, new List<double>(Datasource));
                            }
                        }
                        // this.Mask_Chart.IFunc_senderWaveData(Key, Datasource);
                        break;
                    //Thresh波形
                    case 2:
                        //this.Thresh_Chart.IFunc_senderWaveData(Key, Datasource);
                        break;
                }
            }));
        }

        public void PageQiut()
        {
            MainWindowViewModel.Instance.WCFManager.SorDataCallBackEvent -= WCFManager_SorDataCallBackEvent;

            if (_ShowWaveTimer != null)
            {
                try
                {
                    _ShowWaveTimer.Stop();
                    _ShowWaveTimer = null;
                }
                catch { }
            }
        }


    }
}
