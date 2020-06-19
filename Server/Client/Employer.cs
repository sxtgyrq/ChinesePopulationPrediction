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
                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "Employee-Marry" });
                        }
                    };break;
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
                    };break;
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

                    }
                    client.Close();
                }
            }
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
        }
    }
}
