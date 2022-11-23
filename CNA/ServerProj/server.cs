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

namespace ServerProj
{
    public class ConnectedClient
    {
        private Socket m_socket;
        private NetworkStream m_stream;
        private StreamReader m_reader;
        private StreamWriter m_writer;
        private object m_readLock;
        private object m_writeLock;

        public ConnectedClient(Socket socket)
        {
            m_writeLock = new object();
            m_readLock = new object();
            m_socket = socket;
            m_stream = new NetworkStream(socket);
            m_reader = new StreamReader(m_stream, Encoding.UTF8);
            m_writer = new StreamWriter(m_stream, Encoding.UTF8);
        }
        public void Close()
        {
            m_stream.Close();
            m_socket.Close();
            m_reader.Close();
            m_writer.Close();
        }
        public string Read()
        {
            lock (m_readLock)
            {
                return m_reader.ReadLine();
            }
        }
        public void Send(string message)
        {
            lock (m_writeLock)
            {
                m_writer.WriteLine(message);
                m_writer.Flush();
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
            while(connections <= 5)
            {
                Console.WriteLine("Listening...");
                Socket socket = m_TcpListener.AcceptSocket();
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
            receivedMessage = m_clients[index].Read();
            m_clients[index].Send(GetReturnMessage(receivedMessage));
            if (receivedMessage == "0")
            {
                m_clients[index].Close();
                ConnectedClient c;
                m_clients.TryRemove(index, out c);
            }
            //NetworkStream stream = new NetworkStream(socket);
            //StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            //StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            //writer.WriteLine("You have connected to the server - input 0 to end");
            //writer.Flush();
            /*while((receivedMessage = reader.ReadLine()) != null)
            {
                writer.WriteLine(GetReturnMessage(receivedMessage));
                writer.Flush();
                if (receivedMessage == "0")
                    break;
            }
            socket.Close();*/
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
