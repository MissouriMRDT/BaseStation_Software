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
            Assert.AreEqual(data, packet.GetData<byte>(), "Data Incorrect");
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

            CheckPacket<SByte>(packet, "Hi", 1, 0, data);
        }

        [TestMethod]
        public void Data_Byte()
        {
            Byte data = 3;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<Byte>(packet, "Hi", 1, 1, data);
        }

        [TestMethod]
        public void Data_Int16()
        {
            Int16 data = 200;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<Int16>(packet, "Hi", 1, 2, data);
        }

        [TestMethod]
        public void Data_UInt16()
        {
            UInt16 data = 200;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<UInt16>(packet, "Hi", 1, 3, data);
        }

        [TestMethod]
        public void Data_Int32()
        {
            Int32 data = 200;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<Int32>(packet, "Hi", 1, 4, data);
        }

        [TestMethod]
        public void Data_UInt32()
        {
            UInt32 data = 200;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<UInt32>(packet, "Hi", 1, 5, data);
        }

        [TestMethod]
        public void Data_Int64()
        {
            Int64 data = 200000;
            Packet packet = Packet.Create("Hi", data);

            CheckPacket<Int64>(packet, "Hi", 1, 6, data);
        }

    }
}
