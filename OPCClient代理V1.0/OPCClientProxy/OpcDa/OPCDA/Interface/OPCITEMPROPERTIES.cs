namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=2)]
    public class OPCITEMPROPERTIES
    {
        public int hrErrorID;
        public int dwNumProperties;
        public IntPtr pItemProperties;
        public int dwReserved;
    }
}

