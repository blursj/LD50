namespace OPCDA.NET
{
    using System;
    using System.Xml;

    [Serializable]
    public class ItemList
    {
        public bool ActiveState;
        public bool ActiveStateSpecified;
        public ItemDefinition[] items;
        public string name;
        public XmlQualifiedName ReqType;
        public ListType type;
    }
}

