using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ACUServer
{
    [ServiceContract]
    public interface IUserCallBack
    {
        /*----------------------接口规定--------------------------------*/
        /// <summary>
        /// 回调iuser客户端通知配置文件发生改变
        /// </summary>
        /// <param name="gra"></param>
        [OperationContract]
        void GraphicChanged(Graphic gra);

        /// <summary>
        /// 负压波泄漏回调
        /// </summary>
        /// <param name="leak"></param>
        [OperationContract]
        void PreLeakCallBack(PressureLeak leak);
      
        /// <summary>
        /// 服务器向客户端回调OPC代理连接状态
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="islink"></param>
        [OperationContract]
        void OPCProxyLinkCallBack(string Guid, bool islink);

        /// <summary>
        /// Key类型为siteid-pressureid
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Datasource"></param>
        [OperationContract]
        void PressureDateCallBack(string Key, double[] Datasource);

        /// <summary>
        /// 服务器向客户端回调PLC传感器的连接状态
        /// </summary>
        /// <param name="PLCSign"></param>
        /// <param name="link"></param>
        [OperationContract]
        void PLCPreLinkingCallBack(Dictionary<string,bool> PreSensorLinks);

        /// <summary>
        /// DolPhin系统产生的定位信息
        /// </summary>
        /// <param name="location"></param>
        [OperationContract]
        void DolLocationCallBack(DolLocation location);

        /// <summary>
        /// 定位信息回调
        /// </summary>
        /// <param name="location"></param>
        [OperationContract]
        void LocationCallBack(Location location);

        /// <summary>
        /// 负压波的Mask波形发送
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="MaskData"></param>
        [OperationContract]
        void PreMaskCallBack(string Key,double[] MaskData);

        /// <summary>
        /// 负压波算法输出检测门限值发送
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="ThreshData"></param>
        [OperationContract]
        void PreThreshCallBack(string Key, double[] ThreshData);

        /// <summary>
        /// 回调DOLPHIN系统数据
        /// </summary>
        /// <param name="gra"></param>
        [OperationContract]
        void CallBackDOLPHINData(List<DolPoint> DolPoints);

        /*----------------------接口规定--------------------------------*/
    }
}
