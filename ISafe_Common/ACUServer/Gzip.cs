using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ACUServer
{
    public static class GZip
    {
        public static string CompressString2String(string str)
        {
            var buffer = Compress(Convert.FromBase64String(Convert.ToBase64String(Encoding.Default.GetBytes(str))));
            string result = Convert.ToBase64String(buffer);
            return result;
        }

        public static string DecompressString2String(string str)
        {
            return Encoding.Default.GetString(Decompress(Convert.FromBase64String(str)));
        }

        private static byte[] Compress(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(data, 0, data.Length);
            zip.Close();
            byte[] buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            ms.Close();
            return buffer;
        }

        private static byte[] Decompress(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
            MemoryStream msreader = new MemoryStream();
            byte[] buffer = new byte[0x1000];
            while (true)
            {
                int reader = zip.Read(buffer, 0, buffer.Length);
                if (reader <= 0)
                {
                    break;
                }
                msreader.Write(buffer, 0, reader);
            }
            zip.Close();
            ms.Close();
            msreader.Position = 0;
            buffer = msreader.ToArray();
            msreader.Close();
            return buffer;
        }
    }
}