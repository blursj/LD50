namespace OPCDA.NET
{
    using OPCDA;
    using OPCDA.Interface;
    using System;
    using System.Runtime.InteropServices;

    internal class CustomMarshal
    {
        internal static DateTime[] ToDateTimes(int size, ref IntPtr pTimestamps)
        {
            DateTime[] timeArray = null;
            if (size > 0)
            {
                timeArray = new DateTime[size];
                IntPtr ptr = pTimestamps;
                for (int i = 0; i < size; i++)
                {
                    long fileTime = Marshal.ReadInt64(ptr);
                    timeArray[i] = DateTime.FromFileTime(fileTime);
                    ptr = (IntPtr) (ptr.ToInt32() + 8);
                }
            }
            return timeArray;
        }

        internal static OPCITEMVQT[] ToItemVQT(ItemValue[] items)
        {
            int num = (items == null) ? 0 : items.Length;
            if (num == 0)
            {
                return null;
            }
            OPCITEMVQT[] opcitemvqtArray = new OPCITEMVQT[num];
            for (int i = 0; i < num; i++)
            {
                opcitemvqtArray[i] = new OPCITEMVQT();
                opcitemvqtArray[i].vDataValue = ToVariant(items[i].Value);
                opcitemvqtArray[i].bQualitySpecified = items[i].QualitySpecified;
                opcitemvqtArray[i].wQuality = 0;
                if (items[i].QualitySpecified)
                {
                    opcitemvqtArray[i].wQuality = items[i].Quality.GetCode();
                }
                opcitemvqtArray[i].bTimeStampSpecified = items[i].TimestampSpecified;
                if (items[i].TimestampSpecified)
                {
                    opcitemvqtArray[i].ftTimeStamp = items[i].Timestamp.ToFileTime();
                }
                else
                {
                    opcitemvqtArray[i].ftTimeStamp = DateTime.Now.ToFileTime();
                }
            }
            return opcitemvqtArray;
        }

        internal static object[] ToObjects(int size, ref IntPtr pValues)
        {
            object[] objArray = null;
            if (size > 0)
            {
                objArray = new object[size];
                IntPtr pSrcNativeVariant = pValues;
                for (int i = 0; i < size; i++)
                {
                    try
                    {
                        objArray[i] = Marshal.GetObjectForNativeVariant(pSrcNativeVariant);
                    }
                    catch (Exception)
                    {
                        objArray[i] = null;
                    }
                    pSrcNativeVariant = (IntPtr) (pSrcNativeVariant.ToInt32() + 0x10);
                }
                Marshal.FreeCoTaskMem(pValues);
                pValues = IntPtr.Zero;
            }
            return objArray;
        }

        internal static OPCQuality[] ToQualities(int size, ref IntPtr pQualities)
        {
            OPCQuality[] qualityArray = new OPCQuality[size];
            short[] destination = new short[size];
            if (size > 0)
            {
                Marshal.Copy(pQualities, destination, 0, size);
                for (int i = 0; i < size; i++)
                {
                    qualityArray[i] = new OPCQuality(destination[i]);
                }
                Marshal.FreeCoTaskMem(pQualities);
                pQualities = IntPtr.Zero;
            }
            return qualityArray;
        }

        internal static object ToVariant(object source)
        {
            if (source == null)
            {
                return source;
            }
            Type type = source.GetType();
            if (!type.IsArray || (type.GetElementType() != typeof(decimal)))
            {
                return source;
            }
            decimal[] numArray = (decimal[]) source;
            object[] objArray = new object[numArray.Length];
            for (int i = 0; i < numArray.Length; i++)
            {
                try
                {
                    objArray[i] = numArray[i];
                }
                catch (Exception)
                {
                    objArray[i] = (double) 1.0 / (double) 0.0;
                }
            }
            return objArray;
        }
    }
}

