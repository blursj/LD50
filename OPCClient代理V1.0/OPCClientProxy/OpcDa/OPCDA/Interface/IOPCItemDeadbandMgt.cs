namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("5946DA93-8B39-4ec8-AB3D-AA73DF5BC86F"), ComVisible(true)]
    internal interface IOPCItemDeadbandMgt
    {
        [PreserveSig]
        int SetItemDeadband([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.R4, SizeParamIndex=0)] float[] pPercentDeadband, out IntPtr ppErrors);
        [PreserveSig]
        int GetItemDeadband([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, out IntPtr ppPercentDeadband, out IntPtr ppErrors);
        [PreserveSig]
        int ClearItemDeadband([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, out IntPtr ppErrors);
    }
}

