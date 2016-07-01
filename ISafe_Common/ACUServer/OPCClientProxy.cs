using ISafe_Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace ACUServer
{

    public class OPCClientProxy
    {
        private OperationContext _Context = null;
        /// <summary>
        /// 提供对服务方法的执行上下文访问权限
        /// </summary>
        public OperationContext Context
        {
            get
            {
                return _Context;
            }
            set
            {
                _Context = value;
            }
        }

        private OPCPxoryModel _OPCModel = new OPCPxoryModel();
        /// <summary>
        /// 代理模型
        /// </summary>
        public OPCPxoryModel OPCModel
        {
            get
            {
                return _OPCModel;
            }
            set
            {
                _OPCModel = value;
            }
        }

       

    }
}
