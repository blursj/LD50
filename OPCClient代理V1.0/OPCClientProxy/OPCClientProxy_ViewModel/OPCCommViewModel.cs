using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCDA.NET;
using OPC;
using OPCDA;
using System.Runtime.InteropServices;

namespace OPCClientProxy_ViewModel
{
    /// <summary>
    /// 自定义的结构
    /// 用来保存读取到的数据（值，品质，时间戳）
    /// </summary>
    public struct READRESULT
    {
        public string readValue;
        public string readQuality;
        public string readTimeStamp;
    }

    // 定义用于返回发生变化的项的值和其对应的客户句柄
    public delegate void DataChange(READRESULT[] values, string[] itemsID);

    public class OPCCommViewModel
    {
        private OpcServer _myOPCServer;//server object
        private SyncIOGroup _readWriteGroup;//group object
        private ItemDef _itemData;
        private BrowseTree _itemTree;
        private READRESULT _readRes;
        //供回调函数使用
        private RefreshGroup _asyncRefrGroup;
        private DataChangeEventHandler _dch;
        private READRESULT[] _dataChangeRes; //用来保存变量值发生变化时服务器的回调函数得到的变量数据
        private DataChange _dtChange;

        public READRESULT[] DataChangeRes
        {
            get { return _dataChangeRes; }
            set { _dataChangeRes = value; }
        }

        public BrowseTree ItemTree
        {
            get { return _itemTree; }
            set { _itemTree = value; }
        }

        public ItemDef ItemData
        {
            get { return _itemData; }
            set { _itemData = value; }
        }

        internal READRESULT ReadRes
        {
            get { return _readRes; }
            set { _readRes = value; }
        }

        public DataChange DtChange
        {
            get
            {
                return _dtChange;
            }
            set
            {
                _dtChange = value;
            }
        }

        public OPCCommViewModel()
        {

        }



        #region 获得可访问的服务器名字列表
        /// <summary>
        ///  获得服务器名字列表
        /// </summary>
        /// <returns>返回包含服务器名字的字符串数组</returns>
        public string[] GetSerList(string host)
        {
            OpcServerBrowser serList = new OpcServerBrowser(new OPC.Common.Host(host));
            string[] serNames;
            serList.GetServerList(out serNames);

            return serNames;
        }
        #endregion

        #region 浏览服务器地址空间
        /// <summary>
        /// 浏览服务器地址空间
        /// </summary>
        /// <returns>成功返回0， 失败返回-1</returns>
        public int GetItemList()
        {
            _itemTree = new BrowseTree(_myOPCServer);

            int res = _itemTree.CreateTree();		// Browse server from root
            if (HRESULTS.Succeeded(res))
            {
                return 0;
            }

            return -1;
        }
        #endregion

        #region 连接服务器

        /// <summary>
        /// 判断某个OPServer是否连接，0-连接；-1-未连接
        /// </summary>
        /// <returns></returns>
        public int JudgeLink()
        {
            if (_myOPCServer != null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="serName">服务器的名字</param>
        /// <param name="serIP">服务器所在机器的IP</param>
        /// <returns>成功返回0，失败返回-1</returns>     
        public int Conn2Server(string serName, string serIP, int refresh)
        {
            int res;
            try
            {
                _myOPCServer = new OpcServer();

                res = _myOPCServer.Connect(serIP, serName);
                if (HRESULTS.Failed(res))
                {
                    _myOPCServer = null;
                    return -1;
                }

                _readWriteGroup = new SyncIOGroup(_myOPCServer);
                //供回调函数刷新变量值使用
                _dch = new DataChangeEventHandler(DataChangeHandler);
                if (refresh <= 0)
                {
                    refresh = 100;
                }
                _asyncRefrGroup = new RefreshGroup(_myOPCServer, _dch, refresh);
            }
            catch
            {
                _myOPCServer = null;
                return -1;
            }
            return 0;
        }
        #endregion

        #region 添加项
        /// <summary>
        /// 从组中添加一个变量
        /// </summary>
        /// <param name="itemId">变量ID</param>
        /// <returns>成功返回0， 失败返回-1</returns>
        int AddItem(string itemId)
        {
            _itemData = _readWriteGroup.Item(itemId);
            if (_itemData == null)
            {
                _readWriteGroup.Add(itemId);
                _itemData = _readWriteGroup.Item(itemId);
                if (_itemData == null)
                {
                    return -1;
                }
            }

            return 0;
        }

        /// <summary>
        /// 从组中删除一个变量
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        int RemoveItem(string itemId) 
        {
            _itemData = _readWriteGroup.Item(itemId);
            if (_itemData != null)
            {
                _readWriteGroup.Remove(itemId);
                _itemData = _readWriteGroup.Item(itemId);
                if (_itemData != null)
                {
                    return -1;
                }
            }

            return 0;
        }

        #endregion

        #region 读取一个变量
        /// <summary>
        /// 从服务器上读取数据,读取的数据保存在READRESULT结构中
        /// </summary>
        /// <param name="num">变量ID</param>
        /// <returns>成功返回0， 失败返回-1</returns>
        public int Read(string itemId)
        {
            if (AddItem(itemId) == 0)
            {
                OPCItemState Rslt;
                OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
                int rtc = _readWriteGroup.Read(dsrc, _itemData, out Rslt);

                if (HRESULTS.Succeeded(rtc))		// read from OPC server successful
                {
                    if (Rslt != null)
                    {
                        if (HRESULTS.Succeeded(Rslt.Error))	// item read successful
                        {
                            _readRes.readValue = Rslt.DataValue.ToString();
                            _readRes.readQuality = _readWriteGroup.GetQualityString(Rslt.Quality);
                            DateTime dt = DateTime.FromFileTime(Rslt.TimeStamp);
                            _readRes.readTimeStamp = dt.ToString();

                            return 0;
                        }
                        return -1;
                    }
                    return -1;
                }
                else
                {
                    return -1;
                }
            }
            return -1;
        }
        #endregion

        #region 修改一个变量
        /// <summary>
        /// 修改一个变量
        /// </summary>
        /// <param name="itemId">变量ID</param>
        /// <param name="value">变量的值</param>
        /// <returns>返回写操作结果的字符串</returns>
        public string Write(string itemId, object value)
        {
            if (AddItem(itemId) == 0)
            {
                int res = _readWriteGroup.Write(_itemData, value);

                return _readWriteGroup.GetErrorString(res);
            }
            return null;
        }
        #endregion

        #region 向RefreshGroup组中添加一个变量
        /// <summary>
        /// 向RefreshGroup组中添加一个变量
        /// </summary>
        /// <param name="itemId">变量ID</param>
        /// <returns>成功返回0，失败返回-1</returns>
        public int Add2RefrGroup2(string itemId, ref VarEnum DataType)
        {
            DataType = VarEnum.VT_EMPTY;
            if (AddItem(itemId) == 0)
            {
                DataType = _itemData.OpcIInfo.CanonicalDataType;
                int res = _asyncRefrGroup.Add(_itemData.OpcIDef.ItemID);
                if (HRESULTS.Failed(res))
                {
                    return -1;
                }
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// 添加组变量
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int Add2RefrGroup(string itemId)
        {
            if (AddItem(itemId) == 0)
            {
                int res = _asyncRefrGroup.Add(_itemData.OpcIDef.ItemID);
                if (HRESULTS.Failed(res))
                {
                    return -1;
                }
                return 0;
            }
            return -1;
        }

        public int Remove2RefrGroup(string itemId) 
        {
            if (RemoveItem(itemId) == 0)
            {
                _asyncRefrGroup.Remove(_itemData.OpcIDef.ItemID);
                return 0;
            }
            return -1;
        }

        #endregion

        #region 断开同服务器的连接
        /// <summary>
        /// 断开同服务器的连接
        /// </summary>
        /// <returns>成功返回0，失败返回-1</returns>
        public int DisConn()
        {
            try
            {
                if (_myOPCServer != null)
                {
                    _asyncRefrGroup.Dispose();
                    _readWriteGroup.Dispose();
                    _myOPCServer.Disconnect();
                    _myOPCServer = null;
                }

                return 0;
            }
            catch
            {
                return -1;
            }

        }
        #endregion

        #region 回调函数
        /// <summary>
        /// 回调函数，当RefreshGroup组里的数据发生变化时由服务器自动调用该函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataChangeHandler(object sender, DataChangeEventArgs e)
        {
            int count = e.sts.Length;
            string[] itemsID = new string[count];
            _dataChangeRes = new READRESULT[count];

            for (int i = 0; i < count; ++i)
            {
                int hnd = e.sts[i].HandleClient;
                ItemDef item = _asyncRefrGroup.FindClientHandle(hnd);
                if (item != null)
                {
                    //保存变量的ItemID
                    itemsID[i] = item.OpcIDef.ItemID;
                }

                object val = e.sts[i].DataValue;
                string qt = _asyncRefrGroup.GetQualityString(e.sts[i].Quality);
                DateTime dt = DateTime.FromFileTime(e.sts[i].TimeStamp);
                //将变量的值保存在READRESULT结构中
                if (val != null)
                {
                    _dataChangeRes[i].readValue = val.ToString();
                }
                else { _dataChangeRes[i].readValue = "0"; }
                _dataChangeRes[i].readQuality = qt;
                _dataChangeRes[i].readTimeStamp = dt.ToString();
            }

            _dtChange(_dataChangeRes, itemsID);
        }
        #endregion
    }

}
