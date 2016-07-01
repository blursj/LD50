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
    /// ChanelsTchart.xaml 的交互逻辑
    /// </summary>
    public partial class ChanelsTchart : UserControl
    {

        public ChanelsTchart()
        {
            InitializeComponent();

            _Tcharts = new Dictionary<string, ACUChanelTchart>();
        }

        private event DeleteEventHandle _DeletedTchart;
        //删除显示控件时触发的事件
        public event DeleteEventHandle DeletedTchart
        {
            add
            {
                _DeletedTchart += value;
            }
            remove
            {
                _DeletedTchart -= value;
            }
        }

        private Dictionary<string, ACUChanelTchart> _Tcharts;
        /// <summary>
        /// ACUChanelTchart 控件缓存
        /// </summary>
        public Dictionary<string, ACUChanelTchart> Tcharts
        {
            get
            {
                return _Tcharts;
            }
            set
            {
                _Tcharts = value;
            }
        }

        /// <summary>
        /// 根据拖拽源产生波形控件
        /// </summary>
        /// <param name="ACUDeviceName"></param>
        /// <param name="ChannelNum"></param>
        public void DropDataGenerateTchart(string key)
        {
            if (_Tcharts.Keys.Contains(key))
            {
                return;
            }
            else
            {
                ACUChanelTchart acuchannelchart = new ACUChanelTchart();
                acuchannelchart.ACUName = key.Split('-')[0];
                acuchannelchart.Key = key;
              
                this.conpanel.Children.Add(acuchannelchart);
                _Tcharts.Add(key, acuchannelchart);

                acuchannelchart.DeleteTchart += acuchannelchart_DeleteTchart;
            }
        }

        /// <summary>
        ///向波形控件发送数据
        /// </summary>
        /// <param name="ACUDeviceName"></param>
        /// <param name="ChannelNum"></param>
        /// <param name="senderdata"></param>
        public void SendDataToTchart(string key,double[] data)
        {
            if (_Tcharts.Keys.Contains(key))
            {
                Dictionary<string, double[]> tchartdata = new Dictionary<string, double[]>();
                tchartdata.Add(key, data);
                _Tcharts[key].ChannelDataSource = tchartdata;
            }
        }

        /// <summary>
        /// 删除波形显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void acuchannelchart_DeleteTchart(object sender, EventArgs e)
        {
            this.conpanel.Children.Remove(sender as ACUChanelTchart);

            string key = (sender as ACUChanelTchart).Key;
            string[] mesg = key.Split('-');

            if (_Tcharts.Keys.Contains(key))
            {
                _Tcharts.Remove(key);

                if (_DeletedTchart != null && mesg != null && mesg.Length>2)
                {
                    _DeletedTchart.Invoke(mesg[0], mesg[1], mesg[2]);
                }
            }

        }

        public delegate void DeleteEventHandle(string ACUDIP,string sensorpairindex,string sensorindex);

    }
}
