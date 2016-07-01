namespace OPCDA.NET
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Xml;
    using System.Xml.Serialization;

    public class ItemListLoader
    {
        private ItemLists myIListDef;

        public OPCItemDef[] GetItemList(string listName)
        {
            if (this.myIListDef == null)
            {
                return null;
            }
            ItemList list = this.myIListDef.Find(listName);
            if (list == null)
            {
                return null;
            }
            OPCItemDef[] defArray = new OPCItemDef[list.items.Length];
            int index = 0;
            foreach (ItemDefinition definition in list.items)
            {
                defArray[index] = new OPCItemDef(definition.ItemName, definition.ActiveState, definition.ClientHandle, GetSystemType(definition.ReqType));
                index++;
            }
            return defArray;
        }

        public OPCItemDef[] GetItemList(string listName, int clientHandleBase)
        {
            if (this.myIListDef == null)
            {
                return null;
            }
            ItemList list = this.myIListDef.Find(listName);
            if (list == null)
            {
                return null;
            }
            OPCItemDef[] defArray = new OPCItemDef[list.items.Length];
            int index = 0;
            foreach (ItemDefinition definition in list.items)
            {
                defArray[index] = new OPCItemDef(definition.ItemName, definition.ActiveState, clientHandleBase + index, GetSystemType(definition.ReqType));
                index++;
            }
            return defArray;
        }

        private static string GetSymbolName(XmlQualifiedName name)
        {
            if (name != null)
            {
                foreach (FieldInfo info in typeof(Type).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    object obj2 = info.GetValue(typeof(Type));
                    if (((obj2 != null) && (obj2.GetType() == typeof(XmlQualifiedName))) && (name == ((XmlQualifiedName) obj2)))
                    {
                        return info.Name;
                    }
                }
            }
            return null;
        }

        private static Type GetSystemType(XmlQualifiedName name)
        {
            if (name.Name == "void")
            {
                return typeof(void);
            }
            string symbolName = GetSymbolName(name);
            if (symbolName != null)
            {
                foreach (FieldInfo info in typeof(SystemType).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    if (!(info.Name != symbolName))
                    {
                        object obj2 = info.GetValue(typeof(SystemType));
                        if (obj2 != null)
                        {
                            return (Type) obj2;
                        }
                    }
                }
            }
            return null;
        }

        private void HandleListDefaults(ItemLists cfgList)
        {
            foreach (ItemList list in cfgList.ItemDefLists)
            {
                foreach (ItemDefinition definition in list.items)
                {
                    if (!definition.ActiveStateSpecified)
                    {
                        if (list.ActiveStateSpecified)
                        {
                            definition.ActiveState = list.ActiveState;
                        }
                        else
                        {
                            definition.ActiveState = true;
                        }
                    }
                    if (definition.ReqType == null)
                    {
                        if (list.ReqType != null)
                        {
                            definition.ReqType = list.ReqType;
                        }
                        else
                        {
                            definition.ReqType = new XmlQualifiedName("void", "http://www.w3.org/2001/XMLSchema");
                        }
                    }
                }
            }
        }

        public void Load(Stream stream)
        {
            TextReader textReader = new StreamReader(stream);
            if (textReader == null)
            {
                throw new Exception("The TextReader could not be created");
            }
            XmlSerializer serializer = new XmlSerializer(typeof(ItemLists));
            this.myIListDef = (ItemLists) serializer.Deserialize(textReader);
            textReader.Close();
            this.HandleListDefaults(this.myIListDef);
        }

        public void Load(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ItemLists));
            TextReader textReader = new StreamReader(fileName);
            if (textReader == null)
            {
                throw new Exception("The TextReader could not be created for " + fileName);
            }
            this.myIListDef = (ItemLists) serializer.Deserialize(textReader);
            textReader.Close();
            this.HandleListDefaults(this.myIListDef);
        }

        public void LoadEmbedded(string namespaceFileName)
        {
            TextReader textReader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream(namespaceFileName));
            XmlSerializer serializer = new XmlSerializer(typeof(ItemLists));
            this.myIListDef = (ItemLists) serializer.Deserialize(textReader);
            textReader.Close();
            this.HandleListDefaults(this.myIListDef);
        }

        //public void LoadFromExeDir(string fileName)
        //{
        //    string location = null;
        //    Assembly entryAssembly = Assembly.GetEntryAssembly();
        //    if (entryAssembly != null)
        //    {
        //        location = entryAssembly.Location;
        //        int num = location.LastIndexOf(@"\");
        //        if (num >= 0)
        //        {
        //            location = location.Substring(0, num + 1);
        //        }
        //        else
        //        {
        //            location = "";
        //        }
        //    }
        //    else
        //    {
        //        location = HttpContext.Current.Server.MapPath(".") + @"\";
        //    }
        //    XmlSerializer serializer = new XmlSerializer(typeof(ItemLists));
        //    TextReader textReader = new StreamReader(location + fileName);
        //    if (textReader == null)
        //    {
        //        throw new Exception("The TextReader could not be created for " + fileName);
        //    }
        //    this.myIListDef = (ItemLists) serializer.Deserialize(textReader);
        //    textReader.Close();
        //    this.HandleListDefaults(this.myIListDef);
        //}
    }
}

