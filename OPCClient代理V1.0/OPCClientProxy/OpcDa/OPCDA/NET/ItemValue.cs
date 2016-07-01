namespace OPCDA.NET
{
    using OPCDA;
    using System;

    [Serializable]
    public class ItemValue
    {
        public int ClientHandle = 0;
        public int Error = 0;
        public string ItemID = null;
        public int MaxAge = 0;
        public OPCQuality Quality = new OPCQuality(qualityBits.bad);
        public bool QualitySpecified = true;
        public int ServerHandle = 0;
        public DateTime Timestamp = DateTime.MinValue;
        public bool TimestampSpecified = true;
        public object Value = null;
    }
}

