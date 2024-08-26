using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MBClient
{
    //Write Multiple Coils
    public class ModbusWMCRequest : ModbusMessage
    {
        // 5 + (values.Length + 7) / 8 because: 5 bits of header
        // + provides rounding up to the nearest byte to accommodate all bits
        public ModbusWMCRequest(bool[] values, ushort startAddress)
            : base(0x0F, 5 + (values.Length + 7) / 8)
        {
            Data.FillTwoBytes(1, startAddress);
            Data.FillTwoBytes(3, (ushort)values.Length);
            Data[5] = (byte)((values.Length + 7) / 8);

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i])
                {
                    Data[6 + i / 8] |= (byte)(1 << (i % 8));
                }
            }
            //Explaination:
            //i / 8: Determines which byte in the Data array(starting at index 6) is to be used.Each byte can store 8 coils.
            //i % 8: Determines which bit in a given byte should be set.
            //1 << (i % 8): Creates a bit mask with one bit set at position i % 8.
            //Data[6 + i / 8] |= (byte)(1 << (i % 8)): Sets the corresponding bit in the corresponding byte if the coil value is true.
        }
    }
}
