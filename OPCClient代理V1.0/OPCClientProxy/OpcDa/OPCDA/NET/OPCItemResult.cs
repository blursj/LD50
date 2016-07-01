namespace OPCDA.NET
{
    using OPC;
    using OPCDA;
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class OPCItemResult
    {
        public OPCACCESSRIGHTS AccessRights;
        public byte[] Blob;
        public VarEnum CanonicalDataType;
        public int Error;
        public int HandleServer;

        public Type CanonicalType
        {
            get
            {
                return types.ConvertToSystemType(this.CanonicalDataType);
            }
            set
            {
                this.CanonicalDataType = types.ConvertToVarType(value);
            }
        }
    }
}

