namespace OPCDA.NET
{
    using OPC;
    using OPCDA;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class OpcDataBind
    {
        private OpcServer opcSrv;
        private Form ParentForm;
        private int refreshCancelID;
        internal RefreshGroup refreshGrp;
        private int refreshTransactionID;
        internal Subscriptions subscriptions;
        private int updRate;
        private DataBindCallbackUserHandler userCallbackHandler;
        private DataBindCallbackErrorEventHandler userErrHandler;

        public OpcDataBind(Form parent, string serverID)
        {
            this.opcSrv = null;
            this.refreshGrp = null;
            this.updRate = 500;
            this.refreshTransactionID = 0;
            this.ParentForm = parent;
            this.Connect(serverID);
            this.Start();
        }

        public OpcDataBind(Form parent, OpcServer srv, int rate)
        {
            this.opcSrv = null;
            this.refreshGrp = null;
            this.updRate = 500;
            this.refreshTransactionID = 0;
            this.ParentForm = parent;
            this.opcSrv = srv;
            this.updRate = rate;
            this.Start();
        }

        public OpcDataBind(Form parent, string serverID, int rate)
        {
            this.opcSrv = null;
            this.refreshGrp = null;
            this.updRate = 500;
            this.refreshTransactionID = 0;
            this.ParentForm = parent;
            this.updRate = rate;
            this.Connect(serverID);
            this.Start();
        }

        public string ArrayToMultiLineString(Subscription item, object val)
        {
            string str = "";
            val.GetType().GetElementType();
            foreach (object obj2 in (Array) val)
            {
                if (str != "")
                {
                    str = str + "\r\n";
                }
                str = str + this.ValueToString(item, obj2);
            }
            return str;
        }

        public string[] ArrayToStringArray(Subscription item, object val)
        {
            string[] strArray = new string[((Array) val).Length];
            int num = 0;
            foreach (object obj2 in (Array) val)
            {
                strArray[num++] = this.ValueToString(item, obj2);
            }
            return strArray;
        }

        private void Connect(string id)
        {
            this.opcSrv = new OpcServer();
            int hresultcode = this.opcSrv.Connect(id);
            if (HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, "Connect failed with error 0x" + hresultcode.ToString("X") + ":  " + ErrorDescriptions.GetErrorDescription(hresultcode));
            }
        }

        public void Disconnect()
        {
            if (this.refreshGrp != null)
            {
                this.refreshGrp.Dispose();
            }
            if (this.opcSrv != null)
            {
                this.opcSrv.Disconnect();
            }
        }

        public void Dispose()
        {
            if (this.refreshGrp != null)
            {
                this.refreshGrp.Dispose();
            }
        }

        public void HandleListViewControl(Subscription item, ListView lv)
        {
            object val = item.Value;
            if (!val.GetType().IsArray)
            {
                while (lv.Items.Count < (item.controlIndex + 1))
                {
                    lv.Items.Add("");
                }
                lv.Items[item.controlIndex].Text = this.ValueToString(item, val);
            }
            else
            {
                int length = ((Array) val).Length;
                while (lv.Items.Count < (item.controlIndex + length))
                {
                    lv.Items.Add("");
                }
                int num2 = 0;
                foreach (object obj3 in (Array) val)
                {
                    ListViewItem item2 = lv.Items[item.controlIndex + num2++];
                    if (item.controlIndex2 != 0)
                    {
                        goto Label_00AB;
                    }
                    item2.Text = this.ValueToString(item, obj3);
                    continue;
                Label_0099:
                    item2.SubItems.Add("");
                Label_00AB:
                    if (item2.SubItems.Count < (item.controlIndex2 + 1))
                    {
                        goto Label_0099;
                    }
                    item2.SubItems[item.controlIndex2].Text = this.ValueToString(item, obj3);
                }
            }
        }

        private void iDataChangedHandler(object sender, DataChangeEventArgs e)
        {
            if ((this.ParentForm != null) && this.ParentForm.InvokeRequired)
            {
                this.ParentForm.BeginInvoke(new DataChangeEventHandler(this.iDataChangedHandler), new object[] { sender, e });
            }
            else
            {
                if (e.transactionID == this.refreshTransactionID)
                {
                    this.refreshTransactionID = 0;
                }
                for (int i = 0; i < e.sts.Length; i++)
                {
                    try
                    {
                        Subscription item = this.subscriptions.FindHandle(e.sts[i].HandleClient);
                        if (item != null)
                        {
                            item.idef.OpcIRslt.DataValue = e.sts[i].DataValue;
                            item.idef.OpcIRslt.Error = e.sts[i].Error;
                            item.idef.OpcIRslt.Quality = e.sts[i].Quality;
                            item.idef.OpcIRslt.TimeStamp = e.sts[i].TimeStamp;
                            if ((this.userCallbackHandler == null) || !this.userCallbackHandler(item, this))
                            {
                                System.Type type = item.control.GetType();
                                if ((type == typeof(TextBox)) && ((item.controlProperty == null) || (item.controlProperty == "Text")))
                                {
                                    try
                                    {
                                        TextBox control = (TextBox) item.control;
                                        if (control.Multiline && e.sts[i].DataValue.GetType().IsArray)
                                        {
                                            control.Text = this.ArrayToMultiLineString(item, e.sts[i].DataValue);
                                        }
                                        else
                                        {
                                            control.Text = this.ValueToString(item, e.sts[i].DataValue);
                                        }
                                        goto Label_1125;
                                    }
                                    catch (Exception exception)
                                    {
                                        throw new Exception(exception.Message + "  in TextBox callback from item " + item.ItemName, exception);
                                    }
                                }
                                if ((type == typeof(Label)) && ((item.controlProperty == null) || (item.controlProperty == "Text")))
                                {
                                    try
                                    {
                                        Label label = (Label) item.control;
                                        label.Text = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception2)
                                    {
                                        throw new Exception(exception2.Message + "  in Label callback from item " + item.ItemName, exception2);
                                    }
                                }
                                if ((type == typeof(CheckBox)) && ((item.controlProperty == null) || (item.controlProperty == "Checked")))
                                {
                                    try
                                    {
                                        CheckBox box2 = (CheckBox) item.control;
                                        box2.Checked = Convert.ToBoolean(e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception3)
                                    {
                                        throw new Exception(exception3.Message + "  in CheckBox callback from item " + item.ItemName, exception3);
                                    }
                                }
                                if ((type == typeof(ComboBox)) && ((item.controlProperty == null) || (item.controlProperty == "Text")))
                                {
                                    try
                                    {
                                        ComboBox box3 = (ComboBox) item.control;
                                        box3.Text = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception4)
                                    {
                                        throw new Exception(exception4.Message + "  in ComboBox callback from item " + item.ItemName, exception4);
                                    }
                                }
                                if ((type == typeof(ComboBox.ObjectCollection)) && ((item.controlProperty == null) || (item.controlProperty == "Items")))
                                {
                                    try
                                    {
                                        ComboBox.ObjectCollection objects = (ComboBox.ObjectCollection) item.control;
                                        object dataValue = e.sts[i].DataValue;
                                        if (!dataValue.GetType().IsArray)
                                        {
                                            goto Label_041E;
                                        }
                                        int length = ((Array) dataValue).Length;
                                        while (objects.Count < (item.controlIndex + length))
                                        {
                                            objects.Add("");
                                        }
                                        int num3 = 0;
                                        foreach (object obj3 in (Array) dataValue)
                                        {
                                            objects[item.controlIndex + num3++] = this.ValueToString(item, obj3);
                                        }
                                        goto Label_1125;
                                    Label_0411:
                                        objects.Add("");
                                    Label_041E:
                                        if (objects.Count < (item.controlIndex + 1))
                                        {
                                            goto Label_0411;
                                        }
                                        objects[item.controlIndex] = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception5)
                                    {
                                        throw new Exception(exception5.Message + "  in ComboBox callback from item " + item.ItemName, exception5);
                                    }
                                }
                                if ((type == typeof(ComboBox)) && (item.controlProperty == "Items"))
                                {
                                    try
                                    {
                                        ComboBox box4 = (ComboBox) item.control;
                                        ComboBox.ObjectCollection objects2 = box4.Items;
                                        object obj4 = e.sts[i].DataValue;
                                        if (!obj4.GetType().IsArray)
                                        {
                                            goto Label_0567;
                                        }
                                        int num4 = ((Array) obj4).Length;
                                        while (objects2.Count < (item.controlIndex + num4))
                                        {
                                            objects2.Add("");
                                        }
                                        int num5 = 0;
                                        foreach (object obj5 in (Array) obj4)
                                        {
                                            objects2[item.controlIndex + num5++] = this.ValueToString(item, obj5);
                                        }
                                        goto Label_1125;
                                    Label_055A:
                                        objects2.Add("");
                                    Label_0567:
                                        if (objects2.Count < (item.controlIndex + 1))
                                        {
                                            goto Label_055A;
                                        }
                                        objects2[item.controlIndex] = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception6)
                                    {
                                        throw new Exception(exception6.Message + "  in ComboBox callback from item " + item.ItemName, exception6);
                                    }
                                }
                                if ((type == typeof(ListBox)) && ((item.controlProperty == null) || (item.controlProperty == "Items")))
                                {
                                    try
                                    {
                                        ListBox box5 = (ListBox) item.control;
                                        object obj6 = e.sts[i].DataValue;
                                        if (!obj6.GetType().IsArray)
                                        {
                                            goto Label_06C3;
                                        }
                                        int num6 = ((Array) obj6).Length;
                                        while (box5.Items.Count < (item.controlIndex + num6))
                                        {
                                            box5.Items.Add("");
                                        }
                                        int num7 = 0;
                                        foreach (object obj7 in (Array) obj6)
                                        {
                                            box5.Items[item.controlIndex + num7++] = this.ValueToString(item, obj7);
                                        }
                                        goto Label_1125;
                                    Label_06B1:
                                        box5.Items.Add("");
                                    Label_06C3:
                                        if (box5.Items.Count < (item.controlIndex + 1))
                                        {
                                            goto Label_06B1;
                                        }
                                        box5.Items[item.controlIndex] = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception7)
                                    {
                                        throw new Exception(exception7.Message + "  in ListBox callback from item " + item.ItemName, exception7);
                                    }
                                }
                                if ((type == typeof(ListBox.ObjectCollection)) && ((item.controlProperty == null) || (item.controlProperty == "Items")))
                                {
                                    try
                                    {
                                        ListBox.ObjectCollection objects3 = (ListBox.ObjectCollection) item.control;
                                        object obj8 = e.sts[i].DataValue;
                                        if (!obj8.GetType().IsArray)
                                        {
                                            goto Label_0815;
                                        }
                                        int num8 = ((Array) obj8).Length;
                                        while (objects3.Count < (item.controlIndex + num8))
                                        {
                                            objects3.Add("");
                                        }
                                        int num9 = 0;
                                        foreach (object obj9 in (Array) obj8)
                                        {
                                            objects3[item.controlIndex + num9++] = this.ValueToString(item, obj9);
                                        }
                                        goto Label_1125;
                                    Label_0808:
                                        objects3.Add("");
                                    Label_0815:
                                        if (objects3.Count < (item.controlIndex + 1))
                                        {
                                            goto Label_0808;
                                        }
                                        objects3[item.controlIndex] = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception8)
                                    {
                                        throw new Exception(exception8.Message + "  in ListBox callback from item " + item.ItemName, exception8);
                                    }
                                }
                                if ((type == typeof(ListBox)) && (item.controlProperty == "Items"))
                                {
                                    try
                                    {
                                        ListBox box6 = (ListBox) item.control;
                                        ListBox.ObjectCollection objects4 = box6.Items;
                                        object obj10 = e.sts[i].DataValue;
                                        if (!obj10.GetType().IsArray)
                                        {
                                            goto Label_095E;
                                        }
                                        int num10 = ((Array) obj10).Length;
                                        while (objects4.Count < (item.controlIndex + num10))
                                        {
                                            objects4.Add("");
                                        }
                                        int num11 = 0;
                                        foreach (object obj11 in (Array) obj10)
                                        {
                                            objects4[item.controlIndex + num11++] = this.ValueToString(item, obj11);
                                        }
                                        goto Label_1125;
                                    Label_0951:
                                        objects4.Add("");
                                    Label_095E:
                                        if (objects4.Count < (item.controlIndex + 1))
                                        {
                                            goto Label_0951;
                                        }
                                        objects4[item.controlIndex] = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception9)
                                    {
                                        throw new Exception(exception9.Message + "  in ListBox callback from item " + item.ItemName, exception9);
                                    }
                                }
                                if ((type == typeof(ListView)) && ((item.controlProperty == null) || (item.controlProperty == "Items")))
                                {
                                    try
                                    {
                                        ListView lv = (ListView) item.control;
                                        this.HandleListViewControl(item, lv);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception10)
                                    {
                                        throw new Exception(exception10.Message + "  in ListView callback from item " + item.ItemName, exception10);
                                    }
                                }
                                if ((type == typeof(ListView.ListViewItemCollection)) && ((item.controlProperty == null) || (item.controlProperty == "Items")))
                                {
                                    try
                                    {
                                        ListView.ListViewItemCollection items = (ListView.ListViewItemCollection) item.control;
                                        object obj12 = e.sts[i].DataValue;
                                        if (!obj12.GetType().IsArray)
                                        {
                                            goto Label_0B0E;
                                        }
                                        int num12 = ((Array) obj12).Length;
                                        while (items.Count < (item.controlIndex + num12))
                                        {
                                            items.Add("");
                                        }
                                        int num13 = 0;
                                        foreach (object obj13 in (Array) obj12)
                                        {
                                            items[item.controlIndex + num13++].Text = this.ValueToString(item, obj13);
                                        }
                                        goto Label_1125;
                                    Label_0B01:
                                        items.Add("");
                                    Label_0B0E:
                                        if (items.Count < (item.controlIndex + 1))
                                        {
                                            goto Label_0B01;
                                        }
                                        items[item.controlIndex].Text = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception11)
                                    {
                                        throw new Exception(exception11.Message + "  in ListView callback from item " + item.ItemName, exception11);
                                    }
                                }
                                if ((type == typeof(ListViewItem.ListViewSubItemCollection)) && (item.controlProperty == null))
                                {
                                    try
                                    {
                                        ListViewItem.ListViewSubItemCollection items2 = (ListViewItem.ListViewSubItemCollection) item.control;
                                        object obj14 = e.sts[i].DataValue;
                                        if (!obj14.GetType().IsArray)
                                        {
                                            goto Label_0C4E;
                                        }
                                        int num14 = ((Array) obj14).Length;
                                        while (items2.Count < (item.controlIndex + num14))
                                        {
                                            items2.Add("");
                                        }
                                        int num15 = 0;
                                        foreach (object obj15 in (Array) obj14)
                                        {
                                            items2[item.controlIndex + num15++].Text = this.ValueToString(item, obj15);
                                        }
                                        goto Label_1125;
                                    Label_0C41:
                                        items2.Add("");
                                    Label_0C4E:
                                        if (items2.Count < (item.controlIndex + 1))
                                        {
                                            goto Label_0C41;
                                        }
                                        items2[item.controlIndex].Text = this.ValueToString(item, e.sts[i].DataValue);
                                        goto Label_1125;
                                    }
                                    catch (Exception exception12)
                                    {
                                        throw new Exception(exception12.Message + "  in ListView callback from item " + item.ItemName, exception12);
                                    }
                                }
                                if ((type == typeof(ProgressBar)) && ((item.controlProperty == null) || (item.controlProperty == "Value")))
                                {
                                    try
                                    {
                                        ProgressBar bar = (ProgressBar) item.control;
                                        if (!e.sts[i].DataValue.GetType().IsArray)
                                        {
                                            if (item.format != null)
                                            {
                                                this.Transform(item, ref e.sts[i].DataValue);
                                            }
                                            bar.Value = Convert.ToInt32(e.sts[i].DataValue);
                                        }
                                        goto Label_1125;
                                    }
                                    catch (Exception exception13)
                                    {
                                        throw new Exception(exception13.Message + "  in ProgressBar callback from item " + item.ItemName, exception13);
                                    }
                                }
                                if (type == typeof(DataBindEventHandler))
                                {
                                    DataBindEventHandler handler = (DataBindEventHandler) item.control;
                                    DataBindEventData itemData = new DataBindEventData();
                                    itemData.itemInfo = item.idef;
                                    itemData.val = e.sts[i].DataValue;
                                    itemData.quality = (OPC_QUALITY_STATUS) e.sts[i].Quality;
                                    itemData.timestamp = DateTime.FromFileTime(e.sts[i].TimeStamp);
                                    itemData.error = e.sts[i].Error;
                                    handler(this, itemData);
                                }
                                else
                                {
                                    if (item.controlProperty == null)
                                    {
                                        item.controlProperty = "Text";
                                    }
                                    PropertyInfo property = type.GetProperty(item.controlProperty);
                                    object val = null;
                                    System.Type type3 = e.sts[i].DataValue.GetType();
                                    System.Type propertyType = property.PropertyType;
                                    System.Type elementType = property.PropertyType.GetElementType();
                                    if (propertyType == typeof(DataBindEventData))
                                    {
                                        propertyType = type3;
                                        elementType = type3.GetElementType();
                                    }
                                    if (type3.IsArray)
                                    {
                                        if (propertyType.IsArray)
                                        {
                                            ArrayList list = new ArrayList();
                                            foreach (object obj18 in (Array) e.sts[i].DataValue)
                                            {
                                                object obj19;
                                                if (item.format != null)
                                                {
                                                    if (elementType == typeof(string))
                                                    {
                                                        obj19 = this.ValueToString(item, obj18);
                                                    }
                                                    else
                                                    {
                                                        obj19 = Convert.ChangeType(obj18, elementType);
                                                        this.Transform(item, ref obj19);
                                                    }
                                                }
                                                else
                                                {
                                                    obj19 = Convert.ChangeType(obj18, elementType);
                                                }
                                                list.Add(obj19);
                                            }
                                            val = list.ToArray(elementType);
                                        }
                                        else
                                        {
                                            object obj20 = null;
                                            foreach (object obj21 in (Array) e.sts[i].DataValue)
                                            {
                                                obj20 = obj21;
                                                break;
                                            }
                                            if (propertyType == typeof(string))
                                            {
                                                val = this.ValueToString(item, obj20);
                                            }
                                            else
                                            {
                                                val = Convert.ChangeType(obj20, propertyType);
                                                if (item.format != null)
                                                {
                                                    this.Transform(item, ref val);
                                                }
                                            }
                                        }
                                    }
                                    else if (propertyType.IsArray)
                                    {
                                        object obj22;
                                        if (elementType == typeof(string))
                                        {
                                            obj22 = this.ValueToString(item, e.sts[i].DataValue);
                                        }
                                        else
                                        {
                                            obj22 = Convert.ChangeType(e.sts[i].DataValue, propertyType);
                                            if (item.format != null)
                                            {
                                                this.Transform(item, ref obj22);
                                            }
                                        }
                                        ArrayList list2 = new ArrayList();
                                        list2.Add(obj22);
                                        val = list2.ToArray(elementType);
                                    }
                                    else if (property.PropertyType == typeof(string))
                                    {
                                        val = this.ValueToString(item, e.sts[i].DataValue);
                                    }
                                    else
                                    {
                                        val = Convert.ChangeType(e.sts[i].DataValue, propertyType);
                                        if (item.format != null)
                                        {
                                            this.Transform(item, ref val);
                                        }
                                    }
                                    if (property.PropertyType == typeof(DataBindEventData))
                                    {
                                        DataBindEventData data2 = new DataBindEventData();
                                        data2.val = val;
                                        data2.error = e.sts[i].Error;
                                        data2.quality = (OPC_QUALITY_STATUS) e.sts[i].Quality;
                                        data2.timestamp = DateTime.FromFileTime(e.sts[i].TimeStamp);
                                        data2.itemInfo = item.idef;
                                        property.SetValue(item.control, data2, null);
                                    }
                                    else
                                    {
                                        property.SetValue(item.control, val, null);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception14)
                    {
                        if (this.userErrHandler != null)
                        {
                            this.userErrHandler(this, exception14);
                        }
                    }
                Label_1125:;
                }
            }
        }

        public int Refresh(OPCDATASOURCE src)
        {
            int num;
            if (this.refreshTransactionID != 0)
            {
                num = -1073479163;
                if (this.opcSrv.myErrorsAsExecptions)
                {
                    throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
                }
                return num;
            }
            this.refreshTransactionID = DateTime.Now.TimeOfDay.Milliseconds;
            num = this.refreshGrp.iOpcGrp.Refresh2(src, this.refreshTransactionID, out this.refreshCancelID);
            if (this.opcSrv.myErrorsAsExecptions && HRESULTS.Failed(num))
            {
                throw new OPCException(num, ErrorDescriptions.GetErrorDescription(num));
            }
            return num;
        }

        public int RefreshCancel()
        {
            if (this.refreshTransactionID == 0)
            {
                return 1;
            }
            this.refreshTransactionID = 0;
            int hresultcode = this.refreshGrp.iOpcGrp.Cancel2(this.refreshCancelID);
            if (this.opcSrv.myErrorsAsExecptions && HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            return hresultcode;
        }

        private void Start()
        {
            DataChangeEventHandler dChgHnd = new DataChangeEventHandler(this.iDataChangedHandler);
            this.refreshGrp = new RefreshGroup(this.opcSrv, dChgHnd, this.updRate);
            this.subscriptions = new Subscriptions();
        }

        public Subscription Subscribe(string itemID, object updControl)
        {
            return this.Subscribe(itemID, updControl, null, 0, 0, null);
        }

        public Subscription Subscribe(string itemID, object updControl, DataBindFormat fmt)
        {
            return this.Subscribe(itemID, updControl, null, 0, 0, fmt);
        }

        public Subscription Subscribe(string itemID, object updControl, int index)
        {
            return this.Subscribe(itemID, updControl, null, index, 0, null);
        }

        public Subscription Subscribe(string itemID, object updControl, int index, DataBindFormat fmt)
        {
            return this.Subscribe(itemID, updControl, null, index, 0, fmt);
        }

        public Subscription Subscribe(string itemID, object updControl, string property, DataBindFormat fmt)
        {
            return this.Subscribe(itemID, updControl, property, 0, 0, fmt);
        }

        public Subscription Subscribe(string itemID, object updControl, int index, int param, DataBindFormat fmt)
        {
            return this.Subscribe(itemID, updControl, null, index, param, fmt);
        }

        private Subscription Subscribe(string itemID, object updControl, string property, int index, int index2, DataBindFormat fmt)
        {
            int num;
            VarEnum reqType = VarEnum.VT_EMPTY;
            if ((fmt != null) && (fmt.reqType != null))
            {
                reqType = types.ConvertToVarType(fmt.reqType);
            }
            int hresultcode = this.refreshGrp.Add(itemID, out num, true, reqType);
            if (HRESULTS.Failed(hresultcode))
            {
                throw new OPCException(hresultcode, "Subscribe failed with error " + ErrorDescriptions.GetErrorDescription(hresultcode));
            }
            Subscription sItem = new Subscription();
            sItem.parent = this;
            sItem.ItemName = itemID;
            sItem.control = updControl;
            sItem.controlProperty = property;
            sItem.controlIndex = index;
            sItem.controlIndex2 = index2;
            sItem.clientHandle = num;
            sItem.format = fmt;
            sItem.idef = this.refreshGrp.FindClientHandle(num);
            this.subscriptions.Add(sItem);
            return sItem;
        }

        public void Transform(Subscription item, ref object val)
        {
            if (val.GetType() == typeof(int))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(uint))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(double))
            {
                val = (((double) val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(float))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(long))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(ulong))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(short))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(ushort))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(byte))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(sbyte))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
            else if (val.GetType() == typeof(decimal))
            {
                val = (Convert.ToDouble(val) + item.format.add) * item.format.multiply;
            }
        }

        public string ValueToString(Subscription item, object val)
        {
            string str;
            if (HRESULTS.Failed(item.idef.OpcIRslt.Error))
            {
                return ("***Error 0x" + item.idef.OpcIRslt.Error.ToString("X"));
            }
            if ((item.format == null) || (item.format.format == ""))
            {
                return val.ToString();
            }
            if (val.GetType() == typeof(int))
            {
                str = ((((int) val) + ((int) item.format.add)) * ((int) item.format.multiply)).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(uint))
            {
                str = ((((uint) val) + ((uint) item.format.add)) * ((uint) item.format.multiply)).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(double))
            {
                str = ((((double) val) + item.format.add) * item.format.multiply).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(float))
            {
                str = ((((float) val) + ((float) item.format.add)) * ((float) item.format.multiply)).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(long))
            {
                str = ((((long) val) + ((long) item.format.add)) * ((long) item.format.multiply)).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(ulong))
            {
                str = ((((ulong) val) + ((ulong) item.format.add)) * ((ulong) item.format.multiply)).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(short))
            {
                str = ((short) ((((short) val) + ((short) item.format.add)) * ((short) item.format.multiply))).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(ushort))
            {
                str = ((ushort) ((((ushort) val) + ((ushort) item.format.add)) * ((ushort) item.format.multiply))).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(byte))
            {
                str = ((byte) ((((byte) val) + ((byte) item.format.add)) * ((byte) item.format.multiply))).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(sbyte))
            {
                str = ((sbyte) ((((sbyte) val) + ((sbyte) item.format.add)) * ((sbyte) item.format.multiply))).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(decimal))
            {
                str = ((((decimal) val) + ((decimal) item.format.add)) * ((decimal) item.format.multiply)).ToString(item.format.format);
            }
            else if (val.GetType() == typeof(DateTime))
            {
                str = ((DateTime) val).ToString(item.format.format);
            }
            else
            {
                str = val.ToString();
            }
            if ((item.idef.OpcIRslt.Quality < 0x40) && item.format.showNoValueOnBadQuality)
            {
                str = "";
            }
            if ((item.idef.OpcIRslt.Quality != 0xc0) && item.format.showNonGoodQuality)
            {
                str = str + "(" + item.Quality + ")";
            }
            return str;
        }

        public bool Active
        {
            get
            {
                return this.refreshGrp.iOpcGrp.Active;
            }
            set
            {
                this.refreshGrp.iOpcGrp.Active = value;
            }
        }

        public OpcServer OpcSrv
        {
            get
            {
                return this.opcSrv;
            }
        }

        public RefreshGroup RefreshGrp
        {
            get
            {
                return this.refreshGrp;
            }
        }

        public int UpdateRate
        {
            get
            {
                return this.updRate;
            }
            set
            {
                this.updRate = value;
                this.refreshGrp.UpdateRate = value;
            }
        }

        public DataBindCallbackErrorEventHandler UserCallbackErrorHandler
        {
            get
            {
                return this.userErrHandler;
            }
            set
            {
                this.userErrHandler = value;
            }
        }

        public DataBindCallbackUserHandler UserCallbackHandler
        {
            get
            {
                return this.userCallbackHandler;
            }
            set
            {
                this.userCallbackHandler = value;
            }
        }
    }
}

