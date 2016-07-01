namespace OPCDA.NET
{
    using System;

    [Serializable]
    public class ItemLists
    {
        public ItemList[] ItemDefLists;

        public ItemList Find(string name)
        {
            foreach (ItemList list in this.ItemDefLists)
            {
                if (list.name == name)
                {
                    return list;
                }
            }
            return null;
        }
    }
}

