using MBClient;
using System.IO.Ports;

//ModbusClient client = new ModbusTCPClient("localhost", 502);
ModbusClient client = new ModbusRTUClient("COM3", 9600, Parity.Even, 8, StopBits.One);

float[,] brightness = Bitmaps.GetBrightness("PURC0001.gif", 10, 10);
// GIF DATA
for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 10; j++)
        Console.Write($"{brightness[i, j]}\t");
    Console.WriteLine();
}

Console.WriteLine("WMR");
// WMR
for (int i = 0; i < 10; i++)
{
    var answer =
        client.QA(new ModbusWMRRequest(
            Enumerable.Range(0, 10).Select(x => (ushort)(brightness[i, x] < 0.6 ? 8888 : 1)).ToArray(),
            (ushort)(10 * i)));
}
Console.ReadKey();
Console.WriteLine("RHR");
// RHR
for (int i = 0; i < 10; i++)
{
    var readRequest = new ModbusRHRRequest((ushort)(10 * i), 10);
    var readResponse = client.QA(readRequest);

    if (readResponse.Function == 0x03)
    {
        int byteCount = readResponse.Data[1];
        for (int j = 0; j < byteCount / 2; j++)
        {
            ushort value = readResponse.Data.GetTwoBytes(2 + j * 2);
            Console.Write($"{value}\t");
        }
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Failed to read registers.");
    }
}

Console.WriteLine("WMC");
// WMC
bool[,] coilValues = new bool[10, 10];
for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 10; j++)
    {
        coilValues[i, j] = brightness[i, j] < 0.6;
    }
}

for (int i = 0; i < 10; i++)
{
    bool[] values = new bool[10];
    for (int j = 0; j < 10; j++)
    {
        values[j] = coilValues[i, j];
    }

    var writeRequest = new ModbusWMCRequest(values, (ushort)(10 * i));
    var writeResponse = client.QA(writeRequest);

    if (writeResponse.Function == 0x0F)
    {
        Console.WriteLine($"Wrote coils starting at {(10 * i)} successfully.");
    }
    else
    {
        Console.WriteLine("Failed to write coils.");
    }
}

Console.WriteLine("RC");
// RC
for (int i = 0; i < 10; i++)
{
    var readRequest = new ModbusRCRequest((ushort)(10 * i), 10);
    var readResponse = client.QA(readRequest);

    if (readResponse.Function == 0x01)
    {
        int byteCount = readResponse.Data[1];
        for (int j = 0; j < byteCount; j++)
        {
            byte coilByte = readResponse.Data[2 + j];
            for (int k = 0; k < 8; k++)
            {
                bool coilValue = (coilByte & (1 << k)) != 0;
                Console.Write($"{(coilValue ? 1 : 0)} ");
            }
        }
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Failed to read coils.");
    }
}

Console.WriteLine();
Console.ReadKey();
client.CloseClient();