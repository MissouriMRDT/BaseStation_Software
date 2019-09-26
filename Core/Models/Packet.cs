using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.RoveProtocol;

namespace Core.Models
{
    public struct Packet
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

        public Packet(string name)
        {
            Name = name;
            Data = new byte[] { 4 };
            Count = 1;
            DataType = 0;
        }

        public void Set(string name, byte[] data, byte count, byte type)
        {
            Name = name;
            Data = data;
            Count = count;
            DataType = type;
        }
    }
}
