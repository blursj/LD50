namespace OPCDA.NET
{
    using System;

    public class BrowseElement
    {
        public int Error = 0;
        public bool HasChildren = false;
        public bool IsItem = false;
        public string ItemID = null;
        public string Name = null;
        public Property[] Properties = null;

        public Property GetProperty(int id)
        {
            if (this.Properties != null)
            {
                foreach (Property property in this.Properties)
                {
                    if (property.ID == id)
                    {
                        return property;
                    }
                }
            }
            return null;
        }
    }
}

