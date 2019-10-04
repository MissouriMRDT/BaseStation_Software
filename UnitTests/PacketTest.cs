using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System;
using Core.Models;

namespace UnitTests
{
    [TestClass]
    public class PacketTest
    {
        
        [TestMethod]
        public void TestDataConversions()
        {

            Packet packet = new Packet("");

            Assert.AreEqual(4, packet.GetData<byte>(), "Data Incorrect");
        }
    }
}
