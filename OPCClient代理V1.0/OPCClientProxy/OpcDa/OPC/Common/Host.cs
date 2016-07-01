namespace OPC.Common
{
    using System;

    public class Host
    {
        public string Domain;
        public string HostName;
        public string Password;
        public string UserName;

        public Host()
        {
            this.HostName = null;
            this.UserName = null;
            this.Password = null;
            this.Domain = null;
        }

        public Host(string hostName)
        {
            this.HostName = null;
            this.UserName = null;
            this.Password = null;
            this.Domain = null;
            this.HostName = hostName;
        }
    }
}

