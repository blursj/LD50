namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("39c13a50-011e-11d0-9675-0020afd8adb3")]
    internal interface IOPCGroupStateMgt
    {
        [PreserveSig]
        int GetState(out int pUpdateRate, [MarshalAs(UnmanagedType.Bool)] out bool pActive, [MarshalAs(UnmanagedType.LPWStr)] out string ppName, out int pTimeBias, out float pPercentDeadband, out int pLCID, out int phClientGroup, out int phServerGroup);
        [PreserveSig]
        int SetState([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] pRequestedUpdateRate, out int pRevisedUpdateRate, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] pActive, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] pTimeBias, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] float[] pPercentDeadband, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] pLCID, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] phClientGroup);
        [PreserveSig]
        int SetName([In, MarshalAs(UnmanagedType.LPWStr)] string szName);
        [PreserveSig]
        int CloneGroup([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}

