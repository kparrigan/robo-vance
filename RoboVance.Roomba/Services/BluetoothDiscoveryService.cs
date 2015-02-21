using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Services
{
    public class BluetoothDiscoveryService
    {
        #region Public Methods
        public IEnumerable<String> GetDeviceNames()
        {
            try
            {
                return GetDevices().Select(d => d.DeviceName);
            }
            catch (Exception ex)
            {
                //TODO Log exception
                return new List<String>();
            }
        }

        public BluetoothDeviceInfo GetDevice(string deviceName)
        {
            BluetoothDeviceInfo info = null;

            using (var client = new BluetoothClient())
            {
                foreach (var deviceInfo in client.DiscoverDevices())
                {
                    if (String.Equals(deviceInfo.DeviceName, deviceName))
                    {
                        info = deviceInfo;
                        break;
                    }
                }
            }

            return info;
        }
        #endregion

        #region Private Methods
        private IEnumerable<BluetoothDeviceInfo> GetDevices()
        {
            using (var client = new BluetoothClient())
            {
                foreach (var deviceInfo in client.DiscoverDevices())
                {
                    yield return deviceInfo;
                }
            }
        }
        #endregion
    }
}
