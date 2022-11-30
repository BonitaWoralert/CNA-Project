using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

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
            form = new MainWindow(client);
            string userInput;
            ProcessServerResponse();
            while ((userInput = Console.ReadLine()) != null)
            {
                writer.WriteLine(userInput);
                writer.Flush();
                ProcessServerResponse();
                if (userInput == "0")
                {
                    m_tcpClient.Close();
                    break;
                }
            }
        }
        private void ProcessServerResponse()
        {
            Console.WriteLine("Server says: " + reader.ReadLine());
            Console.WriteLine();
        }
        public void SendMessage(string message)
        {

        }
    }
}
