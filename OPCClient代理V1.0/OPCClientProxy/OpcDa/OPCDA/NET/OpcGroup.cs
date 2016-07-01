namespace OPCDA.NET
{
    using OPC;
    using OPCDA;
    using OPCDA.Interface;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class OpcGroup : IOPCDataCallback
    {
        private int callbackcookie = 0;
        private UCOMIConnectionPoint callbackcpoint = null;
        private UCOMIConnectionPointContainer cpointcontainer = null;
        private IOPCAsyncIO2 ifAsync = null;
        private IOPCAsyncIO3 ifAsync3 = null;
        private IOPCGroupStateMgt2 ifGrpStateMgt2 = null;
        private IOPCItemDeadbandMgt ifItemDeadbandMgt = null;
        private IOPCItemMgt ifItems = null;
        private IOPCItemSamplingMgt ifItemSamplingMgt = null;
        private IOPCGroupStateMgt ifMgt = null;
        private IOPCServer ifServer = null;
        private IOPCSyncIO ifSync = null;
        private IOPCSyncIO2 ifSync2 = null;
        private OpcServer objSrv;
        private readonly int sizeOPCITEMDEF;
        private readonly int sizeOPCITEMRESULT;
        private OPCGroupState state;
        private readonly Type typeOPCITEMDEF;
        private readonly Type typeOPCITEMRESULT;

        public event CancelCompleteEventHandler CancelCompleted;

        public event DataChangeEventHandler DataChanged;

        public event ReadCompleteEventHandler ReadCompleted;

        public event WriteCompleteEventHandler WriteCompleted;

        internal OpcGroup(OpcServer srvObj, string groupName, bool setActive, int requestedUpdateRate, int ClientHandle)
        {
            this.objSrv = srvObj;
            this.ifServer = srvObj.ifServer;
            this.state.Name = groupName;
            this.state.Public = false;
            this.state.UpdateRate = requestedUpdateRate;
            this.state.Active = setActive;
            this.state.TimeBias = 0;
            this.state.PercentDeadband = 0f;
            this.state.LocaleID = 0;
            this.state.HandleClient = ClientHandle;
            this.state.HandleServer = 0;
            this.typeOPCITEMDEF = typeof(OPCITEMDEFintern);
            this.sizeOPCITEMDEF = Marshal.SizeOf(this.typeOPCITEMDEF);
            this.typeOPCITEMRESULT = typeof(OPCITEMRESULTintern);
            this.sizeOPCITEMRESULT = Marshal.SizeOf(this.typeOPCITEMRESULT);
        }

        public int AddItems(OPCItemDef[] arrDef, out OPCItemResult[] aRslt)
        {
            IntPtr ptr2;
            IntPtr ptr3;
            aRslt = null;
            int dwCount = 0;
            for (int i = 0; i < arrDef.Length; i++)
            {
                if (arrDef[i] != null)
                {
                    dwCount++;
                }
            }
            IntPtr pItemArray = Marshal.AllocCoTaskMem(dwCount * this.sizeOPCITEMDEF);
            int num3 = (int)pItemArray;
            OPCITEMDEFintern structure = new OPCITEMDEFintern();
            structure.wReserved = 0;
            foreach (OPCItemDef def in arrDef)
            {
                if (def != null)
                {
                    structure.szAccessPath = def.AccessPath;
                    structure.szItemID = def.ItemID;
                    structure.bActive = def.Active;
                    structure.vtRequestedDataType = (short)def.RequestedDataType;
                    structure.dwBlobSize = 0;
                    structure.pBlob = IntPtr.Zero;
                    structure.hClient = def.HandleClient;
                    Marshal.StructureToPtr(structure, (IntPtr)num3, false);
                    num3 += this.sizeOPCITEMDEF;
                }
            }
            int hresultcode = this.ifItems.AddItems(dwCount, pItemArray, out ptr2, out ptr3);
            num3 = (int)pItemArray;
            for (int j = 0; j < dwCount; j++)
            {
                Marshal.DestroyStructure((IntPtr)num3, this.typeOPCITEMDEF);
                num3 += this.sizeOPCITEMDEF;
            }
            Marshal.FreeCoTaskMem(pItemArray);
            if (HRESULTS.Failed(hresultcode))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
                }
                return hresultcode;
            }
            int num6 = (int)ptr2;
            int num7 = (int)ptr3;
            if ((num6 == 0) || (num7 == 0))
            {
                hresultcode = -2147467260;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
                }
                return hresultcode;
            }
            aRslt = new OPCItemResult[dwCount];
            for (int k = 0; k < dwCount; k++)
            {
                aRslt[k] = new OPCItemResult();
                aRslt[k].Error = Marshal.ReadInt32((IntPtr)num7);
                if (HRESULTS.Succeeded(aRslt[k].Error))
                {
                    aRslt[k].HandleServer = Marshal.ReadInt32((IntPtr)num6);
                    aRslt[k].CanonicalDataType = (VarEnum)Marshal.ReadInt16((IntPtr)(num6 + 4));
                    aRslt[k].AccessRights = (OPCACCESSRIGHTS)Marshal.ReadInt32((IntPtr)(num6 + 8));
                }
                num6 += this.sizeOPCITEMRESULT;
                num7 += 4;
            }
            Marshal.FreeCoTaskMem(ptr2);
            Marshal.FreeCoTaskMem(ptr3);
            return hresultcode;
        }

        public void AdviseIOPCDataCallback()
        {
            Type type = typeof(IOPCDataCallback);
            Guid gUID = type.GUID;
            if ((this.callbackcpoint == null) && (this.cpointcontainer != null))
            {
                this.cpointcontainer.FindConnectionPoint(ref gUID, out this.callbackcpoint);
                if (this.callbackcpoint != null)
                {
                    this.callbackcpoint.Advise(this, out this.callbackcookie);
                }
            }
        }

        public int Cancel2(int cancelID)
        {
            int num;
            if ((this.ifAsync == null) || (this.cpointcontainer == null))
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifAsync.Cancel2(cancelID);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int ClearItemDeadband(int[] handles, out int[] errors)
        {
            int num;
            IntPtr ptr;
            errors = null;
            if (this.ifItemDeadbandMgt == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItemDeadbandMgt.ClearItemDeadband(handles.Length, handles, out ptr);
            if (HRESULTS.Succeeded(num))
            {
                errors = new int[handles.Length];
                Marshal.Copy(ptr, errors, 0, handles.Length);
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int ClearItemSamplingRate(int[] handles, out int[] errors)
        {
            int num;
            IntPtr ptr;
            errors = null;
            if (this.ifItemSamplingMgt == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItemSamplingMgt.ClearItemSamplingRate(handles.Length, handles, out ptr);
            if (HRESULTS.Succeeded(num))
            {
                errors = new int[handles.Length];
                Marshal.Copy(ptr, errors, 0, handles.Length);
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int CloneGroup(string Name, ref Guid riid, out object ppUnk)
        {
            int hresultcode = this.ifMgt.CloneGroup(Name, ref riid, out ppUnk);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        internal OpcEnumItemAttributes CreateAttrEnumerator()
        {
            object obj2;
            Type type = typeof(IEnumOPCItemAttributes);
            Guid gUID = type.GUID;
            int hresultcode = this.ifItems.CreateEnumerator(ref gUID, out obj2);
            if (HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, "GetItemAttributes failed with error code {hr}");
            }
            if ((hresultcode == 1) || (obj2 == null))
            {
                return null;
            }
            IEnumOPCItemAttributes ifEnump = (IEnumOPCItemAttributes)obj2;
            obj2 = null;
            return new OpcEnumItemAttributes(ifEnump);
        }

        ~OpcGroup()
        {
            try
            {
                this.Remove(false);
            }
            catch
            {
            }
        }

        public int GetEnable(out bool isEnabled)
        {
            int enable;
            isEnabled = false;
            if ((this.ifAsync == null) || (this.cpointcontainer == null))
            {
                enable = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(enable, ErrorDescriptions.GetErrorDescription(enable));
                }
                return enable;
            }
            enable = this.ifAsync.GetEnable(out isEnabled);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(enable))
            {
                throw new OPCException(enable, ErrorDescriptions.GetErrorDescription(enable));
            }
            return enable;
        }

        public int GetItemAttributes(out OPCItemAttributes[] attributes)
        {
            int hresultcode = this.CreateAttrEnumerator().Next(0x186a0, out attributes);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int GetItemBufferEnable(int[] handles, out bool[] enableState, out int[] errors)
        {
            int num;
            IntPtr ptr;
            IntPtr ptr2;
            errors = null;
            enableState = null;
            if (this.ifItemSamplingMgt == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItemSamplingMgt.GetItemBufferEnable(handles.Length, handles, out ptr2, out ptr);
            if (HRESULTS.Succeeded(num))
            {
                enableState = new bool[handles.Length];
                int[] destination = new int[handles.Length];
                Marshal.Copy(ptr2, destination, 0, handles.Length);
                for (int i = 0; i < handles.Length; i++)
                {
                    enableState[i] = destination[i] != 0;
                }
                errors = new int[handles.Length];
                Marshal.Copy(ptr, errors, 0, handles.Length);
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int GetItemDeadband(int[] handles, out float[] percentDeadband, out int[] errors)
        {
            int num;
            IntPtr ptr;
            IntPtr ptr2;
            errors = null;
            percentDeadband = null;
            if (this.ifItemDeadbandMgt == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItemDeadbandMgt.GetItemDeadband(handles.Length, handles, out ptr2, out ptr);
            if (HRESULTS.Succeeded(num))
            {
                percentDeadband = new float[handles.Length];
                Marshal.Copy(ptr2, percentDeadband, 0, handles.Length);
                errors = new int[handles.Length];
                Marshal.Copy(ptr, errors, 0, handles.Length);
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int GetItemSamplingRate(int[] handles, out int[] SamplingRate, out int[] errors)
        {
            int num;
            IntPtr ptr;
            IntPtr ptr2;
            errors = null;
            SamplingRate = null;
            if (this.ifItemSamplingMgt == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItemSamplingMgt.GetItemSamplingRate(handles.Length, handles, out ptr2, out ptr);
            if (HRESULTS.Succeeded(num))
            {
                SamplingRate = new int[handles.Length];
                Marshal.Copy(ptr2, SamplingRate, 0, handles.Length);
                errors = new int[handles.Length];
                Marshal.Copy(ptr, errors, 0, handles.Length);
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int GetKeepAlive(out int keepAliveRate)
        {
            int keepAlive;
            int num2;
            keepAliveRate = 0;
            if (this.ifGrpStateMgt2 == null)
            {
                keepAlive = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(keepAlive, ErrorDescriptions.GetErrorDescription(keepAlive));
                }
                return keepAlive;
            }
            keepAlive = this.ifGrpStateMgt2.GetKeepAlive(out num2);
            if (HRESULTS.Succeeded(keepAlive))
            {
                keepAliveRate = num2;
                this.state.KeepAliveRate = keepAliveRate;
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(keepAlive))
            {
                throw new OPCException(keepAlive, ErrorDescriptions.GetErrorDescription(keepAlive));
            }
            return keepAlive;
        }

        public int GetState()
        {
            int hresultcode = this.ifMgt.GetState(out this.state.UpdateRate, out this.state.Active, out this.state.Name, out this.state.TimeBias, out this.state.PercentDeadband, out this.state.LocaleID, out this.state.HandleClient, out this.state.HandleServer);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int GetState(out int updateRate, out bool active, out string name, out int timeBias, out float percentDeadband, out int localeID, out int handleClient, out int handleServer)
        {
            int hresultcode = this.ifMgt.GetState(out this.state.UpdateRate, out this.state.Active, out this.state.Name, out this.state.TimeBias, out this.state.PercentDeadband, out this.state.LocaleID, out this.state.HandleClient, out this.state.HandleServer);
            updateRate = this.state.UpdateRate;
            active = this.state.Active;
            name = this.state.Name;
            timeBias = this.state.TimeBias;
            percentDeadband = this.state.PercentDeadband;
            localeID = this.state.LocaleID;
            handleClient = this.state.HandleClient;
            handleServer = this.state.HandleServer;
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        internal int internalAdd(ref float percentDeadband, int localeID)
        {
            int num;
            object obj2;
            Type type = typeof(IOPCGroupStateMgt);
            Guid gUID = type.GUID;
            if (this.state.Public)
            {
                IOPCServerPublicGroups ifServer = null;
                ifServer = (IOPCServerPublicGroups)this.ifServer;
                if (ifServer == null)
                {
                    num = -2147467262;
                    if (this.objSrv.myErrorsAsExecptions)
                    {
                        throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                    }
                    return num;
                }
                ifServer.GetPublicGroupByName(this.state.Name, ref gUID, out obj2);
                ifServer = null;
            }
            else
            {
                num = this.ifServer.AddGroup(this.state.Name, this.state.Active, this.state.UpdateRate, this.state.HandleClient, IntPtr.Zero, IntPtr.Zero, this.state.LocaleID, out this.state.HandleServer, out this.state.UpdateRate, ref gUID, out obj2);
                if (HRESULTS.Failed(num))
                {
                    if (this.objSrv.myErrorsAsExecptions)
                    {
                        throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                    }
                    return num;
                }
            }
            if (obj2 == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            this.ifMgt = (IOPCGroupStateMgt)obj2;
            this.ifItems = (IOPCItemMgt)this.ifMgt;
            try
            {
                this.ifGrpStateMgt2 = (IOPCGroupStateMgt2)obj2;
            }
            catch
            {
            }
            try
            {
                this.ifItemDeadbandMgt = (IOPCItemDeadbandMgt)obj2;
            }
            catch
            {
            }
            try
            {
                this.ifItemSamplingMgt = (IOPCItemSamplingMgt)obj2;
            }
            catch
            {
            }
            this.ifSync = (IOPCSyncIO)obj2;
            try
            {
                this.ifSync2 = (IOPCSyncIO2)obj2;
            }
            catch
            {
            }
            try
            {
                this.ifAsync = (IOPCAsyncIO2)obj2;
            }
            catch
            {
            }
            try
            {
                this.ifAsync3 = (IOPCAsyncIO3)obj2;
            }
            catch
            {
            }
            try
            {
                this.cpointcontainer = (UCOMIConnectionPointContainer)obj2;
            }
            catch
            {
            }
            if (percentDeadband != 0f)
            {
                this.PercentDeadband = percentDeadband;
            }
            this.GetState();
            return 0;
        }

        void IOPCDataCallback.OnCancelComplete(int dwTransid, int hGroup)
        {
            if (hGroup == this.state.HandleClient)
            {
                CancelCompleteEventArgs e = new CancelCompleteEventArgs(dwTransid, hGroup);
                if (this.CancelCompleted != null)
                {
                    this.CancelCompleted(this, e);
                }
            }
        }

        void IOPCDataCallback.OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, IntPtr phClientItems, IntPtr pvValues, IntPtr pwQualities, IntPtr pftTimeStamps, IntPtr ppErrors)
        {
            //here is  our need to study
            if ((dwCount != 0) && (hGroup == this.state.HandleClient))
            {
                int num = dwCount;
                int num2 = (int)phClientItems;
                int num3 = (int)pvValues;
                int num4 = (int)pwQualities;
                int num5 = (int)pftTimeStamps;
                int num6 = (int)ppErrors;
                DataChangeEventArgs e = new DataChangeEventArgs();
                e.transactionID = dwTransid;
                e.groupHandleClient = hGroup;
                e.masterQuality = hrMasterquality;
                e.masterError = hrMastererror;
                e.sts = new OPCItemState[num];
                for (int i = 0; i < num; i++)
                {
                    e.sts[i] = new OPCItemState();
                    e.sts[i].Error = Marshal.ReadInt32((IntPtr)num6);
                    num6 += 4;
                    e.sts[i].HandleClient = Marshal.ReadInt32((IntPtr)num2);
                    num2 += 4;
                    if (HRESULTS.Succeeded(e.sts[i].Error))
                    {
                        if (Marshal.ReadInt16((IntPtr)num3) == 10)
                        {
                            e.sts[i].Error = Marshal.ReadInt32((IntPtr)(num3 + 8));
                        }
                        e.sts[i].DataValue = Marshal.GetObjectForNativeVariant((IntPtr)num3);
                        e.sts[i].Quality = Marshal.ReadInt16((IntPtr)num4);
                        e.sts[i].TimeStamp = Marshal.ReadInt64((IntPtr)num5);
                    }
                    num3 += DUMMY_VARIANT.ConstSize;
                    num4 += 2;
                    num5 += 8;
                }
                if (this.DataChanged != null)
                {
                    this.DataChanged(this, e);
                }
            }
        }

        void IOPCDataCallback.OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, IntPtr phClientItems, IntPtr pvValues, IntPtr pwQualities, IntPtr pftTimeStamps, IntPtr ppErrors)
        {
            if ((dwCount != 0) && (hGroup == this.state.HandleClient))
            {
                int num = dwCount;
                int num2 = (int)phClientItems;
                int num3 = (int)pvValues;
                int num4 = (int)pwQualities;
                int num5 = (int)pftTimeStamps;
                int num6 = (int)ppErrors;
                ReadCompleteEventArgs e = new ReadCompleteEventArgs();
                e.transactionID = dwTransid;
                e.groupHandleClient = hGroup;
                e.masterQuality = hrMasterquality;
                e.masterError = hrMastererror;
                e.sts = new OPCItemState[num];
                for (int i = 0; i < num; i++)
                {
                    e.sts[i] = new OPCItemState();
                    e.sts[i].Error = Marshal.ReadInt32((IntPtr)num6);
                    num6 += 4;
                    e.sts[i].HandleClient = Marshal.ReadInt32((IntPtr)num2);
                    num2 += 4;
                    if (HRESULTS.Succeeded(e.sts[i].Error))
                    {
                        if (Marshal.ReadInt16((IntPtr)num3) == 10)
                        {
                            e.sts[i].Error = Marshal.ReadInt32((IntPtr)(num3 + 8));
                        }
                        e.sts[i].DataValue = Marshal.GetObjectForNativeVariant((IntPtr)num3);
                        e.sts[i].Quality = Marshal.ReadInt16((IntPtr)num4);
                        e.sts[i].TimeStamp = Marshal.ReadInt64((IntPtr)num5);
                    }
                    num3 += DUMMY_VARIANT.ConstSize;
                    num4 += 2;
                    num5 += 8;
                }
                if (this.ReadCompleted != null)
                {
                    this.ReadCompleted(this, e);
                }
            }
        }

        void IOPCDataCallback.OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, IntPtr pClienthandles, IntPtr ppErrors)
        {
            if ((dwCount != 0) && (hGroup == this.state.HandleClient))
            {
                int num = dwCount;
                int num2 = (int)pClienthandles;
                int num3 = (int)ppErrors;
                WriteCompleteEventArgs e = new WriteCompleteEventArgs();
                e.transactionID = dwTransid;
                e.groupHandleClient = hGroup;
                e.masterError = hrMastererr;
                e.res = new OPCWriteResult[num];
                for (int i = 0; i < num; i++)
                {
                    e.res[i] = new OPCWriteResult();
                    e.res[i].Error = Marshal.ReadInt32((IntPtr)num3);
                    num3 += 4;
                    e.res[i].HandleClient = Marshal.ReadInt32((IntPtr)num2);
                    num2 += 4;
                }
                if (this.WriteCompleted != null)
                {
                    this.WriteCompleted(this, e);
                }
            }
        }

        public static string QualityToString(short Quality)
        {
            StringBuilder builder = new StringBuilder(0x100);
            OPC_QUALITY_MASTER opc_quality_master = (OPC_QUALITY_MASTER)((short)(Quality & 0xc0));
            OPC_QUALITY_STATUS opc_quality_status = (OPC_QUALITY_STATUS)((short)(Quality & 0xfc));
            OPC_QUALITY_LIMIT opc_quality_limit = ((OPC_QUALITY_LIMIT)Quality) & OPC_QUALITY_LIMIT.LIMIT_CONST;
            builder.AppendFormat("{0}+{1}+{2}", opc_quality_master, opc_quality_status, opc_quality_limit);
            return builder.ToString();
        }

        public int Read(OPCDATASOURCE src, int[] aSrvHnd, out OPCItemState[] aResult)
        {
            IntPtr ptr;
            IntPtr ptr2;
            int length = aSrvHnd.Length;
            aResult = null;
            int hresultcode = this.ifSync.Read(src, length, aSrvHnd, out ptr, out ptr2);
            if (HRESULTS.Failed(hresultcode))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
                }
                return hresultcode;
            }
            int num3 = (int)ptr2;
            int num4 = (int)ptr;
            if ((num3 == 0) || (num4 == 0))
            {
                hresultcode = -2147467260;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
                }
                return hresultcode;
            }
            aResult = new OPCItemState[length];
            for (int i = 0; i < length; i++)
            {
                aResult[i] = new OPCItemState();
                aResult[i].Error = Marshal.ReadInt32((IntPtr)num3);
                num3 += 4;
                aResult[i].HandleClient = Marshal.ReadInt32((IntPtr)num4);
                if (HRESULTS.Succeeded(aResult[i].Error))
                {
                    if (Marshal.ReadInt16((IntPtr)(num4 + 0x10)) == 10)
                    {
                        aResult[i].Error = Marshal.ReadInt32((IntPtr)(num4 + 0x18));
                    }
                    aResult[i].TimeStamp = Marshal.ReadInt64((IntPtr)(num4 + 4));
                    aResult[i].Quality = Marshal.ReadInt16((IntPtr)(num4 + 12));
                    aResult[i].DataValue = Marshal.GetObjectForNativeVariant((IntPtr)(num4 + 0x10));
                    object dataValue = aResult[i].DataValue;
                    DUMMY_VARIANT.VariantClear((IntPtr)(num4 + 0x10));
                }
                else
                {
                    aResult[i].DataValue = null;
                }
                num4 += 0x20;
            }
            Marshal.FreeCoTaskMem(ptr);
            Marshal.FreeCoTaskMem(ptr2);
            return hresultcode;
        }

        public int Read(int[] arrHSrv, int transactionID, out int cancelID, out int[] arrErr)
        {
            IntPtr ptr;
            int num2;
            arrErr = null;
            cancelID = 0;
            int length = arrHSrv.Length;
            if ((this.ifAsync == null) || (this.cpointcontainer == null))
            {
                num2 = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            num2 = this.ifAsync.Read(length, arrHSrv, transactionID, out cancelID, out ptr);
            if (HRESULTS.Failed(num2))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            arrErr = new int[length];
            Marshal.Copy(ptr, arrErr, 0, length);
            Marshal.FreeCoTaskMem(ptr);
            return num2;
        }

        public int ReadMaxAge(ItemValue[] items)
        {
            IntPtr ptr;
            IntPtr ptr2;
            IntPtr ptr3;
            IntPtr ptr4;
            int num2;
            int length = items.Length;
            int[] phServer = new int[length];
            int[] pdwMaxAge = new int[length];
            for (int i = 0; i < length; i++)
            {
                phServer[i] = items[i].ServerHandle;
                pdwMaxAge[i] = items[i].MaxAge;
            }
            if (this.ifSync2 == null)
            {
                num2 = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            num2 = this.ifSync2.ReadMaxAge(length, phServer, pdwMaxAge, out ptr, out ptr2, out ptr3, out ptr4);
            if (HRESULTS.Failed(num2))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            object[] objArray = CustomMarshal.ToObjects(length, ref ptr);
            OPCQuality[] qualityArray = CustomMarshal.ToQualities(length, ref ptr2);
            DateTime[] timeArray = CustomMarshal.ToDateTimes(length, ref ptr3);
            int[] destination = new int[length];
            Marshal.Copy(ptr4, destination, 0, length);
            Marshal.FreeCoTaskMem(ptr4);
            for (int j = 0; j < length; j++)
            {
                if (HRESULTS.Failed(destination[j]))
                {
                    items[j].Error = destination[j];
                }
                else
                {
                    items[j].Value = objArray[j];
                    items[j].Quality = qualityArray[j];
                    items[j].QualitySpecified = true;
                    items[j].Timestamp = timeArray[j];
                    items[j].TimestampSpecified = true;
                }
            }
            return num2;
        }

        public int ReadMaxAge(int[] srvHnd, int[] maxAge, int transactionID, out int cancelID, out int[] errors)
        {
            IntPtr ptr;
            int num2;
            int length = srvHnd.Length;
            cancelID = 0;
            errors = null;
            if (this.ifAsync3 == null)
            {
                num2 = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            num2 = this.ifAsync3.ReadMaxAge(length, srvHnd, maxAge, transactionID, out cancelID, out ptr);
            if (HRESULTS.Failed(num2))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            errors = new int[length];
            Marshal.Copy(ptr, errors, 0, length);
            Marshal.FreeCoTaskMem(ptr);
            return num2;
        }

        public int Refresh2(OPCDATASOURCE sourceMode, int transactionID, out int cancelID)
        {
            int num;
            cancelID = 0;
            if ((this.ifAsync == null) || (this.cpointcontainer == null))
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifAsync.Refresh2(sourceMode, transactionID, out cancelID);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int RefreshMaxAge(int maxAge, int transactionID, out int cancelID)
        {
            int num;
            cancelID = 0;
            if (this.ifAsync3 == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifAsync3.RefreshMaxAge(maxAge, transactionID, out cancelID);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int Remove(bool bForce)
        {
            int hresultcode = 0;
            if (this.callbackcpoint != null)
            {
                if (this.callbackcookie != 0)
                {
                    try
                    {
                        this.callbackcpoint.Unadvise(this.callbackcookie);
                    }
                    catch
                    {
                    }
                    this.callbackcookie = 0;
                }
                Marshal.ReleaseComObject(this.callbackcpoint);
                this.callbackcpoint = null;
            }
            this.cpointcontainer = null;
            this.ifItems = null;
            this.ifSync = null;
            this.ifAsync = null;
            if (this.ifMgt != null)
            {                                                                                                                                                         
                try
                {                                                                                                 
                    Marshal.ReleaseComObject(this.ifMgt);
                }
                catch { }

                this.ifMgt = null;
            }
            if (this.ifServer != null)
            {
                if (!this.state.Public)
                {
                    try
                    {
                        hresultcode = this.ifServer.RemoveGroup(this.state.HandleServer, bForce);
                    }
                    catch
                    {
                    }
                }
                this.ifServer = null;
            }
            this.state.HandleServer = 0;
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int RemoveItems(int[] arrHSrv, out int[] arrErr)
        {
            IntPtr ptr;
            arrErr = null;
            int length = arrHSrv.Length;
            int hresultcode = this.ifItems.RemoveItems(length, arrHSrv, out ptr);
            if (HRESULTS.Failed(hresultcode))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
                }
                return hresultcode;
            }
            arrErr = new int[length];
            Marshal.Copy(ptr, arrErr, 0, length);
            Marshal.FreeCoTaskMem(ptr);
            return hresultcode;
        }

        public int SetActiveState(int[] arrHSrv, bool activate, out int[] arrErr)
        {
            IntPtr ptr;
            arrErr = null;
            int length = arrHSrv.Length;
            int hresultcode = this.ifItems.SetActiveState(length, arrHSrv, activate, out ptr);
            if (HRESULTS.Failed(hresultcode))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
                }
                return hresultcode;
            }
            arrErr = new int[length];
            Marshal.Copy(ptr, arrErr, 0, length);
            Marshal.FreeCoTaskMem(ptr);
            return hresultcode;
        }

        public int SetClientHandles(int[] arrHSrv, int[] arrHClt, out int[] arrErr)
        {
            int num;
            IntPtr ptr;
            arrErr = null;
            int length = arrHSrv.Length;
            if (length != arrHClt.Length)
            {
                num = -2147467260;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItems.SetClientHandles(length, arrHSrv, arrHClt, out ptr);
            if (HRESULTS.Failed(num))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            arrErr = new int[length];
            Marshal.Copy(ptr, arrErr, 0, length);
            Marshal.FreeCoTaskMem(ptr);
            return num;
        }

        public int SetDatatypes(int[] arrHSrv, VarEnum[] arrVT, out int[] arrErr)
        {
            int num;
            IntPtr ptr2;
            arrErr = null;
            int length = arrHSrv.Length;
            if (length != arrVT.Length)
            {
                num = -2147467260;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            IntPtr pRequestedDatatypes = Marshal.AllocCoTaskMem(length * 2);
            int num3 = (int)pRequestedDatatypes;
            foreach (VarEnum enum2 in arrVT)
            {
                Marshal.WriteInt16((IntPtr)num3, (short)enum2);
                num3 += 2;
            }
            num = this.ifItems.SetDatatypes(length, arrHSrv, pRequestedDatatypes, out ptr2);
            Marshal.FreeCoTaskMem(pRequestedDatatypes);
            if (HRESULTS.Failed(num))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            arrErr = new int[length];
            Marshal.Copy(ptr2, arrErr, 0, length);
            Marshal.FreeCoTaskMem(ptr2);
            return num;
        }

        public int SetDatatypes(int[] arrHSrv, Type[] arrST, out int[] arrErr)
        {
            VarEnum[] arrVT = new VarEnum[arrST.Length];
            for (int i = 0; i < arrST.Length; i++)
            {
                arrVT[i] = types.ConvertToVarType(arrST[i]);
            }
            int hresultcode = this.SetDatatypes(arrHSrv, arrVT, out arrErr);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int SetEnable(bool doEnable)
        {
            int num;
            if ((this.ifAsync == null) || (this.cpointcontainer == null))
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifAsync.SetEnable(doEnable);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int SetItemBufferEnable(int[] handles, bool[] enableState, out int[] errors)
        {
            int num;
            IntPtr ptr;
            errors = null;
            if (this.ifItemSamplingMgt == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItemSamplingMgt.SetItemBufferEnable(handles.Length, handles, enableState, out ptr);
            if (HRESULTS.Succeeded(num))
            {
                errors = new int[handles.Length];
                Marshal.Copy(ptr, errors, 0, handles.Length);
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int SetItemDeadband(int[] handles, float[] percentDeadband, out int[] errors)
        {
            int num;
            errors = null;
            if (this.ifItemDeadbandMgt == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            IntPtr zero = IntPtr.Zero;
            num = this.ifItemDeadbandMgt.SetItemDeadband(handles.Length, handles, percentDeadband, out zero);
            if (HRESULTS.Succeeded(num))
            {
                errors = new int[handles.Length];
                Marshal.Copy(zero, errors, 0, handles.Length);
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int SetItemSamplingRate(int[] handles, int[] RequestedSamplingRate, out int[] RevisedSamplingRate, out int[] errors)
        {
            int num;
            IntPtr ptr;
            IntPtr ptr2;
            errors = null;
            RevisedSamplingRate = null;
            if (this.ifItemSamplingMgt == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItemSamplingMgt.SetItemSamplingRate(handles.Length, handles, RequestedSamplingRate, out ptr2, out ptr);
            if (HRESULTS.Succeeded(num))
            {
                RevisedSamplingRate = new int[handles.Length];
                Marshal.Copy(ptr2, RevisedSamplingRate, 0, handles.Length);
                errors = new int[handles.Length];
                Marshal.Copy(ptr, errors, 0, handles.Length);
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int SetKeepAlive(int keepAliveRate, out int revKeepAliveRate)
        {
            int num;
            revKeepAliveRate = 0;
            if (this.ifGrpStateMgt2 == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifGrpStateMgt2.SetKeepAlive(keepAliveRate, out revKeepAliveRate);
            if (HRESULTS.Succeeded(num))
            {
                this.state.KeepAliveRate = revKeepAliveRate;
            }
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int SetName(string newName)
        {
            this.state.Name = newName;
            int hresultcode = this.ifMgt.SetName(newName);
            if (this.objSrv.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int ValidateItems(OPCItemDef[] arrDef, out OPCItemResult[] aRslt)
        {
            IntPtr ptr2;
            IntPtr ptr3;
            aRslt = null;
            int dwCount = 0;
            for (int i = 0; i < arrDef.Length; i++)
            {
                if (arrDef[i] != null)
                {
                    dwCount++;
                }
            }
            IntPtr pItemArray = Marshal.AllocCoTaskMem(dwCount * this.sizeOPCITEMDEF);
            int num3 = (int)pItemArray;
            OPCITEMDEFintern structure = new OPCITEMDEFintern();
            structure.wReserved = 0;
            foreach (OPCItemDef def in arrDef)
            {
                if (def != null)
                {
                    structure.szAccessPath = def.AccessPath;
                    structure.szItemID = def.ItemID;
                    structure.bActive = def.Active;
                    structure.vtRequestedDataType = (short)def.RequestedDataType;
                    structure.dwBlobSize = 0;
                    structure.pBlob = IntPtr.Zero;
                    structure.hClient = def.HandleClient;
                    Marshal.StructureToPtr(structure, (IntPtr)num3, false);
                    num3 += this.sizeOPCITEMDEF;
                }
            }
            int hresultcode = this.ifItems.ValidateItems(dwCount, pItemArray, false, out ptr2, out ptr3);
            num3 = (int)pItemArray;
            for (int j = 0; j < dwCount; j++)
            {
                Marshal.DestroyStructure((IntPtr)num3, this.typeOPCITEMDEF);
                num3 += this.sizeOPCITEMDEF;
            }
            Marshal.FreeCoTaskMem(pItemArray);
            if (HRESULTS.Failed(hresultcode))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
                }
                return hresultcode;
            }
            int num6 = (int)ptr2;
            int num7 = (int)ptr3;
            if ((num6 == 0) || (num7 == 0))
            {
                hresultcode = -2147467260;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
                }
                return hresultcode;
            }
            aRslt = new OPCItemResult[dwCount];
            for (int k = 0; k < dwCount; k++)
            {
                aRslt[k] = new OPCItemResult();
                aRslt[k].Error = Marshal.ReadInt32((IntPtr)num7);
                if (HRESULTS.Succeeded(aRslt[k].Error))
                {
                    aRslt[k].HandleServer = Marshal.ReadInt32((IntPtr)num6);
                    aRslt[k].CanonicalDataType = (VarEnum)Marshal.ReadInt16((IntPtr)(num6 + 4));
                    aRslt[k].AccessRights = (OPCACCESSRIGHTS)Marshal.ReadInt32((IntPtr)(num6 + 8));
                }
                num6 += this.sizeOPCITEMRESULT;
                num7 += 4;
            }
            Marshal.FreeCoTaskMem(ptr2);
            Marshal.FreeCoTaskMem(ptr3);
            return hresultcode;
        }

        public int Write(int[] arrHSrv, object[] arrVal, out int[] arrErr)
        {
            int num2;
            IntPtr ptr;
            arrErr = null;
            int dwCount = 0;
            for (int i = 0; i < arrVal.Length; i++)
            {
                if (arrVal[i] != null)
                {
                    dwCount++;
                }
            }
            if (dwCount > arrHSrv.Length)
            {
                num2 = -2147024809;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            num2 = this.ifSync.Write(dwCount, arrHSrv, arrVal, out ptr);
            if (HRESULTS.Failed(num2))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            arrErr = new int[dwCount];
            Marshal.Copy(ptr, arrErr, 0, dwCount);
            Marshal.FreeCoTaskMem(ptr);
            return num2;
        }

        public int Write(int[] arrHSrv, object[] arrVal, int transactionID, out int cancelID, out int[] arrErr)
        {
            int num2;
            IntPtr ptr;
            arrErr = null;
            cancelID = 0;
            int length = arrHSrv.Length;
            if (length != arrVal.Length)
            {
                num2 = -2147467260;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            if ((this.ifAsync == null) || (this.cpointcontainer == null))
            {
                num2 = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            num2 = this.ifAsync.Write(length, arrHSrv, arrVal, transactionID, out cancelID, out ptr);
            if (HRESULTS.Failed(num2))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            arrErr = new int[length];
            Marshal.Copy(ptr, arrErr, 0, length);
            Marshal.FreeCoTaskMem(ptr);
            return num2;
        }

        public int WriteVQT(ItemValue[] items)
        {
            int num;
            IntPtr ptr2;
            int dwCount = (items == null) ? 0 : items.Length;
            if (dwCount == 0)
            {
                return -2147024809;
            }
            if (this.ifSync2 == null)
            {
                num = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            OPCITEMVQT structure = new OPCITEMVQT();
            int num3 = Marshal.SizeOf(structure);
            IntPtr pItemValues = Marshal.AllocCoTaskMem(dwCount * num3);
            int num4 = (int)pItemValues;
            structure.dwReserved = 0;
            structure.wReserved = 0;
            foreach (ItemValue value2 in items)
            {
                if (value2.Quality == null)
                {
                    structure.bQualitySpecified = false;
                }
                else
                {
                    structure.bQualitySpecified = value2.QualitySpecified;
                    structure.wQuality = value2.Quality.GetCode();
                }
                structure.bTimeStampSpecified = value2.TimestampSpecified;
                structure.ftTimeStamp = value2.Timestamp.ToFileTime();
                structure.vDataValue = CustomMarshal.ToVariant(value2.Value);
                Marshal.StructureToPtr(structure, (IntPtr)num4, false);
                num4 += num3;
            }
            int[] phServer = new int[dwCount];
            for (int i = 0; i < dwCount; i++)
            {
                phServer[i] = items[i].ServerHandle;
            }
            num = this.ifSync2.WriteVQT(dwCount, phServer, pItemValues, out ptr2);
            if (HRESULTS.Failed(num))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            int[] destination = new int[dwCount];
            Marshal.Copy(ptr2, destination, 0, dwCount);
            Marshal.FreeCoTaskMem(ptr2);
            for (int j = 0; j < dwCount; j++)
            {
                items[j].Error = destination[j];
            }
            return num;
        }

        public int WriteVQT(ItemValue[] items, int transactionID, out int cancelID)
        {
            int num2;
            IntPtr ptr2;
            cancelID = 0;
            int dwCount = (items == null) ? 0 : items.Length;
            if (dwCount == 0)
            {
                num2 = -2147024809;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            if (this.ifAsync3 == null)
            {
                num2 = -2147467262;
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            int[] phServer = new int[dwCount];
            OPCITEMVQT structure = new OPCITEMVQT();
            int num3 = Marshal.SizeOf(structure);
            IntPtr pItemValues = Marshal.AllocCoTaskMem(dwCount * num3);
            int num4 = (int)pItemValues;
            structure.dwReserved = 0;
            structure.wReserved = 0;
            foreach (ItemValue value2 in items)
            {
                structure.bQualitySpecified = value2.QualitySpecified;
                structure.wQuality = value2.Quality.GetCode();
                structure.bTimeStampSpecified = value2.TimestampSpecified;
                structure.ftTimeStamp = value2.Timestamp.ToFileTime();
                structure.vDataValue = CustomMarshal.ToVariant(value2.Value);
                Marshal.StructureToPtr(structure, (IntPtr)num4, false);
                num4 += num3;
            }
            for (int i = 0; i < dwCount; i++)
            {
                phServer[i] = items[i].ServerHandle;
            }
            num2 = this.ifAsync3.WriteVQT(dwCount, phServer, pItemValues, transactionID, out cancelID, out ptr2);
            if (HRESULTS.Failed(num2))
            {
                if (this.objSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            int[] destination = new int[dwCount];
            Marshal.Copy(ptr2, destination, 0, dwCount);
            Marshal.FreeCoTaskMem(ptr2);
            for (int j = 0; j < dwCount; j++)
            {
                items[j].Error = destination[j];
            }
            return num2;
        }

        public bool Active
        {
            get
            {
                return this.state.Active;
            }
            set
            {
                int[] pActive = new int[1];
                if (value)
                {
                    pActive[0] = -1;
                }
                else
                {
                    pActive[0] = 0;
                }
                this.ifMgt.SetState(null, out this.state.UpdateRate, pActive, null, null, null, null);
                this.state.Active = value;
            }
        }

        public int HandleClient
        {
            get
            {
                return this.state.HandleClient;
            }
            set
            {
                this.ifMgt.SetState(null, out this.state.UpdateRate, null, null, null, null, new int[] { value });
                this.state.HandleClient = value;
            }
        }

        public int HandleServer
        {
            get
            {
                return this.state.HandleServer;
            }
        }

        public int LocaleID
        {
            get
            {
                return this.state.LocaleID;
            }
            set
            {
                this.ifMgt.SetState(null, out this.state.UpdateRate, null, null, null, new int[] { value }, null);
                this.state.LocaleID = value;
            }
        }

        public string Name
        {
            get
            {
                return this.state.Name;
            }
            set
            {
                this.SetName(value);
            }
        }

        public float PercentDeadband
        {
            get
            {
                return this.state.PercentDeadband;
            }
            set
            {
                this.ifMgt.SetState(null, out this.state.UpdateRate, null, null, new float[] { value }, null, null);
                this.state.PercentDeadband = value;
            }
        }

        public bool Public
        {
            get
            {
                return this.state.Public;
            }
        }

        public int TimeBias
        {
            get
            {
                return this.state.TimeBias;
            }
            set
            {
                this.ifMgt.SetState(null, out this.state.UpdateRate, null, new int[] { value }, null, null, null);
                this.state.TimeBias = value;
            }
        }

        public int UpdateRate
        {
            get
            {
                return this.state.UpdateRate;
            }
            set
            {
                this.ifMgt.SetState(new int[] { value }, out this.state.UpdateRate, null, null, null, null, null);
            }
        }
    }
}

