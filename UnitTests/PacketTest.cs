using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System;
using Core.RoveProtocol;

namespace UnitTests
{
    // All tests are run with assumption of little-endian byte order
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
            Packet packet = Packet.Create("Hi");
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
        public void Data_SByte_Array()
        {
            SByte[] data = { SByte.MinValue, SByte.MaxValue, 0};
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<SByte>(packet, "Hi", data.Length, 0, "80-7F-00", data);
        }

        [TestMethod]
        public void Data_Byte_Array()
        {
            Byte[] data = { Byte.MinValue, Byte.MaxValue, 10 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<Byte>(packet, "Hi", data.Length, 1, "00-FF-0A", data);
        }

        [TestMethod]
        public void Data_Int16_Array()
        {
            Int16[] data = { Int16.MinValue, Int16.MaxValue, -1000 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<Int16>(packet, "Hi", data.Length, 2, "00-80-FF-7F-18-FC", data);
        }

        [TestMethod]
        public void Data_UInt16_Array()
        {
            UInt16[] data = { UInt16.MinValue, UInt16.MaxValue, 1000 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<UInt16>(packet, "Hi", data.Length, 3, "00-00-FF-FF-E8-03", data);
        }

        [TestMethod]
        public void Data_Int32_Array()
        {
            Int32[] data = { Int32.MinValue, Int32.MaxValue, 0 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<Int32>(packet, "Hi", data.Length, 4, "00-00-00-80-FF-FF-FF-7F-00-00-00-00", data);
        }

        [TestMethod]
        public void Data_UInt32_Array()
        {
            UInt32[] data = { UInt32.MinValue, UInt32.MaxValue, 1000 };
            Packet packet = Packet.Create("Hi", data);
            CheckPacketArray<UInt32>(packet, "Hi", data.Length, 5, "00-00-00-00-FF-FF-FF-FF-E8-03-00-00", data);
        }
    }
}
