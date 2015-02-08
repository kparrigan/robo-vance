using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Core
{
    public interface IRoomba : IDisposable
    {
        void Start();
        void Stop();
        void PowerDown();
        void Forward();
        void Reverse();
        void TurnRight();
        void TurnLeft();
        void Control();
        void Dock();
    }
}
