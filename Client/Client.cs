using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        public Client()
        {
            using var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                tcpClient.Connect("127.0.0.1", 8888);
                Console.WriteLine($"Connection to {tcpClient.RemoteEndPoint} installed");
                ClientTask(tcpClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        void ClientTask(Socket socket)
        {
            byte[] messageBuffer = new byte[4096];
            int bytesRead;
            try
            {
                bytesRead = socket.Receive(messageBuffer);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var messageFromServer = Encoding.ASCII.GetString(messageBuffer, 0, bytesRead);
            Console.WriteLine($": {messageFromServer}");

            while(true)
            {
                Console.Write("Enter word: ");
                string word = Console.ReadLine();

                byte[] message = Encoding.ASCII.GetBytes(word);
                int bytesSent = socket.Send(message);

                try
                {
                    bytesRead = socket.Receive(messageBuffer);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                string filesFromServer = Encoding.ASCII.GetString(messageBuffer, 0, bytesRead);
                Console.WriteLine(filesFromServer);
                Console.WriteLine();
            }

        }


        
    }
}
