using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer.Handler
{
    public class Protocol
    {
        public static readonly int Length = 11;
        public const int MESSAGE = 0;
        public const int File = 1;

        public static readonly byte[] MsgHeader = Encoding.ASCII.GetBytes("0x1STHEADER");
        public static readonly byte[] FileHeader = Encoding.ASCII.GetBytes("0x2NDHEADER");


        public int dataType(byte[] bytes)
        {

            return 1;
        }

    }
}
