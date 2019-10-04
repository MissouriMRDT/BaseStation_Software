using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.RoveProtocol;

namespace Core.Models
{
    public class Packet
    {
        public string Name;
        public byte[] Data;
        public byte Count;
        public byte DataType;

        private Packet(string name, byte[] data, byte count, byte type)
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

        public static Packet Create<T>(string name, T[] data)
        {   
            return new Packet(name, MapData(data), (byte)data.Length, MapDataType(typeof(T));
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
            return default;
        }
    }
}
