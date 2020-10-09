using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CompulsoryAssignmentFall2020Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(ip, 4646);

            serverSocket.Start();

            while (true)
            {
                TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine("Starting New Service");
                TCPService service = new TCPService(connectionSocket);

                Thread thread = new Thread(service.StartService);
                thread.Start();

            }

            serverSocket.Stop();
        }
    }
}
