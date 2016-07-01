namespace OPCDA.NET
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlIO
    {
        public OPCItemDef[] ReadBuffer(string xmltext)
        {
            OPCItemDef[] defArray;
            BufReader input = new BufReader(xmltext);
            XmlTextReader xmlReader = new XmlTextReader(input);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OPCItemDef[]));
                defArray = (OPCItemDef[]) serializer.Deserialize(xmlReader);
            }
            catch (Exception exception)
            {
                xmlReader.Close();
                throw new Exception(exception.Message, exception);
            }
            xmlReader.Close();
            return defArray;
        }

        public OPCItemDef[] ReadFile(string pathName)
        {
            OPCItemDef[] defArray;
            TextReader textReader = new StreamReader(pathName);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OPCItemDef[]));
                defArray = (OPCItemDef[]) serializer.Deserialize(textReader);
            }
            catch (Exception exception)
            {
                textReader.Close();
                throw new Exception(exception.Message, exception);
            }
            textReader.Close();
            return defArray;
        }

        public void WriteBuffer(OPCItemDef[] defs, out string buffer)
        {
            BufWriter w = new BufWriter();
            XmlTextWriter writer2 = new XmlTextWriter(w);
            try
            {
                new XmlSerializer(typeof(OPCItemDef[])).Serialize((XmlWriter) writer2, defs);
            }
            catch (Exception exception)
            {
                writer2.Close();
                throw new Exception(exception.Message, exception);
            }
            writer2.Close();
            buffer = w.GetString();
        }

        public void WriteFile(OPCItemDef[] defs, string pathName)
        {
            TextWriter textWriter = new StreamWriter(pathName);
            try
            {
                new XmlSerializer(typeof(OPCItemDef[])).Serialize(textWriter, defs);
            }
            catch (Exception exception)
            {
                textWriter.Close();
                throw new Exception(exception.Message, exception);
            }
            textWriter.Close();
        }
    }
}

