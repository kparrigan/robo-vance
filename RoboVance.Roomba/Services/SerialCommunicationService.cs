using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Services
{
    internal class SerialCommunicationService : ICommunicationService
    {
        #region Member Variables
        private SerialPort _port { get; set; }
        #endregion

        #region Constructor
        public SerialCommunicationService(string portName)
        {
            _port = new SerialPort(portName);
            _port.Open();
        }

        ~SerialCommunicationService()
        {
           Dispose(false);
        }
        #endregion

        #region IDisposable
        protected void Dispose(Boolean disposing)
        {
            if (_port != null)
            {
                if (_port.IsOpen)
                {
                    _port.Close();
                }

                _port.Dispose();
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

            _port.Write(buffer, 0, buffer.Length);
        }
        #endregion
    }
}
