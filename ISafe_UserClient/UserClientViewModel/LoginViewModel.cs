using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISafe_Model;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;

namespace UserClientViewModel
{
    public class LoginViewModel : PropertyCallBack
    {
        private LoginViewModel() { }

        private string _LoginServerIP = "127.0.0.1";
        /// <summary>
        /// 服务器连接地址
        /// </summary>
        public string LoginServerIP
        {
            get
            {
                return _LoginServerIP;
            }
            set
            {
                _LoginServerIP = value;
                OnPropertyChanged("LoginServerIP");
            }
        }

        private string _LoginMSG;
        public string LoginMSG
        {
            get
            {
                return _LoginMSG;
            }
            set
            {
                _LoginMSG = value;
                OnPropertyChanged("LoginMSG");
            }
        }

        private static LoginViewModel _LoginInstance = new LoginViewModel();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static LoginViewModel LoginInstance
        {
            get
            {
                return _LoginInstance;
            }
        }

        public void Initial() 
        {
            try
            {
                string address = AppDomain.CurrentDomain.BaseDirectory + "ConfigSet.xml";
                if (File.Exists(address))
                {
                    using (FileStream fStream = new FileStream(address, FileMode.Open))
                    {
                        XmlSerializer deserial = new XmlSerializer(typeof(ConfigSetViewModel));
                        MainWindowViewModel.Instance.ConfigSetManager = (ConfigSetViewModel)deserial.Deserialize(fStream);
                        fStream.Close();

                        LoginServerIP = MainWindowViewModel.Instance.ConfigSetManager.WCFServerIP;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 登录服务器
        /// </summary>
        /// <returns></returns>
        public bool LoginServer()
        {
            bool result = false;
            try
            {
                string regexip = "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$";
                Regex re = new Regex(regexip);

                if (re.IsMatch(LoginServerIP))
                {
                    result = MainWindowViewModel.Instance.WCFManager.LoginServer(LoginServerIP);
                    if (result)
                    {
                        LoginMSG = string.Format("{0}:登录IP地址为{1}的服务器成功！", DateTime.Now.ToString(), LoginServerIP);
                    }
                    else
                    {
                        LoginMSG = string.Format("{0}:登录IP地址为{1}的服务器失败！", DateTime.Now.ToString(), LoginServerIP);
                    }
                }
                else 
                {
                    LoginMSG = "IP地址格式不正确！";
                }

            }
            catch
            {
                //填写日志文件
            }

            return result;
        }
    }
}
