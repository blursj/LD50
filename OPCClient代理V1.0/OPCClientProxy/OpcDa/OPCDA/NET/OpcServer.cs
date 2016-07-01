namespace OPCDA.NET
{
    using Microsoft.Win32;
    using OPC;
    using OPC.Common;
    using OPCDA;
    using OPCDA.Interface;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Windows.Forms;

    [SuppressUnmanagedCodeSecurity, ComVisible(true), ReflectionPermission(SecurityAction.Assert, Unrestricted=true), SecurityPermission(SecurityAction.Assert, UnmanagedCode=true)]
    public class OpcServer : IOPCShutdown
    {
        private UCOMIConnectionPointContainer cpointcontainer = null;
        private IOPCBrowseServerAddressSpace ifBrowse = null;
        private IOPCBrowse ifBrowse3 = null;
        private IOPCCommon ifCommon = null;
        private IOPCItemIO ifItemIO = null;
        private IOPCItemProperties ifItmProps = null;
        private IOPCSecurityNT ifSecurityNT = null;
        private IOPCSecurityPrivate ifSecurityPriv = null;
        internal IOPCServer ifServer = null;
        private bool myConnectThroughNIOS = false;
        internal bool myErrorsAsExecptions;
        private Host myHostInfo = new Host();
        private string myServerName;
        private object OPCserverObj = null;
        private int shutdowncookie = 0;
        private UCOMIConnectionPoint shutdowncpoint = null;

        public event ShutdownRequestEventHandler ShutdownRequested;

        public OpcServer()
        {
           
        }

        public BrowseTree AddBrowseTree()
        {
            return new BrowseTree(this);
        }

        public BrowseTree AddBrowseTree(TreeView tvServer)
        {
            return new BrowseTree(this, tvServer);
        }

        public OpcGroup AddGroup(string groupName, bool setActive, int requestedUpdateRate, int ClientHandle)
        {
            float percentDeadband = 0f;
            return this.AddGroup(groupName, setActive, requestedUpdateRate, ref percentDeadband, 0, ClientHandle);
        }

        public OpcGroup AddGroup(string groupName, bool setActive, int requestedUpdateRate, int ClientHandle, out int err)
        {
            float percentDeadband = 0f;
            return this.AddGroup(groupName, setActive, requestedUpdateRate, ref percentDeadband, 0, ClientHandle, out err);
        }

        public OpcGroup AddGroup(string groupName, bool setActive, int requestedUpdateRate, ref float percentDeadband, int localeID, int ClientHandle)
        {
            int num;
            if (this.ifServer == null)
            {
                num = -2147467262;
                throw new OPCException(num, "AddGroup failed with error " + ErrorDescriptions.GetErrorDescription(num));
            }
            OpcGroup grp = new OpcGroup(this, groupName, setActive, requestedUpdateRate, ClientHandle);
           
            num = grp.internalAdd(ref percentDeadband, localeID);
            if (HRESULTS.Failed(num))
            {
                throw new OPCException(num, "AddGroup failed with error " + ErrorDescriptions.GetErrorDescription(num));
            }
            return grp;
        }

        public OpcGroup AddGroup(string groupName, bool setActive, int requestedUpdateRate, ref float percentDeadband, int localeID, int ClientHandle, out int err)
        {
            if (this.ifServer == null)
            {
                err = -2147467262;
                return null;
            }
            OpcGroup grp = new OpcGroup(this, groupName, setActive, requestedUpdateRate, ClientHandle);
          
            err = grp.internalAdd(ref percentDeadband, localeID);
            if (HRESULTS.Failed(err))
            {
                grp = null;
            }
            return grp;
        }

        public RefreshGroup AddRefreshGroup(RefreshEventHandler UserHnd)
        {
            return new RefreshGroup(this, 500, UserHnd);
        }

        public RefreshGroup AddRefreshGroup(int Rate, RefreshEventHandler UserHnd)
        {
            return new RefreshGroup(this, Rate, UserHnd);
        }

        public SyncIOGroup AddSyncIOGroup()
        {
            return new SyncIOGroup(this);
        }

        private void AdviseIOPCShutdown()
        {
            System.Type type = typeof(IOPCShutdown);
            Guid gUID = type.GUID;
            if (this.cpointcontainer != null)
            {
                this.cpointcontainer.FindConnectionPoint(ref gUID, out this.shutdowncpoint);
                if (this.shutdowncpoint != null)
                {
                    this.shutdowncpoint.Advise(this, out this.shutdowncookie);
                }
            }
        }

        public BrowseElement[] Browse(string itemID, ref string continuationPoint, int maxElements, browseFilter mode, string elementFilter, string vendorFilter, bool returnProperties, bool returnValues, int[] propertyIDs, out bool moreElements)
        {
            if (itemID == null)
            {
                itemID = "";
            }
            int pdwCount = 0;
            IntPtr zero = IntPtr.Zero;
            int hresultcode = this.ifBrowse3.Browse(itemID, ref continuationPoint, maxElements, (OPCBROWSEFILTER) mode, (elementFilter == null) ? "" : elementFilter, (vendorFilter == null) ? "" : vendorFilter, returnProperties, returnValues, (!returnProperties || (propertyIDs == null)) ? 0 : propertyIDs.Length, (!returnProperties || (propertyIDs == null)) ? new int[0] : propertyIDs, out moreElements, out pdwCount, out zero);
            if (HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, "Browse failed with error " + ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            if (pdwCount == 0)
            {
                return null;
            }
            BrowseElement[] elementArray = new BrowseElement[pdwCount];
            int num3 = (int) zero;
            for (int i = 0; i < pdwCount; i++)
            {
                elementArray[i] = new BrowseElement();
                IntPtr ptr = (IntPtr) Marshal.ReadInt32((IntPtr) num3);
                elementArray[i].Name = Marshal.PtrToStringUni(ptr);
                Marshal.FreeCoTaskMem(ptr);
                ptr = (IntPtr) Marshal.ReadInt32((IntPtr) (num3 + 4));
                elementArray[i].ItemID = Marshal.PtrToStringUni(ptr);
                Marshal.FreeCoTaskMem(ptr);
                int num5 = Marshal.ReadInt32((IntPtr) (num3 + 8));
                elementArray[i].HasChildren = (num5 & 1) != 0;
                elementArray[i].IsItem = (num5 & 2) != 0;
                ItemProperties properties = this.UnmarshalProperties((IntPtr) (num3 + 0x10));
                elementArray[i].Properties = properties.Properties;
                elementArray[i].Error = properties.Error;
                num3 += 0x20;
            }
            Marshal.FreeCoTaskMem(zero);
            return elementArray;
        }

        public int BrowseAccessPaths(string itemID, out UCOMIEnumString stringEnumerator)
        {
            int num;
            stringEnumerator = null;
            object ppUnk = null;
            if (this.ifBrowse == null)
            {
                num = -2147467262;
            }
            else
            {
                num = this.ifBrowse.BrowseAccessPaths(itemID, out ppUnk);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            if (HRESULTS.Succeeded(num))
            {
                stringEnumerator = (UCOMIEnumString) ppUnk;
            }
            return num;
        }

        public int BrowseCurrentBranch(OPCBROWSETYPE filterType, string NameFilter, VarEnum dataTypeFilter, OPCACCESSRIGHTS accessRightsFilter, out ArrayList NameArr)
        {
            UCOMIEnumString str;
            NameArr = new ArrayList();
            int rtc = this.BrowseOPCItemIDs(filterType, NameFilter, dataTypeFilter, accessRightsFilter, out str);
            if (rtc == 0)
            {
                if (str == null)
                {
                    rtc = -2147467259;
                    if (this.myErrorsAsExecptions)
                    {
                        throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                    }
                    return rtc;
                }
                NameArr = new ArrayList(500);
                string[] rgelt = new string[100];
                str.Reset();
                do
                {
                    int pceltFetched = 0;
                    rtc = str.Next(rgelt.Length, rgelt, out pceltFetched);
                    if (pceltFetched > 0)
                    {
                        for (int i = 0; i < pceltFetched; i++)
                        {
                            if (rgelt[i] == null)
                            {
                                break;
                            }
                            NameArr.Add(rgelt[i]);
                        }
                    }
                }
                while (rtc == 0);
                Marshal.ReleaseComObject(str);
                str = null;
                NameArr.TrimToSize();
                return 0;
            }
            if (str != null)
            {
                Marshal.ReleaseComObject(str);
            }
            if (this.myErrorsAsExecptions)
            {
                throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
            }
            return rtc;
        }

        public int BrowseCurrentBranch(OPCBROWSETYPE filterType, string NameFilter, System.Type sTypeFilter, OPCACCESSRIGHTS accessRightsFilter, out ArrayList NameArr)
        {
            int hresultcode = this.BrowseCurrentBranch(filterType, NameFilter, types.ConvertToVarType(sTypeFilter), accessRightsFilter, out NameArr);
            if (this.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int BrowseOPCItemIDs(OPCBROWSETYPE filterType, string filterCriteria, VarEnum dataTypeFilter, OPCACCESSRIGHTS accessRightsFilter, out UCOMIEnumString stringEnumerator)
        {
            int num;
            stringEnumerator = null;
            if (this.ifBrowse == null)
            {
                num = -2147467262;
            }
            else
            {
                object obj2;
                num = this.ifBrowse.BrowseOPCItemIDs(filterType, filterCriteria, (short) dataTypeFilter, accessRightsFilter, out obj2);
                stringEnumerator = (UCOMIEnumString) obj2;
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int BrowseOPCItemIDs(OPCBROWSETYPE filterType, string filterCriteria, System.Type sTypeFilter, OPCACCESSRIGHTS accessRightsFilter, out UCOMIEnumString stringEnumerator)
        {
            int hresultcode = this.BrowseOPCItemIDs(filterType, filterCriteria, types.ConvertToVarType(sTypeFilter), accessRightsFilter, out stringEnumerator);
            if (this.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int BrowseOPCItemIDs(OPCBROWSETYPE filterType, string filterCriteria, System.Type sTypeFilter, OPCACCESSRIGHTS accessRightsFilter, out string[] nodes)
        {
            UCOMIEnumString str;
            nodes = new string[0];
            int hresultcode = this.BrowseOPCItemIDs(filterType, filterCriteria, types.ConvertToVarType(sTypeFilter), accessRightsFilter, out str);
            if (hresultcode == 0)
            {
                int num3;
                string[] rgelt = new string[1];
                int celt = 0;
                while (str.Next(1, rgelt, out num3) == 0)
                {
                    celt++;
                }
                nodes = new string[celt];
                str.Reset();
                str.Next(celt, nodes, out num3);
            }
            Marshal.ReleaseComObject(str);
            if (this.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int ChangeBrowsePosition(OPCBROWSEDIRECTION direction, string name)
        {
            int num;
            if (this.ifBrowse == null)
            {
                num = -2147467262;
            }
            else
            {
                num = this.ifBrowse.ChangeBrowsePosition(direction, name);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int ChangeUser()
        {
            int num;
            if (this.ifSecurityNT == null)
            {
                num = -2147467263;
            }
            else
            {
                num = this.ifSecurityNT.ChangeUser();
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int Connect(Guid ClsidOPCserver)
        {
            this.Disconnect();
            System.Type typeFromCLSID = System.Type.GetTypeFromCLSID(ClsidOPCserver);
            if (typeFromCLSID == null)
            {
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(-1073479663, ErrorDescriptions.GetErrorDescription(-1073479663));
                }
                return -1073479663;
            }
            int hresultcode = this.SetInterfaces(Activator.CreateInstance(typeFromCLSID));
            if (HRESULTS.Succeeded(hresultcode))
            {
                this.myServerName = ClsidOPCserver.ToString();
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int Connect(string SrvName)
        {
            int num;
            if (SrvName.StartsWith(@"\\") || SrvName.StartsWith("//"))
            {
                if (SrvName.StartsWith("//"))
                {
                    SrvName.Replace("/", @"\");
                }
                int index = SrvName.IndexOf('\\', 2);
                string srvName = SrvName.Substring(index + 1);
                string computerName = SrvName.Substring(2, index - 2);
                if (srvName == "")
                {
                    num = -2147024809;
                }
                else if (computerName == "")
                {
                    num = this.ConnectLocal(SrvName);
                }
                else if (this.myConnectThroughNIOS)
                {
                    num = this.ConnectRemoteNIOS(computerName, srvName);
                }
                else
                {
                    Host accessInfo = new Host(computerName);
                    num = this.Connect(accessInfo, srvName);
                }
            }
            else
            {
                num = this.ConnectLocal(SrvName);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int Connect(Host accessInfo, Guid ClsidOPCserver)
        {
            this.Disconnect();
            int hresultcode = this.SetInterfaces(ComApi.CreateInstance(ClsidOPCserver, accessInfo));
            if (HRESULTS.Succeeded(hresultcode))
            {
                this.myHostInfo = accessInfo;
                this.myServerName = ClsidOPCserver.ToString();
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int Connect(Host accessInfo, string SrvName)
        {
            int num;
            string[] strArray;
            Guid[] guidArray;
            if (this.myConnectThroughNIOS)
            {
                num = this.ConnectRemoteNIOS(accessInfo.HostName, SrvName);
                if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            OpcServerBrowser browser = new OpcServerBrowser(accessInfo);
            Guid empty = Guid.Empty;
            bool flag = false;
            browser.GetServerList(out strArray, out guidArray);
            if (strArray == null)
            {
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(-1073479663, ErrorDescriptions.GetErrorDescription(-1073479663));
                }
                return -1073479663;
            }
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i] == SrvName)
                {
                    empty = guidArray[i];
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(-1073479663, ErrorDescriptions.GetErrorDescription(-1073479663));
                }
                return -1073479663;
            }
            strArray = null;
            guidArray = null;
            num = this.SetInterfaces(ComApi.CreateInstance(empty, accessInfo));
            if (HRESULTS.Succeeded(num))
            {
                this.myHostInfo = accessInfo;
                this.myServerName = SrvName;
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int Connect(string ComputerName, Guid ClsidOPCserver)
        {
            this.Disconnect();
            Host host = new Host(ComputerName);
            int hresultcode = this.SetInterfaces(ComApi.CreateInstance(ClsidOPCserver, host));
            if (HRESULTS.Succeeded(hresultcode))
            {
                this.myHostInfo.HostName = ComputerName;
                this.myServerName = ClsidOPCserver.ToString();
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int Connect(string ComputerName, string SrvName)
        {
            int num;
            if ((ComputerName != null) && (ComputerName != ""))
            {
                if (this.myConnectThroughNIOS)
                {
                    num = this.ConnectRemoteNIOS(ComputerName, SrvName);
                }
                else
                {
                    Host accessInfo = new Host(ComputerName);
                    num = this.Connect(accessInfo, SrvName);
                }
            }
            else
            {
                num = this.Connect(SrvName);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        private int ConnectLocal(string SrvName)
        {
            System.Type typeFromProgID = null;
            bool throwOnError = false;
            this.Disconnect();
            typeFromProgID = System.Type.GetTypeFromProgID(SrvName, throwOnError);
            if (typeFromProgID == null)
            {
                return -1073479663;
            }
            int hresultcode = this.SetInterfaces(Activator.CreateInstance(typeFromProgID));
            if (HRESULTS.Succeeded(hresultcode))
            {
                this.myHostInfo.HostName = ComApi.GetComputerName();
            }
            this.myServerName = SrvName;
            return hresultcode;
        }

        private int ConnectRemoteNIOS(string ComputerName, string SrvName)
        {
            System.Type type = null;
            bool throwOnError = false;
            this.Disconnect();
            type = System.Type.GetTypeFromProgID(SrvName, ComputerName, throwOnError);
            if (type == null)
            {
                return -1073479663;
            }
            int hresultcode = this.SetInterfaces(Activator.CreateInstance(type));
            if (HRESULTS.Succeeded(hresultcode))
            {
                this.myHostInfo.HostName = ComputerName;
            }
            this.myServerName = SrvName;
            return hresultcode;
        }

        public void Disconnect()
        {
            this.myHostInfo = new Host();
            this.myServerName = "";
            if (this.shutdowncpoint != null)
            {
                if (this.shutdowncookie != 0)
                {
                    try
                    {
                        this.shutdowncpoint.Unadvise(this.shutdowncookie);
                    }
                    catch
                    {
                    }
                    this.shutdowncookie = 0;
                }
                try
                {
                    Marshal.ReleaseComObject(this.shutdowncpoint);
                }
                catch
                {
                }
                this.shutdowncpoint = null;
            }
          
            this.cpointcontainer = null;
            this.ifBrowse = null;
            this.ifItmProps = null;
            this.ifCommon = null;
            this.ifSecurityNT = null;
            this.ifSecurityPriv = null;
            this.ifServer = null;
            if (this.OPCserverObj != null)
            {
                try
                {
                    Marshal.ReleaseComObject(this.OPCserverObj);
                }
                catch
                {
                }
                this.OPCserverObj = null;
            }
        }

        ~OpcServer()
        {
            this.Disconnect();
        }
        public string GetErrorString(int errorCode, int localeID)
        {
            string ppString = null;
            int hresultcode = this.ifServer.GetErrorString(errorCode, localeID, out ppString);
            if (HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, "GetErrorString failed with error code {hr}");
            }
            return ppString;
        }

        public string GetItemID(string itemDataID)
        {
            string szItemID = null;
            int itemID;
            if (this.ifBrowse == null)
            {
                itemID = -2147467262;
            }
            else
            {
                itemID = this.ifBrowse.GetItemID(itemDataID, out szItemID);
            }
            if (HRESULTS.Failed(itemID))
            {
                throw new OPCException(itemID, "GetItemID failed with error " + ErrorDescriptions.GetErrorDescription(itemID));
            }
            return szItemID;
        }

        public int GetItemProperties(string itemID, int[] propertyIDs, out ItemPropertyData[] propertiesData)
        {
            int num;
            IntPtr ptr;
            IntPtr ptr2;
            propertiesData = null;
            int length = propertyIDs.Length;
            if (length < 1)
            {
                num = -2147467259;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            if (this.ifItmProps == null)
            {
                num = -2147467262;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            num = this.ifItmProps.GetItemProperties(itemID, length, propertyIDs, out ptr, out ptr2);
            if (HRESULTS.Failed(num))
            {
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            int num3 = (int) ptr;
            int num4 = (int) ptr2;
            if ((num3 == 0) || (num4 == 0))
            {
                num = -2147467260;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            propertiesData = new ItemPropertyData[length];
            for (int i = 0; i < length; i++)
            {
                propertiesData[i] = new ItemPropertyData();
                propertiesData[i].PropertyID = propertyIDs[i];
                propertiesData[i].Error = Marshal.ReadInt32((IntPtr) num4);
                num4 += 4;
                if (propertiesData[i].Error == 0)
                {
                    propertiesData[i].Data = Marshal.GetObjectForNativeVariant((IntPtr) num3);
                    DUMMY_VARIANT.VariantClear((IntPtr) num3);
                }
                else
                {
                    propertiesData[i].Data = null;
                }
                num3 += DUMMY_VARIANT.ConstSize;
            }
            Marshal.FreeCoTaskMem(ptr);
            Marshal.FreeCoTaskMem(ptr2);
            return 0;
        }

        public int GetLocaleID(out int lcid)
        {
            int localeID;
            lcid = 0;
            if (this.ifCommon == null)
            {
                localeID = -2147467262;
            }
            else
            {
                localeID = this.ifCommon.GetLocaleID(out lcid);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(localeID))
            {
                throw new OPCException(localeID, ErrorDescriptions.GetErrorDescription(localeID));
            }
            return localeID;
        }

        public ItemProperties[] GetProperties(string[] itemIDs, bool returnValues, int[] propertyIDs)
        {
            IntPtr zero = IntPtr.Zero;
            int hresultcode = this.ifBrowse3.GetProperties(itemIDs.Length, itemIDs, returnValues, (propertyIDs == null) ? 0 : propertyIDs.Length, (propertyIDs == null) ? new int[0] : propertyIDs, out zero);
            if (HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, "GetProperties failed with error " + ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            ItemProperties[] propertiesArray = new ItemProperties[itemIDs.Length];
            int num2 = (int) zero;
            for (int i = 0; i < itemIDs.Length; i++)
            {
                propertiesArray[i] = this.UnmarshalProperties((IntPtr) num2);
                propertiesArray[i].ItemID = itemIDs[i];
                num2 += 0x10;
            }
            Marshal.FreeCoTaskMem(zero);
            return propertiesArray;
        }

        public int GetStatus(out SERVERSTATUS serverStatus)
        {
            int status = this.ifServer.GetStatus(out serverStatus);
            if (this.myErrorsAsExecptions && HRESULTS.Failed(status))
            {
                throw new OPCException(status, ErrorDescriptions.GetErrorDescription(status));
            }
            return status;
        }

        public int GetStatus(out SrvStatus serverStatus)
        {
            SERVERSTATUS serverstatus;
            int status = this.ifServer.GetStatus(out serverstatus);
            serverStatus = new SrvStatus();
            serverStatus.CurrentTime = DateTime.FromFileTime(serverstatus.ftCurrentTime);
            serverStatus.LastUpdateTime = DateTime.FromFileTime(serverstatus.ftLastUpdateTime);
            serverStatus.StartTime = DateTime.FromFileTime(serverstatus.ftStartTime);
            serverStatus.BandWidth = serverstatus.dwBandWidth;
            serverStatus.BuildNumber = serverstatus.wBuildNumber;
            serverStatus.GroupCount = serverstatus.dwGroupCount;
            serverStatus.MajorVersion = serverstatus.wMajorVersion;
            serverStatus.MinorVersion = serverstatus.wMinorVersion;
            serverStatus.ServerState = serverstatus.eServerState;
            serverStatus.VendorInfo = serverstatus.szVendorInfo;
            if (this.myErrorsAsExecptions && HRESULTS.Failed(status))
            {
                throw new OPCException(status, ErrorDescriptions.GetErrorDescription(status));
            }
            return status;
        }

        public int GetStatus(out SrvStatus2 serverStatus)
        {
            SERVERSTATUS serverstatus;
            int status = this.ifServer.GetStatus(out serverstatus);
            serverStatus = new SrvStatus2();
            serverStatus.CurrentTime = DateTime.FromFileTime(serverstatus.ftCurrentTime);
            serverStatus.LastUpdateTime = DateTime.FromFileTime(serverstatus.ftLastUpdateTime);
            serverStatus.StartTime = DateTime.FromFileTime(serverstatus.ftStartTime);
            serverStatus.BandWidth = serverstatus.dwBandWidth;
            serverStatus.BuildNumber = serverstatus.wBuildNumber;
            serverStatus.GroupCount = serverstatus.dwGroupCount;
            serverStatus.MajorVersion = serverstatus.wMajorVersion;
            serverStatus.MinorVersion = serverstatus.wMinorVersion;
            serverStatus.ServerState = serverstatus.eServerState;
            serverStatus.VendorInfo = serverstatus.szVendorInfo;
            if (this.myErrorsAsExecptions && HRESULTS.Failed(status))
            {
                throw new OPCException(status, ErrorDescriptions.GetErrorDescription(status));
            }
            return status;
        }

        public int IsAvailableNT(out bool available)
        {
            int num;
            available = false;
            if (this.ifSecurityNT == null)
            {
                num = -2147467263;
            }
            else
            {
                num = this.ifSecurityNT.IsAvailableNT(out available);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int IsAvailablePriv(out bool available)
        {
            int num;
            available = false;
            if (this.ifSecurityPriv == null)
            {
                num = -2147467263;
            }
            else
            {
                num = this.ifSecurityPriv.IsAvailablePriv(out available);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        internal bool isConnected()
        {
            return ((this.OPCserverObj != null) && (this.ifServer != null));
        }

        public int Logoff()
        {
            int num;
            if (this.ifSecurityPriv == null)
            {
                num = -2147467263;
            }
            else
            {
                num = this.ifSecurityPriv.Logoff();
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int Logon(string userID, string password)
        {
            int num;
            if (this.ifSecurityPriv == null)
            {
                num = -2147467263;
            }
            else
            {
                num = this.ifSecurityPriv.Logon(userID, password);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int LookupItemIDs(string itemID, int[] propertyIDs, out ItemPropertyItemID[] propertyItems)
        {
            int num2;
            IntPtr ptr;
            IntPtr ptr2;
            propertyItems = null;
            int length = propertyIDs.Length;
            if (length < 1)
            {
                num2 = -1073479165;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            if (this.ifItmProps == null)
            {
                num2 = -2147467262;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            num2 = this.ifItmProps.LookupItemIDs(itemID, length, propertyIDs, out ptr2, out ptr);
            if (HRESULTS.Failed(num2))
            {
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            int num3 = (int) ptr2;
            int num4 = (int) ptr;
            if ((num3 == 0) || (num4 == 0))
            {
                num2 = -2147467260;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            propertyItems = new ItemPropertyItemID[length];
            for (int i = 0; i < length; i++)
            {
                propertyItems[i] = new ItemPropertyItemID();
                propertyItems[i].PropertyID = propertyIDs[i];
                propertyItems[i].Error = Marshal.ReadInt32((IntPtr) num4);
                num4 += 4;
                if (propertyItems[i].Error == 0)
                {
                    IntPtr ptr3 = (IntPtr) Marshal.ReadInt32((IntPtr) num3);
                    propertyItems[i].newItemID = Marshal.PtrToStringUni(ptr3);
                    Marshal.FreeCoTaskMem(ptr3);
                }
                else
                {
                    propertyItems[i].newItemID = null;
                }
                num3 += 4;
            }
            Marshal.FreeCoTaskMem(ptr2);
            Marshal.FreeCoTaskMem(ptr);
            return 0;
        }

        void IOPCShutdown.ShutdownRequest(string shutdownReason)
        {
            ShutdownRequestEventArgs e = new ShutdownRequestEventArgs(shutdownReason);
            if (this.ShutdownRequested != null)
            {
                this.ShutdownRequested(this, e);
            }
        }

        public int QueryAvailableLocaleIDs(out int[] lcids)
        {
            int num;
            IntPtr ptr;
            lcids = null;
            int rtc = 0;
            if (this.ifCommon == null)
            {
                rtc = -2147467262;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            rtc = this.ifCommon.QueryAvailableLocaleIDs(out num, out ptr);
            if (HRESULTS.Failed(rtc))
            {
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            if (((int) ptr) == 0)
            {
                rtc = -2147467259;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            if (num < 1)
            {
                Marshal.FreeCoTaskMem(ptr);
                rtc = -2147467259;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(rtc, ErrorDescriptions.GetErrorDescription(rtc));
                }
                return rtc;
            }
            lcids = new int[num];
            Marshal.Copy(ptr, lcids, 0, num);
            Marshal.FreeCoTaskMem(ptr);
            return 0;
        }

        public int QueryAvailableProperties(string itemID, out OPCItemProperty[] opcProperties)
        {
            int num2;
            opcProperties = null;
            int dwCount = 0;
            IntPtr zero = IntPtr.Zero;
            IntPtr ppDescriptions = IntPtr.Zero;
            IntPtr ppvtDataTypes = IntPtr.Zero;
            if (this.ifItmProps == null)
            {
                num2 = -2147467262;
            }
            else
            {
                num2 = this.ifItmProps.QueryAvailableProperties(itemID, out dwCount, out zero, out ppDescriptions, out ppvtDataTypes);
            }
            if ((HRESULTS.Failed(num2) || (dwCount == 0)) || (dwCount > 0x2710))
            {
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            int num3 = (int) zero;
            int num4 = (int) ppDescriptions;
            int num5 = (int) ppvtDataTypes;
            if (((num3 == 0) || (num4 == 0)) || (num5 == 0))
            {
                num2 = -2147467260;
                if (this.myErrorsAsExecptions)
                {
                    throw new OPCException(num2, ErrorDescriptions.GetErrorDescription(num2));
                }
                return num2;
            }
            opcProperties = new OPCItemProperty[dwCount];
            for (int i = 0; i < dwCount; i++)
            {
                opcProperties[i] = new OPCItemProperty();
                opcProperties[i].PropertyID = Marshal.ReadInt32((IntPtr) num3);
                num3 += 4;
                IntPtr ptr = (IntPtr) Marshal.ReadInt32((IntPtr) num4);
                num4 += 4;
                opcProperties[i].Description = Marshal.PtrToStringUni(ptr);
                Marshal.FreeCoTaskMem(ptr);
                opcProperties[i].DataType = (VarEnum) Marshal.ReadInt16((IntPtr) num5);
                num5 += 2;
            }
            Marshal.FreeCoTaskMem(zero);
            Marshal.FreeCoTaskMem(ppDescriptions);
            Marshal.FreeCoTaskMem(ppvtDataTypes);
            return 0;
        }

        public int QueryMinImpersonationLevel(out int minImpLevel)
        {
            int num;
            minImpLevel = 0;
            if (this.ifSecurityNT == null)
            {
                num = -2147467263;
            }
            else
            {
                num = this.ifSecurityNT.QueryMinImpersonationLevel(out minImpLevel);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public OPCNAMESPACETYPE QueryOrganization()
        {
            int num;
            OPCNAMESPACETYPE pNameSpaceType = OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL;
            if (this.ifBrowse == null)
            {
                num = -2147467262;
            }
            else
            {
                num = this.ifBrowse.QueryOrganization(out pNameSpaceType);
            }
            if (num != 0)
            {
                throw new OPCException(num, "QueryOrganization failed with error " + ErrorDescriptions.GetErrorDescription(num));
            }
            return pNameSpaceType;
        }

        public ItemValue[] Read(string[] itemIDs, int[] maxAges)
        {
            int count = (itemIDs == null) ? 0 : itemIDs.Length;
            IntPtr zero = IntPtr.Zero;
            IntPtr ppwQualities = IntPtr.Zero;
            IntPtr ppftTimeStamps = IntPtr.Zero;
            IntPtr ppErrors = IntPtr.Zero;
            int hresultcode = this.ifItemIO.Read(count, itemIDs, maxAges, out zero, out ppwQualities, out ppftTimeStamps, out ppErrors);
            if (HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, "Read failed with error " + ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            object[] objArray = CustomMarshal.ToObjects(count, ref zero);
            OPCQuality[] qualityArray = CustomMarshal.ToQualities(count, ref ppwQualities);
            DateTime[] timeArray = CustomMarshal.ToDateTimes(count, ref ppftTimeStamps);
            int[] destination = new int[count];
            Marshal.Copy(ppErrors, destination, 0, count);
            ItemValue[] valueArray = new ItemValue[count];
            for (int i = 0; i < count; i++)
            {
                valueArray[i] = new ItemValue();
                valueArray[i].ItemID = itemIDs[i];
                valueArray[i].ClientHandle = 0;
                valueArray[i].ServerHandle = 0;
                valueArray[i].Value = objArray[i];
                valueArray[i].Quality = qualityArray[i];
                valueArray[i].Timestamp = timeArray[i];
                valueArray[i].Error = destination[i];
            }
            Marshal.FreeCoTaskMem(ppErrors);
            return valueArray;
        }

        public ItemValue[] Read(string[] itemIDs, int maxAge)
        {
            int[] maxAges = new int[itemIDs.Length];
            for (int i = 0; i < itemIDs.Length; i++)
            {
                maxAges[i] = maxAge;
            }
            return this.Read(itemIDs, maxAges);
        }

        public int RemoveGroup(OpcGroup groupObject, bool bForce)
        {
            int hresultcode = groupObject.Remove(bForce);
            if (this.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        public int SetClientName(string name)
        {
            int num;
            if (this.ifCommon == null)
            {
                num = -2147467262;
            }
            else
            {
                num = this.ifCommon.SetClientName(name);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        private int SetInterfaces(object srvObj)
        {
           
            this.OPCserverObj = srvObj;
            this.ifServer = (IOPCServer) this.OPCserverObj;
            if (this.ifServer == null)
            {
                return -2147220992;
            }
            try
            {
                this.ifCommon = (IOPCCommon) this.OPCserverObj;
            }
            catch
            {
            }
            try
            {
                this.ifSecurityNT = (IOPCSecurityNT) this.OPCserverObj;
            }
            catch
            {
            }
            try
            {
                this.ifSecurityPriv = (IOPCSecurityPrivate) this.OPCserverObj;
            }
            catch
            {
            }
            try
            {
                this.ifBrowse = (IOPCBrowseServerAddressSpace) this.ifServer;
            }
            catch
            {
            }
            try
            {
                this.ifItmProps = (IOPCItemProperties) this.ifServer;
            }
            catch
            {
            }
            try
            {
                this.cpointcontainer = (UCOMIConnectionPointContainer) this.OPCserverObj;
                this.AdviseIOPCShutdown();
            }
            catch
            {
            }
            try
            {
                this.ifBrowse3 = (IOPCBrowse) this.ifServer;
            }
            catch
            {
            }
            try
            {
                this.ifItemIO = (IOPCItemIO) this.ifServer;
            }
            catch
            {
            }
            return 0;
        }

        public int SetLocaleID(int lcid)
        {
            int num;
            if (this.ifCommon == null)
            {
                num = -2147467262;
            }
            else
            {
                num = this.ifCommon.SetLocaleID(lcid);
            }
            if (this.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public OPCDA.NET.ShowBrowseTree ShowBrowseTree(TreeView tvServer)
        {
            return new OPCDA.NET.ShowBrowseTree(this, tvServer);
        }

        public OPCDA.NET.ShowBrowseTreeList ShowBrowseTreeList(TreeView tvBranches, ListView lvItems)
        {
            return new OPCDA.NET.ShowBrowseTreeList(this, tvBranches, lvItems);
        }

        private ItemProperties UnmarshalProperties(IntPtr iProp)
        {
            int num = (int) iProp;
            ItemProperties properties = new ItemProperties();
            properties.Error = Marshal.ReadInt32((IntPtr) num);
            int num2 = Marshal.ReadInt32((IntPtr) (num + 4));
            IntPtr ptr = (IntPtr) Marshal.ReadInt32((IntPtr) (num + 8));
            if (!HRESULTS.Succeeded(properties.Error) || ((num2 != 0) && !HRESULTS.Failed(properties.Error)))
            {
                properties.Properties = new Property[num2];
                int num3 = (int) ptr;
                for (int i = 0; i < num2; i++)
                {
                    properties.Properties[i] = new Property();
                    properties.Properties[i].DataType = (VarEnum) Marshal.ReadInt16((IntPtr) num3);
                    properties.Properties[i].ID = Marshal.ReadInt32((IntPtr) (num3 + 4));
                    IntPtr ptr2 = (IntPtr) Marshal.ReadInt32((IntPtr) (num3 + 8));
                    properties.Properties[i].ItemID = Marshal.PtrToStringUni(ptr2);
                    Marshal.FreeCoTaskMem(ptr2);
                    ptr2 = (IntPtr) Marshal.ReadInt32((IntPtr) (num3 + 12));
                    properties.Properties[i].Description = Marshal.PtrToStringUni(ptr2);
                    Marshal.FreeCoTaskMem(ptr2);
                    object objectForNativeVariant = Marshal.GetObjectForNativeVariant((IntPtr) (num3 + 0x10));
                    properties.Properties[i].Error = Marshal.ReadInt32((IntPtr) (num3 + 0x20));
                    properties.Properties[i].Value = this.UnmarshalPropertyValue(properties.Properties[i].ID, objectForNativeVariant);
                    num3 += 40;
                }
                Marshal.FreeCoTaskMem(ptr);
            }
            return properties;
        }

        private object UnmarshalPropertyValue(int propertyID, object propertyValue)
        {
            switch (propertyID)
            {
                case 1:
                    return Enum.ToObject(typeof(VarEnum), Convert.ToInt16(propertyValue));

                case 2:
                case 4:
                case 6:
                    return propertyValue;

                case 3:
                    return new OPCQuality((qualityBits) Convert.ToInt16(propertyValue));

                case 5:
                    return Enum.ToObject(typeof(OPCACCESSRIGHTS), Convert.ToInt32(propertyValue));

                case 7:
                    return Enum.ToObject(typeof(OPCEUTYPE), Convert.ToInt32(propertyValue));
            }
            return propertyValue;
        }

        private static string WrapperVersion()
        {
            string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            int length = fileVersion.LastIndexOf('.');
            return fileVersion.Substring(0, length);
        }

        public ItemValue[] Write(ItemValue[] items)
        {
            int dwCount = (items == null) ? 0 : items.Length;
            string[] pszItemIDs = new string[dwCount];
            OPCITEMVQT structure = new OPCITEMVQT();
            int num2 = Marshal.SizeOf(structure);
            IntPtr pItemVQT = Marshal.AllocCoTaskMem(dwCount * num2);
            int num3 = (int) pItemVQT;
            structure.dwReserved = 0;
            structure.wReserved = 0;
            foreach (ItemValue value2 in items)
            {
                structure.vDataValue = CustomMarshal.ToVariant(value2.Value);
                structure.bQualitySpecified = value2.QualitySpecified;
                structure.wQuality = value2.Quality.GetCode();
                structure.bTimeStampSpecified = value2.TimestampSpecified;
                structure.ftTimeStamp = value2.Timestamp.ToFileTime();
                Marshal.StructureToPtr(structure, (IntPtr) num3, false);
                num3 += num2;
            }
            for (int i = 0; i < dwCount; i++)
            {
                pszItemIDs[i] = items[i].ItemID;
            }
            IntPtr zero = IntPtr.Zero;
            int hresultcode = this.ifItemIO.WriteVQT(dwCount, pszItemIDs, pItemVQT, out zero);
            if (HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, "Write failed with error " + ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            int[] destination = new int[dwCount];
            Marshal.Copy(zero, destination, 0, dwCount);
            for (int j = 0; j < dwCount; j++)
            {
                items[j].Error = destination[j];
            }
            Marshal.FreeCoTaskMem(zero);
            return items;
        }

        public bool ConnectThroughNIOS
        {
            get
            {
                return this.myConnectThroughNIOS;
            }
            set
            {
                this.myConnectThroughNIOS = value;
            }
        }

        public bool ErrorsAsExecptions
        {
            get
            {
                return this.myErrorsAsExecptions;
            }
            set
            {
                this.myErrorsAsExecptions = value;
            }
        }

        public Host HostInfo
        {
            get
            {
                return this.myHostInfo;
            }
        }

        public string ServerName
        {
            get
            {
                return this.myServerName;
            }
        }

        public static string Version
        {
            get
            {
                return WrapperVersion();
            }
        }
    }
}

