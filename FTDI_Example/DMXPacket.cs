namespace ConsoleApp1;

public class DMXPacket
{
    private const byte START = 0x7E;
    private const byte STOP = 0xE7;
    private const byte LABEL = 0x6;
    
    public static byte[] ToByteArray(byte[] data)
    {
        var length = data.Length;
        byte[] packet = new byte[length + 5];
        
        packet[0] = START;
        packet[1] = LABEL;
        var lsb = (byte)(length & 0x01);
        var msb = (byte)(length >> 8);
        packet[2] = lsb;
        packet[3] = msb;
        for (int i = 0; i < data.Length; i++)
        {
            packet[i + 4] = data[i];
        }

        packet[^1] = STOP;
        return packet;
    }
}