using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    public class ModbusTCPClient : ModbusClient
    {
        TcpClient tcpClient;
        NetworkStream stream;
        public ModbusTCPClient(string serverName, int portNo) {
            tcpClient = new TcpClient(serverName, portNo);
            stream = tcpClient.GetStream();
        }


        protected override ModbusMessage ReadMessage()
        {
            byte[] mbapHeader = new byte[7];
            stream.Read(mbapHeader, 0, mbapHeader.Length);
            ushort length = mbapHeader.GetTwoBytes(4);
            ModbusMessage answer = new ModbusMessage(0, length-1);
            stream.Read(answer.Data, 0, length-1);
            return answer;
        }

        ushort transactionId;
        protected override bool WriteRequest(ModbusMessage modbusRequest)
        {
            byte[] mbapHeader = new byte[7];
            mbapHeader.FillTwoBytes(0, ++transactionId);
            mbapHeader.FillTwoBytes(2, 0);
            mbapHeader.FillTwoBytes(4, (ushort)(modbusRequest.Size + 1));
            mbapHeader[6] = 1;

            try
            {
                stream.Write(mbapHeader, 0, mbapHeader.Length);
                stream.Write(modbusRequest.Data, 0, modbusRequest.Size);
            } catch { return false; }

            return true;
        }

        public override void CloseClient()
        {
            tcpClient.Close();
        }
    }
}
