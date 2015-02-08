using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Services
{
    internal class BluetoothCommunicationService : ICommunicationService
    {
        #region Member Variables
        private Stream _stream;
        private BluetoothClient _client;
        #endregion

        #region Constructor
        public BluetoothCommunicationService(string deviceName)
        {
            if (String.IsNullOrEmpty(deviceName))
            {
                throw new ArgumentNullException("deviceName");
            }

            Init(deviceName);
        }

        ~BluetoothCommunicationService()
        {
           Dispose(false);
        }
        #endregion

        #region IDisposable
        protected void Dispose(Boolean disposing)
        {
            if (_stream != null)
            {
                _stream.Dispose();
            }

            if (_client != null)
            {
                _client.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region ICommunicator
        public void DoCommand(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("Invalid Command. Buffer is empty.");
            }

            _stream.Write(buffer, 0, buffer.Length);
        }
        #endregion

        #region Private Methods
        private void Init(string deviceName)
        {
            var discoService = new BluetoothDiscoveryService();
            var deviceInfo = discoService.GetDevice(deviceName);

            if (deviceInfo == null)
            {
                throw new ArgumentException(String.Format("Device not found: {0}", deviceName));
            }

            var ep = new BluetoothEndPoint(deviceInfo.DeviceAddress, BluetoothService.SerialPort);                        
            _client = new BluetoothClient();
            _client.Connect(ep);
            _stream = _client.GetStream();
        }
        #endregion
    }
}
