namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("39227004-A18F-4b57-8B0A-5235670F4468"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
    internal interface IOPCBrowse
    {
        [PreserveSig]
        int GetProperties([In] int dwItemCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszItemIDs, [In, MarshalAs(UnmanagedType.Bool)] bool bReturnPropertyValues, [In] int dwPropertyCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] pdwPropertyIDs, out IntPtr ppItemProperties);
        [PreserveSig]
        int Browse([In, MarshalAs(UnmanagedType.LPWStr)] string szItemID, [In, Out, MarshalAs(UnmanagedType.LPWStr)] ref string pszContinuationPoint, [In] int dwMaxElementsReturned, [In, MarshalAs(UnmanagedType.U4)] OPCBROWSEFILTER dwBrowseFilter, [In, MarshalAs(UnmanagedType.LPWStr)] string szElementNameFilter, [In, MarshalAs(UnmanagedType.LPWStr)] string szVendorFilter, [In, MarshalAs(UnmanagedType.Bool)] bool bReturnAllProperties, [In, MarshalAs(UnmanagedType.Bool)] bool bReturnPropertyValues, [In] int dwPropertyCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=8)] int[] pdwPropertyIDs, [MarshalAs(UnmanagedType.Bool)] out bool pbMoreElements, out int pdwCount, out IntPtr ppBrowseElements);
    }
}

