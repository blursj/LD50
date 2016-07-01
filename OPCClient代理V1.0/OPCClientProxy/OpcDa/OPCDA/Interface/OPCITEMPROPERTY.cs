namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=2)]
    public class OPCITEMPROPERTY
    {
        public short vtDataType;
        public short wReserved;
        public int dwPropertyID;
        public string szItemID;
        public string szDescription;
        public object vValue;
        public int hrErrorID;
        public int dwReserved;
    }
}

