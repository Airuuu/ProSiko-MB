using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    //Read Holding Registers
    public class ModbusRHRRequest : ModbusMessage
    {
        public ModbusRHRRequest(ushort startAddress, ushort numberOfRegisters)
        : base(0x03, 5)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, numberOfRegisters);
        }
    }
}
