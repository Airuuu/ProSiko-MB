using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    public class ModbusMessage
    {
        public byte[] Data { get; private set; }
        public byte Function { get { return Data[0]; } set { Data[0] = value; } }
        public int Size { get { return Data.Length; } }

        public ModbusMessage(byte function, int dataSize) {
            Data = new byte[dataSize + 1];
            Function = function;
        }
    }
}
