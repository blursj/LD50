/*
* Copyright (c) 2015,北京昆仑卓越信息技术有限公司
* All rights reserved.
* 
* 文件名称：Log.cs
* 文件标识：见配置管理计划书
* 摘    要：简要描述本文件的内容
* 
* 当前版本：1.0
* 作    者：吴士杰
* 完成日期：2015/2/3 10:09:46
*
* 取代版本：
* 原作者  ：
* 完成日期：
*/

using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogHelper
{
    /// <Summary>
    /// 作者:吴士杰
    /// 创建时间:2013/8/21 14:46:52
    /// <Summary>
    public static class MyLog
    {
        #region 私有变量
        private static ILog _ILog = null;
        #endregion

        #region 类型构造
        /// <summary>
        /// 静态类型构造
        /// </summary>
        static MyLog()
        {
            string direct = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_config");
            string file = System.IO.Path.Combine(direct, AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "") + "Logger.config");

            //创建文件夹
            if (!System.IO.Directory.Exists(direct))
            {
                System.IO.Directory.CreateDirectory(direct);
            }
            //创建文件
            if (!System.IO.File.Exists(file))
            {
                CreateConfig(file);
            }

            XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(file));
            _ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        #endregion

        /// <summary>
        /// 日志对象
        /// </summary>
        public static ILog Log
        {
            get
            {
                return _ILog;
            }
        }

        /// <summary>
        /// 创建配置文件
        /// </summary>
        /// <param name="path"></param>
        private static void CreateConfig(string path)
        {
            using (var stream = System.IO.File.Create(path))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(@"<?xml version=""1.0""?>");
                sb.AppendLine(@"<log4net>");
                sb.AppendLine(@"<appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">");
                sb.AppendLine(@"<param name=""file"" type=""log4net.Util.PatternString"" value=""log\%appdomain%date{yyyyMMddHHmm}.log""/>");
                sb.AppendLine(@"<param name=""AppendToFile"" value=""true"" />");
                sb.AppendLine(@"<param name=""MaxSizeRollBackups"" value=""10"" />");
                sb.AppendLine(@"<param name=""MaximumFileSize"" value=""1MB"" />");
                sb.AppendLine(@"<!--Once  Size Date Composite  -->");
                sb.AppendLine(@"<param name=""RollingStyle"" value=""Size"" />");
                sb.AppendLine(@"<param name=""StaticLogFileName"" value=""true"" />");
                sb.AppendLine(@"<layout type=""log4net.Layout.PatternLayout"">");
                sb.AppendLine(@"<param name=""ConversionPattern"" value=""%date [%thread] %-5level - %m%n"" />");
                sb.AppendLine(@"</layout>");
                sb.AppendLine(@"</appender>");
                sb.AppendLine(@"<appender name=""ConsoleAppender"" type=""log4net.Appender.ConsoleAppender"">");
                sb.AppendLine(@"<layout type=""log4net.Layout.PatternLayout"">");
                sb.AppendLine(@"<conversionPattern value=""%d [%t] %p - %m%n"" /> ");
                sb.AppendLine(@"</layout>");
                sb.AppendLine(@"</appender>");
                sb.AppendLine(@"<root>");
                sb.AppendLine(@"<!--级别FATAL > ERROR > WARN > INFO > DEBUG-->");
                sb.AppendLine(@"<level value=""DEBUG""/>");
                sb.AppendLine(@"<appender-ref ref=""ConsoleAppender""/>");
                sb.AppendLine(@"<appender-ref ref=""RollingLogFileAppender""/>");
                sb.AppendLine(@"</root>");
                sb.AppendLine(@"</log4net>");
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
        }
    }
}
