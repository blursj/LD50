/*

* All rights reserved.
* 
* 文件名称：EnumType.cs
* 文件标识：见配置管理计划书
* 摘    要：简要描述本文件的内容
* 
* 当前版本：1.0
* 作    者：吴士杰
* 完成日期：2015/2/5 9:14:26
*
* 取代版本：
* 原作者  ：
* 完成日期：
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACUServer
{
    /// <Summary>
    /// 作者:吴士杰
    /// 创建时间:2015/2/5 9:14:26
    /// 保存的数据的类型
    /// <Summary>
    public enum EnumType
    {
        //infrasonic,//次声波
        //sgps,//GPS
        //pressure,//压力
        //status//传感器

        TwoSignal = 0,      //一级信号
        oneSignal = 1,      //二级信号
        FiltSignal = 2,     //过滤后信号
        OrigSignal = 3,     //原始信号
        GPSSignal = 4       //GPS信号
    }

    /// <summary>
    /// 通道名称
    /// </summary>
    public enum Aisle
    {
        A0,
        A1,
        A2,
        A3,
        A4,
        A5,
        A6,
        A7,
        A8,
        A9,
        A10,
        A11,
        A12,
        A13,
        A14,
        A15,
    }

    //ACU波形查看请求动作 0-整个ACU所有通道 1-任意单个通道（并行） 2-任意单个通道（叠加）
    public enum RequestType
    {
        ALLChannels = 0,
        PChannel = 1,
        DChannel = 2,
    }

    public enum ACUState
    {
        stop = 0,   //停止状态
        start = 1,  //运行状态
    }
}
