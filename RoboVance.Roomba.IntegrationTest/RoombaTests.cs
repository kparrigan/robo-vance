using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboVance.Roomba.Services;
using RoboVance.Roomba.Core;

namespace RoboVance.Roomba.IntegrationTest
{
    // Despite this being a unit test library, these are integration tests. They are 
    // integration tests because they require:
    // 1) A physical roomba
    // 2) A fat, sweaty, white guy to turn said Roomba on and observe it's behavior during test.
    [TestClass]
    public class RoombaTests
    {
        private const string BT_NAME = "FireFly-84F6";

        [TestMethod]
        public void Can_Move_Roomba_Serial()
        {
            int forwardTime = 1500;
            int reverseTime = 2000;
            int stopTime = 5000;

            using(var roomba = new RoombaCreate("COM3"))
            {
                roomba.Start();
                roomba.Control();
                roomba.Forward();
                System.Threading.Thread.Sleep(forwardTime);
                roomba.Stop();
                System.Threading.Thread.Sleep(stopTime);
                roomba.Reverse();
                System.Threading.Thread.Sleep(reverseTime);
                roomba.Stop();
                roomba.PowerDown();
            }
        }

        [TestMethod]
        public void Can_Move_Roomba_BlueTooth()
        {
            int forwardTime = 1500;
            int reverseTime = 2000;
            int stopTime = 5000;

            using (var roomba = new RoombaCreate(BT_NAME, CommunicationMethod.Bluetooth))
            {
                roomba.Start();
                roomba.Control();
                roomba.Forward();
                System.Threading.Thread.Sleep(forwardTime);
                roomba.Stop();
                System.Threading.Thread.Sleep(stopTime);
                roomba.Reverse();
                System.Threading.Thread.Sleep(reverseTime);
                roomba.Stop();
                roomba.PowerDown();
            }
        }

        [TestMethod]
        public void Can_Discover_Roomba()
        {
            var discoveryService = new BluetoothDiscoveryService();
            var names = discoveryService.GetDeviceNames();

            Assert.IsTrue(names.Count() > 0);
            Assert.IsTrue(names.Contains(BT_NAME));
        }

        [TestMethod]
        public void Can_Dock_Roomba()
        {
            using (var roomba = new RoombaCreate(BT_NAME, CommunicationMethod.Bluetooth))
            {
                roomba.Start();
                roomba.Dock();
            }
        }

        [TestMethod]
        public void Can_UnDock_Roomba()
        {
            int reverseTime = 5000;
            using (var roomba = new RoombaCreate(BT_NAME, CommunicationMethod.Bluetooth))
            {
                roomba.Start();
                roomba.Control();
                roomba.Reverse();
                System.Threading.Thread.Sleep(reverseTime);
                roomba.Stop();
                roomba.PowerDown();
            }
        }

        [TestMethod]
        public void Can_Turn_Roomba()
        {
            int turnTime = 5000;

            using (var roomba = new RoombaCreate(BT_NAME, CommunicationMethod.Bluetooth))
            {
                roomba.Start();
                roomba.Control();
                roomba.TurnRight();
                System.Threading.Thread.Sleep(turnTime);
                roomba.Stop();
                roomba.TurnLeft();
                System.Threading.Thread.Sleep(turnTime);
                roomba.Stop();
                roomba.PowerDown();
            }
        }
    }
}
