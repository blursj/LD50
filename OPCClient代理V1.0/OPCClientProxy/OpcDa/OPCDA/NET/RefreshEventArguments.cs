namespace OPCDA.NET
{
    using System;

    public class RefreshEventArguments
    {
        public ItemDef[] items;
        public RefreshEventReason Reason;
        public int TransactionId;
    }
}

