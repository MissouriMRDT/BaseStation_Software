using RED.RoveProtocol;
using System;

namespace RED.Models.Network
{
    public readonly struct Packet
    {
        public readonly string Name;
        public readonly byte[] Data;
        public readonly int Count;
        public readonly byte DataType;

        public Packet(string name, byte[] data, int count, byte type)
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
            Data = new byte[] { 0 };
            Count = 1;
            DataType = 0;
        }
    }
}
