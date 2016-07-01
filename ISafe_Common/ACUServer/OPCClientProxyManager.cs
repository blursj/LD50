using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LogHelper;

namespace ACUServer
{
    /// <summary>
    /// 代理管理
    /// </summary>
    public class OPCClientProxyManager
    {
        private OPCClientProxyManager()
        {
            _OPCClientProxyCollection = new ObservableCollection<OPCClientProxy>();
        }

        private static OPCClientProxyManager _Instance = new OPCClientProxyManager();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static OPCClientProxyManager Instance
        {
            get
            {
                return _Instance;
            }
        }

        private ObservableCollection<OPCClientProxy> _OPCClientProxyCollection;
        public ObservableCollection<OPCClientProxy> OPCClientProxyCollection
        {
            get
            {
                return _OPCClientProxyCollection;
            }
            set
            {
                _OPCClientProxyCollection = value;
            }
        }

        /// <summary>
        /// 查找符合要求的元素
        /// </summary>
        /// <returns></returns>
        public OPCClientProxy GetOPCClientProxyByGUID(string GUID)
        {
            try
            {
                var find_opcclientproxy = _OPCClientProxyCollection.FirstOrDefault(para => para.OPCModel.GUID.Equals(GUID));
                return find_opcclientproxy;
            }
            catch
            {
                return null;
                //填写日志文件信息
            }
        }

        /// <summary>
        /// 删除OPCClient
        /// </summary>
        public void DelClientProxyByGUID(string GUID)
        {
            try
            {

                var find_opcclientproxy = _OPCClientProxyCollection.FirstOrDefault(para => para.OPCModel.GUID.Equals(GUID));
                if (find_opcclientproxy != null)
                {
                    _OPCClientProxyCollection.Remove(find_opcclientproxy);
                    find_opcclientproxy = null;
                }


            }
            catch
            {
                MyLog.Log.Error(string.Format("{0}:删除OPCClient时失败！", DateTime.Now.ToString()));
            }
        }


    }
}
