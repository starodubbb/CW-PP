using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
      
        public Server(InvertedIndex invertedIndex)
        {
            using Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //вказуємо локальну точку (адрес), на якій сокет буде приймати підключення від клієнтів (127.0.0.1:8888)
                tcpServer.Bind(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8888));
                tcpServer.Listen();    // запуск прослуховування підключень
                Console.WriteLine("Server is started. Waiting connections... ");

                while (true)
                {
                    var tcpClient = tcpServer.Accept(); // отримуємо підключення у вигляді TcpClient
                    Console.WriteLine($"\n{tcpClient.RemoteEndPoint} \tConnection installed");
                    Thread thr = new Thread(() => ProcessClient(tcpClient, invertedIndex));
                    thr.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }         
        }

        public void ProcessClient(Socket tcpClient, InvertedIndex invertedIndex)
        {
            byte[] message = Encoding.ASCII.GetBytes("Connected!");
            int bytesSent = tcpClient.Send(message);
       
            byte[] messageBuffer = new byte[4096];
            while (true)
            {
                int bytesRead;
                try
                {
                    bytesRead = tcpClient.Receive(messageBuffer);
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

                var wordFromClient = Encoding.ASCII.GetString(messageBuffer, 0, bytesRead);
                Console.WriteLine($"Word from client: {wordFromClient}");

                List<string> listOfFiles = invertedIndex.GetAllFileNamesByWord(wordFromClient.ToLower());
                string files = "";
                if(listOfFiles.Count == 0)
                {
                    files = "No files were found";
                }
                else
                {
                    foreach (var file in listOfFiles)
                    {
                        files += (file + "\n");
                    }
                }
                           
                message = Encoding.ASCII.GetBytes(files);
                bytesSent = tcpClient.Send(message);
            }

            tcpClient.Close();
        }
    }
}
