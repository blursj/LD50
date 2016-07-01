namespace OPCDA
{
    using System;

    public class SrvStatus
    {
        public int BandWidth;
        public short BuildNumber;
        public DateTime CurrentTime;
        public int GroupCount;
        public DateTime LastUpdateTime;
        public short MajorVersion;
        public short MinorVersion;
        public OpcServerState ServerState;
        public DateTime StartTime;
        public string VendorInfo;
    }
}

