namespace OPCDA.NET
{
    using System;

    public class ShutdownRequestEventArgs : EventArgs
    {
        public string shutdownReason;

        internal ShutdownRequestEventArgs(string shutdownReasonp)
        {
            this.shutdownReason = shutdownReasonp;
        }
    }
}

