namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("8E368666-D72E-4f78-87ED-647611C61C9F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCGroupStateMgt2
    {
        [PreserveSig]
        int GetState(out int pUpdateRate, [MarshalAs(UnmanagedType.Bool)] out bool pActive, [MarshalAs(UnmanagedType.LPWStr)] out string ppName, out int pTimeBias, out float pPercentDeadband, out int pLCID, out int phClientGroup, out int phServerGroup);
        [PreserveSig]
        int SetState([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] pRequestedUpdateRate, out int pRevisedUpdateRate, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] pActive, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] pTimeBias, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] float[] pPercentDeadband, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] pLCID, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=1)] int[] phClientGroup);
        [PreserveSig]
        int SetName([In, MarshalAs(UnmanagedType.LPWStr)] string szName);
        [PreserveSig]
        int CloneGroup([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
        [PreserveSig]
        int SetKeepAlive([In] int dwKeepAliveTime, out int pdwRevisedKeepAliveTime);
        [PreserveSig]
        int GetKeepAlive(out int pdwKeepAliveTime);
    }
}

