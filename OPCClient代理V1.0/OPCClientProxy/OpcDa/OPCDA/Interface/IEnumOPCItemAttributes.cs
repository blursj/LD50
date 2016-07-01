namespace OPCDA.Interface
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, ComVisible(true), Guid("39c13a55-011e-11d0-9675-0020afd8adb3"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumOPCItemAttributes
    {
        [PreserveSig]
        int Next([In] int celt, out IntPtr ppItemArray, out int pceltFetched);
        [PreserveSig]
        int Skip([In] int celt);
        [PreserveSig]
        int Reset();
        [PreserveSig]
        int Clone([MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}

