using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace CTAPIHelper
{
    public class CTAPIHelper
    {
        private static MyTask _task = new MyTask();

        /// <summary>
        /// 获取指定参数的值
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static String GetValue(string tag)
        {
            if (_task == null)
            {
                _task = new MyTask();
            }

            return _task.GetValue(tag);            
        }

        /// <summary>
        /// 关闭scada链接
        /// </summary>
        public static void Close()
        {
            _task.Close();
            _task = null;
        }
    }
}
