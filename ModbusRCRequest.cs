using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MBClient
{
    //Read Coils
    public class ModbusRCRequest : ModbusMessage
    {
        public ModbusRCRequest(ushort startAddress, ushort numberOfCoils)
            : base(0x01, 5)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, numberOfCoils);
        }
    }
}
