namespace OPCDA.NET
{
    using OPCDA;
    using System;
    using System.Runtime.InteropServices;

    public class Subscription
    {
        public int clientHandle;
        public object control;
        public int controlIndex;
        public int controlIndex2;
        public string controlProperty;
        internal DataBindFormat format;
        public ItemDef idef;
        public string ItemName;
        public OpcDataBind parent;

        public int Read(OPCDATASOURCE src, out OPCItemState val)
        {
            return this.parent.refreshGrp.Read(src, this.idef, out val);
        }

        public int Remove()
        {
            int num = this.parent.refreshGrp.Remove(this.idef);
            this.parent.subscriptions.Remove(this);
            return num;
        }

        public int Write(object val)
        {
            return this.parent.refreshGrp.Write(this.idef, val);
        }

        public int Error
        {
            get
            {
                return this.idef.OpcIRslt.Error;
            }
        }

        public DataBindFormat Format
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format = value;
            }
        }

        public ItemDef ItemInfo
        {
            get
            {
                return this.idef;
            }
        }

        public string Quality
        {
            get
            {
                return ((OPC_QUALITY_STATUS) this.idef.OpcIRslt.Quality).ToString().ToLower();
            }
        }

        public OPC_QUALITY_STATUS QualityCode
        {
            get
            {
                return (OPC_QUALITY_STATUS) this.idef.OpcIRslt.Quality;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return DateTime.FromFileTime(this.idef.OpcIRslt.TimeStamp);
            }
        }

        public object Value
        {
            get
            {
                return this.idef.OpcIRslt.DataValue;
            }
        }
    }
}

