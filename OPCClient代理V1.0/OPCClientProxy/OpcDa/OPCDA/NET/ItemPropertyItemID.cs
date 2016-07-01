namespace OPCDA.NET
{
    using System;

    [Serializable]
    public class ItemPropertyItemID
    {
        public int Error;
        public string newItemID;
        public int PropertyID;

        public override string ToString()
        {
            if (this.Error == 0)
            {
                return string.Concat(new object[] { "ID:", this.PropertyID, " newID:", this.newItemID });
            }
            return string.Concat(new object[] { "ID:", this.PropertyID, " Error:", this.Error.ToString() });
        }
    }
}

