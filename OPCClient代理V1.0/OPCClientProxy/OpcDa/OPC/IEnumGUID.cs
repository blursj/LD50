namespace OPC
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0002E000-0000-0000-C000-000000000046")]
    internal interface IEnumGUID
    {
        int Next([In] int celt, [In] ref Guid rgelt, out int pceltFetched);
        int Skip([In] int celt);
        int Reset();
        int Clone([MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}

