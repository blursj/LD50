namespace OPCDA.Interface
{
    using OPCDA;
    using System;
    using System.Runtime.InteropServices;

    [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("39c13a4d-011e-11d0-9675-0020afd8adb3")]
    internal interface IOPCServer
    {
        [PreserveSig]
        int AddGroup([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In, MarshalAs(UnmanagedType.Bool)] bool bActive, [In] int dwRequestedUpdateRate, [In] int hClientGroup, [In] IntPtr pTimeBias, [In] IntPtr pPercentDeadband, [In] int dwLCID, out int phServerGroup, out int pRevisedUpdateRate, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
        [PreserveSig]
        int GetErrorString([In] int dwError, [In] int dwLocale, [MarshalAs(UnmanagedType.LPWStr)] out string ppString);
        void GetGroupByName([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
        [PreserveSig]
        int GetStatus([MarshalAs(UnmanagedType.LPStruct)] out SERVERSTATUS ppServerStatus);
        [PreserveSig]
        int RemoveGroup([In] int hServerGroup, [In, MarshalAs(UnmanagedType.Bool)] bool bForce);
        [PreserveSig]
        int CreateGroupEnumerator([In] int dwScope, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}

