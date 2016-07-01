namespace OPCDA.NET
{
    using System;
    using System.Xml;

    [Serializable]
    public class ItemDefinition
    {
        public string AccessPath;
        public bool ActiveState;
        public bool ActiveStateSpecified;
        public int ClientHandle;
        public string ItemName;
        public XmlQualifiedName ReqType;
    }
}

