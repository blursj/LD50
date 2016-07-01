using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CTAPIHelper
{
    internal class MyTask : IDisposable
    {
        private Dictionary<string, string> _Params = new Dictionary<string, string>();//需要读取的各参数的值
        private string _Path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scada.xml");
        private IntPtr _hCTAPI = IntPtr.Zero;
        private Thread _thread = null;
        private bool _Flag = true;
        private int _Multi = 5;
        public MyTask()
        {
            _hCTAPI = Connect();
            if (_hCTAPI != IntPtr.Zero)
            {
                _thread = new Thread(new ThreadStart(() =>
                {
                    while (_Flag)
                    {
                        //读取配置文件xml,然后循环读取数据

                        if (_Params.Count == 0)
                        {
                            var doc = XDocument.Load(_Path);
                            var args = doc.Root.Elements("param");
                            foreach (var item in args)
                            {
                                string par = item.Attribute("tag").Value;
                                if (!string.IsNullOrEmpty(par))
                                {
                                    if (!_Params.ContainsKey(par))
                                    {
                                        int result = 0;
                                        if (!string.IsNullOrEmpty(item.Attribute("length").Value))
                                        {
                                            int.TryParse(item.Attribute("length").Value, out result);
                                        }
                                        _Params[par] = result.ToString();
                                    }
                                }
                            }
                        }

                        foreach (var item in _Params)
                        {
                            int length = 100;
                            int.TryParse(item.Value,out length);


                            StringBuilder result = new StringBuilder(length);
                            
                            //读取tag值,value将写进vsl中，读取此数据需要时间，但是不能确定需要多少时间如果时间较长则会出现延迟问题
                            bool flag = CTAPI.ctTagRead(_hCTAPI, item.Key, result, length);
                            if (flag)
                            {
                                _Params[item.Key] = result.ToString();
                            }
                            else
                            {
                                _Params[item.Key] = "0";
                            }
                        }
                        Thread.Sleep(_Multi * 100);//毫秒
                    }
                }));
                _thread.IsBackground = true;
                _thread.Start();
            }
            else
            {
                Debug.Assert(false, "无法连接scada服务器");
            }
        }

        /// <summary>
        /// 连接scada服务器
        /// </summary>
        /// <returns></returns>
        private IntPtr Connect()
        {
            IntPtr handle = IntPtr.Zero;
            
            if (!System.IO.File.Exists(_Path))
            {
                CreateConfig(_Path);
                return handle;
            }
            var doc = XDocument.Load(_Path);
            var server = doc.Root.Element("server");
            int.TryParse(server.Attribute("refresh").Value, out _Multi);
            
            handle = CTAPI.ctOpen(server.Attribute("ip").Value, server.Attribute("username").Value, server.Attribute("password").Value, 0);
            if (handle == IntPtr.Zero)
            {
                uint dwStatus = CTAPI.GetLastError(); // get error
            }
            return handle;
        }


        /// <summary>
        /// 创建配置文件
        /// </summary>
        /// <param name="path"></param>
        private void CreateConfig(string path)
        {
            using (var stream = System.IO.File.Create(path))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(@"<?xml version=""1.0""?>");
                sb.AppendLine(@"<root>");     
                sb.AppendLine(@"<!--refresh 数据读取时间间隔，以100毫秒为单位  -->");
                sb.AppendLine(@"<server ip=""127.0.0.1"" username=""admin"" password=""123456"" refresh=""5""/>");
                sb.AppendLine(@"<!--需要读取的参数列表，以及各参数的值长度 -->");
                sb.AppendLine(@"<param tag=""tag"" length=""100"" />");                
                sb.AppendLine(@"</root>");
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
        }

        /// <summary>
        /// 断开scada链接
        /// </summary>
        public void Close()
        {
            _Flag = false;
            if (_hCTAPI != IntPtr.Zero)
            {
                CTAPI.ctClose(_hCTAPI);
            }
            _Params.Clear();
            _hCTAPI = IntPtr.Zero;
        }

        /// <summary>
        /// 根据tag获取值
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetValue(string tag)
        {
            if (_Params.ContainsKey(tag))
            {
                return _Params[tag];
            }
            else
            {
                Debug.Assert(false, "配置文件中不存在此名称的参数或者目前scada服务器尚未连接成功");
                return "-1";
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
