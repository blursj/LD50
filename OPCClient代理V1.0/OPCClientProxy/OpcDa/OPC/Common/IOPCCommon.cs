namespace OPC.Common
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("F31DFDE2-07B6-11d2-B2D8-0060083BA1FB"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCCommon
    {
        [PreserveSig]
        int SetLocaleID([In] int dwLcid);
        [PreserveSig]
        int GetLocaleID(out int pdwLcid);
        [PreserveSig]
        int QueryAvailableLocaleIDs(out int pdwCount, out IntPtr pdwLcid);
        [PreserveSig]
        int GetErrorString([In] int dwError, [MarshalAs(UnmanagedType.LPWStr)] out string ppString);
        [PreserveSig]
        int SetClientName([In, MarshalAs(UnmanagedType.LPWStr)] string szName);
    }
}

