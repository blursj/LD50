namespace OPCDA.NET
{
    using OPC;
    using OPCDA.Interface;
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class OPCItemProperty
    {
        public VarEnum DataType;
        public string Description;
        public int PropertyID;

        public override string ToString()
        {
            return string.Concat(new object[] { "ID:", this.PropertyID, " '", this.Description, "' T:", DUMMY_VARIANT.VarEnumToString(this.DataType) });
        }

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

