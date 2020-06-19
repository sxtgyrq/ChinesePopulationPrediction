using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        TcpListener server = null;

        int minitues { get; set; }

        int seconds { get; set; }
        public Server(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();

            var operateTimeNow = DateTime.Now;
            this.minitues = operateTimeNow.Minute;
            this.seconds = operateTimeNow.Second;
            //StartListener();
        }

        public void StartListener(ref Data data)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name}-{DateTime.Now.ToString("yyyy-MM-dd")}:Waiting for a connection...");
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}-{DateTime.Now.ToString("yyyy-MM-dd")}:Connected!");

                        using (var stream = client.GetStream())
                        {
                            try
                            {
                                Byte[] bytes = new Byte[2 * 1024 * 1024];//2M

                                stream.Read(bytes, 0, bytes.Length);

                                var ss = System.Text.Encoding.UTF8.GetString(bytes);

                                var result = dealWithMsg(ss, ref data);
                                string str = result.Trim();
                                Byte[] reply = System.Text.Encoding.UTF8.GetBytes(str);
                                stream.Write(reply, 0, reply.Length);
                            }
                            catch
                            {

                            }
                        }

                        // s.Read()



                    }

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }

        private string dealWithMsg(string ss, ref Data data)
        {
            //  return "";
            try
            {
                var c = Newtonsoft.Json.JsonConvert.DeserializeObject<Command>(ss);
                switch (c.Type)
                {
                    //case "GetNext":
                    //    {
                    //        var gn = Newtonsoft.Json.JsonConvert.DeserializeObject<ConsoleModel.GetNext>(c.JsonValue);
                    //        CityRunBussinessManager.WalkInTheMapManager.GetData gd = new CityRunBussinessManager.WalkInTheMapManager.GetData(data.GetData);
                    //        var selectionResult = CityRunBussinessManager.WalkInTheMapManager.GetJson(gn.roadCode, gn.roadOrder, gn.roadPercent, gd);
                    //        return selectionResult;
                    //    };
                    //case "GetCountOfFP":
                    //    {
                    //        var count = data.Get61Fp();
                    //        var countOfFP = new { Count = count };
                    //        var selectionResult = Newtonsoft.Json.JsonConvert.SerializeObject(countOfFP);
                    //        return selectionResult;
                    //    }; break;
                    //case "GetByIndex":
                    //    {
                    //        var gi = Newtonsoft.Json.JsonConvert.DeserializeObject<ConsoleModel.GetByIndex>(c.JsonValue);
                    //        decimal reward;
                    //        var fp = data.GetFpByIndex(gi.IndexValule, out reward);
                    //        if (fp != null)
                    //            return Newtonsoft.Json.JsonConvert.SerializeObject(new { fp = fp, reward = reward });
                    //    }; break;
                    //case "GetFPOnlyByIndex":
                    //    {
                    //        var gi = Newtonsoft.Json.JsonConvert.DeserializeObject<ConsoleModel.GetByIndex>(c.JsonValue);

                    //        var fp = data.GetFpByIndex(gi.IndexValule);
                    //        if (fp != null)
                    //            return Newtonsoft.Json.JsonConvert.SerializeObject(new { fp = fp, reward = 0 });
                    //    }; break;
                    //case "GI":
                    //    {
                    //        var gr = Newtonsoft.Json.JsonConvert.DeserializeObject<ConsoleModel.GetRoadInfo>(c.JsonValue);
                    //        return $"{{\"result\":{data.getCarInOpposeDirection(gr.RoadCode)}}}";
                    //    }; break;
                    //case "GetCost":
                    //    {
                    //        //var cost = data.getCurrentTimeCostOfEveryStep();
                    //        //return $"{{\"result\":{cost}}}";
                    //    }; break;
                    //case "SetReward":
                    //    {
                    //        var reward = Newtonsoft.Json.JsonConvert.DeserializeObject<ConsoleModel.Reward>(c.JsonValue);
                    //        data.SetReward(reward);
                    //        return "";
                    //    }; break;
                    //case "AToB":
                    //    {
                    //        //4s  373209M
                    //        var fpID1 = Convert.ToString(c.JsonValue.Substring(0, 10));
                    //        var fpID2 = Convert.ToString(c.JsonValue.Substring(10, 10));
                    //        return data.GetAFromB(fpID1, fpID2);
                    //    }; break;
                    //case "CToT":
                    //    {
                    //        //current position to target position
                    //        var cp = Newtonsoft.Json.JsonConvert.DeserializeObject<ConsoleModel.CurrentPosition>(c.JsonValue);
                    //        return data.ToTarget(cp);
                    //    };
                    case "employerGetRateOfRise":
                        {
                            return employerGetRateOfRise(data);
                            // return data.FirstIndex();
                        }
                    case "Refresh":
                        {
                            return Refresh(ref data);
                        }; break;
                    case "Employee-Love":
                        {
                            return Employee.Love(data, c);
                        }; break;

                }
                return "";
            }
            catch
            {
                Console.WriteLine($"{ss}_执行没结果");
                return "";
            }
            //throw new NotImplementedException();
        }

        public class Employer
        {
            public static int employerStrategyCount = 7;
        }

        public class Employee
        {
            /// <summary>
            /// 员工的来爱成功率
            /// </summary>
            /// <param name="data"></param>
            /// <param name="c"></param>
            /// <returns></returns>
            internal static string Love(ref Random rm, Data data, Command c)
            {
                /*
                 * 我们假设员工的恋爱成功率是受govementPosition影响的 
                 */
                var govementPosition = data.govementPosition.Last();

                //正常那么恋爱成功率为0.05~0.8
                var successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.375 + 0.425;

                var sumActionCount = data.employerActions.Sum(item => item.Count);

                short employerAction = -1;

                if (sumActionCount > 0)
                {
                    var randPosition = rm.Next(sumActionCount);
                    for (int i = 0; i < data.employerActions.Count; i++)
                    {
                        if (randPosition >= data.employerActions[i].Count)
                        {
                            randPosition -= data.employerActions[i].Count;
                            continue;
                        }
                        else
                        {
                            employerAction = data.employerActions[i][randPosition];
                            break;
                        }
                    }
                }

                if (employerAction == 1)
                {
                    //打工的时候，想谈恋爱，遇到了扯淡的996是福报论，那么恋爱成功率变为了0.05~0.7
                    successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.325 + 0.375;

                }
                else if (employerAction == 2)
                {
                    //打工的时候，想谈恋爱，遇到了单位只招年轻人，暂时假设，其不对恋爱成功率进行影响

                }
                else if (employerAction == 3)
                {
                    //打工的时候，想谈恋爱，遇到了老板招聘时，还歧视未婚未育女性，那么恋爱成功率变为了0.05~0.5
                    successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.225 + 0.275;
                }
                else if (employerAction == 4)
                {
                    //打工的时候，想谈恋爱，遇到了老板引进新技术时，
                    //  successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.225 + 0.275;
                }
                else if (employerAction == 5) 
                {
                    //打工的时候，想谈恋爱，遇到了老板引进新技术时，
                }

                // var employorIndex = rm.Next(0, Employer.employerStrategyCount);


                if (rm.NextDouble() < successLimet)
                {
                    return "1-True";
                }
                else
                {
                    return "1-False";
                }
            }
        }
        //private string Employee_Love(ref Data data)
        //{
        //    throw new NotImplementedException();
        //}

        private string employerGetRateOfRise(Data data)
        {
            return $"{{\"businessRate\":{data.businessRate}}}";
        }

        private string Refresh(ref Data data)
        {
            var operateTime = DateTime.Now.AddMinutes(-this.minitues).AddSeconds(-this.seconds);
            var currentState = operateTime.ToString("yyyy-MM-dd-HH");

            if (data.state == currentState) { }
            else
            {
                //企业收入，对企业增长率的影响，类似于政府调整企业税收。
                if (data.employerSumEarn.Sum() > 24 * 30)
                {
                    data.businessRate += 0.01;
                }
                else
                {
                    data.businessRate -= 0.01;
                }
                data.businessRate = Math.Min(data.businessRate, 1.20);
                data.businessRate = Math.Max(data.businessRate, 1.05);

                //打工收入，对教育成本的影响，类似于收入增加，个人所得税增加。这里表现为教育成本增加
                if (data.employteenSumEarn.Sum() > 24 * 30)
                {

                }

                var lastItem = data.rencaiLevel.Last();
                data.rencaiLevel.Add(new long[] { 0, lastItem[1] });
                while (data.rencaiLevel.Count > 24)
                {
                    data.rencaiLevel.RemoveAt(0);
                }

                data.govementPosition.Add(data.govementPosition.Last());
                while (data.govementPosition.Count > 24)
                {
                    data.govementPosition.RemoveAt(0);
                }

                data.employteenSumEarn.Add(0);
                while (data.employteenSumEarn.Count > 24)
                {
                    data.employteenSumEarn.RemoveAt(0);
                }

                data.employerSumEarn.Add(0);
                while (data.employerSumEarn.Count > 24)
                {
                    data.employerSumEarn.RemoveAt(0);
                }

                data.educationCost.Add(new List<double>());
                while (data.educationCost.Count > 24)
                {
                    data.educationCost.RemoveAt(0);
                }

                data.employerActions.Add(new List<short>());
                while (data.employerActions.Count > 24)
                {
                    data.employerActions.RemoveAt(0);
                }

                data.employerCountLast24Hour.Add(0);
                while (data.employerCountLast24Hour.Count > 24)
                {
                    data.employerCountLast24Hour.RemoveAt(0);
                }

                data.employteenActions.Add(new List<short>());
                while (data.employteenActions.Count > 24)
                {
                    data.employteenActions.RemoveAt(0);
                }



                data.state = currentState;

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                File.WriteAllText("Data.json", json);
            }

            return $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--Refrest OK";

        }

        public class Command
        {
            public string Type { get; set; }
            public string JsonValue { get; set; }
        }
    }
}
