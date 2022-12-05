using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Packets;

namespace ServerProj
{
    public class ConnectedClient
    {
        private Socket m_socket;
        private NetworkStream m_stream;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;
        private BinaryFormatter m_formatter;
        private object m_readLock;
        private object m_writeLock;

        public ConnectedClient(Socket socket)
        {
            m_writeLock = new object();
            m_readLock = new object();
            m_socket = socket;
            m_stream = new NetworkStream(socket);
            m_reader = new BinaryReader(m_stream, Encoding.UTF8);
            m_writer = new BinaryWriter(m_stream, Encoding.UTF8);
            m_formatter = new BinaryFormatter();
        }
        public void Close()
        {
            m_stream.Close();
            m_socket.Close();
            m_reader.Close();
            m_writer.Close();
        }
        public Packet Read()
        {
            lock (m_readLock)
            {
                int numberOfBytes;
                if ((numberOfBytes = m_reader.ReadInt32()) != -1)
                {}
                byte[] buffer = m_reader.ReadBytes(numberOfBytes);
                MemoryStream memstream = new MemoryStream(buffer);
                return m_formatter.Deserialize(memstream) as Packet;
            }
        }
        public void Send(Packet message)
        {
            lock (m_writeLock)
            {
                MemoryStream memstream = new MemoryStream();
                m_formatter.Serialize(memstream, message);
                byte[] buffer = memstream.GetBuffer();
                m_writer.Write(buffer.Length); //writes length so size can be checked
                m_writer.Write(buffer); //writes data
                m_writer.Flush(); //flush
            }
        }
    }


    internal class Server
    {
        private TcpListener m_TcpListener;
        private ConcurrentDictionary<int, ConnectedClient> m_clients;

        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_TcpListener = new TcpListener(ip, port);
        }
        public void Start()
        {
            m_clients = new ConcurrentDictionary<int, ConnectedClient>();
            int clientIndex = 0;
            int connections = 0;
            m_TcpListener.Start();
            while(connections < 5)
            {
                Console.WriteLine("Listening...");
                Socket socket = m_TcpListener.AcceptSocket();
                connections++;
                Console.WriteLine("Connection Made");
                ConnectedClient client = new ConnectedClient(socket);
                int index = clientIndex;
                clientIndex++;
                m_clients.TryAdd(index, client);
                Thread thread = new Thread(() => { ClientMethod(index); });
                thread.Start();
            }
        }
        public void Stop()
        {
            m_TcpListener.Stop();
        }
        private void ClientMethod(int index)
        {
            string receivedMessage;
            m_clients[index].Send("You have connected to the server - input 0 to end");
            while((receivedMessage = m_clients[index].Read()) != null)
            {
                m_clients[index].Send(GetReturnMessage(receivedMessage));
                if (receivedMessage == "0")
                {
                    break;
                }
            }
            //close client
            m_clients[index].Close();
            ConnectedClient c;
            m_clients.TryRemove(index, out c);
        }
        private string GetReturnMessage(string code)
        {
            if (code == "0")
                return "0";
            else if (code == "Hi")
                return "Hello";
            else
                return "!!!!!!!!!";
        }
    }
}
