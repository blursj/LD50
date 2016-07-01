using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ACUServer
{
    /// <summary>
    /// OPCClient代理端接口
    /// </summary>
    [ServiceContract(CallbackContract=typeof(IOPCCallBack))]
    public interface IOPCClientProxy
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool Login(string GUID);

        /// <summary>
        /// 心跳
        /// </summary>
        [OperationContract(IsOneWay=true)]
        void BeatHeart();

        /// <summary>
        /// 向服务器发送PLC采集的压力数据
        /// </summary>
        [OperationContract]
        void SendPLCData(string GUID,List<PrePoint> Prepoints);

        /// <summary>
        /// 向服务器发送DOLPHIN点数据
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="DolPoints"></param>
        [OperationContract]
        void SendDolPhinData(string GUID,List<DolPoint> DolPoints);

    }
     
    [ServiceContract]
    public interface IOPCCallBack
    {
        /// <summary>
        /// 设置DOL点变量值
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        bool SetDolPointValue(string pointID,object value);
    }
}
