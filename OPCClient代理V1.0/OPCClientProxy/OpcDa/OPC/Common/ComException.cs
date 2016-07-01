namespace OPC.Common
{
    using System;
    using System.Runtime.InteropServices;

    internal class ComException : ApplicationException
    {
        internal int Error;

        internal ComException(Exception e, string message) : base(message, e)
        {
            this.Error = 0;
            this.Error = Marshal.GetHRForException(e);
        }
    }
}

