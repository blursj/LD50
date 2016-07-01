namespace OPCDA.NET
{
    using OPCDA;
    using OPCDA.Interface;
    using System;
    using System.Runtime.InteropServices;

    internal class OpcEnumItemAttributes
    {
        private IEnumOPCItemAttributes ifEnum;

        internal OpcEnumItemAttributes(IEnumOPCItemAttributes ifEnump)
        {
            this.ifEnum = ifEnump;
        }

        public void Dispose()
        {
            if (this.ifEnum != null)
            {
                Marshal.ReleaseComObject(this.ifEnum);
                this.ifEnum = null;
            }
        }

        ~OpcEnumItemAttributes()
        {
            this.Dispose();
        }

        public int Next(int enumcountmax, out OPCItemAttributes[] attributes)
        {
            IntPtr ptr;
            int num;
            attributes = null;
            int num2 = this.ifEnum.Next(enumcountmax, out ptr, out num);
            int num3 = (int) ptr;
            if (((num3 != 0) && (num > 0)) && (num <= enumcountmax))
            {
                attributes = new OPCItemAttributes[num];
                for (int i = 0; i < num; i++)
                {
                    attributes[i] = new OPCItemAttributes();
                    IntPtr ptr2 = (IntPtr) Marshal.ReadInt32((IntPtr) num3);
                    attributes[i].AccessPath = Marshal.PtrToStringUni(ptr2);
                    Marshal.FreeCoTaskMem(ptr2);
                    ptr2 = (IntPtr) Marshal.ReadInt32((IntPtr) (num3 + 4));
                    attributes[i].ItemID = Marshal.PtrToStringUni(ptr2);
                    Marshal.FreeCoTaskMem(ptr2);
                    attributes[i].Active = Marshal.ReadInt32((IntPtr) (num3 + 8)) != 0;
                    attributes[i].HandleClient = Marshal.ReadInt32((IntPtr) (num3 + 12));
                    attributes[i].HandleServer = Marshal.ReadInt32((IntPtr) (num3 + 0x10));
                    attributes[i].AccessRights = (OPCACCESSRIGHTS) Marshal.ReadInt32((IntPtr) (num3 + 20));
                    attributes[i].RequestedDataType = (VarEnum) Marshal.ReadInt16((IntPtr) (num3 + 0x20));
                    attributes[i].CanonicalDataType = (VarEnum) Marshal.ReadInt16((IntPtr) (num3 + 0x22));
                    attributes[i].EUType = (OPCEUTYPE) Marshal.ReadInt32((IntPtr) (num3 + 0x24));
                    attributes[i].EUInfo = Marshal.GetObjectForNativeVariant((IntPtr) (num3 + 40));
                    DUMMY_VARIANT.VariantClear((IntPtr) (num3 + 40));
                    int num5 = Marshal.ReadInt32((IntPtr) (num3 + 0x1c));
                    if (num5 != 0)
                    {
                        int length = Marshal.ReadInt32((IntPtr) (num3 + 0x18));
                        if (length > 0)
                        {
                            attributes[i].Blob = new byte[length];
                            Marshal.Copy((IntPtr) num5, attributes[i].Blob, 0, length);
                        }
                        Marshal.FreeCoTaskMem((IntPtr) num5);
                    }
                    num3 += 0x38;
                }
                Marshal.FreeCoTaskMem(ptr);
            }
            return num2;
        }

        public int Reset()
        {
            return this.ifEnum.Reset();
        }

        public int Skip(int celt)
        {
            return this.ifEnum.Skip(celt);
        }
    }
}

