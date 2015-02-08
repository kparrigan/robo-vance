using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboVance.Roomba.Core;

namespace RoboVance.Roomba.UnitTest
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void Can_Get_Bytes_Short()
        {
            Int16 val = 200;
            var bytes = val.ToBytes();
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(200, bytes[1]);
        }

        [TestMethod]
        public void Can_Get_Bytes_Int()
        {
            Int32 val = 200;
            var bytes = val.ToBytes();
            Assert.AreEqual(0, bytes[0]);
            Assert.AreEqual(200, bytes[1]);
        }
    }
}
