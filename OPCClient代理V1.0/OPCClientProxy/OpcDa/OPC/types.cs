namespace OPC
{
    using System;
    using System.Runtime.InteropServices;

    internal class types
    {
        internal static Type ConvertToSystemType(VarEnum vt)
        {
            if ((vt & VarEnum.VT_ARRAY) != VarEnum.VT_EMPTY)
            {
                vt &= ~VarEnum.VT_ARRAY;
                if (vt == VarEnum.VT_BOOL)
                {
                    return typeof(bool[]);
                }
                if (vt == VarEnum.VT_BSTR)
                {
                    return typeof(string[]);
                }
                if (vt == VarEnum.VT_VARIANT)
                {
                    return typeof(object[]);
                }
                if (vt == VarEnum.VT_I1)
                {
                    return typeof(sbyte[]);
                }
                if (vt == VarEnum.VT_UI1)
                {
                    return typeof(byte[]);
                }
                if (vt == VarEnum.VT_I2)
                {
                    return typeof(short[]);
                }
                if (vt == VarEnum.VT_UI2)
                {
                    return typeof(ushort[]);
                }
                if (vt == VarEnum.VT_I4)
                {
                    return typeof(int[]);
                }
                if (vt == VarEnum.VT_UI4)
                {
                    return typeof(uint[]);
                }
                if (vt == VarEnum.VT_I8)
                {
                    return typeof(long[]);
                }
                if (vt == VarEnum.VT_UI8)
                {
                    return typeof(ulong[]);
                }
                if (vt == VarEnum.VT_R4)
                {
                    return typeof(float[]);
                }
                if (vt == VarEnum.VT_R8)
                {
                    return typeof(double[]);
                }
                if (vt == VarEnum.VT_DECIMAL)
                {
                    return typeof(decimal[]);
                }
                if (vt == VarEnum.VT_FILETIME)
                {
                    return typeof(DateTime[]);
                }
            }
            else
            {
                if (vt == VarEnum.VT_EMPTY)
                {
                    return typeof(void);
                }
                if (vt == VarEnum.VT_BOOL)
                {
                    return typeof(bool);
                }
                if (vt == VarEnum.VT_BSTR)
                {
                    return typeof(string);
                }
                if (vt == VarEnum.VT_VARIANT)
                {
                    return typeof(object);
                }
                if (vt == VarEnum.VT_I1)
                {
                    return typeof(sbyte);
                }
                if (vt == VarEnum.VT_UI1)
                {
                    return typeof(byte);
                }
                if (vt == VarEnum.VT_I2)
                {
                    return typeof(short);
                }
                if (vt == VarEnum.VT_UI2)
                {
                    return typeof(ushort);
                }
                if (vt == VarEnum.VT_I4)
                {
                    return typeof(int);
                }
                if (vt == VarEnum.VT_UI4)
                {
                    return typeof(uint);
                }
                if (vt == VarEnum.VT_I8)
                {
                    return typeof(long);
                }
                if (vt == VarEnum.VT_UI8)
                {
                    return typeof(ulong);
                }
                if (vt == VarEnum.VT_R4)
                {
                    return typeof(float);
                }
                if (vt == VarEnum.VT_R8)
                {
                    return typeof(double);
                }
                if (vt == VarEnum.VT_DECIMAL)
                {
                    return typeof(decimal);
                }
                if (vt == VarEnum.VT_FILETIME)
                {
                    return typeof(DateTime);
                }
            }
            return typeof(void);
        }

        internal static VarEnum ConvertToVarType(Type st)
        {
            if (st != typeof(void))
            {
                if (st == typeof(bool))
                {
                    return VarEnum.VT_BOOL;
                }
                if (st == typeof(string))
                {
                    return VarEnum.VT_BSTR;
                }
                if (st == typeof(object))
                {
                    return VarEnum.VT_VARIANT;
                }
                if (st == typeof(sbyte))
                {
                    return VarEnum.VT_I1;
                }
                if (st == typeof(byte))
                {
                    return VarEnum.VT_UI1;
                }
                if (st == typeof(short))
                {
                    return VarEnum.VT_I2;
                }
                if (st == typeof(ushort))
                {
                    return VarEnum.VT_UI2;
                }
                if (st == typeof(int))
                {
                    return VarEnum.VT_I4;
                }
                if (st == typeof(uint))
                {
                    return VarEnum.VT_UI4;
                }
                if (st == typeof(long))
                {
                    return VarEnum.VT_I8;
                }
                if (st == typeof(ulong))
                {
                    return VarEnum.VT_UI8;
                }
                if (st == typeof(float))
                {
                    return VarEnum.VT_R4;
                }
                if (st == typeof(double))
                {
                    return VarEnum.VT_R8;
                }
                if (st == typeof(decimal))
                {
                    return VarEnum.VT_DECIMAL;
                }
                if (st == typeof(DateTime))
                {
                    return VarEnum.VT_FILETIME;
                }
                if (st == typeof(TimeSpan))
                {
                    return VarEnum.VT_FILETIME;
                }
                if (st == typeof(bool[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_BOOL);
                }
                if (st == typeof(string[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_BSTR);
                }
                if (st == typeof(object[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_VARIANT);
                }
                if (st == typeof(sbyte[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_I1);
                }
                if (st == typeof(byte[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_UI1);
                }
                if (st == typeof(short[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_I2);
                }
                if (st == typeof(ushort[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_UI2);
                }
                if (st == typeof(int[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_I4);
                }
                if (st == typeof(uint[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_UI4);
                }
                if (st == typeof(long[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_I8);
                }
                if (st == typeof(ulong[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_UI8);
                }
                if (st == typeof(float[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_R4);
                }
                if (st == typeof(double[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_R8);
                }
                if (st == typeof(decimal[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_DECIMAL);
                }
                if (st == typeof(DateTime[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_FILETIME);
                }
                if (st == typeof(TimeSpan[]))
                {
                    return (VarEnum.VT_ARRAY | VarEnum.VT_FILETIME);
                }
            }
            return VarEnum.VT_EMPTY;
        }
    }
}

