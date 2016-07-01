using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISafe_Model
{
    public enum ChannelTypeEnum 
    {
        TwoSignal = 0,      //一级信号
        oneSignal = 1,      //二级信号
        FiltSignal = 2,     //过滤后信号
        OrigSignal = 3,     //原始信号
        GPSSignal = 4       //GPS信号
    }

    public enum ACUState 
    {
        stop = 0,         //停止状态
        run = 1           //运行状态
    }
}
