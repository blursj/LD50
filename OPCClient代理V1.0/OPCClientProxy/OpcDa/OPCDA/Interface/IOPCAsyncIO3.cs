namespace OPCDA.Interface
{
    using OPCDA;
    using System;
    using System.Runtime.InteropServices;

    [Guid("0967B97B-36EF-423e-B6F8-6BFF1E40D39D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCAsyncIO3
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
        [PreserveSig]
        int ReadMaxAge([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] pdwMaxAge, [In] int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors);
        [PreserveSig]
        int WriteVQT([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In] IntPtr pItemValues, [In] int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors);
        [PreserveSig]
        int RefreshMaxAge([In] int dwMaxAge, [In] int dwTransactionID, out int pdwCancelID);
    }
}

