namespace OPCDA
{
    using System;

    [Flags]
    public enum OPCACCESSRIGHTS
    {
        OPC_UNKNOWN,
        OPC_READABLE,
        OPC_WRITEABLE,
        OPC_READWRITEABLE
    }
}

