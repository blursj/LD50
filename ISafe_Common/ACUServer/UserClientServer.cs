using ISafe_Algorithm;
using LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;

namespace ACUServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class UserClientServer : IUser
    {

        private UserClient _client = null;//对象对应的客户端

        public UserClientServer()
        {
            OperationContext.Current.Channel.Closed += Channel_Closed;
            _client = new UserClient(OperationContext.Current);
        }

        private void Channel_Closed(object sender, EventArgs e)
        {
            if (_client != null)
            {
                UserClientManager.Instance.Remove(_client);
                _client.Dispose();
                MyLog.Log.Info(string.Format("展示端IP:{0},于服务器时间:{1},断开连接", _client.IP, DateTime.Now.ToString()));
            }
        }

        public void BeatHeart()
        {
            //do nothing 
        }

        /// <summary>
        /// 客户端登录
        /// </summary>
        public void Login()
        {
            var endpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            _client.IP = endpoint.Address;
            if (UserClientManager.Instance.Add(_client))
            {
                MyLog.Log.Info(string.Format("展示端IP:{0},于服务器时间:{1},连接至服务", _client.IP, DateTime.Now.ToString()));

                //登录完成之后，向客户端发送DOLPHIN系统数据
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    Thread.Sleep(1000);
                    _client.Context.GetCallbackChannel<IUserCallBack>().CallBackDOLPHINData(UserClientManager.Instance.DolPointsCache);

                },null);
            }

        }

        public Graphic GetGraphic()
        {
            var endpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            var find_user = UserClientManager.Instance.Exist(endpoint.Address);
            if (find_user == null)
            {
                return null;
            }
            else
            {
                //将所有ACU配置信息返回
                return XmlHelper.Graphic;
            }
        }



        /// <summary>
        /// 重置服务端配置
        /// </summary>
        /// <param name="gra"></param>
        public void SetGraphic(Graphic gra)
        {
            if (gra == null)
            {
                return;
            }

            //重新修改配置信息
            XmlHelper.Save(gra);

            var endpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            //向所有连接的展示端发送配置信息已发生改变
            try
            {
                for (int USERClientNum = 0; USERClientNum < UserClientManager.Instance.Clients.Count; USERClientNum++)
                {
                    if (!UserClientManager.Instance.Clients[USERClientNum].IP.Equals(endpoint.Address))
                    {
                        UserClientManager.Instance.Clients[USERClientNum].Context.GetCallbackChannel<IUserCallBack>().GraphicChanged(XmlHelper.Graphic);
                    }

                }
            }
            catch
            {
                //填写日志文件信息
            }

        }

        /// <summary>
        /// 重置压力检测门限值
        /// </summary>
        /// <param name="PreSensorIndex"></param>
        /// <param name="threshMax"></param>
        /// <param name="threshMin"></param>
        public bool ResetPreSensorThresh(int PreSensorIndex, float threshMax, float threshMin)
        {
            try
            {
                if(!LD50AlarmsManager.Instance.IsLD50Initialized)
                {
                    return false;
                }
                int result = Algorithm.UpdatePressureThresh(PreSensorIndex, threshMax, threshMin);
                if (result == 1)
                {
                    MyLog.Log.Info(string.Format("重置编号为{0}的的压力传感器检测门限成功！", PreSensorIndex));
                    return true;
                }
                else
                {
                    MyLog.Log.Info(string.Format("重置编号为{0}的的压力传感器检测门限失败！", PreSensorIndex));
                    return false;
                }
            }
            catch
            {
                MyLog.Log.Error(string.Format("重置编号为{0}的的压力传感器检测门限时发生异常！", PreSensorIndex));
                return false;
            }
        }

        /// <summary>
        /// 修改DOL系统变量点值
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetDolPointValue(string pointID, object value)
        {
            bool result = false;
            try
            {
                var find_opcclient = OPCClientProxyManager.Instance.GetOPCClientProxyByGUID("OPCProxy_DOLPHIN");
                if (find_opcclient != null)
                {
                    result = find_opcclient.Context.GetCallbackChannel<IOPCCallBack>().SetDolPointValue(pointID, value);
                }

            }
            catch
            {
                return result;
            }
            return result;
        }
    }
}
