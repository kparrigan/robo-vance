using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.Roomba.Core
{
    public static class Extensions
    {
        public static Byte[] ToBytes(this Int16 val)
        {
            var bytes = new Byte[2];
            bytes[0] = (byte)(val >> 8);
            bytes[1] = (byte)(val & 0xff);

            return bytes;
        }

        public static Byte[] ToBytes(this Int32 val)
        {
            var bytes = new Byte[2];
            bytes[0] = (byte)(val >> 8);
            bytes[1] = (byte)(val & 0xff);

            return bytes;
        }
    }
}
