using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.ConsoleClient
{
    class temp
    {
        static void MMain(string[] args)
        {
            try
            {
                byte[] input = BitConverter.GetBytes(1);
                byte[] buffer = new byte[4096];
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                socket.Bind(new IPEndPoint(IPAddress.Parse("10.240.1.249"), 0));
                socket.IOControl(IOControlCode.ReceiveAll, input, null);

                int bytes = 0;
                do
                {
                    bytes = socket.Receive(buffer);
                    if (bytes > 0)
                    {
                        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytes));
                    }
                } while (bytes > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
