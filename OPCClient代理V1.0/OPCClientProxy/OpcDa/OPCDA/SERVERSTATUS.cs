namespace OPCDA
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=2)]
    public class SERVERSTATUS
    {
        public long ftStartTime;
        public long ftCurrentTime;
        public long ftLastUpdateTime;
        [MarshalAs(UnmanagedType.U4)]
        public OpcServerState eServerState;
        public int dwGroupCount;
        public int dwBandWidth;
        public short wMajorVersion;
        public short wMinorVersion;
        public short wBuildNumber;
        public short wReserved;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szVendorInfo;
    }
}

