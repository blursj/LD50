namespace OPCDA.NET
{
    using OPC;
    using System;
    using System.Runtime.InteropServices;
    using System.Xml.Serialization;

    [Serializable]
    public class OPCItemDef
    {
        public string AccessPath;
        public bool Active;
        [XmlArrayItem(Type=typeof(byte))]
        public byte[] Blob;
        public int HandleClient;
        public string ItemID;
        public VarEnum RequestedDataType;

        public OPCItemDef()
        {
            this.AccessPath = "";
            this.Blob = null;
        }

        public OPCItemDef(string id, bool activ, int hclt, VarEnum vt)
        {
            this.AccessPath = "";
            this.Blob = null;
            this.ItemID = id;
            this.Active = activ;
            this.HandleClient = hclt;
            this.RequestedDataType = vt;
        }

        public OPCItemDef(string id, bool activ, int hclt, Type st)
        {
            this.AccessPath = "";
            this.Blob = null;
            this.ItemID = id;
            this.Active = activ;
            this.HandleClient = hclt;
            this.RequestedDataType = types.ConvertToVarType(st);
        }

        [XmlIgnore]
        public Type RequestedType
        {
            get
            {
                return types.ConvertToSystemType(this.RequestedDataType);
            }
            set
            {
                this.RequestedDataType = types.ConvertToVarType(value);
            }
        }
    }
}

