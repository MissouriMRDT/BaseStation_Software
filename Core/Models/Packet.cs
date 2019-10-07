using System;
using Core.RoveProtocol;

namespace Core.Models
{
    public class Packet
    {
        public string Name;
        public byte[] Data;
        public byte Count;
        public byte DataType;

        public Packet(string name, byte[] data, byte count, byte type)
        {
            Name = name;
            Data = data;
            Count = count;
            DataType = type;
        }

        public Packet(string name)
        {
            Name = name;
            Data = new byte[] { 4 };
            Count = 1;
            DataType = 0;
        }

        public Packet(string name, SByte data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = (byte)RoveProtocol.DataTypes.INT8_T;
        }

        public Packet(string name, byte data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = (byte)RoveProtocol.DataTypes.UINT8_T;
        }

        public Packet(string name, Int16 data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = (byte)RoveProtocol.DataTypes.INT16_T;
        }

        public Packet(string name, UInt16 data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = (byte)RoveProtocol.DataTypes.UINT16_T;
        }

        public Packet(string name, Int32 data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = (byte)RoveProtocol.DataTypes.INT32_T;
        }

        public Packet(string name, UInt32 data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = (byte)RoveProtocol.DataTypes.UINT32_T;
        }

        public static Packet Create<T>(string name, T[] data)
        {   
            return new Packet(name, MapData(data), (byte)data.Length, MapDataType(typeof(T)));
        }

        public static Packet Create<T>(string name, T data)
        {
            return Create<T>(name, new T[] { data });
        }

        private static byte MapDataType(Type type)
        {
            if(type == typeof(SByte))
            {
                return (byte)DataTypes.INT8_T;
            }
            else if(type == typeof(Byte))
            {
                return (byte)DataTypes.UINT8_T;
            }
            else if(type == typeof(Int16))
            {
                return (byte)DataTypes.INT16_T;
            }
            else if(type == typeof(UInt16))
            {
                return (byte)DataTypes.UINT16_T;
            }
            else if(type == typeof(Int32))
            {
                return (byte)DataTypes.INT32_T;
            }
            else if(type == typeof(UInt32))
            {
                return (byte)DataTypes.UINT32_T;
            }
            else if(type == typeof(Int64))
            {
                return (byte)DataTypes.INT64_T;
            }
            return 10;
        }

        private static byte[] MapData<T>(T[] data)
        {
            Type type = typeof(T);
            if (type == typeof(SByte))
            {
                byte[] result = new byte[data.Length * sizeof(SByte)];
                Buffer.BlockCopy((SByte[])(object)data, 0, result, 0, ((SByte[])(object)data).Length * sizeof(SByte));
                return result;
            }
            else if (type == typeof(Byte))
            {
                byte[] result = new byte[data.Length * sizeof(Byte)];
                Buffer.BlockCopy((Byte[])(object)data, 0, result, 0, ((Byte[])(object)data).Length * sizeof(Byte));
                return result;
            }
            else if (type == typeof(Int16))
            {
                byte[] result = new byte[data.Length * sizeof(Int16)];
                Buffer.BlockCopy((Int16[])(object)data, 0, result, 0, ((Int16[])(object)data).Length * sizeof(Int16));
                return result;
            }
            else if (type == typeof(UInt16))
            {
                byte[] result = new byte[data.Length * sizeof(UInt16)];
                Buffer.BlockCopy((UInt16[])(object)data, 0, result, 0, ((UInt16[])(object)data).Length * sizeof(UInt16));
                return result;
            }
            else if (type == typeof(Int32))
            {
                byte[] result = new byte[data.Length * sizeof(Int32)];
                Buffer.BlockCopy((Int32[])(object)data, 0, result, 0, ((Int32[])(object)data).Length * sizeof(Int32));
                return result;
            }
            else if (type == typeof(UInt32))
            {
                byte[] result = new byte[data.Length * sizeof(UInt32)];
                Buffer.BlockCopy((UInt32[])(object)data, 0, result, 0, ((UInt32[])(object)data).Length * sizeof(UInt32));
                return result;
            }
            else if (type == typeof(Int64))
            {
                byte[] result = new byte[data.Length * sizeof(Int64)];
                Buffer.BlockCopy((Int64[])(object)data, 0, result, 0, ((Int64[])(object)data).Length * sizeof(Int64));
                return result;
            }
            return null;
        }

        public T[] GetDataArray<T>()
        {
            return null;
        }

        public T GetData<T>()
        {
            Type type = typeof(T);
            if (Count != 1)
            {
                return default;
            }
            else if(type == typeof(SByte))
            {
                return (T)(object)unchecked((sbyte)Data[0]);
            }
            else if (type == typeof(Byte))
            {
                return (T)(object)Data[0];
            }
            else if (type == typeof(Int16))
            {
                return (T)(object)BitConverter.ToInt16(Data, 0);
            }
            else if (type == typeof(UInt16))
            {
                return (T)(object)BitConverter.ToUInt16(Data, 0);
            }
            else if (type == typeof(Int32))
            {
                return (T)(object)BitConverter.ToInt32(Data, 0);
            }
            else if (type == typeof(UInt32))
            {
                return (T)(object)BitConverter.ToUInt32(Data, 0);
            }
            else if (type == typeof(Int64))
            {
                return (T)(object)BitConverter.ToInt64(Data, 0);
            }

            return default;
        }
    }
}
