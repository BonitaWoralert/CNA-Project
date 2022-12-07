using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using Packets;

namespace CNA
{
    public class Client
    {
        private TcpClient m_tcpClient;
        private NetworkStream m_stream;
        private BinaryWriter m_writer;
        private BinaryReader m_reader;
        private BinaryFormatter m_formatter;
        private MainWindow form;

        public Client()
        {
            m_tcpClient = new TcpClient();
        }
        public bool Connect(string ipAddress, int port)
        {
            try
            {
                m_tcpClient.Connect(ipAddress, port);
                m_stream = m_tcpClient.GetStream();
                m_writer = new BinaryWriter(m_stream);
                m_reader = new BinaryReader(m_stream);
                m_formatter = new BinaryFormatter();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
        }
        public void Run()
        {
            form = new MainWindow(this);
            string userInput;
            Thread thread = new Thread(() => { ProcessServerResponse(); });
            thread.Start();
            form.ShowDialog();

        }
        private void ProcessServerResponse()
        {
            try
            {
                while (m_tcpClient.Connected)
                {
                    int numberOfBytes;
                    if ((numberOfBytes = m_reader.ReadInt32()) != -1)
                    {
                        byte[] buffer = m_reader.ReadBytes(numberOfBytes);
                        MemoryStream memstream = new MemoryStream(buffer);
                        Packet receivedMessage = m_formatter.Deserialize(memstream) as Packet;

                        if(receivedMessage != null)
                        {
                            switch(receivedMessage.m_PacketType)
                            {
                                case PacketType.ChatMessage:
                                    ChatMessagePacket chatPacket = (ChatMessagePacket)receivedMessage;
                                    form.UpdateChatBox(chatPacket.m_message);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        public void SendMessage(Packet message)
        {
            MemoryStream memstream = new MemoryStream();
            m_formatter.Serialize(memstream, message);
            byte[] buffer = memstream.GetBuffer();
            m_writer.Write(buffer.Length);
            m_writer.Write(buffer);
            //form.UpdateChatBox(message);
            m_writer.Flush();
        }
    }
}
