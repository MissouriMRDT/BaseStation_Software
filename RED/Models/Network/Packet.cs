using System;

namespace RED.Models.Network
{
    public class Packet
    {
        public readonly string Name;
        public readonly byte[] Data;
        public readonly int Count;
        public readonly Type DataType;

        public Packet(string name, byte[] data, int count, Type type)
        {
            Name = name;
            Data = data;
            Count = count;
            DataType = type;
        }

        public Packet(string name, Int16 data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = typeof(Int16);
        }

        public Packet(string name, ushort data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = typeof(ushort);
        }

        public Packet(string name, int data)
        {
            Name = name;
            Data = BitConverter.GetBytes(data);
            Count = 1;
            DataType = typeof(int);
        }

        public Packet(string name)
        {
            Name = name;
            Data = new byte[] { 0 };
            Count = 1;
            DataType = typeof(byte);
        }
    }
}
