using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACUServer
{
    public static class EncryptionTools
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strValue">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncoderString(string strValue)
        {
            byte[] encodeBytes = System.Text.Encoding.Unicode.GetBytes(strValue);
            return System.Convert.ToBase64String(encodeBytes);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strValue">需要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DecoderString(string strValue)
        {
            byte[] encodeBytes = System.Convert.FromBase64String(strValue);
            return System.Text.Encoding.Unicode.GetString(encodeBytes);
        }
    }
}
