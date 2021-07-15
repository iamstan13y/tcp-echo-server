using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TcpEchoServer
{
    class Program
    {
        private const int BUFFSIZE = 32;
        static void Main(string[] args)
        {
            int port = 8080;
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                Environment.Exit(se.ErrorCode);
            }

            byte[] rcvBuffer = new byte[BUFFSIZE];
            int bytesRcvd;

            for (;;)
            {
                TcpClient client = null;
                NetworkStream netStream = null;

                try
                {
                    client = listener.AcceptTcpClient();
                    netStream = client.GetStream();
                    Console.Write("Handling Client - ");

                    int totalBytesEchoed = 0;
                    while ((bytesRcvd = netStream.Read(rcvBuffer, 0, rcvBuffer.Length)) > 0)
                    {
                        netStream.Write(rcvBuffer, 0, bytesRcvd);
                        totalBytesEchoed += bytesRcvd;
                    }

                    Console.WriteLine("CLIENT: {0} ", Encoding.ASCII.GetString(rcvBuffer, 0, totalBytesEchoed));
                    netStream.Close();
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    netStream.Close();
                }
            }

        }
    }
}
