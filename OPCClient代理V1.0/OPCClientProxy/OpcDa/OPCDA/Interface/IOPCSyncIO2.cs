namespace OPCDA.Interface
{
    using OPCDA;
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("730F5F0F-55B1-4c81-9E18-FF8A0904E1FA"), ComVisible(true)]
    internal interface IOPCSyncIO2
    {
        [PreserveSig]
        int Read([In, MarshalAs(UnmanagedType.U4)] OPCDATASOURCE dwSource, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);
        [PreserveSig]
        int Write([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] object[] pItemValues, out IntPtr ppErrors);
        [PreserveSig]
        int ReadMaxAge([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] pdwMaxAge, out IntPtr ppValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors);
        [PreserveSig]
        int WriteVQT([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In] IntPtr pItemValues, out IntPtr ppErrors);
    }
}

