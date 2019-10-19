using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System;
using Core.Models;

namespace UnitTests
{
    [TestClass]
    public class PacketTest
    {

        private void CheckPacket<T>(Packet packet, string name, int count, byte type, T data)
        {
            Assert.AreEqual(name, packet.Name, "Name Incorrect");
            Assert.AreEqual(count, packet.Count, "Count Incorrect");
            Assert.AreEqual(type, packet.DataType, "DataType Incorrect");
            Assert.AreEqual(data, packet.GetData<T>(), "Data Incorrect");
        }

        private void CheckPacket<T>(Packet packet, string name, int count, byte type, string raw , T data)
        {
            Assert.AreEqual(name, packet.Name, "Name Incorrect");
            Assert.AreEqual(count, packet.Count, "Count Incorrect");
            Assert.AreEqual(raw, BitConverter.ToString(packet.Data), "Raw Data Incorrect");
            Assert.AreEqual(type, packet.DataType, "DataType Incorrect");
            Assert.AreEqual(data, packet.GetData<T>(), "Data Incorrect");
        }

        private void CheckPacketArray<T>(Packet packet, string name, int count, byte type, string raw, T[] data)
        {
            Assert.AreEqual(name, packet.Name, "Name Incorrect");
            Assert.AreEqual(count, packet.Count, "Count Incorrect");
            Assert.AreEqual(raw, BitConverter.ToString(packet.Data), "Raw Data Incorrect");
            Assert.AreEqual(type, packet.DataType, "DataType Incorrect");

            T[] result = packet.GetDataArray<T>();

            Assert.IsNotNull(result, "Data result is null");

            for (int i = 0; i < packet.Count; i++)
            {
                Assert.AreEqual(data[i], result[i], "Data index " + i + " is incorrect");
            }
        }

        [TestMethod]
        public void Data_Null()
        {

            Packet packet = new Packet("Hi");

            CheckPacket<byte>(packet, "Hi", 1, 0, 4);
        }

        [TestMethod]
        public void Data_SByte()
        {
            SByte data = -3;
            Packet packet = Packet.Create("Hi", data);
            CheckPacket<SByte>(packet, "Hi", 1, 0, "FD", data);
        }

        [TestMethod]
        public void Data_Byte()
        {
            Byte data = 3;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<Byte>(packet, "Hi", 1, 1, "03", data);
        }

        [TestMethod]
        public void Data_Int16()
        {
            Int16 data = -200;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<Int16>(packet, "Hi", 1, 2, "38-FF", data);
        }

        [TestMethod]
        public void Data_UInt16()
        {
            UInt16 data = 200;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<UInt16>(packet, "Hi", 1, 3, "C8-00", data);
        }

        [TestMethod]
        public void Data_Int32()
        {
            Int32 data = -123000;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<Int32>(packet, "Hi", 1, 4, "88-1F-FE-FF", data);
        }

        [TestMethod]
        public void Data_UInt32()
        {
            UInt32 data = 123000;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<UInt32>(packet, "Hi", 1, 5, "78-E0-01-00", data);
        }

        [TestMethod]
        public void Data_Int64()
        {
            Int64 data = -123000;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<Int64>(packet, "Hi", 1, 6, "88-1F-FE-FF-FF-FF-FF-FF", data);
        }

        [TestMethod]
        public void Data_SByte_Array()
        {
            SByte[] data = { -3, -4, -5};
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<SByte>(packet, "Hi", 3, 0, "FD-FC-FB", data);
        }

        [TestMethod]
        public void Data_Byte_Array()
        {
            Byte[] data = { 3, 4, 5 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<Byte>(packet, "Hi", 3, 1, "03-04-05", data);
        }

        [TestMethod]
        public void Data_Int16_Array()
        {
            Int16[] data = { 300, 400, 500 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<Int16>(packet, "Hi", 3, 2, "2C-01-90-01-F4-01", data);
        }

        [TestMethod]
        public void Data_UInt16_Array()
        {
            UInt16[] data = { 300, 400, 500 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<UInt16>(packet, "Hi", 3, 3, "2C-01-90-01-F4-01", data);
        }

        [TestMethod]
        public void Data_Int32_Array()
        {
            UInt32[] data = { 300, 400, 500 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<UInt32>(packet, "Hi", 3, 4, "2C-01-90-01-F4-01", data);
        }

        [TestMethod]
        public void Data_UInt32_Array()
        {
            UInt32[] data = { 300, 400, 500 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<UInt32>(packet, "Hi", 3, 5, "2C-01-90-01-F4-01", data);
        }

        [TestMethod]
        public void Data_Int64_Array()
        {
            Int64[] data = { 300, 400, 500 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<Int64>(packet, "Hi", 3, 5, "2C-01-90-01-F4-01", data);
        }
    }
}
