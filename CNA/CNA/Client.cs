using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace CNA
{
    public class Client
    {
        private TcpClient m_tcpClient;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
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
                stream = m_tcpClient.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);
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
            while(m_tcpClient.Connected)
            {
                form.UpdateChatBox(reader.ReadLine());
            }
            Console.WriteLine("Server says: " + reader.ReadLine());
            Console.WriteLine();
        }
        public void SendMessage(string message)
        {
            writer.WriteLine(message);
            form.UpdateChatBox(message);
            writer.Flush();
        }
    }
}
