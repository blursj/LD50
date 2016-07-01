namespace OPCDA.NET
{
    using OPCDA;
    using System;

    public class DataBindEventData
    {
        public int error;
        public ItemDef itemInfo;
        public OPC_QUALITY_STATUS quality;
        public DateTime timestamp;
        public object val;
    }
}

