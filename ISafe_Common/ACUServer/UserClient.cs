using LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ACUServer
{
    public class UserClient
    {

        public UserClient(OperationContext context)
        {
            _CallBack = context;
        }

        public string IP
        {
            get;
            set;
        }

        private OperationContext _CallBack;//回调
        public OperationContext Context
        {
            get
            {
                return _CallBack;
            }
        }

        internal void Dispose()
        {
            _CallBack = null;
        }

    }
}
