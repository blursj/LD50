namespace OPCDA.NET
{
    using OPC;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class RefreshGroup : SyncIOGroup
    {
        private event RefreshEventHandler UserCallback;

        public RefreshGroup(OpcServer srv, DataChangeEventHandler dChgHnd)
        {
            this.UserCallback = null;
            this.createGroup(srv, dChgHnd, 500);
        }

        public RefreshGroup(OpcServer srv, RefreshEventHandler UserHnd)
        {
            this.UserCallback = UserHnd;
            DataChangeEventHandler dChgHnd = new DataChangeEventHandler(this.iDataChangedHandler);
            this.createGroup(srv, dChgHnd, 500);
        }

        public RefreshGroup(OpcServer srv, DataChangeEventHandler dChgHnd, int Rate)
        {
            this.UserCallback = null;
            this.createGroup(srv, dChgHnd, Rate);
        }

        public RefreshGroup(OpcServer srv, int Rate, RefreshEventHandler UserHnd)
        {
            this.UserCallback = UserHnd;
            DataChangeEventHandler dChgHnd = new DataChangeEventHandler(this.iDataChangedHandler);
            this.createGroup(srv, dChgHnd, Rate);
        }

        private void createGroup(OpcServer srv, DataChangeEventHandler dChgHnd, int Rate)
        {
            base.Srv = srv;
            base.iClientHandle = SyncIOGroup.GrpCounter;
            SyncIOGroup.GrpCounter++;
            base.iOpcGrp = srv.AddGroup("RefreshGroup" + SyncIOGroup.GrpCounter.ToString(), true, Rate, base.iClientHandle);
            base.Items = new ItemCollection();
            ReadCompleteEventHandler handler = new ReadCompleteEventHandler(this.iRCHandler);
            WriteCompleteEventHandler handler2 = new WriteCompleteEventHandler(this.iWCHandler);
            base.iOpcGrp.DataChanged += new DataChangeEventHandler(dChgHnd.Invoke);
            base.iOpcGrp.ReadCompleted += new ReadCompleteEventHandler(handler.Invoke);
            base.iOpcGrp.WriteCompleted += new WriteCompleteEventHandler(handler2.Invoke);
            base.iOpcGrp.AdviseIOPCDataCallback();
        }

        ~RefreshGroup()
        {
            this.Dispose();
        }

        private void iDataChangedHandler(object sender, DataChangeEventArgs e)
        {
            ItemDef[] defArray = new ItemDef[e.sts.Length];
            for (int i = 0; i < e.sts.Length; i++)
            {
                int handleClient = e.sts[i].HandleClient;
                ItemDef def = base.FindClientHandle(handleClient);
                defArray[i] = def;
                if (HRESULTS.Succeeded(e.sts[i].Error))
                {
                    def.OpcIRslt.DataValue = e.sts[i].DataValue;
                    def.OpcIRslt.Quality = e.sts[i].Quality;
                    def.OpcIRslt.TimeStamp = e.sts[i].TimeStamp;
                    def.OpcIRslt.Error = 0;
                }
                else
                {
                    def.OpcIRslt.Error = e.sts[i].Error;
                }
            }
            if (this.UserCallback != null)
            {
                RefreshEventArguments arguments = new RefreshEventArguments();
                arguments.TransactionId = e.transactionID;
                arguments.Reason = RefreshEventReason.DataChanged;
                arguments.items = defArray;
                this.UserCallback(this, arguments);
            }
        }

        private void iRCHandler(object sender, ReadCompleteEventArgs e)
        {
            ItemDef[] defArray = new ItemDef[e.sts.Length];
            for (int i = 0; i < e.sts.Length; i++)
            {
                int handleClient = e.sts[i].HandleClient;
                ItemDef def = base.FindClientHandle(handleClient);
                defArray[i] = def;
                if (HRESULTS.Succeeded(e.sts[i].Error))
                {
                    def.OpcIRslt.DataValue = e.sts[i].DataValue;
                    def.OpcIRslt.Quality = e.sts[i].Quality;
                    def.OpcIRslt.TimeStamp = e.sts[i].TimeStamp;
                    def.OpcIRslt.Error = 0;
                }
                else
                {
                    def.OpcIRslt.Error = e.sts[i].Error;
                }
            }
            if (this.UserCallback != null)
            {
                RefreshEventArguments arguments = new RefreshEventArguments();
                arguments.TransactionId = e.transactionID;
                arguments.Reason = RefreshEventReason.ReadCompleted;
                arguments.items = defArray;
                this.UserCallback(this, arguments);
            }
        }

        private void iWCHandler(object sender, WriteCompleteEventArgs e)
        {
            ItemDef[] defArray = new ItemDef[e.res.Length];
            for (int i = 0; i < e.res.Length; i++)
            {
                int handleClient = e.res[i].HandleClient;
                ItemDef def = base.FindClientHandle(handleClient);
                defArray[i] = def;
                def.OpcIRslt.Error = e.res[i].Error;
            }
            if (this.UserCallback != null)
            {
                RefreshEventArguments arguments = new RefreshEventArguments();
                arguments.TransactionId = e.transactionID;
                arguments.Reason = RefreshEventReason.WriteCompleted;
                arguments.items = defArray;
                this.UserCallback(this, arguments);
            }
        }

        public int Read(ItemDef idef, int TransactionId)
        {
            int[] numArray2;
            int rtc = -1073479672;
            int cancelID = 0;
            if (idef == null)
            {
                if (base.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            int[] arrHSrv = new int[] { idef.OpcIInfo.HandleServer };
            rtc = base.iOpcGrp.Read(arrHSrv, TransactionId, out cancelID, out numArray2);
            if (rtc != 0)
            {
                return rtc;
            }
            return numArray2[0];
        }

        public int Read(string name, int TransactionId)
        {
            int num;
            int rtc = -1073479672;
            if (!base.Items.Contains(name) && (base.Add(name) != 0))
            {
                if (base.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            ItemDef idef = base.Items.Item(name);
            return this.Read(idef, TransactionId, out num);
        }

        public int Read(ItemDef idef, int TransactionId, out int CancelId)
        {
            int[] numArray2;
            int rtc = -1073479672;
            CancelId = 0;
            if (idef == null)
            {
                if (base.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            int[] arrHSrv = new int[] { idef.OpcIInfo.HandleServer };
            rtc = base.iOpcGrp.Read(arrHSrv, TransactionId, out CancelId, out numArray2);
            if (rtc != 0)
            {
                return rtc;
            }
            return numArray2[0];
        }

        public int Read(string name, int TransactionId, out int CancelId)
        {
            CancelId = 0;
            int rtc = -1073479672;
            if (!base.Items.Contains(name) && (base.Add(name) != 0))
            {
                if (base.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            ItemDef idef = base.Items.Item(name);
            return this.Read(idef, TransactionId, out CancelId);
        }

        public int Write(ItemDef idef, object val, int TransactionId)
        {
            int num;
            return this.Write(idef, val, TransactionId, out num);
        }

        public int Write(string name, object val, int TransactionId)
        {
            int num2;
            int rtc = -1073479672;
            if (!base.Items.Contains(name) && (base.Add(name) != 0))
            {
                if (base.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            ItemDef idef = base.Items.Item(name);
            return this.Write(idef, val, 0, out num2);
        }

        public int Write(ItemDef idef, object val, int TransactionId, out int CancelId)
        {
            int[] numArray2;
            int rtc = -1073479672;
            CancelId = 0;
            if (idef == null)
            {
                if (base.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            int[] arrHSrv = new int[] { idef.OpcIInfo.HandleServer };
            object[] arrVal = new object[] { val };
            rtc = base.iOpcGrp.Write(arrHSrv, arrVal, TransactionId, out CancelId, out numArray2);
            if (rtc != 0)
            {
                return rtc;
            }
            if (numArray2[0] != 0)
            {
                return numArray2[0];
            }
            return 0;
        }

        public int Write(string name, object val, int TransactionId, out int CancelId)
        {
            int rtc = -1073479672;
            CancelId = 0;
            if (!base.Items.Contains(name) && (base.Add(name) != 0))
            {
                if (base.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            ItemDef idef = base.Items.Item(name);
            return this.Write(idef, val, TransactionId, out CancelId);
        }

        public int UpdateRate
        {
            get
            {
                return base.iOpcGrp.UpdateRate;
            }
            set
            {
                base.iOpcGrp.UpdateRate = value;
            }
        }
    }
}

