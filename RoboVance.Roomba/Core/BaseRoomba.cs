using RoboVance.Roomba.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Core
{
    public abstract class BaseRoomba : IRoomba
    {
        #region Members
        private TimeSpan _commandLag;
        private DateTime _lastCommand;
        private ICommunicationService _communicationService;
        private Int32 _velocity;
        private Byte _velocityHighByte;
        private Byte _velocityLowByte;
        #endregion

        #region Properties
        public Int32 Velocity 
        { 
            get { return _velocity; } 
            set 
            { 
                _velocity = value;
                var bytes = value.ToBytes();
                _velocityHighByte = bytes[0];
                _velocityLowByte = bytes[1];
            } 
        }
        #endregion

        #region Constructor/Destructor
        public BaseRoomba(string deviceName)
        {
            if (String.IsNullOrEmpty(deviceName))
            {
                throw new ArgumentNullException("deviceName");
            }

            _communicationService = new SerialCommunicationService(deviceName);

            // TODO remove magic number
            _commandLag = TimeSpan.FromMilliseconds(200);

            this.Velocity = 200;
        }

        public BaseRoomba(string deviceName, CommunicationMethod communicationMethod)
        {
            if (String.IsNullOrEmpty(deviceName))
            {
                throw new ArgumentNullException("deviceName");
            }

            // TODO move this to factory
            switch(communicationMethod)
            {
                case CommunicationMethod.Serial:
                    _communicationService = new SerialCommunicationService(deviceName);
                    break;
                case CommunicationMethod.Bluetooth:
                    _communicationService = new BluetoothCommunicationService(deviceName);
                    break;
                default:
                    _communicationService = new SerialCommunicationService(deviceName);
                    break;
            }


            // TODO remove magic number
            _commandLag = TimeSpan.FromMilliseconds(200);

            this.Velocity = 200;
        }

        ~BaseRoomba()
        {
           Dispose(false);
        }
        #endregion

        #region IDisposable
        protected void Dispose(Boolean disposing)
        {
            if (_communicationService != null)
            {

                _communicationService.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IRoomba
        public virtual void Start()
        {
            var buffer = new byte[] 
            {
                OpCodes.Start
            };

            this.DoCommand(buffer);
        }

        public virtual void Control()
        {
            var buffer = new byte[] 
            {
                OpCodes.Control
            };

            this.DoCommand(buffer);
        }

        public virtual void Forward()
        {
            var buffer = new byte[]
            {
                OpCodes.Drive
                ,_velocityHighByte
                ,_velocityLowByte
                ,128
                ,0
            };

            this.DoCommand(buffer);
        }

        public virtual void Stop()
        {
            var buffer = new byte[] 
            {
                OpCodes.Drive
                ,0
                ,0
                ,0
                ,0
            };

            this.DoCommand(buffer);
        }

        public virtual void Reverse()
        {
            var bytes = (-1 * _velocity).ToBytes();

            var buffer = new byte[]
            {
                OpCodes.Drive
                ,bytes[0]
                ,bytes[1]
                ,128
                ,0
            };

            this.DoCommand(buffer);
        }

        public virtual void PowerDown()
        {
            var buffer = new byte[]
            {
                OpCodes.Power
            };

            this.DoCommand(buffer);
        }

        public virtual void Dock()
        {
            var buffer = new byte[]
            {
                OpCodes.Dock
            };

            this.DoCommand(buffer);
        }

        public virtual void TurnRight()
        {
            var buffer = new byte[]
            {
                OpCodes.Drive
                ,_velocityHighByte
                ,_velocityLowByte
                ,255
                ,255
            };

            this.DoCommand(buffer);
        }

        public virtual void TurnLeft()
        {
            var buffer = new byte[]
            {
                OpCodes.Drive
                ,_velocityHighByte
                ,_velocityLowByte
                ,0
                ,0
            };

            this.DoCommand(buffer);
        }
        #endregion

        #region Protected
        protected void DoCommand(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException("Buffer is empty.");
            }

            // Roomba will ignore commands if it gets them too quickly.
            // Check when it executed it's last command, and sleep 
            // until we've reached the lag time if it was too recent.
            // TODO better way to handle command lag/command queing
            var lastRun = (DateTime.Now - _lastCommand);
            if (lastRun < _commandLag)
            {
                System.Threading.Thread.Sleep(_commandLag - lastRun);
            }

            _communicationService.DoCommand(buffer);
            _lastCommand = DateTime.Now;
        }
        #endregion
    }
}
