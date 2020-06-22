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

        public class State
        {
            public bool canLove { get; set; }
            public bool canBeMarried { get; set; }
            public bool canGetFirstBaby { get; set; }
            public bool canGetSecondBaby { get; set; }
            public bool canEducate { get; set; }
            public bool canSingleWork { get; set; }
            public bool canPlayWithChildren { get; set; }
            public bool canStrugle { get; set; }



            public int? fisrtBabyAge = null;
            public int? secondBabyAge = null;
            public int? thirdBabyAge = null;
            public int? fourthBabyAge = null;

            public double sumSave = 0;

            public int age = 22;

            public int year { get; set; }
        }
        public State state { get; set; }


        public string ageDisplay
        {
            get { return $"你今天{this.state.age}岁。"; }
        }

        public string ageSumSave
        {
            get { return $"你现在有{this.state.sumSave.ToString("f2")}金币的储蓄！"; }
        }



        //int age = 22;
        internal string selection
        {
            get
            {
                var msg = "";
                if (this.state.canLove)
                {
                    msg += "1.恋爱";
                    msg += Environment.NewLine;
                }
                if (this.state.canBeMarried)
                {
                    msg += "2.结婚";
                    msg += Environment.NewLine;
                }
                if (this.state.canGetFirstBaby)
                {
                    msg += "3.要一胎";
                    msg += Environment.NewLine;
                }
                if (this.state.canGetSecondBaby)
                {
                    msg += "4.要二胎";
                    msg += Environment.NewLine;
                }
                if (this.state.canEducate)
                {
                    msg += "5.教育";
                    msg += Environment.NewLine;
                }
                if (this.state.canSingleWork)
                {
                    msg += "6.男方上班，女方顾家";
                    msg += Environment.NewLine;
                }
                if (this.state.canPlayWithChildren)
                {
                    msg += "7.陪伴小孩，亲子";
                    msg += Environment.NewLine;
                }
                if (this.state.canStrugle)
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
                        if (this.state.canLove)
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
                        if (this.state.canBeMarried)
                        {
                            var action = "Employee-Marry";
                            if (this.actions.Contains(action))
                            {

                            }
                            else
                            {
                                this.actions.Add(action);
                            }
                        }
                        //if (this.canBeMarried)
                        //{
                        //    var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { sumSave = this.sumSave, canStrugle = this.canStrugle });
                        //    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Marry", JsonValue = JsonValue });
                        //}
                    }; break;
                case "3":
                    {
                        if (this.state.canGetFirstBaby)
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
                        if (this.state.canGetSecondBaby)
                        {
                            //var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                            //msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-GetSecondBaby", canStrugle = this.canStrugle });
                        }
                    }; break;
                case "5":
                    {
                        if (this.state.canEducate)
                        {
                            //var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                            //msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Educate" });
                        }
                    }; break;
                case "6":
                    {
                        if (this.state.canSingleWork)
                        {
                            //var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                            //msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-SingleIncome" });
                        }
                    }; break;
                case "7":
                    {
                        if (this.state.canPlayWithChildren)
                        {
                            //var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { canStrugle = this.canStrugle });
                            //msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-PlayWithChildren" });
                        }
                    }; break;
                case "8":
                    {
                        if (this.state.canStrugle)
                        {

                            //msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Strugle" });
                        }
                    }; break;
                case "Next":
                    {
                        if (this.state.canStrugle)
                        {

                            // msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Strugle" });
                        }
                    }; break;

                    //case (command) { }
            }
            if (command == "Next")
            {
                if (this.actions.Count != 0)
                {
                    msg = this.ToString();
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


                            var PassObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObj>(response);
                            this.actions = PassObj.actions;
                            this.state = PassObj.state;

                        }
                        client.Close();
                    }
                }


            }
            if (!string.IsNullOrEmpty(msg))
            {

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



        public string yearDisplay { get { return $"这是你打工第{this.state.year + 1}年"; } }
        public Employee()
        {
            this.state.year = 0;
            this.state.canLove = true;
            this.state.canBeMarried = false;
            this.state.canGetFirstBaby = false;
            this.state.canGetSecondBaby = false;
            this.state.canEducate = false;
            this.state.canSingleWork = false;
            this.state.canPlayWithChildren = false;
            this.state.canStrugle = true;

            var rm = Math.Abs(DateTime.Now.GetHashCode()) % 3;
            this.state.age = 22 + rm;

            this.state.fisrtBabyAge = null;
            this.state.secondBabyAge = null;
            this.state.thirdBabyAge = null;
            this.state.fourthBabyAge = null;
        }


        public class PassObj
        {
            public State state { get; set; }
            public List<string> actions { get; set; }
        }
        public override string ToString()
        {
            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new PassObj
            {
                state = this.state,
                actions = this.actions
            });
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Type = "Employee",
                JsonValue = JsonValue
            });
        }
    }
}
