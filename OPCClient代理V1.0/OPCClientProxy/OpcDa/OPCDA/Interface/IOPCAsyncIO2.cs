namespace OPCDA.Interface
{
    using OPCDA;
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("39c13a71-011e-11d0-9675-0020afd8adb3"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCAsyncIO2
    {
        [PreserveSig]
        int Read([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In] int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors);
        [PreserveSig]
        int Write([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] object[] pItemValues, [In] int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors);
        [PreserveSig]
        int Refresh2([In, MarshalAs(UnmanagedType.U4)] OPCDATASOURCE dwSource, [In] int dwTransactionID, out int pdwCancelID);
        [PreserveSig]
        int Cancel2([In] int dwCancelID);
        [PreserveSig]
        int SetEnable([In, MarshalAs(UnmanagedType.Bool)] bool bEnable);
        [PreserveSig]
        int GetEnable([MarshalAs(UnmanagedType.Bool)] out bool pbEnable);
    }
}

