using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MysqlCore;

namespace CaseManagerCore
{
    public class BitCoinQuestionPage
    {
        /// <summary>
        /// 这个参数的作用是为了防止客户非法调用SaveAddress方法
        /// </summary>
        bool pleaseInput = false;

        const int minScore = 98;
        bool getRewardSuccess = false;
        Dictionary<int, int> reciprocalValues = new Dictionary<int, int>();
        Dictionary<int, int> reciprocalValuesN2 = new Dictionary<int, int>();
        Random rm;
        int startId = 0;
        const string idPrevious = "bqp";
        public BitCoinQuestionPage()
        {
            this.rm = new Random(DateTime.Now.GetHashCode());
            rm.Next();
            rm.Next();
            this.step = 0;

            PassObj p = new PassObj()
            {
                ObjType = "html",
                msg = $"老师您好，现在是{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")}。我给您讲讲比特币，你看看对不对，不对您要及时指出我的错误哦！",
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };

            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
        }
        string msg = "";
        int step = 0;
        public string GetMsg()
        {
            return this.msg;
        }
        bool error = false;
        bool isEnd = false;
        string rightMsg = "";
        string errorMsg = "";
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

                    PassObj p = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"我先给您讲讲数字签名",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"
                    };

                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);

                    do
                    {
                        this.a = rm.Next(2, 10);
                        this.x1 = rm.Next(1, 10);
                        this.x2 = rm.Next(1, 10);
                        this.x3 = rm.Next(1, 10);
                    }
                    while (this.x1 == this.x2 || this.x1 == this.x3 || this.x2 == this.x3 || this.x1 + this.x2 + this.x3 == this.x1 * this.x2 * this.x3);
                }
                else if (this.step == 2)
                {
                    PassObj p = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"我先对一元三次方程有一些猜想",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"
                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                }
                else if (this.step == 3)
                {
                    var eMsg = $"$${a}(x-{x1})(x-{x2})(x-{x3})=0$$";
                    PassObj p = new PassObj()
                    {
                        ObjType = "html",
                        //var msg = $"$${a}(x-{x1})(x-{x2})(x-{x3})=0$$";
                        msg = $"{eMsg}",

                        //$$x = { -b \pm \sqrt{ b ^ 2 - 4ac } \over 2a }.$$
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"
                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                }
                else if (this.step == 4)
                {
                    this.error = true;
                    int eroorx1 = this.x1;
                    int eroorx2 = this.x2;
                    int eroorx3 = this.x3;
                    do
                    {
                        if (rm.NextDouble() < 0.1) eroorx1 = this.rm.Next(1, 10);
                        if (rm.NextDouble() < 0.1) eroorx2 = this.rm.Next(1, 10);
                        if (rm.NextDouble() < 0.1) eroorx3 = this.rm.Next(1, 10);
                    }
                    while (x123Equal(eroorx1, eroorx2, eroorx3));

                    var msgRight = $"这个方程的根为$$ x_1 = {x1} , x_2 = {x2} , x_3 = {x3} $$";
                    var msgWrong = $"这个方程的根为$$ x_1 = {eroorx1} , x_2 = {eroorx2} , x_3 = {eroorx3} $$";
                    DealWithRightAndWrong(msgRight, msgWrong);
                    int rmV = this.rm.Next(0, 2);
                }
                else if (this.step == 5)
                {
                    var preMsg1 = $@"经过推导得出$${a}(x-{this.x1})(x^2-{this.x2 + this.x3}x+{this.x2 * this.x3})=0$$
$${a}(x^3-{x2 + x3}x^2+{x2 * x3}x-{x1}x^2+{x1 * (x2 + x3)}x-{x1 * x2 * x3})=0$$
$${a}(x^3-{this.x1 + this.x2 + this.x3}x^2+{this.x1 * this.x2 + this.x1 * this.x3 + this.x2 * this.x3}x-{this.x1 * this.x2 * this.x3})=0$$" +
                        $"$$ {a}x^3-{(this.x1 + this.x2 + this.x3) * a}x^2+{(this.x1 * this.x2 + this.x1 * this.x3 + this.x2 * this.x3) * a}x-{(this.x1 * this.x2 * this.x3) * a} =0$$";

                    PassObj p = new PassObj()
                    {
                        ObjType = "html",
                        msg = preMsg1,
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                }
                else if (this.step == 6)
                {

                    //  var preMsg1 = $"我发现";
                    int rmV = this.rm.Next(0, 2);
                    if (this.x1 + this.x2 + this.x3 == this.x1 * this.x2 * this.x3)
                    {
                        throw new Exception("");
                    }

                    var msgWrong = $"我发现$$x_1+x_2+x_3={this.x1}+{this.x2}+{this.x3}={this.x1 + this.x2 + this.x3}={{{(this.x1 * this.x2 * this.x3) * a} \\over {a}}}$$";
                    var msgRight = $"我发现$$x_1+x_2+x_3={this.x1}+{this.x2}+{this.x3}={this.x1 + this.x2 + this.x3}={{{(this.x1 + this.x2 + this.x3) * a} \\over {a}}}$$";

                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 7)
                {
                    var msgRight = $"我猜想$$ax^3+bx^2+cx^3+d=0$$的三个根$$x_1,x_2,x_3$$满足$$x_1+x_2+x_3=-{{b \\over a}}$$";
                    var msgWrong = $"我猜想$$ax^3+bx^2+cx^3+d=0$$的三个根$$x_1,x_2,x_3$$满足$$x_1+x_2+x_3=-{{c \\over a}}$$";
                    DealWithRightAndWrong(msgRight, msgWrong);
                    int rmV = this.rm.Next(0, 2);
                }

                else if (this.step == 8)
                {
                    DealWithMsg($"我只是猜想，求各位大神在我的知乎@要瑞卿 给出证明。后面咱们继续愉快玩耍的时候要用到这个$$x_1+x_2+x_3=-{{b \\over a}}$$公式。我们这里标记其为猜想推理①.");
                }

                else if (this.step == 9)
                {
                    this.N1 = this.N1SelectV[this.rm.Next(this.N1SelectV.Length)];
                    var g1List = new List<int>();

                    for (int i = 0; i < this.N1; i++)
                    {
                        g1List.Add(i);
                    }
                    this.G1 = g1List.ToArray();
                    PassObj show = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"然后咱们聊聊素数$$(2,3,5,7,11,13,17,19,23,…)$$我取一个素数，例如$$N_1={this.N1}$$那么我就得到有限域$$G_1={G1Str}$$",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
                }
                else if (this.step == 10)
                {
                    var msg1 = "$$例如有限域内G_1的加法：$$";
                    string msg2 = "";
                    string msg3 = "";
                    string msg4 = "我们这样定义有限域内的加法 $$a+b \\equiv (a+b)\\pmod {N_1}$$";
                    string msg5 = "";
                    string msg6 = "";
                    {
                        int p1, p2;
                        do
                        {
                            p1 = this.G1[this.rm.Next(this.G1.Length)];
                            p2 = this.G1[this.rm.Next(this.G1.Length)];
                        } while (p1 + p2 >= this.G1.Length);
                        msg2 = $"$${p1}+{p2}\\equiv {p1 + p2}\\pmod {{{N1}}}$$";
                        msg5 = $"$${p1}+{p2}= {p1 + p2}\\equiv {p1 + p2}\\pmod{{{N1}}}  $$";
                    }

                    {
                        int p1, p2;
                        do
                        {
                            p1 = this.G1[this.rm.Next(this.G1.Length)];
                            p2 = this.G1[this.rm.Next(this.G1.Length)];
                        } while (p1 + p2 < this.G1.Length);
                        msg3 = $"$${p1}+{p2}\\equiv {(p1 + p2) % N1}\\pmod {{{N1}}}$$";
                        msg6 = $"$${p1}+{p2}= {p1 + p2}\\equiv { (p1 + p2) % N1}\\pmod{{{N1}}}  $$";
                    }

                    PassObj show = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"{msg1}{msg2}{msg3}{msg4}{msg5}{msg6}",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
                }

                else if (this.step == 11)
                {
                    var msg1 = "$$例如有限域内G_1的减法：$$";
                    string msg2 = "";
                    string msg3 = "";
                    string msg4 = "我们这样定义有限域内的加法 $$a-b \\equiv (a - b)\\pmod {N_1}$$";
                    string msg5 = "";
                    string msg6 = "";
                    {
                        int p1, p2;
                        do
                        {
                            p1 = this.G1[this.rm.Next(this.G1.Length)];
                            p2 = this.G1[this.rm.Next(this.G1.Length)];
                        } while (p1 - p2 < 0);
                        msg2 = $"$${p1}-{p2}\\equiv {p1 - p2}\\pmod {{{N1}}}$$";
                        msg5 = $"$${p1}-{p2}= {p1 - p2}\\equiv {p1 - p2}\\pmod{{{N1}}}  $$";
                    }

                    {
                        int p1, p2;
                        do
                        {
                            p1 = this.G1[this.rm.Next(this.G1.Length)];
                            p2 = this.G1[this.rm.Next(this.G1.Length)];
                        } while (p1 - p2 >= 0);
                        msg3 = $"$${p1}-{p2}\\equiv {(p1 - p2 + N1) % N1}\\pmod {{{N1}}}$$";
                        msg6 = $"$${p1}-{p2}= {p1 - p2}\\equiv { (p1 - p2 + N1) % N1}\\pmod{{{N1}}}  $$";
                    }

                    PassObj show = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"{msg1}{msg2}{msg3}{msg4}{msg5}{msg6}",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
                }

                else if (this.step == 12)
                {
                    var msg1 = "$$例如有限域内G_1的乘法：$$";
                    string msg2 = "";
                    string msg3 = "";
                    string msg4 = "我们这样定义有限域内的乘法 $$a\\times b \\equiv (a \\times b)\\pmod {N_1}$$";
                    string msg5 = "";
                    string msg6 = "";

                    string msg7 = "";
                    string msg8 = "";
                    {
                        int p1, p2;
                        do
                        {
                            p1 = this.G1[this.rm.Next(this.G1.Length)];
                            p2 = this.G1[this.rm.Next(this.G1.Length)];
                        } while (p1 * p2 >= N1 || (p1 * p2 % N1 == 1));
                        msg2 = $"$${p1} \\times {p2}\\equiv {p1 * p2}\\pmod {{{N1}}}$$";
                        msg5 = $"$${p1} \\times {p2}= {p1 * p2}\\equiv {p1 * p2}\\pmod{{{N1}}}  $$";
                    }

                    {
                        int p1, p2;
                        do
                        {
                            p1 = this.G1[this.rm.Next(this.G1.Length)];
                            p2 = this.G1[this.rm.Next(this.G1.Length)];
                        } while (p1 * p2 < N1 || (p1 * p2 % N1 == 1));
                        msg3 = $"$${p1} \\times {p2}\\equiv {(p1 * p2) % N1}\\pmod {{{N1}}}$$";
                        msg6 = $"$${p1} \\times {p2}= {p1 * p2}\\equiv { (p1 * p2 + N1) % N1}\\pmod{{{N1}}}  $$";
                    }

                    {
                        int p1, p2;
                        do
                        {
                            p1 = this.G1[this.rm.Next(this.G1.Length)];
                            p2 = this.G1[this.rm.Next(this.G1.Length)];
                        } while (p1 * p2 % N1 != 1 || p1 == 1);
                        msg7 = $"$${p1} \\times {p2}\\equiv {(p1 * p2) % N1}\\pmod {{{N1}}}$$";
                        msg8 = $"$${p1} \\times {p2}= {p1 * p2}\\equiv { (p1 * p2 + N1) % N1}\\pmod{{{N1}}}  $$";

                        this.divExample1 = p1;
                        this.divExample2 = p2;
                    }

                    PassObj show = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"{msg1}{msg2}{msg3}{msg7}{msg4}{msg5}{msg6}{msg8}",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
                }

                else if (this.step == 13)
                {
                    var msg1 = "对于除法";
                    var msg2 = $"咱们把$${this.divExample1} \\times {this.divExample2} \\equiv  {this.divExample1 * this.divExample2 % this.N1} \\pmod {{{this.N1}}}$$拿出来研究研究。其相当于有理数域内的$$4\\times 0.25=1$$";
                    var msg3 = $"所以在有限域$G_1$内，我们得到${divExample1}$,${divExample2}$互为倒数，即${this.divExample2}\\equiv \\frac{{1}}{{{this.divExample1}}} \\pmod {{{this.N1}}}$";


                    PassObj show = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"{msg1}{msg2}{msg3}",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
                }
                else if (this.step == 14)
                {
                    var msg1 = "$$同理，得到有限域G_1内元素的倒数表$$";
                    var msg2 = $"$$\\frac{{1}}{{0}}\\pmod {{{this.N1}}}没有意义$$";

                    var msg3 = $"$$\\frac{{1}}{{1}}\\equiv 1 \\pmod {{{this.N1}}}$$";

                    this.reciprocalValues.Add(1, 1);

                    PassObj show = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"{msg1}{msg2}{msg3}",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
                }
                else if (this.step == 15)
                {
                    int rmV = this.rm.Next(0, 2);
                    this.step--;

                    var v1 = this.G1[this.divStartNumber];
                    int v2_Error;

                    do
                    {
                        v2_Error = this.G1[this.rm.Next(this.G1.Length)];
                    }
                    while (v2_Error == 0 || v2_Error == 1 || (v2_Error * v1) % this.N1 == 1);

                    int v2_Right;

                    do
                    {
                        v2_Right = this.G1[this.rm.Next(this.G1.Length)];
                    }
                    while ((v2_Right * v1) % this.N1 != 1);
                    this.reciprocalValues.Add(this.divStartNumber, v2_Right);

                    var msgRight = $"$$\\frac{{1}}{{{v1}}}\\equiv {v2_Right} \\pmod {{{this.N1}}}$$";
                    var msgWrong = $"$$\\frac{{1}}{{{v1}}}\\equiv {v2_Error} \\pmod {{{this.N1}}}$$";
                    DealWithRightAndWrong(msgRight, msgWrong);

                    this.divStartNumber++;
                    if (this.divStartNumber == this.N1)
                    {
                        this.step++;
                    }

                }

                else if (this.step == 16)
                {
                    int a = 0, b = 0, c = 0, d = 1;

                    var resultRight = 0;
                    do
                    {
                        a = this.G1[this.rm.Next(0, this.G1.Length)];
                        b = this.G1[this.rm.Next(0, this.G1.Length)];
                        c = this.G1[this.rm.Next(0, this.G1.Length)];
                        d = this.G1[this.rm.Next(1, this.G1.Length)];
                        resultRight = ((a * b + b - c) * this.reciprocalValues[d] + this.N1) % this.N1;
                    } while (a == b || a == c || a == d || b == c || b == d || c == d);

                    var msg = $"那么我在包含{this.N1}个元素的有限域$G_1$内计算$${{{{{a} \\times {b} +{b} -{c}}}\\over {d} }} \\pmod {{{this.N1}}}$$$$={{{{{(a * b) % this.N1} +{b} -{c}}}\\over {d} }}\\pmod {{{this.N1}}}$$$$={{{{{(a * b + b) % this.N1}  -{c}}}\\over {d} }}\\pmod {{{this.N1}}}$$$$={{ {(a * b + b - c + this.N1) % this.N1} \\over {d} }}\\pmod {{{this.N1}}}$$$$= {(a * b + b - c + this.N1) % this.N1} \\times {this.reciprocalValues[d]}  \\pmod {{{this.N1}}}$$$$= {((a * b + b - c + this.N1) * this.reciprocalValues[d]) % this.N1}\\pmod {{{this.N1}}}  $$";


                    PassObj show = new PassObj()
                    {
                        ObjType = "html",
                        msg = $"{msg} ",
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show); ;
                }

                else if (this.step == 17)
                {
                    int a = 0, b = 0, c = 0, d = 1;

                    var resultRight = 0;
                    do
                    {
                        a = this.G1[this.rm.Next(0, this.G1.Length)];
                        b = this.G1[this.rm.Next(0, this.G1.Length)];
                        c = this.G1[this.rm.Next(0, this.G1.Length)];
                        d = this.G1[this.rm.Next(1, this.G1.Length)];
                        resultRight = ((a * b + b - c + this.N1) * this.reciprocalValues[d] % this.N1 + this.N1) % this.N1;
                    } while (a == b || a == c || a == d || b == c || b == d || c == d || d == 1);
                    var resultError = 0;
                    do
                    {
                        resultError = this.G1[this.rm.Next(0, this.G1.Length)];
                    } while (resultRight == resultError);
                    int rmV = this.rm.Next(0, 2);
                    var msg_Right = $"那么我在有限域$G_1$内计算$${{{{{a} \\times {b} +{b} -{c}}}\\over {d} }}={resultRight}\\pmod {{{this.N1}}}$$";
                    var msg_Wrong = $"那么我在有限域$G_1$内计算$${{{{{a} \\times {b} +{b} -{c}}}\\over {d} }}={resultError}\\pmod {{{this.N1}}}$$"; ;
                    DealWithRightAndWrong(msg_Right, msg_Wrong);
                }

                else if (this.step == 18)
                {
                    var msg1 = "而且我在这里猜想，若$N_1$为素数，那么有限域内$G_1$的非0数，其倒数和有限域内的数是一一对应的。我的这个猜想，希望大神与各位吃瓜群众给出证明。";

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

                else if (this.step == 19)
                {
                    var msg1 = "请您在我的知乎【@要瑞卿】 欣赏完数学家的艺术后，咱们再聊聊椭圆曲线。";

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
                else if (this.step == 20)
                {
                    var msg1 = "我们在笛卡尔坐标系中绘制出曲线$$E=\\{(x,y)| y^2=x^3+ax+b \\}\\cup \\{Ο\\}$$";

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

                else if (this.step == 21)
                {

                    //int rmV = this.rm.Next(0, 2);
                    var msg_Right = "通过方程知道$$y^2=(-y)^2=x^3+ax+b$$.我们知道这个曲线在笛卡尔坐标系中是关于$x$轴对称的。";
                    var msg_Wrong = "通过方程知道$$y^2=(-y)^2=x^3+ax+b$$.我们知道这个曲线在笛卡尔坐标系中是关于$y$轴对称的。";
                    DealWithRightAndWrong(msg_Right, msg_Wrong);

                }
                else if (this.step == 22)
                {
                    var msg1 = "$P(x_P,y_P)$(已知)、$Q(x_Q,y_Q)$(已知)为曲线$E$上的两点,那么除去$P$、$Q$两点，直线$PQ$和曲线E还存在第三交点$R'(x_{R'},y_{R'})$。点$R(x_{R},y_{R})$为$R'$关于$x$轴的对称点。我们定义$$R=P+Q$$";

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
                else if (this.step == 23)
                {
                    int rmV = this.rm.Next(0, 2);
                    var msg_Right = "依据定义，我们得到$$x_{R'}=x_{R}$$$$y_{R'}=-y_{R}$$";
                    var msg_Wrong = "依据定义，我们得到$$x_{R'}=-x_{R}$$$$y_{R'}=y_{R}$$";

                    DealWithRightAndWrong(msg_Right, msg_Wrong);
                }
                else if (this.step == 24)
                {
                    var msg_Right = "我们定义$s$为直线$PQ$的斜率，那么$$s=\\frac{y_P-y_Q}{x_P-x_Q}$$";
                    var msg_Wrong = "我们定义$s$为直线$PQ$的斜率，那么$$s=\\frac{x_P-x_Q}{y_P-y_Q}$$";
                    DealWithRightAndWrong(msg_Right, msg_Wrong);
                }
                else if (this.step == 25)
                {
                    var v = this.rm.Next(0, 2);
                    string msg_Right;
                    if (v == 0)
                    {
                        this.UseP = true;
                        msg_Right = "那么直线$PQ$的方程为$$y=s(x-x_P)+y_P$$";
                    }
                    else
                    {
                        this.UseP = false;
                        msg_Right = "那么直线$PQ$的方程为$$y=s(x-x_Q)+y_Q$$";
                    };
                    DealWithMsg(msg_Right);
                }
                else if (this.step == 26)
                {

                    //this.UseP = this.rm.Next(0, 2) == 0;
                    if (this.UseP)
                    {
                        var msg1 = "将直线方程带入曲线$E$得到";
                        var msg2 = "$${[s(x-x_P)+y_P]}^2$$$$=x^3+ax+b$$";
                        var msg3 = "最终展开整理得到";
                        var msg6 = "$$x^3-s^2x^2+Mx+N=0$$其中$$M$$$$=(a+2s^2x_P-2sy_P)$$$$N$$$$=(b-s^2x_P^2+2sx_Py_P-y_P^2)$$";
                        var msg = $"{msg1}{msg2}{msg3}{msg6}";
                        DealWithMsg(msg);
                    }
                    else
                    {
                        var msg1 = "将直线方程带入曲线$E$得到";
                        var msg2 = "$${[s(x-x_Q)+y_Q]}^2$$$$=x^3+ax+b$$";
                        var msg3 = "最终展开整理得到";
                        var msg6 = "$$x^3-s^2x^2+Mx+N=0$$其中$$M$$$$=(a+2s^2x_Q-2sy_Q)$$$$N$$$$=(b-s^2x_Q^2+2sx_Qy_Q-y_Q^2)$$";
                        var msg = $"{msg1}{msg2}{msg3}{msg6}";
                        DealWithMsg(msg);
                    }

                }
                else if (this.step == 27)
                {
                    var msg_Right = "依据猜想推理①，我们得到$$x_P+x_Q+x_{R'}$$$$=-\\frac{-s^2}{1}$$";
                    var msg_Wrong = "依据猜想推理①，我们得到$$x_P+x_Q+x_{R'}$$$$=-\\frac{-M}{1}$$";
                    if (rm.Next(0, 2) == 1)
                    {
                        msg_Wrong = "依据猜想推理①，我们得到$$x_P+x_Q+x_{R'}$$$$=-\\frac{-N}{1}$$";
                    }
                    DealWithRightAndWrong(msg_Right, msg_Wrong);
                }
                else if (this.step == 28)
                {
                    var msg_Right = "进一步得到$$x_{R'}=s^2-(x_P+x_Q)$$$$x_{R}=s^2-(x_P+x_Q)$$";
                    var msg_Wrong = "进一步得到$$x_{R'}=s^2-(x_P+x_Q)$$$$x_{R}=-s^2+(x_P+x_Q)$$";
                    DealWithRightAndWrong(msg_Right, msg_Wrong);
                }
                else if (this.step == 29)
                {
                    var msg1 = "因为$R'$在直线PQ上,所以";
                    //var msg2 = $"$$y_R=s(x_R-{(this.UseP ? "x_P" : "x_Q")})+{((this.UseP ? "y_P" : "y_Q"))}$$";
                    var msg3 = $"$$y_{{R'}}=s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})+{((this.UseP ? "y_P" : "y_Q"))}$$";

                    //var msg_Wrong = $"{msg1}{msg2}";
                    var msg_Right = $"{msg1}{msg3}";
                    DealWithMsg(msg_Right);
                }
                else if (this.step == 30)
                {
                    var msg1 = "再进一步,得到";
                    var msg2 = $"$$y_R=y_{{R'}}=s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})+{((this.UseP ? "y_P" : "y_Q"))}$$";
                    var msg3 = $"$$y_R=-y_{{R'}}=-s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})-{((this.UseP ? "y_P" : "y_Q"))}$$";

                    var msg_Wrong = $"{msg1}{msg2}";
                    var msg_Right = $"{msg1}{msg3}";
                    DealWithRightAndWrong(msg_Right, msg_Wrong);
                }
                else if (this.step == 31)
                {
                    var msg1 = "那么我得到结论，在已知$P(x_P,y_P)$,$Q(x_Q,y_Q)$的情况下，可以利用下面三个公式，求$R(R_x,R_y)$点的坐标";
                    var msg2 = "$$s=\\frac{y_P - y_Q}{x_P - x_Q}$$";
                    var msg3 = "$$x_{R}=s^2-(x_P+x_Q)$$";
                    var msg4 = $"$$y_R=-s(x_{{R}}-{(this.UseP ? "x_P" : "x_Q")})-{((this.UseP ? "y_P" : "y_Q"))}$$";
                    var msg5 = "而且这些公式的有效性不仅仅在实数域中，在有限域中$G_1$中同样有效。我们把上面的三个公式称之为猜想推理②.";
                    //var msg2 = $"$$y_R=y_{{R'}}=s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})+{((this.UseP ? "y_P" : "y_Q"))}$$";
                    // var msg3 = $"$$y_R=-y_{{R'}}=-s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})-{((this.UseP ? "y_P" : "y_Q"))}$$";
                    DealWithMsg($"{msg1}{msg2}{msg3}{msg4}{msg5}");
                }
                else if (this.step == 32)
                {
                    var msg2 = "接下来，咱们定义$2\\times P=P+P$.可以理解为点$P$与点$Q$重合的情况。";
                    DealWithMsg(msg2);
                }
                else if (this.step == 33)
                {
                    var msg2 = "依据推理猜想②，计算s时，我们得到一个分母为0的情况。";
                    var msg3 = "但是这种情况在曲线上是曲线E在P点的切线.";
                    var msg4 = "学过微积分的朋友们，可以得到$$s=\\frac {dy}{dx}=\\frac{3x_P^2+a}{2y_P}$$";
                    DealWithMsg($"{msg2}{msg3}{msg4}");
                }
                else if (this.step == 34)
                {
                    var msg2 = "依据推理猜想①，得到";
                    var msg3 = "$$x_R=s^2-(x_P+x_P)=s^2-2x_P$$";
                    var msg4 = "与推导猜想②一样，咱们得到$$y_R=-s(x_R-x_P)-y_P$$";
                    DealWithMsg($"{msg2}{msg3}{msg4}");
                }
                else if (this.step == 35)
                {
                    var msg1 = "那么我得到结论，在已知$P(x_P,y_P)$的情况下，可以利用下面三个公式，求$R(R_x,R_y)=2P$点的坐标";
                    var msg2 = "$$s=\\frac{3x_P^2+a}{2y_P}$$";
                    var msg3 = "$$x_R=s^2-2x_P$$";
                    var msg4 = $"$$y_R=-s(x_R-x_P)-y_P$$";
                    var msg5 = "而且这些公式的有效性不仅仅在实数域中，在有限域中$G_1$中同样有效。我们把上面的三个公式称之为猜想推理③.";
                    //var msg2 = $"$$y_R=y_{{R'}}=s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})+{((this.UseP ? "y_P" : "y_Q"))}$$";
                    // var msg3 = $"$$y_R=-y_{{R'}}=-s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})-{((this.UseP ? "y_P" : "y_Q"))}$$";
                    DealWithMsg($"{msg1}{msg2}{msg3}{msg4}{msg5}");
                }
                else if (this.step == 36)
                {
                    var msg1 = "但是曲线$$E=\\{(x,y)| y^2=x^3+ax+b \\}\\cup \\{Ο\\}$$还有后边的$\\{Ο\\}$";
                    var msg2 = "脑洞大开的数学家定义这个这个点$Ο$的坐标为的坐标为$(x_x,+\\infty)$,而且当用猜想推理②时，遇到$x_P-x_Q=0$且$y_P+y_Q=0$,$y_P \\neq 0$的情况时，数学家规定直线$P$$Q$经过$Ο$";
                    var msg3 = "即当$x_P-x_Q=0$时$$P+Q=Ο$$";
                    var msg4 = $"经过变换，我们得到当$x_P-x_Q=0$时$$P=Ο-Q$$";
                    //var msg2 = $"$$y_R=y_{{R'}}=s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})+{((this.UseP ? "y_P" : "y_Q"))}$$";
                    // var msg3 = $"$$y_R=-y_{{R'}}=-s(x_{{R'}}-{(this.UseP ? "x_P" : "x_Q")})-{((this.UseP ? "y_P" : "y_Q"))}$$";
                    DealWithMsg($"{msg1}{msg2}{msg3}{msg4}");
                }
                else if (this.step == 37)
                {
                    var msg1 = $"我们要在包含{this.N1}个元素的有限域$G_1$内进行计算";
                    do
                    {
                        this.P1x = this.G1[this.rm.Next(0, this.G1.Length)];
                        this.P1y = this.G1[this.rm.Next(0, this.G1.Length)];
                        this.Ea = this.G1[this.rm.Next(0, this.G1.Length)];
                        this.Eb = this.G1[this.rm.Next(0, this.G1.Length)];
                    } while (
                    (this.P1y * this.P1y) % this.N1 != (this.P1x * this.P1x * this.P1x + this.Ea * this.P1x + this.Eb) % this.N1 ||
                    this.P1y == 0 ||
                    this.Ea * this.Ea * this.Ea * 4 + 27 * this.Eb * this.Eb == 0 || (!GetN1Success()));
                    var msg2 = $"我们取曲线$$E=\\{{(x,y)| y^2=x^3{(this.Ea == 0 ? "" : "+")}{(this.Ea == 0 ? "" : this.Ea.ToString())}{(this.Ea == 0 ? "" : "x")}{(this.Eb == 0 ? "" : "+")}{(this.Eb == 0 ? "" : this.Eb.ToString())} \\}}\\cup \\{{Ο\\}}$$,其中$$a={this.Ea}$$$$b={this.Eb}$$";
                    var msg3 = $"令$P_1(x_1,y_1)=({this.P1x},{this.P1y})$ ";
                    DealWithMsg($"{msg1}{msg2}{msg3}");
                    this.G2.Add(new int[] { this.P1x, this.P1y });
                }
                else if (this.step == 38)
                {
                    if (this.PStartIndex % 2 == 0)
                    {
                        var childIndex = this.PStartIndex / 2;
                        var msg1 = $"在$G_1$内,当$N_1={this.N1}时，依据猜想推理③$";
                        var msg2 = $"$$P_{{{this.PStartIndex}}}=2P_{{{this.PStartIndex / 2}}}$$";
                        var msg3 = $"$$s_{{{this.PStartIndex}}}=\\frac{{3x_{{{childIndex}}}^2+a}}{{2y_{{{childIndex}}}}} \\pmod{{{this.N1}}}$$";
                        var msg4 = $"$$s_{{{this.PStartIndex}}}=\\frac{{3 \\times {this.G2[childIndex - 1][0]}\\times {this.G2[childIndex - 1][0]}+{this.Ea}}}{{2 \\times {this.G2[childIndex - 1][1]}}} \\pmod{{{this.N1}}}$$";

                        var xValue = this.G2[childIndex - 1][0];
                        int sValue;
                        try
                        {
                            sValue = ((3 * xValue * xValue + this.Ea) * this.reciprocalValues[2] * this.reciprocalValues[this.G2[childIndex - 1][1]]) % this.N1;

                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        var msg7 = $"$$s_{{{this.PStartIndex}}}=  {sValue} \\pmod{{{this.N1}}}$$";
                        //var s2 = ((3 * this.G2[childIndex][0] * this.G2[childIndex][0] + a) * this.reciprocalValues[(2 * this.G2[childIndex][1])]) % this.N1;
                        var msgRight = $"{msg1}{msg2}{msg3}{msg4}{msg7}";

                        int errorNumber = 0;
                        do
                        {
                            errorNumber = this.G1[this.rm.Next(0, this.G1.Length)];
                        } while (errorNumber == sValue);
                        var msg9 = $"$$s_{{{this.PStartIndex}}}=  {errorNumber} \\pmod{{{this.N1}}}$$";
                        var msgWrong = $"{msg1}{msg2}{msg3}{msg4}{msg9}";
                        DealWithRightAndWrong(msgRight, msgWrong);
                    }
                    else
                    {
                        this.childIndex1 = this.rm.Next(1, this.PStartIndex);// this.PStartIndex -1;
                        this.childIndex2 = this.PStartIndex - childIndex1;

                        var msg1 = $"在$G_1$内,当$N_1={this.N1}时，依据猜想推理②$";
                        var msg2 = $"$$P_{{{this.PStartIndex}}}={childIndex1}P_1+{childIndex2}P_1=P_{{{childIndex1}}}+P_{{{childIndex2}}}$$";
                        var sValue = Mod((this.G2[childIndex1 - 1][1] - this.G2[childIndex2 - 1][1]) * this.reciprocalValues[Mod(this.G2[childIndex1 - 1][0] - this.G2[childIndex2 - 1][0], this.N1)], this.N1);

                        var msg3 = $"$$s_{{{this.PStartIndex}}}=\\frac{{y_{{{childIndex1}}}-y_{{{childIndex2}}}}}{{x_{{{childIndex1}}}-x_{{{childIndex2}}}}}  $$";
                        var msg5 = $"$$s_{{{this.PStartIndex}}}=\\frac{{ {this.G2[childIndex1 - 1][1]}- {this.G2[childIndex2 - 1][1]}}}{{ {this.G2[childIndex1 - 1][0]}- {this.G2[childIndex2 - 1][0]}}}  \\pmod{{{this.N1}}}$$";
                        var msg6 = $"$$s_{{{this.PStartIndex}}}= {sValue} \\pmod{{{this.N1}}}$$";

                        int errorNumber = 0;
                        do
                        {
                            errorNumber = this.G1[this.rm.Next(0, this.G1.Length)];
                        } while (errorNumber == sValue);
                        var msg4 = $"$$s_{{{this.PStartIndex}}}= {errorNumber} \\pmod{{{this.N1}}}$$";
                        var msgRight = $"{msg1}{msg2}{msg3}{msg5}{msg6}";
                        var msgWrong = $"{msg1}{msg2}{msg3}{msg5}{msg4}";
                        DealWithRightAndWrong(msgRight, msgWrong);
                    }
                }
                else if (this.step == 39)
                {
                    if (this.PStartIndex % 2 == 0)
                    {
                        var childIndex = this.PStartIndex / 2;
                        var xValue = this.G2[childIndex - 1][0];
                        var sValue = ((3 * xValue * xValue + this.Ea) * this.reciprocalValues[2] * this.reciprocalValues[this.G2[childIndex - 1][1]]) % this.N1;
                        var xValueNew = Mod(sValue * sValue - 2 * xValue, this.N1);

                        var msg1 = $"$$x_{{{this.PStartIndex}}}=s_{{{this.PStartIndex}}}^2-2x_{{{childIndex}}}$$";
                        var msg2 = $"$$x_{{{this.PStartIndex}}}={sValue} \\times {sValue}-2 \\times{xValue}  \\pmod{{{this.N1}}}$$";
                        var msg3 = $"$$x_{{{this.PStartIndex}}}={xValueNew} \\pmod{{{this.N1}}}$$";

                        var msgRight = $"{msg1}{msg2}{msg3}";


                        int xValueNew_Error = 0;
                        do
                        {
                            xValueNew_Error = this.G1[this.rm.Next(0, this.G1.Length)];
                        } while (xValueNew_Error == xValueNew);
                        var msg4 = $"$$x_{{{this.PStartIndex}}}={xValueNew_Error} \\pmod{{{this.N1}}}$$";
                        var msgWrong = $"{msg1}{msg2}{msg4}";
                        DealWithRightAndWrong(msgRight, msgWrong);
                    }
                    else
                    {
                        var xValue1 = this.G2[this.childIndex1 - 1][0];
                        var xValue2 = this.G2[this.childIndex2 - 1][0];
                        var sValue = Mod((this.G2[childIndex1 - 1][1] - this.G2[childIndex2 - 1][1]) * this.reciprocalValues[Mod(this.G2[childIndex1 - 1][0] - this.G2[childIndex2 - 1][0], this.N1)], this.N1);

                        var xValueNew = Mod(sValue * sValue - (xValue1 + xValue2), this.N1);

                        var msg1 = $"$$x_{{{this.PStartIndex}}}=s_{{{this.PStartIndex}}}^2-(x_{{{this.childIndex1}}}+x_{{{this.childIndex2}}})$$";
                        var msg2 = $"$$x_{{{this.PStartIndex}}}={sValue} \\times {sValue}-({xValue1}+{xValue2})   \\pmod{{{this.N1}}}$$";
                        var msg3 = $"$$x_{{{this.PStartIndex}}}={xValueNew} \\pmod{{{this.N1}}}$$";
                        int xValueNew_Error = 0;
                        do
                        {
                            xValueNew_Error = this.G1[this.rm.Next(0, this.G1.Length)];
                        } while (xValueNew_Error == xValueNew);

                        var msg4 = $"$$x_{{{this.PStartIndex}}}={xValueNew_Error} \\pmod{{{this.N1}}}$$";
                        var msgRight = $"{msg1}{msg2}{msg3}";
                        var msgWrong = $"{msg1}{msg2}{msg4}";
                        DealWithRightAndWrong(msgRight, msgWrong);
                    }
                }

                else if (this.step == 40)
                {

                    if (this.PStartIndex % 2 == 0)
                    {
                        var childIndex = this.PStartIndex / 2;
                        var xValue = this.G2[childIndex - 1][0];
                        var yValue = this.G2[childIndex - 1][1];
                        var sValue = ((3 * xValue * xValue + this.Ea) * this.reciprocalValues[2] * this.reciprocalValues[this.G2[childIndex - 1][1]]) % this.N1;
                        var xValueNew = Mod(sValue * sValue - 2 * xValue, this.N1);
                        var yValueNew = Mod(-sValue * (xValueNew - xValue) - yValue, this.N1);

                        var msg1 = $"$$y_{{{this.PStartIndex}}}=-s_{{{this.PStartIndex}}}(x_{{{this.PStartIndex}}}-x_{{{childIndex}}})-y_{{{childIndex}}} $$";
                        var msg2 = $"$$y_{{{this.PStartIndex}}}=-{sValue} \\times({xValueNew} -{xValue}) -{yValue} \\pmod{{{this.N1}}}$$";
                        var msg3 = $"$$y_{{{this.PStartIndex}}}={yValueNew} \\pmod{{{this.N1}}}$$";
                        var msg5 = $"我们得到$P_{{{this.PStartIndex}}}({xValueNew},{yValueNew})$";
                        var msgRight = $"{msg1}{msg2}{msg3}{msg5}";


                        int yValueNew_Error = 0;
                        do
                        {
                            yValueNew_Error = this.G1[this.rm.Next(0, this.G1.Length)];
                        } while (yValueNew_Error == yValueNew);
                        var msg4 = $"$$y_{{{this.PStartIndex}}}={yValueNew_Error}  \\pmod{{{this.N1}}}$$";
                        var msg6 = $"我们得到$P_{{{this.PStartIndex}}}({xValueNew},{yValueNew_Error})$";
                        var msgWrong = $"{msg1}{msg2}{msg4}{msg6}";
                        DealWithRightAndWrong(msgRight, msgWrong);

                        this.G2.Add(new int[] { xValueNew, yValueNew });


                        this.PStartIndex++;
                        if (xValueNew == this.G2[0][0])
                        { }
                        else
                        {

                            this.step = this.step - 3;
                        }
                    }
                    else
                    {
                        var xValue1 = this.G2[this.childIndex1 - 1][0];
                        var yValue1 = this.G2[this.childIndex1 - 1][1];
                        var xValue2 = this.G2[this.childIndex2 - 1][0];
                        var yValue2 = this.G2[this.childIndex2 - 1][1];
                        var sValue = Mod((this.G2[childIndex1 - 1][1] - this.G2[childIndex2 - 1][1]) * this.reciprocalValues[Mod(this.G2[childIndex1 - 1][0] - this.G2[childIndex2 - 1][0], this.N1)], this.N1);

                        var xValueNew = Mod(sValue * sValue - (xValue1 + xValue2), this.N1);

                        var yValueNew = Mod(-sValue * (xValueNew - xValue1) - yValue1, this.N1);

                        var childIndex = this.UseP ? this.childIndex1 : this.childIndex2;
                        var xValue = this.UseP ? xValue1 : xValue2;
                        var yValue = this.UseP ? yValue1 : yValue2;

                        var msg1 = $"$$y_{{{this.PStartIndex}}}=-s_{{{this.PStartIndex}}}(x_{{{this.PStartIndex}}}-x_{{{childIndex}}})-y_{{{childIndex}}} $$";
                        var msg2 = $"$$y_{{{this.PStartIndex}}}=-{sValue} \\times({xValueNew} -{xValue}) -{yValue} \\pmod{{{this.N1}}}$$";
                        var msg3 = $"$$y_{{{this.PStartIndex}}}={yValueNew} \\pmod{{{this.N1}}}$$";
                        var msg5 = $"我们得到$P_{{{this.PStartIndex}}}({xValueNew},{yValueNew})$";
                        var msgRight = $"{msg1}{msg2}{msg3}{msg5}";


                        int yValueNew_Error = 0;
                        do
                        {
                            yValueNew_Error = this.G1[this.rm.Next(0, this.G1.Length)];
                        } while (yValueNew_Error == yValueNew);
                        var msg4 = $"$$y_{{{this.PStartIndex}}}={yValueNew_Error}  \\pmod{{{this.N1}}}$$";
                        var msg6 = $"我们得到$P_{{{this.PStartIndex}}}({xValueNew},{yValueNew_Error})$";
                        var msgWrong = $"{msg1}{msg2}{msg4}{msg6}";
                        DealWithRightAndWrong(msgRight, msgWrong);

                        this.G2.Add(new int[] { xValueNew, yValueNew });

                        this.PStartIndex++;
                        this.step = this.step - 3;
                    }
                }
                else if (this.step == 41)
                {
                    this.childIndex1 = this.rm.Next(1, this.PStartIndex);// this.PStartIndex -1;
                    this.childIndex2 = this.PStartIndex - childIndex1;
                    this.N2 = this.G2.Count + 1;
                    var msg1 = "最后";
                    var msg2 = $"$$P_{{{this.N2}}}=P_{{{this.N2 - 1}}}+P_{{1}}$$";
                    var msg3 = "由于";
                    var msg4 = $"$$x_{{{this.N2 - 1}}}=x_{{1}},y_{{{this.N2}}}+y_{{1}}={this.N1}$$"; ;
                    var msg5 = $"所以$P_{{{this.N2}}}=P_0=Ο$";

                    var msg6 = "[$Ο$,";
                    for (int i = 1; i < this.N2; i++)
                    {
                        msg6 += $"$P_{{{i}}}$,";

                    }
                    msg6 = msg6.Substring(0, msg6.Length - 1);
                    msg6 += "]";
                    var msg7 = $"而且，我猜想由{msg6}组成一个新的有限域$F_2$,且这个有限域有${this.N2}$个元素，我们称${this.N2}$为有限域的阶(order),有限域$F_2$的阶$N_2={this.N2}$";
                    DealWithMsg($"{msg1}{msg2}{msg3}{msg4}{msg5}{msg7}");
                }
                else if (this.step == 42)
                {
                    List<double> lines = new List<double>();
                    for (int i = 0; i <= this.N1; i++)
                    {
                        var x1 = 2 + 96.0 / this.N1 * i;
                        var y1 = 2;
                        var x2 = x1;
                        var y2 = 98;
                        lines.Add(x1);
                        lines.Add(y1);
                        lines.Add(x2);
                        lines.Add(y2);
                    }
                    for (int i = 0; i <= this.N1; i++)
                    {
                        var x1 = 2;
                        var y1 = 2 + 96.0 / this.N1 * i;
                        var x2 = 98;
                        var y2 = y1;
                        lines.Add(x1);
                        lines.Add(y1);
                        lines.Add(x2);
                        lines.Add(y2);
                    }
                    List<double> dots = new List<double>();
                    for (int i = 0; i < this.G2.Count; i++)
                    {
                        var x = this.G2[i][0];
                        var y = this.G2[i][1];

                        var xValue = 2 + 96.0 / this.N1 * x;
                        var yValue = 2 + 96.0 / this.N1 * y;

                        var radius = 2;
                        dots.Add(xValue);
                        dots.Add(yValue);
                        dots.Add(radius);
                    }

                    DealWithPic(lines.ToArray(), dots.ToArray());
                }
                else if (this.step == 43)
                {
                    var msg1_2 = this.Ea == 0 ? "" : $"+{this.Ea}x";
                    var msg1_3 = this.Eb == 0 ? "" : $"+{this.Eb}";
                    var msg1_1 = $"$$E=\\{{(x,y) \\vert  y^2=x^3{msg1_2}{msg1_3} \\}}\\cup \\{{ Ο\\}}$$";
                    var msg1 = $"上图为方程{msg1_1}在有限域$G_1$内的曲线。";
                    var msg2 = "那么有一些属性可以借鉴。";
                    var msg3 = "$$P_0=Ο$$";
                    for (int i = 0; i < this.G2.Count; i++)
                    {
                        msg3 += $"$$P_{{{i + 1}}}=({this.G2[i][0]},{this.G2[i][1]})$$";
                    }
                    var msg4 = $"$$P_{{{this.N2}}}=Ο$$";
                    DealWithMsg($"{msg1}{msg2}{msg3}{msg4}");
                }
                else if (this.step == 44)
                {
                    var msg = "这里我们总结下，有一些特性我们可以利用！！！";
                    DealWithMsg(msg);
                }
                else if (this.step == 45)
                {
                    var msgRight = $"1.对于点$P_n$和点$P_{{{this.N2}-n}}$,存在$x_n=x_{{{this.N2}-n}}$,$y_n+y_{{{this.N2}-n}}={this.N1}$，我这里称之为对称性.";
                    var msgWrong = $"1.对于点$P_n$和点$P_{{{this.N2}-n}}$,存在$x_n+x_{{{this.N2}-n}}={this.N1}$,$y_n=y_{{{this.N2}-n}}$，我这里称之为对称性.";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 46)
                {
                    this.SelectPointIndex = this.rm.Next(3, this.N2 - 3);
                    var msgRight = $"2.我们知道坐标$({this.G2[this.SelectPointIndex][0]}, {this.G2[this.SelectPointIndex][1]})$,除了遍历$P_1$至$P_{{{this.N2 - 1}}}$进行匹配，才能得出 $P_{{{this.SelectPointIndex + 1}}}=({this.G2[this.SelectPointIndex][0]}, {this.G2[this.SelectPointIndex][1]})$";

                    int erroNumber = 1;

                    do
                    {
                        erroNumber = this.rm.Next(1, this.N2 - 1);
                    } while (erroNumber == this.SelectPointIndex);

                    var msgWrong = $"2.我们知道坐标$({this.G2[this.SelectPointIndex][0]}, {this.G2[this.SelectPointIndex][1]})$,除了遍历$P_1$至$P_{{{this.N2 - 1}}}$进行匹配，才能得出 $P_{{{ erroNumber + 1}}}=({this.G2[this.SelectPointIndex][0]}, {this.G2[this.SelectPointIndex][1]})$";


                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 47)
                {
                    var msgRight = $"3.我们知道 $P_{{{this.SelectPointIndex + 1}}}$ ，可以通过猜想公式①②③，很快计算出 $P_{{{this.SelectPointIndex + 1}}}=({this.G2[this.SelectPointIndex][0]}, {this.G2[this.SelectPointIndex][1]})$";

                    int erroNumber = 1;

                    do
                    {
                        erroNumber = this.rm.Next(0, this.N2);
                    } while (erroNumber == this.SelectPointIndex);

                    var msgWrong = $"3.我们知道 $P_{{{this.SelectPointIndex + 1}}}$ ，可以通过猜想公式①②③，很快计算出 $P_{{{this.SelectPointIndex + 1}}}=({this.G2[erroNumber][0]}, {this.G2[erroNumber][1]})$";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 48)
                {
                    var msg = "同时在下面的有限域$F_2$中进行计算要用到下列倒数表";
                    var msg2 = $"$$\\frac{{1}}{{0}} 没有意义$$";
                    var msg3 = $"$$\\frac{{1}}{{1}}\\equiv 1 \\pmod {{{this.N2}}}$$";
                    DealWithMsg($"{msg}{msg2}{msg3}");
                    this.reciprocalValuesN2.Add(1, 1);
                }
                else if (this.step == 49)
                {
                    this.step--;

                    if (this.divStartNumberN2 < this.N2)
                    {
                        int v1 = this.divStartNumberN2;
                        int v2_Error;
                        do
                        {
                            v2_Error = this.rm.Next(2, this.N2);
                        } while ((v1 * v2_Error % this.N2) == 1);

                        int v2_Right;
                        do
                        {
                            v2_Right = this.rm.Next(2, this.N2);
                        } while ((v1 * v2_Right % this.N2) != 1);
                        this.reciprocalValuesN2.Add(v1, v2_Right);
                        var msgRight = $"$$\\frac{{1}}{{{v1}}}\\equiv {v2_Right} \\pmod {{{this.N2}}}$$";
                        var msgWrong = $"$$\\frac{{1}}{{{v1}}}\\equiv {v2_Error} \\pmod {{{this.N2}}}$$";
                        DealWithRightAndWrong(msgRight, msgWrong);
                        this.divStartNumberN2++;
                    }
                    else if (this.divStartNumber == this.N1)
                    {
                        this.step++;
                    }
                }
                else if (this.step == 50)
                {
                    this.SelectPointIndex = this.rm.Next(3, this.N2 - 3);
                    string msg = $"然后我们在这里做一些定义，例如";
                    string msg2 = $"$$P_{{{this.SelectPointIndex + 1}}}=({this.G2[this.SelectPointIndex][0]},{this.G2[this.SelectPointIndex][1]})$$";
                    DealWithMsg($"{msg}{msg2}");
                }
                else if (this.step == 51)
                {
                    string msgRight = $"这里的 $P_n$其中$n={this.SelectPointIndex + 1}$ 我们称之为私钥$d_A$";
                    string msgWrong = $"这里的 $P_n$其中$n={this.SelectPointIndex + 1}$ 我们称之为公钥$Q_A$";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 52)
                {
                    string msgRight = $"这里的$({this.G2[this.SelectPointIndex][0]},{this.G2[this.SelectPointIndex][1]})$我们称之为公钥$Q_A$";
                    string msgWrong = $"这里的$({this.G2[this.SelectPointIndex][0]},{this.G2[this.SelectPointIndex][1]})$我们称之为私钥$d_A$";
                    DealWithRightAndWrong(msgRight, msgWrong);


                }
                else if (this.step == 53)
                {
                    string msg = $"然后我们就可以对[0,{this.N2})以内的数字进行签名和验证了";
                    DealWithMsg(msg);
                }

                else if (this.step == 54)
                {
                    this.msgE = this.rm.Next(0, this.N2);
                    string msg = $"首先，{NameChenMing}把要发送的消息进行编码。例如结果是e={this.msgE};";
                    DealWithMsg(msg);
                }
                else if (this.step == 55)
                {
                    this.msgRandomK = this.rm.Next(1, this.N2);
                    string msg = $"第二步，{NameChenMing}在$[1,N_2-1]$即$[1,{this.N2 - 1}]$内取随机值$k$。例如，我取的随机值为$k={this.msgRandomK}$";
                    DealWithMsg(msg);
                }
                else if (this.step == 56)
                {
                    string msg1 = $"第三步，{NameChenMing}计算$P_k$的坐标，即";

                    string msg2 = $"$$P_k=P_{{{this.msgRandomK}}}=({this.G2[this.msgRandomK - 1][0]},{this.G2[this.msgRandomK - 1][1]})$$";

                    var indexError = this.msgRandomK;
                    do
                    {
                        indexError = this.rm.Next(1, this.N2);
                    }
                    while (this.msgRandomK == indexError);

                    string msg3 = $"$$P_k=P_{{{this.msgRandomK}}}=({this.G2[indexError - 1][0]},{this.G2[indexError - 1][1]})$$";

                    DealWithRightAndWrong($"{msg1}{msg2}", $"{msg1}{msg3}");
                }
                else if (this.step == 57)
                {
                    string msg1 = $"第四步，{NameChenMing}定义";

                    string msg2 = "$$r=x_k \\pmod {N_2}$$";
                    string msg3 = $"$$r=x_{{{this.msgRandomK}}} \\pmod {{{this.N2}}}={ this.G2[this.msgRandomK - 1][0]}$$";
                    this.rCal = this.G2[this.msgRandomK - 1][0];

                    DealWithMsg($"{msg1}{msg2}{msg3}");



                }
                else if (this.step == 58)
                {
                    if (this.G2[this.msgRandomK - 1][0] == 0)
                    {
                        string msgRight = $"由于$r={ this.G2[this.msgRandomK - 1][0]}$,{NameChenMing}需要回到第二步，重新选取$k$";
                        string msgWrong = $"由于$r={ this.G2[this.msgRandomK - 1][0]}$,{NameChenMing}可以继续往下执行$k$";
                        DealWithRightAndWrong(msgRight, msgWrong);
                        this.step -= 4;
                    }
                    else
                    {
                        string msgWrong = $"由于$r={ this.G2[this.msgRandomK - 1][0]}$,{NameChenMing}需要回到第二步，重新选取$k$";
                        string msgRight = $"由于$r={ this.G2[this.msgRandomK - 1][0]}$,{NameChenMing}可以继续往下执行$k$";
                        DealWithRightAndWrong(msgRight, msgWrong);

                    }
                }
                else if (this.step == 59)
                {
                    string msg = $"在有限域 $F_2$内{NameChenMing}定义$$s=k^{{-1}}(e+rd_A) \\pmod {{N_2}} $$";

                    string msg2 = $"$$s=\\frac{{{this.msgE }+{this.G2[this.msgRandomK - 1][0]} \\times{this.SelectPointIndex + 1}}}{{{this.msgRandomK}}} \\pmod {{{this.N2}}}$$";

                    this.sSecCal = (this.msgE + this.G2[this.msgRandomK - 1][0] * (this.SelectPointIndex + 1)) * this.reciprocalValuesN2[this.msgRandomK] % this.N2;

                    int sSecCalWrong = this.sSecCal;
                    do
                    {
                        sSecCalWrong = this.rm.Next(0, this.N2);
                    }
                    while (sSecCalWrong == this.sSecCal);

                    string msg3 = $"$$s={this.sSecCal}$$";
                    string msg4 = $"$$s={sSecCalWrong}$$";

                    DealWithRightAndWrong($"{msg}{msg2}{msg3}", $"{msg}{msg2}{msg4}");
                }
                else if (this.step == 60)
                {
                    if (this.sSecCal == 0)
                    {
                        string msgRight = $"由于$s={ this.sSecCal}$,{NameChenMing}需要回到第二步，重新选取$k$";
                        string msgWrong = $"由于$r={ this.sSecCal}$,{NameChenMing}可以继续往下执行$k$";
                        DealWithRightAndWrong(msgRight, msgWrong);
                        this.step -= 6;
                    }
                    else
                    {
                        string msgWrong = $"由于$s={ this.sSecCal}$,{NameChenMing}需要回到第二步，重新选取$k$";
                        string msgRight = $"由于$r={ this.sSecCal}$,{NameChenMing}可以继续往下执行$k$";
                        DealWithRightAndWrong(msgRight, msgWrong);

                    }
                }
                else if (this.step == 61)
                {
                    string msg1 = $"第六步，{NameChenMing}在全世界大声广播$(r,s)$即$({this.G2[this.msgRandomK - 1][0]},{this.sSecCal})$ 、消息$e$即{this.msgE}、还有公钥 $Q_A({this.G2[this.SelectPointIndex][0]},{this.G2[this.SelectPointIndex][1]})$ 与$N_1={this.N1}$,$N_2={this.N2}$ ，曲线$E$的参数$a={this.Ea}$,$b={this.Eb}$,同时销毁$k$(签名完成后，自己可以忘记，但是不能泄露)，对私钥$d_A$进行保密，以备下一次使用。";
                    string msg2 = $"这样我们就完成了一次签名。";
                    DealWithMsg($"{msg1}{msg2}");
                }

                else if (this.step == 62)
                {
                    string msg1 = $"当郝花在小黑板上看到r={this.G2[this.msgRandomK - 1][0]} ,s={this.sSecCal}, e={this.msgE},$N1$={this.N1} , $N2$={this.N2}，曲线参数$a$={this.Ea} ，$b$={this.Eb}时";
                    string msg2 = $"这样我们就完成了一次签名。";
                    DealWithMsg($"{msg1}{msg2}");
                }

                else if (this.step == 63)
                {
                    string msg1 = $"郝花在有限域$G_2$内计算$$u_1=es^{{-1}} \\pmod {{{this.N2}}}$$";
                    string msg2 = $"$$u_1=es^{{-1}}=\\frac {{{this.msgE}}}{{{this.sSecCal}}}  \\pmod {{{this.N2}}}={(this.msgE * this.reciprocalValuesN2[this.sSecCal]) % this.N2}$$";
                    DealWithMsg($"{msg1}{msg2}");
                }
                else if (this.step == 64)
                {
                    string msg1 = $"郝花在有限域$G_2$内计算$$u_2=rs^{{-1}} \\pmod {{{this.N2}}}$$";
                    string msg2 = $"$$u_2=\\frac {{{this.rCal}}}{{{this.sSecCal}}}  \\pmod {{{this.N2}}}={(this.rCal * this.reciprocalValuesN2[this.sSecCal]) % this.N2}$$";
                    DealWithMsg($"{msg1}{msg2}");
                }
                else if (this.step == 65)
                {



                    var calResult = (((this.msgE * this.reciprocalValuesN2[this.sSecCal]) % this.N2) + (this.SelectPointIndex + 1) * ((this.rCal * this.reciprocalValuesN2[this.sSecCal]) % this.N2)) % this.N2;


                    string msg1 = $"第三部，小花在由点组成的有限域$F_2$内计算$$P_{{cal}}(x_{{cal}},y_{{cal}})=u_1P_1+u_2Q_A$$";
                    string msg2 = $"$$P_{{cal}}(x_{{cal}},y_{{cal}})={(this.msgE * this.reciprocalValuesN2[this.sSecCal]) % this.N2} \\times ({this.G2[0][0]},{this.G2[0][1]}) +{(this.rCal * this.reciprocalValuesN2[this.sSecCal]) % this.N2} \\times ({this.G2[this.SelectPointIndex][0]},{this.G2[this.SelectPointIndex][1]})$$";
                    string msg3 = $"$$P_{{cal}}(x_{{cal}},y_{{cal}})=({this.G2[calResult - 1][0]},{this.G2[calResult - 1][1]})$$";

                    string msgRight = $"{msg1}{msg2}{msg3}";

                    int doCalIndex = calResult;
                    do
                    {
                        doCalIndex = this.rm.Next(1, this.N2);
                    }
                    while (doCalIndex == this.N2 - calResult || doCalIndex == calResult);
                    string msg4 = $"$$P_{{cal}}(x_{{cal}},y_{{cal}})=({this.G2[doCalIndex - 1][0]},{this.G2[doCalIndex - 1][1]})$$";

                    DealWithRightAndWrong($"{msg1}{msg2}{msg3}", $"{msg1}{msg2}{msg4}");
                }
                else if (this.step == 66)
                {
                    var calResult = (((this.msgE * this.reciprocalValuesN2[this.sSecCal]) % this.N2) + (this.SelectPointIndex + 1) * ((this.rCal * this.reciprocalValuesN2[this.sSecCal]) % this.N2)) % this.N2;
                    string msg = $"由于$$x_{{cal}}={this.G2[calResult - 1][0]}$$ $$r={this.G2[calResult - 1][0]}$$所以$$x_{{cal}}=r$$";
                    string msg2 = $"郝花最终确认消息$e={this.msgE}$ 来自陈明";
                    DealWithMsg($"{msg}{msg2}");
                }
                else if (this.step >= 67 && this.step < 100)
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

        public string SaveAddress(string address)
        {
            if (this.pleaseInput)
            {
                if (this.score >= minScore)
                {
                    if (!this.getRewardSuccess)
                    {
                        decimal moneycountAddV;
                        MysqlCore.BaseItem b = new MysqlCore.BaseItem("bitcoinquestionpage");
                        b.AddAddressValue(address, out moneycountAddV);
                        //  MySql.bitcoinquestionpage.AddAddressValue(address, out moneycountAddV);
                        this.getRewardSuccess = true;
                        this.step = 100;
                        var msgPass = $"您好，您往<div>{address}</div>  添加了一枚椭圆加密算法勋章，并且获得了{moneycountAddV.ToString("f2")}金币！<div> <a href=\"XunZhang.html\" style=\"color: whitesmoke;\">查看勋章</a></div>";
                        DealWithMsg(msgPass);
                    }

                }
                this.pleaseInput = false;
            }

            return this.msg;
        }



        private void CloseSocket()
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

        private void InputAddress()
        {
            if ((!pleaseInput) && (!getRewardSuccess))
            {
                InputAddress show = new InputAddress()
                {
                    ObjType = "inputaddress",
                    msg = "请输入您的获奖地址(只支持比特币地址) 如1MhoP61wXyV5uCAZk36JFFQfV95mzfLFdw",
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

        private void DealWithPic(double[] lines, double[] dots)
        {
            var objMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new { lines = lines, dots = dots });
            PassCanvas show = new PassCanvas()
            {
                ObjType = "canvas",
                msg = objMsg,
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg",
                CanvasID = $"canvas{startId++}"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(show);
        }

        public string GetScore()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { ObjType = "score", score = this.score });
        }

        private bool GetN1Success()
        {
            int P2x, P2y;
            {
                var xValue = this.P1x;
                var yValue = this.P1y;
                var sValue = ((3 * xValue * xValue + this.Ea) * this.reciprocalValues[2] * this.reciprocalValues[this.P1y]) % this.N1;
                var xValueNew = Mod(sValue * sValue - 2 * xValue, this.N1);
                var yValueNew = Mod(-sValue * (xValueNew - xValue) - yValue, this.N1);
                P2x = xValueNew;
                P2y = yValueNew;
            }
            if (P2x == P1x && P2y + P1y == this.N1)
            {
                return false;
            }
            int startNumber = 3;
            int Pnx = P2x;
            int Pny = P2y;
            do
            {
                startNumber++;
                var xValue1 = this.P1x;
                var yValue1 = this.P1y;
                var xValue2 = Pnx;
                var yValue2 = Pny;
                var sValue = Mod((yValue1 - yValue2) * this.reciprocalValues[Mod(xValue1 - xValue2, this.N1)], this.N1);

                var xValueNew = Mod(sValue * sValue - (xValue1 + xValue2), this.N1);

                var yValueNew = Mod(-sValue * (xValueNew - xValue1) - yValue1, this.N1);
                Pnx = xValueNew;
                Pny = yValueNew;
            }
            while (!(Pnx == P1x && Pny + P1y == this.N1));
            //Console.WriteLine($"计算N2={startNumber}");
            if (
                startNumber == 11 ||
                startNumber == 13 ||
                startNumber == 17 ||
                startNumber == 19 ||
                startNumber == 23 ||
                startNumber == 29 ||
                startNumber == 31 ||
                startNumber == 37 ||
                startNumber == 41 ||
                startNumber == 43 ||
                startNumber == 47 ||
                startNumber == 53)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int Mod(int v, int n)
        {
            v = v % n;
            if (v < 0)
            {
                v = v + n;
            }
            return v;
        }

        private void DealWithMsg(string msg1)
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

        private void DealWithRightAndWrong(string msg_Right, string msg_Wrong)
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

        private bool x123Equal(int eroorx1, int eroorx2, int eroorx3)
        {
            List<int> a = new List<int>() { eroorx1, eroorx2, eroorx3 };
            a = a.OrderBy(item => item).ToList();

            List<int> b = new List<int>() { this.x1, this.x2, this.x3 };
            b = b.OrderBy(item => item).ToList();

            return a[0] == b[0] && a[1] == b[1] && a[2] == b[2];
        }

        int a; int x1; int x2; int x3;

        public int score = 100;
        public int AddScore = 0;


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
            //this.msg = this.changemsg;
        }
        int N1 = 7;
        int[] N1SelectV = new int[] { 11, 13 };
        int[] G1 = new int[] { };
        string G1Str
        {
            get
            {
                var v = "[";
                for (int i = 0; i < G1.Length; i++)
                {
                    v += i;
                    v += ",";
                }
                v = v.Substring(0, v.Length - 1);
                v += "]";
                return v;
            }
        }

        int divExample1 = 0, divExample2 = 0;
        int divStartNumber = 2;
        int divStartNumberN2 = 2;

        bool UseP = true;

        int P1x = 0;
        int P1y = 0;
        int Ea = 0;
        int Eb = 0;

        //int Pnx = 0;
        int PStartIndex = 2;
        List<int[]> G2 = new List<int[]>();
        Dictionary<int, int> sCal = new Dictionary<int, int>();

        int childIndex1 = 0;// this.PStartIndex -1;
        int childIndex2 = 0;

        int N2 = 0;

        int SelectPointIndex = 0;

        int msgE = 0;
        int msgRandomK = 0;

        int rCal = 0;
        int sSecCal = 0;

        string NameChenMing = "陈明";
    }
}
