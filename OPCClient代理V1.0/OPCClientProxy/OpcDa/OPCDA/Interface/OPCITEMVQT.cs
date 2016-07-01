namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, CharSet=CharSet.Unicode, Pack=2)]
    internal class OPCITEMVQT
    {
        [FieldOffset(0x10)]
        public bool bQualitySpecified;
        [FieldOffset(0x18)]
        public bool bTimeStampSpecified;
        [FieldOffset(0x1c)]
        public int dwReserved;
        [FieldOffset(0x20)]
        public long ftTimeStamp;
        [MarshalAs(UnmanagedType.Struct), FieldOffset(0)]
        public object vDataValue;
        [FieldOffset(20)]
        public short wQuality;
        [FieldOffset(0x16)]
        public short wReserved;
    }
}

