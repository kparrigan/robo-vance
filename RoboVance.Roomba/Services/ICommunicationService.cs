using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Services
{
    internal interface ICommunicationService : IDisposable
    {
        void DoCommand(byte[] buffer);
    }
}
