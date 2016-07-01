/*
* Copyright (c) 2014,北京昆仑卓越信息技术有限公司
* All rights reserved.
* 
* 文件名称：PropertyCallBack.cs
* 文件标识：见配置管理计划书
* 摘    要：PropertyCallBack结构
* 
* 当前版本：V1.5
* 作    者：刘建伟
* 完成日期：
*
* 取代版本：
* 原作者  ：刘建伟
* 完成日期：
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ISafe_Model
{
    public class PropertyCallBack:INotifyPropertyChanged
    {
        #region 实现INotifyPropertyChanged

        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性改变事件触发
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
