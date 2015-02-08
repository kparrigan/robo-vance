using RoboVance.Roomba.Core;
using RoboVance.Roomba.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Core
{
    public class RoombaCreate : BaseRoomba
    {
        #region Constructor
        public RoombaCreate(string portName)
            : base (portName)
        {           
        }

        public RoombaCreate(string portName, CommunicationMethod communicationMethod)
            : base(portName, communicationMethod)
        {
        }
        #endregion
    }
}
