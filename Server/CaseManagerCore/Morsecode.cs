using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagerCore
{
    public abstract class CaseManager
    {
        protected bool error = false;
        protected Random rm;
        protected int step = 0;
        protected string msg = "";
        public string GetMsg()
        {
            return this.msg;
        }
        public int startId = 0;
        public string rightMsg = "";
        public string errorMsg = "";
        public int score = 100;
        public int AddScore = 0;

        public void DealWithMsg(string msg1)
        {
            PassObj show = new PassObj()
            {
                ObjType = "html",
                msg = $"{msg1} ",
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
        }
        protected string idPrevious = "code";
        protected bool isEnd = false;

        public void errorRecovery()
        {
            this.error = !this.error;
            if (this.error)
            {
                this.msg = this.errorMsg;
                this.AddScore = -10;
            }
            else
            {
                this.msg = this.rightMsg;
                this.AddScore = 1;
            }
        }

        public string GetScore()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { ObjType = "score", score = this.score });
        }

        public void DealWithRightAndWrong(string msg_Right, string msg_Wrong)
        {
            int rmV = this.rm.Next(0, 2);
            if (rmV == 0)
            {
                this.error = true;
                PassObj show = new PassObj()
                {
                    ObjType = "html",
                    msg = $"{msg_Wrong} ",
                    showContinue = true,
                    showIsError = true,
                    isEnd = false,
                    ObjID = $"{idPrevious}{startId++}",
                    styleStr = "msg"

                };
                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
            }
            else
            {
                this.error = false;
                PassObj show = new PassObj()
                {
                    ObjType = "html",
                    msg = $"{msg_Right} ",
                    showContinue = true,
                    showIsError = true,
                    isEnd = false,
                    ObjID = $"{idPrevious}{startId++}",
                    styleStr = "msg"

                };
                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
            }
            {
                PassObj pError = new PassObj()
                {
                    ObjType = "change",
                    msg = msg_Wrong,
                    showContinue = true,
                    showIsError = true,
                    isEnd = false,
                    ObjID = $"{idPrevious}{startId - 1}",
                    styleStr = "msg"

                };
                PassObj pRight = new PassObj()
                {
                    ObjType = "change",
                    msg = msg_Right,
                    showContinue = true,
                    showIsError = true,
                    isEnd = false,
                    ObjID = $"{idPrevious}{startId - 1}",
                    styleStr = "msg"

                };
                this.errorMsg = Newtonsoft.Json.JsonConvert.SerializeObject(pError);
                this.rightMsg = Newtonsoft.Json.JsonConvert.SerializeObject(pRight);
                this.AddScore = 1;
            }
        }

        public bool getRewardSuccess = false;

        public void InputAddress()
        {
            if ((!this.pleaseInput) && (!this.getRewardSuccess))
            {
                InputAddress show = new InputAddress()
                {
                    ObjType = "inputaddress",
                    msg = "请输入您的获奖地址(只支持比特币地址) 如1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr",
                    showContinue = false,
                    showIsError = false,
                    isEnd = false,
                    ObjID = $"{idPrevious}{startId++}",
                    styleStr = "msg",

                };
                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show);
                this.step++;
                this.pleaseInput = true;
            }
        }

        public void CloseSocket()
        {
            Close show = new Close()
            {
                ObjType = "close",
                msg = "已结束",
                showContinue = false,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg",

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show);
        }


        public string SaveAddress(string address, string Name, int minScore, MysqlCore.DelegateF.AddAddressValue addF)
        {
            if (this.pleaseInput)
            {
                if (this.score >= minScore)
                {
                    if (!this.getRewardSuccess)
                    {
                        decimal moneycountAddV;
                        addF(address, out moneycountAddV);//MySql.bitcoinquestionpage.AddAddressValue(address);
                        this.getRewardSuccess = true;
                        this.step = 100;
                        var msgPass = $"您好，您往<div>{address}</div>  添加了一枚{Name}奖章，并且获得了{moneycountAddV.ToString("f2")}金币！<div> <a href=\"https://www.nyrq123.com/XunZhang.html\" style=\"color: whitesmoke;\">查看勋章</a></div>";
                        DealWithMsg(msgPass);
                    }

                }
                this.pleaseInput = false;
            }
            return this.msg;
        }


        public string SaveAddress(string address,decimal moneycountAddV ,string Name, int minScore, MysqlCore.DelegateF.AddAddressValueWithOutTimeLimit addF)
        {
            if (this.pleaseInput)
            {
                if (this.score >= minScore)
                {
                    if (!this.getRewardSuccess)
                    { 
                        addF(address, moneycountAddV);//MySql.bitcoinquestionpage.AddAddressValue(address);
                        this.getRewardSuccess = true;
                        this.step = 100;
                        var msgPass = $"您好，您往<div>{address}</div>  添加了一枚{Name}勋章，并且获得了{moneycountAddV.ToString("f2")}金币！<div> <a href=\"https://www.nyrq123.com/XunZhang.html\" style=\"color: whitesmoke;\">查看勋章</a></div>";
                        DealWithMsg(msgPass);
                    }

                }
                this.pleaseInput = false;
            }
            return this.msg;
        }
        /// <summary>
        /// 这个参数的作用是为了防止客户非法调用SaveAddress方法
        /// </summary>
        bool pleaseInput = false;

        public string Important(string material, string importantPart)
        {
            return string.Format(material, $"<strong><span style=\"color: yellow\">{importantPart}</span></strong>");
        }

        public string Important(string material, params string[] importantParts)
        {
            List<string> addItems = new List<string>();
            for (var i = 0; i < importantParts.Length; i++)
            {
                addItems.Add($"<strong><span style=\"color: yellow\">{importantParts[i]}</span></strong>");
            };
            return string.Format(material, addItems.ToArray());
        }
    }

    public class Morsecode : CaseManager
    {
        const int minScore = 98;
        public string[] Material = new string[]
        {
            "hello i am Yaoruiqing at zhihu",
            "Quality matters more than quantity",
            "All is not at hand that helps",
            "Judge not according to the appearance",
            "Failure is the mother of success",
            "Old birds are not caught with new nests",
            "Seeing is believing",
            "Time tries truth",
            "Practice makes perfect",
            "Tall trees catch much wind",
            "Great minds think alike",
            "Learn to walk before you run",
            "God helps those who help themselves",
            "Union is strength",
            "Sharp tools make good work",
            "Believe in yourself",
            "Never to old to learn",
            "All rivers run into sea",
            "jiafa",
            "jianfa",
            "chengfa",
            "Ill news travels fast"
        };

        public string[] Result = new string[]
        {
            "你好我是要瑞卿@知乎",
            "质量比数量重要",
            "有用的东西并不都是垂手可得的",
            "不要以貌取人",
            "失败是成功之母",
            "新巢捉不到老鸟",
            "眼见为实",
            "时间检验真理",
            "熟能生巧",
            "树大招风",
            "英雄所见略同",
            "先学走，再学跑",
            "自助者天助之",
            "团结就是力量",
            "工欲善其器，必先利其器",
            "相信自己",
            "活到老，学到老",
            "海纳百川",
            "",
            "",
            "",
            "坏事传千里"
        };
        int CurrentIndex = 0;
        public Morsecode()
        {


            this.idPrevious = "mscode";
            this.rm = new Random(DateTime.Now.GetHashCode());
            rm.Next();
            rm.Next();
            this.step = 0;
            int plusResult;
            int minusResult;
            {
                int a = rm.Next(1, 4);
                int b = rm.Next(1, 5);
                Material[18] = $"{a} jia {b} dengyu duoshao?";
                Result[18] = $"结果为：{a + b}";
                plusResult = a + b;
            }
            {
                int a, b;
                do
                {
                    a = rm.Next(1, 9);
                    b = rm.Next(1, 8);
                } while (a <= b || a - b == plusResult);

                minusResult = a - b;
                Material[19] = $"{a} jian {b} dengyu duoshao?";
                Result[19] = $"结果为：{a - b}";
            }
            {
                int a, b;
                do
                {
                    a = rm.Next(1, 4);
                    b = rm.Next(1, 4);
                } while (a * b == plusResult || a * b == minusResult);


                Material[20] = $"{a} cheng {b} dengyu duoshao?";
                Result[20] = $"结果为：{a * b}";
            }

            PassObj p = new PassObj()
            {
                ObjType = "html",
                msg = $"老师您好，现在是{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")}。我给您讲讲摩尔斯码，你看看对不对，不对您要及时指出我的错误哦！关注知乎@要瑞卿，了解更多。",
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);

        }

        Dictionary<int, bool> usedIndex = new Dictionary<int, bool>();

        public void Continue()
        {
            if (error)
            {
                //if (!this.isEnd)
                //{
                //    this.isEnd = true;
                //}
                this.msg = this.rightMsg;
                this.error = !error;
                if (this.score > 0)
                    this.score -= 10;
                if (this.score < 0)
                {
                    this.score = 0;
                }
                this.AddScore = 0;
                return;
            }
            else
            {
                this.score += this.AddScore;
                if (this.score > 100)
                {
                    this.score = 100;
                }
                this.AddScore = 0;
            }
            if (this.isEnd)
            {
                PassObj p = new PassObj()
                {
                    ObjType = "html_Error",
                    msg = $"重来",// $"<div style=\"text-align:center;border:1px solid #4cff00;padding:5px;margin:5px;background-color:yellow;color:red;\">老师，我讲错了，刷新网页我重新给你讲吧！！！</div>",
                    showContinue = false,
                    showIsError = false,
                    isEnd = true,
                    styleStr = "error"
                };

                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                return;
            }
            else
            {
                this.step++;
                if (this.step == 1)
                {
                    DealWithMsg($"我先给您讲讲摩尔斯电码，更多信息关注知乎@要瑞卿。请您翻译我的电码信息哈。如果做的不对，我得重新回到知乎，访问知乎@要瑞卿的文章，重新进行学习。");
                }
                else if (this.step == 2)
                {
                    DealWithMsgSheet("MorseCode");
                }
                else if (this.step >= 3 && this.step < 45 && this.step % 2 == 1)
                {
                    do
                    {
                        this.CurrentIndex = this.rm.Next(0, this.Material.Length);
                    }
                    while (this.usedIndex.ContainsKey(this.CurrentIndex));
                    this.usedIndex.Add(this.CurrentIndex, true);
                    DealWithMsg2();
                }
                else if (this.step >= 3 && this.step < 45 && this.step % 2 == 0)
                {
                    var wrongIndex = this.CurrentIndex;
                    do
                    {
                        wrongIndex = this.rm.Next(0, this.Material.Length);
                    }
                    while (wrongIndex == this.CurrentIndex);
                    var rightMsg = $"以上电文翻译成英语，再翻译成汉语，是“{this.Result[this.CurrentIndex]}”。";
                    var wrongMsg = $"以上电文翻译成英语，再翻译成汉语，是“{this.Result[wrongIndex]}”。";
                    //  var wrongMsg = this.Result[wrongIndex];
                    DealWithRightAndWrong(rightMsg, wrongMsg);
                }
                else if (this.step == 45)
                {
                    DealWithMsg($"你的最终得分是{this.score}.");
                }
                else if (this.step > 45 && this.step < 100)
                {
                    if (this.getRewardSuccess)
                    {
                        this.step = 100;
                    }
                    else
                    {
                        if (this.score >= minScore)
                        {
                            InputAddress();
                            this.step = 67;
                        }
                        else
                        {
                            var msg = $"您的得分为{this.score}。小于{minScore}将不能获得勋章，继续努力吧！少年！！！";
                            DealWithMsg(msg);
                            this.step = 100;
                        }
                    }

                }
                else if (this.step >= 100)
                {

                    CloseSocket();
                }
            }
        }

        private void DealWithMsgSheet(string v)
        {
            PassObj show = new PassObj()
            {
                ObjType = $"html_Sheet_{v}",
                msg = $"",
                showContinue = false,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
        }

        private void DealWithMsg2()
        {
            var strValue = this.Material[this.CurrentIndex];

            int start = 0;
            List<MorseCodeT> paramters = new List<MorseCodeT>();
            List<int> codeShowT = new List<int>();
            for (int i = 0; i < strValue.Length; i++)
            {
                string p = strValue[i].ToString().ToUpper();
                switch (p)
                {
                    case "A":
                        {
                            OperateDiDa(ref paramters, ".-", ref start, ref codeShowT);
                        }; break;
                    case "B":
                        {
                            OperateDiDa(ref paramters, "-...", ref start, ref codeShowT);
                        }; break;
                    case "C":
                        {
                            OperateDiDa(ref paramters, "-.-.", ref start, ref codeShowT);
                        }; break;
                    case "D":
                        {
                            OperateDiDa(ref paramters, "-..", ref start, ref codeShowT);
                        }; break;
                    case "E":
                        {
                            OperateDiDa(ref paramters, ".", ref start, ref codeShowT);
                        }; break;
                    case "F":
                        {
                            OperateDiDa(ref paramters, "..-.", ref start, ref codeShowT);
                        }; break;
                    case "G":
                        {
                            OperateDiDa(ref paramters, "--.", ref start, ref codeShowT);
                        }; break;
                    case "H":
                        {
                            OperateDiDa(ref paramters, "....", ref start, ref codeShowT);
                        }; break;
                    case "I":
                        {
                            OperateDiDa(ref paramters, "..", ref start, ref codeShowT);
                        }; break;
                    case "J":
                        {
                            OperateDiDa(ref paramters, ".---", ref start, ref codeShowT);
                        }; break;
                    case "K":
                        {
                            OperateDiDa(ref paramters, "-.-", ref start, ref codeShowT);
                        }; break;
                    case "L":
                        {
                            OperateDiDa(ref paramters, ".-..", ref start, ref codeShowT);
                        }; break;
                    case "M":
                        {
                            OperateDiDa(ref paramters, "--", ref start, ref codeShowT);
                        }; break;
                    case "N":
                        {
                            OperateDiDa(ref paramters, "-.", ref start, ref codeShowT);
                        }; break;
                    case "O":
                        {
                            OperateDiDa(ref paramters, "---", ref start, ref codeShowT);
                        }; break;
                    case "P":
                        {
                            OperateDiDa(ref paramters, ".--.", ref start, ref codeShowT);
                        }; break;
                    case "Q":
                        {
                            OperateDiDa(ref paramters, "--.-", ref start, ref codeShowT);
                        }; break;
                    case "R":
                        {
                            OperateDiDa(ref paramters, ".-.", ref start, ref codeShowT);
                        }; break;
                    case "S":
                        {
                            OperateDiDa(ref paramters, "...", ref start, ref codeShowT);
                        }; break;
                    case "T":
                        {
                            OperateDiDa(ref paramters, "-", ref start, ref codeShowT);
                        }; break;
                    case "U":
                        {
                            OperateDiDa(ref paramters, "..-", ref start, ref codeShowT);
                        }; break;
                    case "V":
                        {
                            OperateDiDa(ref paramters, "...-", ref start, ref codeShowT);
                        }; break;
                    case "W":
                        {
                            OperateDiDa(ref paramters, ".--", ref start, ref codeShowT);
                        }; break;
                    case "X":
                        {
                            OperateDiDa(ref paramters, "-..-", ref start, ref codeShowT);
                        }; break;
                    case "Y":
                        {
                            OperateDiDa(ref paramters, "-.--", ref start, ref codeShowT);
                        }; break;
                    case "Z":
                        {
                            OperateDiDa(ref paramters, "--..", ref start, ref codeShowT);
                        }; break;
                    case "1":
                        {
                            OperateDiDa(ref paramters, ".----", ref start, ref codeShowT);
                        }; break;
                    case "2":
                        {
                            OperateDiDa(ref paramters, "..---", ref start, ref codeShowT);
                        }; break;
                    case "3":
                        {
                            OperateDiDa(ref paramters, "...--", ref start, ref codeShowT);
                        }; break;
                    case "4":
                        {
                            OperateDiDa(ref paramters, "....-", ref start, ref codeShowT);
                        }; break;
                    case "5":
                        {
                            OperateDiDa(ref paramters, ".....", ref start, ref codeShowT);
                        }; break;
                    case "6":
                        {
                            OperateDiDa(ref paramters, "-....", ref start, ref codeShowT);
                        }; break;
                    case "7":
                        {
                            OperateDiDa(ref paramters, "--...", ref start, ref codeShowT);
                        }; break;
                    case "8":
                        {
                            OperateDiDa(ref paramters, "---..", ref start, ref codeShowT);
                        }; break;
                    case "9":
                        {
                            OperateDiDa(ref paramters, "----.", ref start, ref codeShowT);
                        }; break;
                    case "0":
                        {
                            OperateDiDa(ref paramters, "-----", ref start, ref codeShowT);
                        }; break;
                    case "?":
                        {
                            OperateDiDa(ref paramters, "..--..", ref start, ref codeShowT);
                        }; break;
                    case " ":
                        {
                            start += 7;
                            codeShowT.Add(3);
                            codeShowT.Add(start);
                        }; break;
                    default:
                        {
                            start += 7;
                            codeShowT.Add(3);
                            codeShowT.Add(start);
                        }; break;
                }
            }
            MorseCodeModel show = new MorseCodeModel()
            {
                ObjType = "html_Sound",
                msg = $"",
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg",
                paramters = paramters,
                codeShowT = codeShowT

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show);
        }



        private void OperateDiDa(ref List<MorseCodeT> paramters, string v, ref int start, ref List<int> codeShowT)
        {
            for (int i = 0; i < v.Length; i++)
            {
                switch (v[i])
                {
                    case '.':
                        {
                            paramters.Add(MorseCodeT.Di(ref start));
                            codeShowT.Add(0);
                            codeShowT.Add(start);
                        }; break;
                    case '-':
                        {
                            paramters.Add(MorseCodeT.Da(ref start));
                            codeShowT.Add(1);
                            codeShowT.Add(start);
                        }; break;
                }
            }
            start += 3;
            codeShowT.Add(2);
            codeShowT.Add(start);
        }




        public string SaveAddress(string address)
        {
            var b = new MysqlCore.BaseItem("morsecode");
            return this.SaveAddress(address, b.xunzhangName, minScore, new MysqlCore.BaseItem("morsecode").AddAddressValue);//MySql.morsecode.AddAddressValue);
        }
    }
}
