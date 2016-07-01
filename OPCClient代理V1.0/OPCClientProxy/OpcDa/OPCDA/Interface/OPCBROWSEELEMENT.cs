namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=2)]
    internal class OPCBROWSEELEMENT
    {
        public string szName;
        public string szItemID;
        public int dwFlagValue;
        public int dwReserved;
        public OPCITEMPROPERTIES ItemProperties;
    }
}

