namespace OPCDA.NET
{
    using OPC;
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class Property
    {
        public VarEnum DataType = VarEnum.VT_EMPTY;
        public string Description = null;
        public int Error = 0;
        public int ID = 0;
        public string ItemID = null;
        public object Value = null;

        public Type DataTypeS
        {
            get
            {
                return types.ConvertToSystemType(this.DataType);
            }
            set
            {
                this.DataType = types.ConvertToVarType(value);
            }
        }
    }
}

