using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    abstract public class ModbusClient
    {
        public ModbusMessage QA(ModbusMessage modbusRequest)
        {
            if (!WriteRequest(modbusRequest))
                return null;
            return ReadMessage();
        }

        protected abstract bool WriteRequest(ModbusMessage modbusRequest);
        protected abstract ModbusMessage ReadMessage();

        public abstract void CloseClient();

    }
}
