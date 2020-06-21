using System;
using System.Collections.Generic;
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

        internal void DealWithCommand(string command)
        {
            var msg = "";
            switch (command)
            {
                case "1":
                    {
                        if (this.canLove)
                        {
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Love" });
                        }
                    }; break;
                case "2":
                    {
                        if (this.canBeMarried)
                        {
                            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { sumSave = this.sumSave });
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Marry", JsonValue = JsonValue });
                        }
                    }; break;
                case "3":
                    {
                        if (this.canGetFirstBaby)
                        {
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-GetFirstBaby" });
                        }
                    }; break;
                case "4":
                    {
                        if (this.canGetSecondBaby)
                        {
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-GetSecondBaby" });
                        }
                    }; break;
                case "5":
                    {
                        if (this.canEducate)
                        {
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Educate" });
                        }
                    }; break;
                case "6":
                    {
                        if (this.canSingleWork)
                        {
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-SingleIncome" });
                        }
                    }; break;
                case "7":
                    {
                        if (this.canPlayWithChildren)
                        {
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

                    //case (command) { }
            }

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
                                    }
                                    else if (result.result == "love-failure")
                                    {
                                        Console.WriteLine("你恋爱失败喽！再接再厉！");
                                        this.year++;
                                        this.age++;
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
        }
    }
}
