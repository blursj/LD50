namespace OPCDA.NET
{
    using OPC;
    using OPCDA;
    using System;
    using System.Runtime.InteropServices;

    public class SyncIOGroup
    {
        internal static int GrpCounter = 1;
        public int iClientHandle;
        internal OpcGroup iOpcGrp;
        internal static int ItemClientHandle = 1;
        internal ItemCollection Items;
        internal OpcServer Srv;

        internal SyncIOGroup()
        {
        }

        public SyncIOGroup(OpcServer srv)
        {
            this.createGroup(srv, 0x3e8);
        }

        public SyncIOGroup(OpcServer srv, int Rate)
        {
            this.createGroup(srv, Rate);
        }

        public int Add(string name)
        {
            int num;
            return this.Add(name, out num);
        }

        internal int Add(string name, out int ClientHandle)
        {
            return this.Add(name, out ClientHandle, false, VarEnum.VT_EMPTY);
        }

        internal int Add(string name, out int ClientHandle, bool allowMulti, VarEnum reqType)
        {
            ClientHandle = 0;
            int rtc = -1073479668;
            if (!allowMulti && this.Items.Contains(name))
            {
                if (this.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            OPCItemDef[] arrDef = new OPCItemDef[1];
            ClientHandle = ItemClientHandle++;
            arrDef[0] = new OPCItemDef(name, true, ClientHandle, reqType);
            OPCItemResult[] aRslt = new OPCItemResult[1];
            rtc = this.iOpcGrp.AddItems(arrDef, out aRslt);
            switch (rtc)
            {
                case 0:
                {
                    ItemDef gItem = new ItemDef();
                    gItem.OpcIDef = arrDef[0];
                    gItem.OpcIInfo = aRslt[0];
                    gItem.OpcIRslt = new OPCItemState();
                    this.Items.Add(gItem);
                    arrDef = null;
                    aRslt = null;
                    return 0;
                }
                case 1:
                    return aRslt[0].Error;
            }
            return rtc;
        }

        private void createGroup(OpcServer srv, int Rate)
        {
            this.Srv = srv;
            this.iClientHandle = GrpCounter;
            GrpCounter++;
            this.iOpcGrp = srv.AddGroup("SyncIOGroup" + GrpCounter.ToString(), true, Rate, this.iClientHandle);
            this.Items = new ItemCollection();
        }

        public virtual void Dispose()
        {
            try
            {
                if (this.iOpcGrp != null)
                {
                    this.iOpcGrp.Remove(true);
                }
                this.iOpcGrp = null;
                this.Items = null;
            }
            catch
            {
            }
        }

        ~SyncIOGroup()
        {
            this.Dispose();
        }

        public ItemDef FindClientHandle(int hnd)
        {
            return this.Items.FindClientHandle(hnd);
        }

        public string GetErrorString(int err)
        {
            int num;
            this.Srv.GetLocaleID(out num);
            return this.Srv.GetErrorString(err, num);
        }

        public string GetQualityString(short quality)
        {
            OPC_QUALITY_STATUS opc_quality_status = (OPC_QUALITY_STATUS) quality;
            return opc_quality_status.ToString();
        }

        public ItemDef Item(string name)
        {
            return this.Items.Item(name);
        }

        public virtual int Read(OPCDATASOURCE src, ItemDef idef, out OPCItemState val)
        {
            OPCItemState[] stateArray;
            val = null;
            int rtc = -1073479672;
            if (idef == null)
            {
                if (this.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            int[] aSrvHnd = new int[] { idef.OpcIInfo.HandleServer };
            rtc = this.iOpcGrp.Read(src, aSrvHnd, out stateArray);
            if (HRESULTS.Succeeded(rtc))
            {
                idef.OpcIRslt = stateArray[0];
                val = stateArray[0];
            }
            return rtc;
        }

        public virtual int Read(OPCDATASOURCE src, string name, out OPCItemState val)
        {
            val = null;
            int rtc = -1073479672;
            if (!this.Items.Contains(name) && (this.Add(name) != 0))
            {
                if (this.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            ItemDef idef = this.Items.Item(name);
            return this.Read(src, idef, out val);
        }

        internal int Remove(ItemDef item)
        {
            int[] numArray2;
            int[] arrHSrv = new int[] { item.OpcIInfo.HandleServer };
            this.Items.Remove(item);
            return this.iOpcGrp.RemoveItems(arrHSrv, out numArray2);
        }

        public void Remove(string name)
        {
            ItemDef gItem = this.Items.Item(name);
            if (gItem != null)
            {
                int[] numArray2;
                int[] arrHSrv = new int[] { gItem.OpcIInfo.HandleServer };
                this.iOpcGrp.RemoveItems(arrHSrv, out numArray2);
                this.Items.Remove(gItem);
            }
        }

        public virtual int Write(ItemDef idef, object val)
        {
            int[] numArray2;
            int rtc = -1073479673;
            if (idef == null)
            {
                if (this.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            int[] arrHSrv = new int[] { idef.OpcIInfo.HandleServer };
            object[] arrVal = new object[] { val };
            rtc = this.iOpcGrp.Write(arrHSrv, arrVal, out numArray2);
            if (HRESULTS.Failed(rtc))
            {
                return rtc;
            }
            if (numArray2[0] != 0)
            {
                return numArray2[0];
            }
            return 0;
        }

        public virtual int Write(string name, object val)
        {
            int rtc = -1073479673;
            if (!this.Items.Contains(name) && (this.Add(name) != 0))
            {
                if (this.Srv.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            ItemDef idef = this.Items.Item(name);
            return this.Write(idef, val);
        }

        public int ClientHandle
        {
            get
            {
                return this.iClientHandle;
            }
        }

        public OpcGroup OpcGrp
        {
            get
            {
                return this.iOpcGrp;
            }
        }
    }
}

