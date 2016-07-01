using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LogHelper;

namespace ACUServer
{
    public class UserClientManager
    {
        private List<DolPoint> _DolPointsCache;
        /// <summary>
        /// 最新接收到的DOL系统数据缓存，用来更新用户显示
        /// </summary>
        public List<DolPoint> DolPointsCache
        {
            get
            {
                return _DolPointsCache;
            }
            set
            {
                _DolPointsCache = value;
            }
        }

        /// <summary>
        /// 对象锁
        /// </summary>
        private object _lock = new object();

        /// <summary>
        /// 连接的客户端集合
        /// </summary>
        private ObservableCollection<UserClient> _Clients = new ObservableCollection<UserClient>();
        internal ObservableCollection<UserClient> Clients
        {
            get 
            {
                return _Clients;
            }
            private set 
            {
                _Clients = value;
            }
        }

        private UserClientManager() { }

        private static UserClientManager _Instance = new UserClientManager();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static UserClientManager Instance 
        {
            get 
            {
                return _Instance;
            }
        }

        #region 公开方法
        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="client"></param>
        internal bool Add(UserClient client)
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(client.IP))
                {
                    MyLog.Log.Error("连接的客户端ip为空，拒绝对此客户端进行服务");
                    return false;
                }
                _Clients.Add(client);
                return true;
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        internal UserClient Exist(string ip)
        {
            lock (_lock)
            {
                foreach (var item in _Clients)
                {
                    if (item.IP.Equals(ip))
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 移除客户端
        /// </summary>
        /// <param name="client"></param>
        internal void Remove(UserClient client)
        {
            lock (_lock)
            {
                _Clients.Remove(client);
            }
        }

       
        #endregion

    }
}
