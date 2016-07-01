namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("39c13a4e-011e-11d0-9675-0020afd8adb3"), ComVisible(true)]
    internal interface IOPCServerPublicGroups
    {
        [PreserveSig]
        int GetPublicGroupByName([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
        [PreserveSig]
        int RemovePublicGroup([In] int hServerGroup, [In, MarshalAs(UnmanagedType.Bool)] bool bForce);
    }
}

