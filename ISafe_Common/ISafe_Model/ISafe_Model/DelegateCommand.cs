/*
* Copyright (c) 2014,北京昆仑卓越信息技术有限公司
* All rights reserved.
* 
* 文件名称：DelegateCommand.cs
* 文件标识：见配置管理计划书
* 摘    要：DelegateCommand结构
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISafe_Model
{
    public class DelegateCommand : ICommand
    {
        #region 私有变量
        private Action<object> _Action = null;//要执行的方法
        private Func<object, bool> _CanExecute = null;//是否可以执行判断逻辑
        private bool _CanExecuteFlag = false;//命令是否可以执行
        private bool _State = false;//命令是否在执行过程中
        #endregion

        #region 构造方法
        public DelegateCommand(Action<object> action)
        {
            _Action = action;
        }

        public DelegateCommand(Action<object> action, Func<object, bool> canExecute)
        {
            _Action = action;
            _CanExecute = canExecute;
        }
        #endregion

        #region 实现ICommand接口
        /// <summary>
        /// 是否可执行改变事件
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 能否执行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_CanExecute == null)
            {
                return true;
            }
            bool flag = _CanExecute(parameter);
            if (flag != _CanExecuteFlag)
            {
                _CanExecuteFlag = flag;

                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }
            return _CanExecuteFlag;
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (_State)//防止两次连续执行某个动作
            {
                return;
            }
            _State = true;
            _Action.Invoke(parameter);
            _State = false;
        }
        #endregion
    }
}
