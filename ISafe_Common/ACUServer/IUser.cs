using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ACUServer
{
    [ServiceContract(CallbackContract = typeof(IUserCallBack))]
    public interface IUser
    {
        /*----------------------接口规定--------------------------------*/
        /// <summary>
        /// 心跳
        /// </summary>
        [OperationContract]
        void BeatHeart();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [OperationContract]
        void Login();

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Graphic GetGraphic();


        ///<summary>
        /// 重置服务端配置
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void SetGraphic(Graphic gra);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        bool SetDolPointValue(string pointID, object value);
        /*----------------------接口规定--------------------------------*/

        /// <summary>
        /// 重置某个压力传感器的检测门限值
        /// </summary>
        [OperationContract]
        bool ResetPreSensorThresh(int PreSensorIndex, float threshMax, float threshMin);

    }
}
