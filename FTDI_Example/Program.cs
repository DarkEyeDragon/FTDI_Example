// See https://aka.ms/new-console-template for more information


using System.Collections;
using ConsoleApp1;
using FTD2XX_NET;

Console.WriteLine("Devices:");
var ftdi = new FTDI();
var status = FTDI.FT_STATUS.FT_DEVICE_NOT_FOUND;
uint deviceCount = 0;
ftdi.GetNumberOfDevices(ref deviceCount);
var devices = new FTDI.FT_DEVICE_INFO_NODE[deviceCount];
status = ftdi.GetDeviceList(devices);
var index = 0;
foreach (var ftDeviceInfoNode in devices)
{
    Console.WriteLine($"==========={ftDeviceInfoNode.SerialNumber}===========");
    Console.WriteLine($"Index: {index++}");
    Console.WriteLine($"ID: {ftDeviceInfoNode.ID}");
    Console.WriteLine($"Description: {ftDeviceInfoNode.Description}");
    Console.WriteLine($"Description: {ftDeviceInfoNode.Type}");
    Console.WriteLine("------------------------------");
}
Console.WriteLine("Insert the INDEX to connect to...");
var idStr = Console.ReadLine();
uint.TryParse(idStr, out uint id);
Console.WriteLine("Connecting...");
status = ftdi.OpenByIndex(id);
Console.WriteLine($"connection: {status}");
status = ftdi.ResetDevice();
Console.WriteLine($"Reset: {status}");
status = ftdi.SetBaudRate(250000);
Console.WriteLine($"Baudrate: {status}");
status = ftdi.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, 2,0);
Console.WriteLine($"Data characteristics: {status}");
status = ftdi.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_NONE, 0,0);
Console.WriteLine($"Flow control: {status}");
status = ftdi.SetRTS(false);
Console.WriteLine($"RTS: {status}");
status = ftdi.Purge(2);
Console.Write($"Purge TX: {status} | ");
status = ftdi.Purge(2);
Console.WriteLine($"Purge RX: {status}");
var data = new byte[513];
data[0] = 0x0; //DMX start
data[1] = 0;
data[3] = 90;
data[7] = 50;
for (int i = 9; i < 20; i++)
{
    data[i] = 255;
}

var packet = DMXPacket.ToByteArray(data);          

Console.ReadKey();
uint bytesWritten = 0;
var random = new Random();
var delta = 0;
bool revert = false;
static double ToRadians(double angle)
{
    return (Math.PI / 180) * angle;
}

while (!Console.KeyAvailable)
{
    if (delta == 360) revert = true;
    if (delta == 0) revert = false;
    if (!revert)
    {
        delta++;
    }
    else
    {
        delta--;
    }
    var sinByte = (byte)Math.Floor(Math.Sin(ToRadians(delta))*255);
    /*data[9] = (byte)random.Next(0, 256);
    data[13] = (byte)random.Next(0, 256);
    data[17] = (byte)random.Next(0, 256);*/
    data[6] = sinByte;
    packet = DMXPacket.ToByteArray(data);
    status = ftdi.Write(packet, packet.Length, ref bytesWritten);
    Console.WriteLine(status);
    Console.WriteLine($"Bytes written: {bytesWritten}");
    Thread.Sleep(25);
}
//RESET DEVICE
packet = DMXPacket.ToByteArray(new byte[513]);
status = ftdi.Write(packet, packet.Length, ref bytesWritten);
ftdi.Close();
