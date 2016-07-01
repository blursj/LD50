using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ACUServer
{
    public class FileManager
    {
        private FileManager() { }

        private static FileManager _Instance = new FileManager();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static FileManager Instance
        {
            get
            {
                return _Instance;
            }
        }

        /// <summary>
        /// 上次保存的天
        /// </summary>
        private DateTime _PreHistoryDay = DateTime.MinValue;

        /// <summary>
        /// _WaveControls操作锁
        /// </summary>
        private object _WaveCtrlsLock = new object();

        /// <summary>
        /// 波形文件流缓存
        /// </summary>
        private Dictionary<string, WaveControl> _WaveControls = new Dictionary<string, WaveControl>();

        /// <summary>
        /// _LeakIo 操作锁
        /// </summary>
        private object _LeakIoobject = new object();

        /// <summary>
        /// 保存泄漏信息的文件流
        /// </summary>
        private Dictionary<string, StreamWriter> _LeakIo = new Dictionary<string, StreamWriter>();

        /// <summary>
        /// 判断日期是否来到第二天
        /// </summary>
        /// <returns></returns>
        private bool DateJudge()
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"), dtFormat);

            //如果时间来到第二天 则关闭前一天所有文件流并清空
            if (DateTime.Compare(_PreHistoryDay, now) < 0)
            {
                lock (_WaveCtrlsLock)
                {
                    foreach (var item in _WaveControls)
                    {
                            item.Value.Stop();
                        }
                    _WaveControls.Clear();
                }

                lock (_LeakIoobject)
                {
                    foreach (var item in _LeakIo)
                    {
                        item.Value.Flush();
                        item.Value.Close();
                    }
                    _LeakIo.Clear();
                }

                _PreHistoryDay = now;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 保存负压波泄漏报警信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="FileName"></param>
        public void SaveAlarmToFile(string key, string value, string FileName)
        {
            //时间判断
            DateJudge();

            if (value != null) 
            {
                string rootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
                if (!Directory.Exists(rootDirectory))
                {
                    Directory.CreateDirectory(rootDirectory);
                }

                string day = DateTime.Now.ToString("yyyy-MM-dd");

                string dayDirectory = Path.Combine(rootDirectory, day);
                if (!Directory.Exists(dayDirectory))
                {
                    Directory.CreateDirectory(dayDirectory);
                }

                lock (_LeakIoobject)
                {
                    string filepath = Path.Combine(dayDirectory, key + ".dat");
                    StreamWriter sw = null;
                    if (_LeakIo.Keys.Contains(key))
                    {
                        sw = _LeakIo[key];
                    }
                    else 
                    {
                        sw = new StreamWriter(filepath,true);
                        _LeakIo.Add(key,sw);
                    }

                    sw.Write(value);
                }
            }
        }

        /// <summary>
        /// 将数据保存为本地波形文件
        /// </summary>
        public void SaveDateToWaveFile(string key, Int16[] WaveData,string FileName)
        {
            //时间判断
            DateJudge();

            if (WaveData != null)
            {
                string rootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
                if (!Directory.Exists(rootDirectory))
                {
                    Directory.CreateDirectory(rootDirectory);
                }

                string day = DateTime.Now.ToString("yyyy-MM-dd");

                string dayDirectory = Path.Combine(rootDirectory, day);
                if (!Directory.Exists(dayDirectory))
                {
                    Directory.CreateDirectory(dayDirectory);
                }

                lock (_WaveCtrlsLock)
                {
                    foreach (var item in WaveData)
                    {
                        string filepath = Path.Combine(dayDirectory, key + ".wav");

                        WaveControl fs = null;

                        if (_WaveControls.Keys.Contains(key))
                        {
                            fs = _WaveControls[key];
                        }
                        else
                        {
                            fs = new WaveControl(filepath);

                            _WaveControls.Add(key, fs);

                        }

                        //写入到文件流
                        fs.Write(WaveData);

                    }

                }

            }
        }

    }
}
