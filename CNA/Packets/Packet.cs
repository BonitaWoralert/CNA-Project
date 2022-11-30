namespace Packets
{
    enum PacketType
    {
        ChatMessage,
        PrivateMessage,
        ClientName
    }
    public class Packet
    {
        public PacketType PacketType { get; set; }
    }
}