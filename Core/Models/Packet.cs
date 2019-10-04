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
                return GetBytes(data, sizeof(SByte));
            }
            else if (type == typeof(Byte))
            {
                return GetBytes(data, sizeof(Byte));
            }
            else if (type == typeof(Int16))
            {
                return GetBytes(data, sizeof(Int16));
            }
            else if (type == typeof(UInt16))
            {
                return GetBytes(data, sizeof(UInt16));
            }
            else if (type == typeof(Int32))
            {
                return GetBytes(data, sizeof(Int32));
            }
            else if (type == typeof(UInt32))
            {
                return GetBytes(data, sizeof(UInt32));
            }
            else if (type == typeof(Int64))
            {
                return GetBytes(data, sizeof(Int64));
            }
            return null;
        }

        private static byte[] GetBytes<T>(T[] data, int dataSize)
        {
            byte[] byteData = new byte[data.Length * dataSize];
            Buffer.BlockCopy(data, 0, byteData, 0, data.Length);
            return byteData;
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
                return (T)(object)Data;
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
