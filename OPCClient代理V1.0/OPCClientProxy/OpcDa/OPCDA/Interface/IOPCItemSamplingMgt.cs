namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3E22D313-F08B-41a5-86C8-95E95CB49FFC"), ComVisible(true)]
    internal interface IOPCItemSamplingMgt
    {
        [PreserveSig]
        int SetItemSamplingRate([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] pdwRequestedSamplingRate, out IntPtr ppdwRevisedSamplingRate, out IntPtr ppErrors);
        [PreserveSig]
        int GetItemSamplingRate([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, out IntPtr ppdwSamplingRate, out IntPtr ppErrors);
        [PreserveSig]
        int ClearItemSamplingRate([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, out IntPtr ppErrors);
        [PreserveSig]
        int SetItemBufferEnable([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Bool, SizeParamIndex=0)] bool[] pbEnable, out IntPtr ppErrors);
        [PreserveSig]
        int GetItemBufferEnable([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, out IntPtr ppbEnable, out IntPtr ppErrors);
    }
}

