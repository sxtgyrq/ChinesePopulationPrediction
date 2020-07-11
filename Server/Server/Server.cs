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
                    case "Employee":
                        {
                            return Employee.DoAction(ref this.rm, data, c);
                        }; break;
                        //case "Employee-Love":
                        //    {
                        //        return Employee.Love(ref this.rm, data, c);
                        //    }; break;
                        //case "Employee-Marry":
                        //    {
                        //        return Employee.Marry(ref this.rm, data, c);
                        //    }; break;

                        //case "Employee-GetFirstBaby":
                        //    {
                        //        return Employee.GetFirstBaby(ref this.rm, data, c);
                        //    }; break;


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
            internal static string DoAction(ref Random rm, Data data, Command c)
            {
                Client.Employee.PassObj passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Client.Employee.PassObj>(c.JsonValue);

                passObj.state.age++;
                passObj.state.year++;

                if (passObj.state.fourthBabyAge != null)
                {
                    passObj.state.fourthBabyAge++;
                }

                if (passObj.state.thirdBabyAge != null)
                {
                    passObj.state.thirdBabyAge++;
                }

                if (passObj.state.secondBabyAge != null)
                {
                    passObj.state.secondBabyAge++;
                }

                if (passObj.state.firstBabyAge != null)
                {
                    passObj.state.firstBabyAge++;

                    setChildEducate(passObj.state.firstBabyAge, 1, ref passObj, data);
                    //if (passObj.state.firstBabyAge > 3) 
                    //{
                    //    if (passObj.actions.Contains("Employee-Love"))
                    //}
                }







                if (passObj.actions.Contains("Employee-Love"))
                {
                    return Love(ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-Marry"))
                {
                    return Marry(ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-GetFirstBaby"))
                {
                    return GetFirstBaby(ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-GetSecondBaby"))
                {
                    return GetSecondBaby(ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-GetThirdBaby"))
                {
                    return GetThirdBaby(ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-GetFourthBaby"))
                {
                    return GetFourthBaby(ref rm, data, c, ref passObj);
                }
                //else if (passObj.actions.Contains("Employee-GetFourthBaby")) 
                //{

                //}
                else
                {
                    Console.WriteLine($"以下这些命令不知如何处理！");
                    foreach (var item in passObj.actions)
                    {
                        Console.WriteLine($"    {item}");
                    }
                    Console.WriteLine($"此致");
                    return "";
                }
                //throw new NotImplementedException();
            }

            private static void setChildEducate(int? babyAge, int babyIndex, ref Client.Employee.PassObj passObj, Data data)
            {
                var selfPay = Math.Round(1.0 / 50 * data.govementPosition.Last(), 2);
                var govementPay = 1 - selfPay;
                //bool moneyIsNotEnought = false;
                if (babyAge != null && babyAge.Value > 3)
                {

                    while (passObj.actions.Contains("Employee-Educate-Perfect"))
                    {
                        List<double> prices = new List<double>();
                        for (var i = 0; i < data.educationCost.Count; i++)
                        {
                            for (var j = 0; j < data.educationCost[i].Count; j++)
                            {
                                prices.Add(data.educationCost[i][j]);
                            }
                        }
                        var newPrice = (prices.OrderByDescending(item => item)).ToList();

                        double price = 0.01;

                        if (prices.Count == 0)
                        {
                            price = 0.01;
                        }
                        else
                        {
                            //2% ~10%
                            var p = -0.08 / 100 * data.govementPosition.Last() + 0.10;
                            var indexV = Convert.ToInt32(Math.Floor(p * newPrice.Count));
                            price = newPrice[indexV] + 0.01;
                        }
                        if (passObj.state.sumSave - (price * selfPay) >= 0)
                        {
                            passObj.state.sumSave -= (price * selfPay);
                            passObj.state.educationCost[babyIndex].Add(price);
                            passObj.notifyMsgs.Add($"您的第{babyIndex}个孩子接受到了优质教育，个人花费${(price * selfPay).ToString("f2")}金币");
                            if (govementPay > 0)
                            {
                                passObj.notifyMsgs.Add($"您的第{babyIndex}个孩子接受到了优质教育，政府资助您${(price * govementPay).ToString("f2")}金币");
                            }
                            else if (govementPay < 0)
                            {
                                passObj.notifyMsgs.Add($"您的第{babyIndex}个孩子接受到了优质教育，您资助政府${(price * (-govementPay)).ToString("f2")}金币作为教育附加税。");
                            }
                            //    Console.WriteLine();

                            return;
                        }
                        else
                        {
                            break;
                        }
                    }

                    while (passObj.actions.Contains("Employee-Educate-Good") || passObj.actions.Contains("Employee-Educate-Perfect"))
                    {
                        List<double> prices = new List<double>();
                        for (var i = 0; i < data.educationCost.Count; i++)
                        {
                            for (var j = 0; j < data.educationCost[i].Count; j++)
                            {
                                prices.Add(data.educationCost[i][j]);
                            }
                        }
                        var newPrice = (prices.OrderByDescending(item => item)).ToList();

                        double price = 0.01;
                        if (prices.Count == 0)
                        {
                            price = 0.01;
                        }
                        else
                        {
                            //98% ~10%
                            var p = -0.88 / 100 * data.govementPosition.Last() + 0.98;
                            var indexV = Convert.ToInt32(Math.Floor(p * newPrice.Count));
                            price = newPrice[indexV] + 0.01;
                        }


                        if (passObj.state.sumSave - (price * selfPay) >= 0)
                        {
                            passObj.state.sumSave -= (price * selfPay);
                            if (passObj.actions.Contains("Employee-Educate-Perfect"))
                            {
                                passObj.notifyMsgs.Add($"由于您的钱不够支持您的第{babyIndex}个孩子的顶级教育费用，他只能接受到了优质教育，个人花费${(price * selfPay).ToString("f2")}金币");
                                if (govementPay > 0)
                                {
                                    passObj.notifyMsgs.Add($"由于您的钱不够支持您的第{babyIndex}个孩子的顶级教育费用，他只能接受到了优质教育，政府资助${(price * govementPay).ToString("f2")}金币");
                                }
                                else if (govementPay < 0)
                                {
                                    passObj.notifyMsgs.Add($"由于您的钱不够支持您的第{babyIndex}个孩子的顶级教育费用，他只能接受到了优质教育，您资助政府${(price * -govementPay).ToString("f2")}金币");
                                }
                            }
                            else
                            {
                                passObj.notifyMsgs.Add($"由于您的第{babyIndex}个孩子接受到了优质教育，个人花费${(price * selfPay).ToString("f2")}金币");
                                if (govementPay > 0)
                                {
                                    passObj.notifyMsgs.Add($"您的第{babyIndex}个孩子接受到了优质教育，政府资助${(price * govementPay).ToString("f2")}金币");
                                }
                                else if (govementPay < 0)
                                {
                                    passObj.notifyMsgs.Add($"您的第{babyIndex}个孩子接受到了优质教育，您资助政府${(price * -govementPay).ToString("f2")}金币");
                                }
                            }
                            passObj.state.educationCost[babyIndex].Add(price);
                            // Console.WriteLine($"您的第{babyIndex}个孩子接受到了良好教育，花费${price.ToString("f2")}金币");
                            return;
                        }
                        else
                        {

                        }
                        break;
                    }

                    while (true)
                    {
                        List<double> prices = new List<double>();
                        for (var i = 0; i < data.educationCost.Count; i++)
                        {
                            for (var j = 0; j < data.educationCost[i].Count; j++)
                            {
                                prices.Add(data.educationCost[i][j]);
                            }
                        }
                        var newPrice = (prices.OrderByDescending(item => item)).ToList();

                        double price = 0.01;
                        if (prices.Count == 0)
                        {
                            price = 0.01;
                        }
                        else
                        {
                            //98% ~10%
                            //var p = -0.88 / 100 * data.govementPosition.Last() + 0.98;
                            //var indexV = Convert.ToInt32(Math.Floor(p * newPrice.Count));
                            price = data.govementPosition.Last() + 0.01;
                        }

                        passObj.state.sumSave -= price;
                        if (passObj.actions.Contains("Employee-Educate-Perfect"))
                        {
                            passObj.notifyMsgs.Add($"由于您的钱不够支持您的第{babyIndex}个孩子的顶级教育费用，他只能接受到了普通教育，花费${price.ToString("f2")}金币");
                        }
                        else if (passObj.actions.Contains("Employee-Educate-Good"))
                        {
                            passObj.notifyMsgs.Add($"由于您的钱不够支持您的第{babyIndex}个孩子的优质教育费用，他只能接受到了普通教育，花费${price.ToString("f2")}金币");
                        }
                        passObj.state.educationCost[babyIndex].Add(price);
                        Console.WriteLine($"您的第{babyIndex}个孩子接受到了普通教育，花费${price.ToString("f2")}金币");
                        return;

                        break;
                    }
                }
                throw new NotImplementedException();
            }

            private static string GetFourthBaby(ref Random rm, Data data, Command c, ref Client.Employee.PassObj passObj)
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

                passObj.notifyMsgs.Add($"打工收入，获得{salary.ToString("f2")}金币；");
                passObj.state.sumSave += salary;
                if (rm.NextDouble() < successLimet)
                {
                    passObj.state.canGetFourthBaby = false;
                    passObj.state.fourthBabyAge = 0;
                    passObj.state.educationCost.Add(4, new List<double>());
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
                else
                {

                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
            }

            private static string GetThirdBaby(ref Random rm, Data data, Command c, ref Client.Employee.PassObj passObj)
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

                passObj.notifyMsgs.Add($"打工收入，获得{salary.ToString("f2")}金币；");
                passObj.state.sumSave += salary;
                if (rm.NextDouble() < successLimet)
                {
                    passObj.state.canGetThirdBaby = false;
                    passObj.state.canGetFourthBaby = true;
                    passObj.state.thirdBabyAge = 0;
                    passObj.state.educationCost.Add(3, new List<double>());
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
                else
                {

                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
            }

            private static string GetSecondBaby(ref Random rm, Data data, Command c, ref Client.Employee.PassObj passObj)
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

                passObj.notifyMsgs.Add($"打工收入，获得{salary.ToString("f2")}金币；");
                passObj.state.sumSave += salary;
                if (rm.NextDouble() < successLimet)
                {
                    passObj.state.canGetSecondBaby = false;
                    passObj.state.canGetThirdBaby = true;
                    passObj.state.secondBabyAge = 0;
                    passObj.state.educationCost.Add(2, new List<double>());
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
                else
                {

                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
            }

            internal static string GetFirstBaby(ref Random rm, Data data, Command c, ref Client.Employee.PassObj passObj)
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

                passObj.notifyMsgs.Add($"打工收入，获得{salary.ToString("f2")}金币；");
                passObj.state.sumSave += salary;
                if (rm.NextDouble() < successLimet)
                {
                    passObj.state.firstBabyAge = 0;
                    passObj.state.canGetSecondBaby = true;
                    passObj.state.canGetFirstBaby = false;
                    passObj.state.canPlayWithChildren = true;
                    passObj.state.canEducate = true;
                    passObj.state.educationCost.Add(1, new List<double>());
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
                else
                {

                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
            }

            /// <summary>
            /// 员工的来爱成功率
            /// </summary>
            /// <param name="data"></param>
            /// <param name="c"></param>
            /// <returns></returns>
            internal static string Love(ref Random rm, Data data, Command c, ref Client.Employee.PassObj passObj)
            {
                if (passObj.state.canLove) { }
                else
                {
                    return "";
                }
                //Client.Employee.PassObj passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Client.Employee.PassObj>(c.JsonValue);
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
                passObj.state.sumSave += salary;
                {
                    //恋爱、政府、企业主对打工收入的影响。
                }
                // var employorIndex = rm.Next(0, Employer.employerStrategyCount);



                if (rm.NextDouble() < successLimet)
                {
                    passObj.notifyMsgs.Add("你恋爱成功了！");
                    passObj.notifyMsgs.Add("你开启了新技能--结婚！");
                    passObj.actions = new List<string>();
                    passObj.state.canLove = false;
                    passObj.state.canBeMarried = true;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj); ;
                }
                else
                {
                    passObj.notifyMsgs.Add("你恋爱失败了，被甩了！");
                    passObj.notifyMsgs.Add("再接再厉！");
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj); ;
                }
            }
            /// <summary>
            /// 员工的恋爱成功率
            /// </summary>
            /// <param name="data"></param>
            /// <param name="c"></param>
            /// <returns></returns>
            internal static string Marry(ref Random rm, Data data, Command c, ref Client.Employee.PassObj passObj)
            {
                var govementPosition = data.govementPosition.Last();
                double salary = Math.Cos(govementPosition / 100 * Math.PI) * 0.5 + 1;//

                passObj.state.sumSave += salary;
                {
                    //恋爱、政府、企业主对打工收入的影响。
                }
                passObj.notifyMsgs.Add($"你获得了打工的薪水{salary.ToString("f2")}金币");

                if (passObj.state.sumSave >= data.housePrice * 2)
                {
                    //  var marryCondition = Newtonsoft.Json.JsonConvert.DeserializeObject<MarryCondition>(c.JsonValue);
                    /*
                     * 我们假设员工的恋爱成功率是受govementPosition影响的 
                     */
                    //var govementPosition = data.govementPosition.Last();

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




                    //嫁妆


                    if (rm.NextDouble() < successLimit)
                    {
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
                        passObj.state.sumSave -= data.housePrice * 2;
                        passObj.notifyMsgs.Add($"彩礼和房价总共花费{(data.housePrice * 2).ToString("f2")}金币！");

                        passObj.state.sumSave += dowry;
                        passObj.notifyMsgs.Add($"你获得了{dowry.ToString("f2")}金币作为长辈对你结婚的奖赏！");



                        passObj.state.canBeMarried = false;
                        passObj.state.canGetFirstBaby = true;
                        return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    }
                    else
                    {
                        passObj.notifyMsgs.Add($"结婚还是失败！");
                        return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    }
                }
                else
                {
                    passObj.notifyMsgs.Add($"你的储蓄不够房价+彩礼{data.housePrice * 2},结婚失败！");
                    passObj.notifyMsgs.Add("再接再厉！");
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj); ;
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
                    data.housePrice += 0.01;
                    data.housePrice *= 1.01;

                }
                else
                {
                    data.housePrice *= 0.9;
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
