namespace OPC
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("13486D50-4821-11D2-A494-3CB306C10000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
    internal interface IOPCServerList
    {
        int EnumClassesOfCategories([In] int cImplemented, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] catidImpl, [In] int cRequired, [In] Guid[] catidReq, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
        int GetClassDetails([In] ref Guid clsid, [MarshalAs(UnmanagedType.LPWStr)] out string ppszProgID, [MarshalAs(UnmanagedType.LPWStr)] out string ppszUserType);
        int CLSIDFromProgID([In, MarshalAs(UnmanagedType.LPWStr)] string szProgId, out Guid clsid);
    }
}

