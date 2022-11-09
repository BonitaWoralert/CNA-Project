using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ServerProj
{
    internal class Server
    {
        private TcpListener m_TcpListener;
        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_TcpListener = new TcpListener(ip, port);
        }
        public void Start()
        {
            m_TcpListener.Start();
            Console.WriteLine("Listening...");
            Socket socket = m_TcpListener.AcceptSocket();
            Console.WriteLine("Connection Made");
            ClientMethod(socket);
        }
        public void Stop()
        {
            m_TcpListener.Stop();
        }
        private void ClientMethod(Socket socket)
        {
            string receivedMessage;
            NetworkStream stream = new NetworkStream(socket);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine("You have connected to the server - input 0 to end");
            writer.Flush();
            while((receivedMessage = reader.ReadLine()) != null)
            {
                writer.WriteLine(GetReturnMessage(receivedMessage));
                writer.Flush();
                if (receivedMessage == "0")
                    break;
            }
            socket.Close();
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
