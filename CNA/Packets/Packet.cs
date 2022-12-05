namespace Packets
{
    public enum PacketType
    {
        ChatMessage,
        PrivateMessage,
        ClientName
    }

    [Serializable]
    public abstract class Packet
    {
        public PacketType m_PacketType { get; protected set; }
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string m_message { get; private set; }
        public ChatMessagePacket(string message)
        {
            m_message = message;
            m_PacketType = PacketType.ChatMessage;
        }
    }
}