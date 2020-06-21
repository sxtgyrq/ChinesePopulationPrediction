using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Assist
    {
        internal static void Do(string select)
        {
            int port;
            string ip = "10.80.52.218";
            //    Data dt;

            if (select.ToUpper() == "DEBUG")
            {
                port = 20701;
                ip = "127.0.0.1";
            }
            else
            {
                port = 20702;
                throw new Exception("");
            }
            while (true)
            {
                Thread.Sleep(1000 * 60);
                using (TcpClient client = new TcpClient(ip, port))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        var msg = "{\"Type\":\"Refresh\"}";
                        Byte[] msgData = System.Text.Encoding.UTF8.GetBytes(msg);
                        // Send the message to the connected TcpServer. 
                        stream.Write(msgData, 0, msgData.Length);
                        Console.WriteLine("RefreshSent: {0}", msg);
                        // Bytes Array to receive Server Response.
                        msgData = new Byte[2 * 1024 * 1024];
                        String response = String.Empty;
                        // Read the Tcp Server Response Bytes.
                        Int32 bytes = stream.Read(msgData, 0, msgData.Length);


                        response = System.Text.Encoding.UTF8.GetString(msgData, 0, bytes).Trim();
                        //Console.WriteLine($"收到二进制的长度{bytes};文本长度：{response.Length}");
                        Console.WriteLine("{0}Received: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), response);
                        stream.Close();

                    }
                    client.Close();
                }
            }
        }
    }
}
