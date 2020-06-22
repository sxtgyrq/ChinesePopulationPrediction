using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    public class Employer
    {
    }
    public class Employee
    {
        bool canLove, canBeMarried, canGetFirstBaby, canGetSecondBaby, canEducate, canSingleWork, canPlayWithChildren, canStrugle;

        int? fisrtBabyAge, secondBabyAge, thirdBabyAge, fourthBabyAge;
        public string ageDisplay
        {
            get { return $"你今天{this.age}岁。"; }
        }

        public string ageSumSave
        {
            get { return $"你现在有{this.sumSave.ToString("f2")}金币的储蓄！"; }
        }
        double sumSave = 0;


        int age = 22;
        internal string selection
        {
            get
            {
                var msg = "";
                if (this.canLove)
                {
                    msg += "1.恋爱";
                    msg += Environment.NewLine;
                }
                if (this.canBeMarried)
                {
                    msg += "2.结婚";
                    msg += Environment.NewLine;
                }
                if (this.canGetFirstBaby)
                {
                    msg += "3.要一胎";
                    msg += Environment.NewLine;
                }
                if (this.canGetSecondBaby)
                {
                    msg += "4.要二胎";
                    msg += Environment.NewLine;
                }
                if (this.canEducate)
                {
                    msg += "5.教育";
                    msg += Environment.NewLine;
                }
                if (this.canSingleWork)
                {
                    msg += "6.男方上班，女方顾家";
                    msg += Environment.NewLine;
                }
                if (this.canPlayWithChildren)
                {
                    msg += "7.陪伴小孩，亲子";
                    msg += Environment.NewLine;
                }
                if (this.canStrugle)
                {
                    msg += "8.奋斗";
                    msg += Environment.NewLine;
                }

                return msg;
            }
        }

        public List<string> actions = new List<string>();
        internal void DealWithCommand(string command)
        {
            var msg = "";
            switch (command)
            {
                case "1":
                    {
                        if (this.canLove)
                        {
                            var action = "Employee-Love";
                            if (this.actions.Contains(action))
                            {

                            }
                            else
                            {
                                this.actions.Add(action);
                            }
                            //var JsonValue = this.ToString();
                            //msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Love", });
                        }
                    }; break;
                case "2":
                    {
                        var action = "Employee-Marry";
                        if (this.actions.Contains(action))
                        {

                        }
                        else
                        {
                            this.actions.Add(action);
                        }
                        //if (this.canBeMarried)
                        //{
                        //    var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { sumSave = this.sumSave, canStrugle = this.canStrugle });
                        //    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Marry", JsonValue = JsonValue });
                        //}
                    }; break;
                case "3":
                    {
                        if (this.canGetFirstBaby)
                        {
                            var action = "Employee-GetFirstBaby";
                            if (this.actions.Contains(action))
                            {

                            }
                            else
                            {
                                this.actions.Add(action);
                            }
                        }
                        //if (this.canGetFirstBaby)
                        //{
                        //    var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                        //    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-GetFirstBaby", canStrugle = this.canStrugle });
                        //}
                    }; break;
                case "4":
                    {
                        if (this.canGetSecondBaby)
                        {
                            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-GetSecondBaby", canStrugle = this.canStrugle });
                        }
                    }; break;
                case "5":
                    {
                        if (this.canEducate)
                        {
                            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Educate" });
                        }
                    }; break;
                case "6":
                    {
                        if (this.canSingleWork)
                        {
                            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-SingleIncome" });
                        }
                    }; break;
                case "7":
                    {
                        if (this.canPlayWithChildren)
                        {
                            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-PlayWithChildren" });
                        }
                    }; break;
                case "8":
                    {
                        if (this.canStrugle)
                        {

                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Strugle" });
                        }
                    }; break;
                case "Next":
                    {
                        if (this.canStrugle)
                        {

                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Strugle" });
                        }
                    }; break;

                    //case (command) { }
            }
            if (command == "Next") { }
            if (!string.IsNullOrEmpty(msg))
            {
                var ip = "127.0.0.1";
                int port = 20701;
                using (TcpClient client = new TcpClient(ip, port))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        //var msg = "{\"Type\":\"Refresh\"}";
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

                        switch (command)
                        {
                            case "1":
                                {
                                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<LoveResult>(response);
                                    if (result.result == "love-success")
                                    {
                                        this.canBeMarried = true;
                                        this.canLove = false;
                                        Console.WriteLine("你恋爱成功了，开启了新技能-结婚！");
                                        this.year++;
                                        this.age++;

                                        Console.WriteLine($"今年打工时，获得了{ result.salary.ToString("f2")}金币");
                                        this.sumSave += result.salary;
                                    }
                                    else if (result.result == "love-failure")
                                    {
                                        Console.WriteLine("你恋爱失败喽！再接再厉！");
                                        this.year++;
                                        this.age++;

                                        Console.WriteLine($"今年打工时，获得了{ result.salary.ToString("f2")}金币");
                                        this.sumSave += result.salary;
                                    }
                                    else
                                    {
                                        throw new Exception(result.result);
                                    }
                                }; break;
                            case "2":
                                {
                                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<MarryResult>(response);
                                    if (result.result == "marry-success")
                                    {
                                        this.canBeMarried = false;
                                        this.canGetFirstBaby = true;
                                        Console.WriteLine("你开启了新技能，生一胎！！！");


                                        this.sumSave -= result.housePrice * 2;
                                        Console.WriteLine($"结婚时，买房和彩礼，一共花费掉了你{(result.housePrice * 2).ToString("f2")}金币。");

                                        this.sumSave += result.dowry;
                                        Console.WriteLine($"结婚时，你收到了来自长辈的祝福——{ result.dowry.ToString("f2")}金币。");

                                        this.age++;
                                        Console.WriteLine($"今年打工时，获得了{ result.salary.ToString("f2")}金币");
                                        this.sumSave += result.salary;
                                    }
                                    else if (result.result == "marry-failure-bride")
                                    {
                                        Console.WriteLine("由于你的积蓄不够彩礼和房款，你的另一半跟人跑了");
                                        this.canBeMarried = false;
                                        this.canLove = true;
                                        this.age++;
                                        Console.WriteLine($"今年打工时，获得了{ result.salary.ToString("f2")}金币");
                                        this.sumSave += result.salary;
                                    }
                                    else if (result.result == "marry-failure")
                                    {
                                        //result.
                                        Console.WriteLine("由于各种原因，你们还是没有组成婚姻。");
                                        this.canBeMarried = false;
                                        this.canLove = true;
                                        this.age++;
                                        Console.WriteLine($"今年打工时，获得了{ result.salary.ToString("f2")}金币");
                                        this.sumSave += result.salary;
                                    }
                                    else
                                    {
                                        throw new Exception("");
                                    }
                                }; break;
                            case "3":
                                {
                                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFirstBabyResult>(response);
                                    if (result.result == "getfirstbaby-success")
                                    {
                                        this.age++;

                                        Console.WriteLine("生一胎--成功！");
                                        this.canGetFirstBaby = false;

                                        Console.WriteLine($"你开启了新技能--生二胎");
                                        this.canGetSecondBaby = true;
                                        this.fisrtBabyAge = 0;

                                        Console.WriteLine($"你开始了新技能--教育下一代");
                                        this.canEducate = true;

                                        Console.WriteLine($"你开始了新技能--陪伴下一代");
                                        this.canPlayWithChildren = true;

                                        Console.WriteLine($"生一胎时，您花费了{result.cost.ToString("f2")}金币");
                                        this.sumSave -= result.salary;

                                        Console.WriteLine($"今年打工时，获得了{ result.salary.ToString("f2")}金币");
                                        this.sumSave += result.salary;

                                    }
                                    else if (result.result == "getfirstbaby-failure")
                                    {
                                        this.age++;

                                        Console.WriteLine("生一胎--失败！");
                                        //this.canGetFirstBaby = false;
                                        //this.canGetSecondBaby = true;
                                        //this.fisrtBabyAge = 0;

                                        //Console.WriteLine($"生一胎时，您花费了{result.cost.ToString("f2")}金币");
                                        //this.sumSave -= result.salary;

                                        Console.WriteLine($"今年打工时，获得了{ result.salary.ToString("f2")}金币");
                                        this.sumSave += result.salary;
                                    }
                                    else
                                    {
                                        throw new Exception("");
                                    }
                                }; break;
                        }
                        //var obj = new { result = "love-success", employerAction = employerAction };
                        //return Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                    }
                    client.Close();
                }
            }
        }

        public class LoveResult
        {
            public string result { get; set; }
            public short employerAction { get; set; }

            /// <summary>
            /// 打工者的薪水
            /// </summary>
            public double salary { get; set; }
        }

        public class MarryResult
        {
            //         var obj = new { result = "marry-failure", employerAction = employerAction, salary = salary, dowry = dowry, housePrice = data.housePrice };
            /// <summary>
            /// 结果
            /// </summary>
            public string result { get; set; }
            /// <summary>
            /// 雇主属性
            /// </summary>
            public short employerAction { get; set; }

            /// <summary>
            /// 薪水
            /// </summary>
            public double salary { get; set; }

            /// <summary>
            /// 嫁妆
            /// </summary>
            public double dowry { get; set; }

            /// <summary>
            /// 房价
            /// </summary>
            public double housePrice { get; set; }

        }

        public class GetFirstBabyResult
        {
            /// <summary>
            /// 结果
            /// </summary>
            public string result { get; set; }

            /// <summary>
            /// 打工遇到的雇主
            /// </summary>
            public short employerAction { get; set; }
            /// <summary>
            /// 获得的薪水
            /// </summary>
            public double salary { get; set; }
            /// <summary>
            /// 生一胎的花费
            /// </summary>
            public double cost { get; set; }
        }

        public int year { get; set; }

        public string yearDisplay { get { return $"这是你打工第{this.year + 1}年"; } }
        public Employee()
        {
            this.year = 0;
            this.canLove = true;
            this.canBeMarried = false;
            this.canGetFirstBaby = false;
            this.canGetSecondBaby = false;
            this.canEducate = false;
            this.canSingleWork = false;
            this.canPlayWithChildren = false;
            this.canStrugle = true;

            var rm = Math.Abs(DateTime.Now.GetHashCode()) % 3;
            this.age = 22 + rm;

            this.fisrtBabyAge = null;
            this.secondBabyAge = null;
            this.thirdBabyAge = null;
            this.fourthBabyAge = null;
        }



        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            //  return base.ToString();
        }
    }
}
