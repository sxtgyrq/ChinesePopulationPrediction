﻿using System;
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

        Random rm;
        public Server(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();

            var operateTimeNow = DateTime.Now;
            this.minitues = operateTimeNow.Minute;
            this.seconds = operateTimeNow.Second;
            //StartListener();
            this.rm = new Random(DateTime.Now.GetHashCode());
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
                            return Refresh(ref rm, ref data);
                        }; break;
                    case "Employee-Love":
                        {
                            return Employee.Love(ref this.rm, data, c);
                        }; break;
                    case "Employee-Marry":
                        {
                            return Employee.Marry(ref this.rm, data, c);
                        }; break;

                    case "Employee-GetFirstBaby":
                        {
                            return Employee.GetFirstBaby(ref this.rm, data, c);
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
            internal static string GetFirstBaby(ref Random rm, Data data, Command c)
            {
                var govementPosition = data.govementPosition.Last();
                var successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.375 + 0.425;

                var cost = data.housePrice * 0.2;
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
                double salary = Math.Cos(govementPosition / 100 * Math.PI) * 0.5 + 1;//
                if (rm.NextDouble() < successLimet)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new { result = "getfirstbaby-success", employerAction = employerAction, salary = salary, cost = cost }); ;
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new { result = "getfirstbaby-failure", employerAction = employerAction, salary = salary, cost = cost }); ;
                }
            }

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
                    //打工的时候，想谈恋爱，遇到了老板引进新技术时，成功时
                    successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.225 + 0.275;
                }
                else if (employerAction == 5)
                {
                    //打工的时候，想谈恋爱，遇到了老板引进新技术时，失败时，0.05~0.7
                    successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.325 + 0.375;
                }
                else if (employerAction == 6)
                {
                    //打工的时候，遇上老板转移产业，反而有助于提升恋爱率0.05~0.85
                    successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.4 + 0.45;

                }
                else if (employerAction == 7)
                {
                    //打工的时候，遇上老板提高福利，应该是有利于提升恋爱成功率的。0.1~0.9
                    successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.4 + 0.5;
                }

                double salary = Math.Cos(govementPosition / 100 * Math.PI) * 0.5 + 1;//
                {
                    //恋爱、政府、企业主对打工收入的影响。
                }
                // var employorIndex = rm.Next(0, Employer.employerStrategyCount);


                if (rm.NextDouble() < successLimet)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new { result = "love-success", employerAction = employerAction, salary = salary }); ;
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new { result = "love-failure", employerAction = employerAction, salary = salary }); ;
                }
            }
            /// <summary>
            /// 员工的恋爱成功率
            /// </summary>
            /// <param name="data"></param>
            /// <param name="c"></param>
            /// <returns></returns>
            internal static string Marry(ref Random rm, Data data, Command c)
            {
                var marryCondition = Newtonsoft.Json.JsonConvert.DeserializeObject<MarryCondition>(c.JsonValue);
                /*
                 * 我们假设员工的恋爱成功率是受govementPosition影响的 
                 */
                var govementPosition = data.govementPosition.Last();

                //正常那么恋爱成功率为0.05~0.8
                var successLimit = Math.Sin((govementPosition - 50) / 100 * Math.PI) * 0.2 + 0.7;

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
                    // successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.325 + 0.375;

                }
                else if (employerAction == 2)
                {
                    //打工的时候，想谈恋爱，遇到了单位只招年轻人，暂时假设，其不对恋爱成功率进行影响

                }
                else if (employerAction == 3)
                {
                    //打工的时候，想谈恋爱，遇到了老板招聘时，还歧视未婚未育女性，那么恋爱成功率变为了0.05~0.5
                    //  successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.225 + 0.275;
                }
                else if (employerAction == 4)
                {
                    //打工的时候，想谈恋爱，遇到了老板引进新技术时，成功时
                    //  successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.225 + 0.275;
                }
                else if (employerAction == 5)
                {
                    //打工的时候，想谈恋爱，遇到了老板引进新技术时，失败时，0.05~0.7
                    // successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.325 + 0.375;
                }
                else if (employerAction == 6)
                {
                    //打工的时候，遇上老板转移产业，反而有助于提升恋爱率0.05~0.85
                    //successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.4 + 0.45;

                }
                else if (employerAction == 7)
                {
                    //打工的时候，遇上老板提高福利，应该是有利于提升恋爱成功率的。0.1~0.9
                    //successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.4 + 0.5;
                }
                // var employorIndex = rm.Next(0, Employer.employerStrategyCount);

                double salary = Math.Cos(govementPosition / 100 * Math.PI) * 0.5 + 1;
                {
                    //恋爱、政府、企业主对打工收入的影响。
                }
                //嫁妆
                double dowry;
                if (govementPosition < 50)
                {
                    dowry = (Math.Cos(govementPosition / 100 * Math.PI) * 2 + 1) * data.housePrice;

                }
                else
                {
                    dowry = (Math.Cos(govementPosition / 100 * Math.PI) * 1 + 1) * data.housePrice;
                }
                {
                    //政府、企业对嫁妆的影响。
                }

                if (rm.NextDouble() < successLimit)
                {

                    if (marryCondition.sumSave >= data.housePrice * 2)
                    {

                        var obj = new { result = "marry-success", employerAction = employerAction, salary = salary, dowry = dowry, housePrice = data.housePrice };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    }
                    else
                    {
                        var obj = new { result = "marry-failure-bride", employerAction = employerAction, salary = salary, dowry = dowry, housePrice = data.housePrice };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    }

                    //return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                }
                else
                {
                    var obj = new { result = "marry-failure", employerAction = employerAction, salary = salary, dowry = dowry, housePrice = data.housePrice };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
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

        private string Refresh(ref Random rm, ref Data data)
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
                    if (rm.Next(99) < data.govementPosition.Last())
                    {
                        //政府偏向资本时，且来百姓有钱时，政府更偏向于提高房价
                        data.housePrice += 0.01;
                        data.housePrice *= 1.01;
                    }
                    else
                    {
                        //政府偏向人民使，且百姓有钱时，政府更偏向于调高教育质量
                        //所以教育资本会增加
                        data.educateBasePrice += 0.01;
                        data.educateBasePrice *= 1.01;
                    }

                }
                else
                {
                    if (rm.Next(99) < data.govementPosition.Last())
                    {
                        //政府偏向资本时，且来百姓没钱时，政府更偏向于降低房价

                        data.housePrice *= 0.9;
                    }
                    else
                    {
                        //政府偏向人民使，且百姓有钱时，政府更偏向于降低教育支出
                        //所以教育资本会增加
                        data.educateBasePrice *= 0.9;
                    }
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

        public class MarryCondition
        {
            public double sumSave { get; set; }
        }
    }
}
