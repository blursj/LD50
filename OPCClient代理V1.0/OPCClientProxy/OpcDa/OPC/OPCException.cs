namespace OPC
{
    using System;

    public class OPCException : Exception
    {
        private string msg;

        public OPCException(int rtc)
        {
            base.HResult = rtc;
        }

        public OPCException(int rtc, string emsg)
        {
            if (this.msg.IndexOf("{hr}") >= 0)
            {
                this.msg = this.msg.Replace("{hr}", "0x" + rtc.ToString("X") + " - " + ErrorDescriptions.GetErrorDescription(rtc));
            }
            base.HResult = rtc;
            this.msg = emsg;
        }

        public override string Message
        {
            get
            {
                return this.msg;
            }
        }

        public int Result
        {
            get
            {
                return base.HResult;
            }
        }
    }
}

