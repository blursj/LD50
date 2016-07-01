namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("85C0B427-2893-4cbc-BD78-E5FC5146F08F"), ComVisible(true)]
    internal interface IOPCItemIO
    {
        [PreserveSig]
        int Read(int Count, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszItemIDs, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)] int[] pdwMaxAge, out IntPtr ppvValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors);
        [PreserveSig]
        int WriteVQT([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszItemIDs, [In] IntPtr pItemVQT, out IntPtr ppErrors);
    }
}

