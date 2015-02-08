using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Core
{
    public static class OpCodes
    {
        public const byte Start = 128;
        public const byte Control = 130;
        public const byte Safe = 131;
        public const byte Full = 132;
        public const byte Power = 133;
        public const byte Drive = 137;
        public const byte Dock = 143;
    }
}
