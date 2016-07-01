using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_UserClient
{
    public interface iPage
    {
        /// <summary>
        /// 页面初始化
        /// </summary>
        void PageInitial();

        /// <summary>
        /// 页面推出时
        /// </summary>
        void PageQiut();
    }
}
