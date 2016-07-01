namespace OPCDA.NET
{
    using System;

    [Serializable]
    public class ItemPropertyData
    {
        public object Data;
        public int Error;
        public int PropertyID;

        public override string ToString()
        {
            if (this.Error == 0)
            {
                return string.Concat(new object[] { "ID:", this.PropertyID, " Data:", this.Data.ToString() });
            }
            return string.Concat(new object[] { "ID:", this.PropertyID, " Error:", this.Error.ToString() });
        }
    }
}

