namespace OPC.Common
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("7AA83A01-6C77-11d3-84F9-00008630A38B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
    internal interface IOPCSecurityNT
    {
        [PreserveSig]
        int IsAvailableNT([MarshalAs(UnmanagedType.Bool)] out bool pbAvailable);
        [PreserveSig]
        int QueryMinImpersonationLevel(out int pdwMinImpLevel);
        [PreserveSig]
        int ChangeUser();
    }
}

