using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class MainServer
    {
        internal static void Do(string select)
        {
            int port;
            string ip = "10.80.52.218";
            Data dt;

            if (select.ToUpper() == "DEBUG")
            {
                port = 20701;
                ip = "127.0.0.1";
                initialData();

                var fileName = "Data.json";
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    var json = sr.ReadToEnd();
                    dt = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(json);


                }
                Console.WriteLine("读取数据成功，按任意键继续");
                // Console.ReadLine();
            }
            else
            {
                port = 20702;
                throw new Exception("");
            }
            // var ip = data.IP;
            var s = new Server(ip, port);
            s.StartListener(ref dt);
        }

        private static void initialData()
        {
            //  File.WriteAllText("Data.json", json);
            if (File.Exists("Data.json"))
            {
                return;
            }
            else
            {
                var data = new Data()
                {
                    businessRate = 1.12,
                    educationBasePercent = 1,
                    educationCost = new List<List<double>>()
                    {
                        new List<double>(),new List<double>(),new List<double>(),
                        new List<double>(),new List<double>(),new List<double>(),
                        new List<double>(),new List<double>(),new List<double>(),
                        new List<double>(),new List<double>(),new List<double>(),
                        new List<double>(),new List<double>(),new List<double>(),
                        new List<double>(),new List<double>(),new List<double>(),
                        new List<double>(),new List<double>(),new List<double>(),
                        new List<double>(),new List<double>(),new List<double>(),
                    },
                    employerActions = new List<List<short>>()
                     {
                        new List<short>(),new List<short>(),new List<short>(),
                        new List<short>(),new List<short>(),new List<short>(),
                        new List<short>(),new List<short>(),new List<short>(),
                        new List<short>(),new List<short>(),new List<short>(),
                        new List<short>(),new List<short>(),new List<short>(),
                        new List<short>(),new List<short>(),new List<short>(),
                        new List<short>(),new List<short>(),new List<short>(),
                        new List<short>(),new List<short>(),new List<short>(),
                     },
                    employerCountLast24Hour = new List<int>()
                     {
                         0,0,0,0,0,0,0,0,
                         0,0,0,0,0,0,0,0,
                         0,0,0,0,0,0,0,0
                     },
                    employerSumEarn = new List<double>()
                      {
                          0,0,0,0,0,0,0,0,
                          0,0,0,0,0,0,0,0,
                          0,0,0,0,0,0,0,0
                      },
                    employteenActions = new List<List<short>>()
                     {
                          new List<short>(),new List<short>(),new List<short>(),
                          new List<short>(),new List<short>(),new List<short>(),
                          new List<short>(),new List<short>(),new List<short>(),
                          new List<short>(),new List<short>(),new List<short>(),
                          new List<short>(),new List<short>(),new List<short>(),
                          new List<short>(),new List<short>(),new List<short>(),
                          new List<short>(),new List<short>(),new List<short>(),
                          new List<short>(),new List<short>(),new List<short>(),
                     },
                    employteenSumEarn = new List<double>()
                     {
                         0,0,0,0,0,0,0,0,
                         0,0,0,0,0,0,0,0,
                         0,0,0,0,0,0,0,0
                     },
                    govementPosition = new List<double>()
                    {
                        50,50,50,50,50,50,50,50,
                        50,50,50,50,50,50,50,50,
                        50,50,50,50,50,50,50,50
                    },
                    rencaiLevel = new List<long[]>
                     {
                         new long[]{ 0,0},new long[]{ 0,0},new long[]{ 0,0},
                         new long[]{ 0,0},new long[]{ 0,0},new long[]{ 0,0},
                         new long[]{ 0,0},new long[]{ 0,0},new long[]{ 0,0},
                         new long[]{ 0,0},new long[]{ 0,0},new long[]{ 0,0},
                         new long[]{ 0,0},new long[]{ 0,0},new long[]{ 0,0},
                         new long[]{ 0,0},new long[]{ 0,0},new long[]{ 0,0},
                         new long[]{ 0,0},new long[]{ 0,0},new long[]{ 0,0},
                         new long[]{ 0,0},new long[]{ 0,0},new long[]{ 0,0},
                     },
                    state = DateTime.Now.ToString("yyyy-MM-dd-HH"),
                    housePrice = 1

                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                File.WriteAllText("Data.json", json);
                return;
            }
        }
    }
}
