using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core_Shengyulv
{
    public class DataConmunicate
    {

        internal static void DoAsync(string select)
        {
            //  Thread.CurrentThread.Name = "Main";
            //using (StreamReader sr = new StreamReader(""))
            //{
            //    // Read the stream to a string, and write the string to the console.
            //    json = sr.ReadToEnd();
            //}
            //Data d= 
            int port = CaseManagerCore.IpAndPortManager.Population_Port;
            string ip = CaseManagerCore.IpAndPortManager.Population_Ip;
            CaseManagerCore.Population.Data dt;

            {
                var fileName = "Data.dt";
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    var json = sr.ReadToEnd();
                    dt = Newtonsoft.Json.JsonConvert.DeserializeObject<CaseManagerCore.Population.Data>(json);


                }
                Console.WriteLine("读取数据Data.json成功，按任意键继续");
            }
            // var ip = data.IP;
            var s = new Server(ip, port);
            _ = s.StartListenerAsync(dt);
        }
    }
}
