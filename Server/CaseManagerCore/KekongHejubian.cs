using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagerCore
{
    public class KekongHejubian
    {
        /// <summary>
        /// 这个参数的作用是为了防止客户非法调用SaveAddress方法
        /// </summary>
        bool pleaseInput = false;

        const int minScore = 98;
        bool getRewardSuccess = false;
        int startId = 0;
        const string idPrevious = "yhq";
        public KekongHejubian()
        {
            this.rm = new Random(DateTime.Now.GetHashCode());
            rm.Next();
            rm.Next();
            this.step = 0;

            PassObj p = new PassObj()
            {
                ObjType = "html",
                msg = $"老师您好，现在是{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")}。我给您讲讲“人造太阳”，你看看对不对，不对您要及时指出我的错误哦！",
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };

            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
        }
        string msg = "";
        private Random rm;
        private int step;

        public string GetMsg()
        {
            return this.msg;
        }

        // this.rm = new Random(DateTime.Now.GetHashCode());
        //rm.Next();
        //    rm.Next();
        //    this.step = 0;

        //    PassObj p = new PassObj()
        //    {
        //        ObjType = "html",
        //        msg = $"老师您好，现在是{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")}。我给您讲讲比特币，你看看对不对，不对您要及时指出我的错误哦！",
        //        showContinue = true,
        //        showIsError = false,
        //        isEnd = false,
        //        ObjID = $"{idPrevious}{startId++}",
        //        styleStr = "msg"

        //    };

        //    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);

        bool error = false;
        bool isEnd = false;
        string rightMsg = "";
        string errorMsg = "";
        public int score = 100;
        public int AddScore = 0;
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
                    var msgRight = "物理学中，把重核分裂成质量较小的核，释放核能的反应叫做裂变。";
                    var msgWrong = "物理学中，把重核分裂成质量较小的核，释放核能的反应叫做聚变。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 2)
                {
                    var msgRight = "物理学中，把较轻的核结合成质量较大的核，释放核能的反应叫做聚变。";
                    var msgWrong = "物理学中，把较轻的核结合成质量较大的核，释放核能的反应叫做裂变。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 3)
                {
                    var msgRight = "原子弹爆炸利用的是核裂变。";
                    var msgWrong = "原子弹爆炸利用的是核聚变。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 4)
                {
                    var msgRight = "氢弹爆炸利用的是核聚变。";
                    var msgWrong = "氢弹爆炸利用的是核裂变。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 5)
                {
                    var msg = "我们今天要讲的利用核聚变的“人工小太阳”。";
                    DealWithMsg(msg);
                }
                else if (this.step == 6)
                {
                    var msgRight = "氘是氢的一种同位素。质量约为2个质子的重量";
                    var msgWrong = "氘是氢的一种同位素。质量约为3个质子的重量";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 7)
                {
                    var msg = "核素符号为$D$或$_1^2H$";
                    DealWithMsg(msg);
                }
                else if (this.step == 8)
                {
                    var msgRight = "利用$HD$、$HH$、$DD$三种物质的沸点不同，可通过普通液态氢的精馏过程进行分离。";
                    var msgWrong = "利用$HD$、$HH$、$DD$三种物质的燃点不同，可通过普通液态氢的精馏过程进行分离。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 9)
                {
                    var msg = "对于氘，本身就存在于水中。而且储量巨大。";
                    DealWithMsg(msg);
                }
                else if (this.step == 10)
                {
                    var msg = "自然界的氢中本身就含有0.02%的氘。";
                    DealWithMsg(msg);
                }
                else if (this.step == 11)
                {
                    var msgRight = "氚也是氢的一种同位素。质量约为3个质子质量";
                    var msgWrong = "氚也是氢的一种同位素。质量约为2个质子质量";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 12)
                {
                    var msgRight = "由于氚的半衰期为12.43年，所以自然界不存在氚。";
                    var msgWrong = "由于氚的半衰期为12.43年，氚和氘一样普遍存在于自然界。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 13)
                {
                    List<double> lines = new List<double>()
                    {
                       0.0000  ,  49.7003   ,5.0242  ,  50.4063,
5.0242  ,  50.4063,20.6932  ,  55.4248,
                        20.6932  ,  55.4248,26.3184  ,  58.4227,
26.3184  ,  58.4227,27.6246  ,  59.2375,
  27.6246  ,  59.2375,28.1898  ,  59.9740,
    28.1898  ,  59.9740,  28.5450  ,  60.8317,
       28.5450  ,  60.8317,  28.6662  ,  61.7520,
        28.6662  ,  61.7520, 28.5450  ,  62.6724,
       28.5450  ,  62.6724,  28.1898  ,  63.5301,
    28.1898  ,  63.5301,   27.6246  ,  64.2665,
    27.6246  ,  64.2665,     26.8882  ,  64.8317,
      26.8882  ,  64.8317,     26.0305  ,  65.1869,
      26.0305  ,  65.1869,   25.1101  ,  65.3081,
          25.1101  ,  65.3081,     24.1898  ,  65.1869,
           24.1898  ,  65.1869,     23.3321  ,  64.8317,
             23.3321  ,  64.8317,      22.5956  ,  64.2665,
             22.5956  ,  64.2665,   22.0305  ,  63.5301,
            22.0305  ,  63.5301,   21.6753  ,  62.6724,
            21.6753  ,  62.6724,      21.5541  ,  61.7520,
           21.5541  ,  61.7520,     21.6753  ,  60.8317,
            21.6753  ,  60.8317,      22.0305  ,  59.9740,
             22.0305  ,  59.9740,    22.5956  ,  59.2375,
            22.5956  ,  59.2375,   23.3321  ,  58.6724,
            23.3321  ,  58.6724,   24.1898  ,  58.3172,
            24.1898  ,  58.3172,      25.1101  ,  58.1960,
             25.1101  ,  58.1960,    26.4341  ,  58.4843,
             26.4341  ,  58.4843,     38.7729  ,  65.0602,
             38.7729  ,  65.0602,   43.8101  ,  70.2967,
             43.8101  ,  70.2967,      43.8101  ,   0.0000 ,

             9.2146,55.4266,21.7808,60.482,
             21.7808,60.482,19.0881,60.482,
             21.7808,60.482,19.8383,58.6172,

             9.9809,63.1767,12.5048,57.5149,
             9.9809,63.1767,12.5048,64.6693,

             20.2786,95.3352,16.1008,87.9284,
             18.1897,91.6318,25.6694,91.6318,
             18.9949,88.6993,23.3903,88.6993,

             16.4591,85.4051,24.8273,85.4051,
             24.8273,85.4051,24.8273,75.3537,
             24.8273,75.3537,29.7299,75.3537,
             29.7299,75.3537,29.7299,79.2391,

             17.5579,83.9692,17.5579,79.0702,
             17.5579,79.0702,15.6983,75.5226,
             21.2772,83.7158,21.2772,74.8469,


            100.0000  ,  49.7003   ,          94.9758  ,  50.4063 ,
          94.9758  ,  50.4063   ,     79.3068  ,  55.4248,
           79.3068  ,  55.4248  ,  73.6816  ,  58.4227,
          73.6816  ,  58.4227   ,     72.3754  ,  59.2375,
           72.3754  ,  59.2375   ,     72.3754  ,  59.2375,
        71.8102  ,  59.9740   ,        71.8102  ,  59.9740,
         71.4550  ,  60.8317  , 71.4550  ,  60.8317,
          71.3338  ,  61.7520  ,         71.3338  ,  61.7520,
           71.4550  ,  62.6724  , 71.8102  ,  63.5301,
           71.8102  ,  63.5301  ,    72.3754  ,  64.2665,
            72.3754  ,  64.2665   ,     73.1118  ,  64.8317,
            73.1118  ,  64.8317   ,     73.9695  ,  65.1869,
             73.9695  ,  65.1869  ,         74.8899  , 65.3081,
           74.8899  , 65.3081   ,      75.8102  , 65.1869,
            75.8102  , 65.1869   ,         76.6679  , 64.8317,
            76.6679  , 64.8317   ,  77.4044  , 64.2665,
            77.4044  , 64.2665  ,      77.9695  , 63.5301,
            77.9695  , 63.5301   ,     78.3247  , 62.6724,
            78.3247  , 62.6724  ,  78.4459  , 61.7520,
            78.4459  , 61.7520  ,      78.3247  , 60.8317,
            78.3247  , 60.8317   ,      77.9695  , 59.9740,
            77.9695  , 59.9740   ,       77.4044  , 59.2375,
           77.4044  , 59.2375   ,   76.6679  , 58.6724,
             76.6679  , 58.6724   ,    75.8102  , 58.3172,
           75.8102  , 58.3172   ,    74.8899  , 58.1960,
           74.8899  , 58.1960   ,  73.5659  , 58.4843,
            73.5659  , 58.4843   , 61.2271  , 65.0602,
           61.2271  , 65.0602   ,56.1899  , 70.2967,
         56.1899  , 70.2967  ,   56.1899  , 0.0000 ,

         90.7854,55.4266,78.2192,60.482,
         78.2192,60.482,80.9119,60.482,
         78.2192,60.482,80.1617,58.6172,

         89.5167,63.0876,86.8018,57.5149,
         88.1593,60.3013,90.925,59.3273,
         89.5167,63.0876,92.2824,62.1137,

         76.3203,95.3352,72.1425,87.9284,
         74.2314,91.6318,81.711,91.6318,
         75.0366,88.6993,79.432,88.6993,
         72.5007,85.4051,80.869,85.4051,
         80.869,85.4051,80.869,75.3537,
         80.869,75.3537,85.7716,75.3537,
         85.7716,75.3537,85.7716,79.2391,
         73.5996,83.9692,73.5996,79.0702,
        73.5996,79.0702,71.74,75.5226,
        75.9664,83.7158,75.9664,74.8469,

        78.7557,83.7158,78.7557,74.8469,

        1,1,99,1,
        99,1,99,99,
        99,99,1,99,
        1,99,1,1
                    };
                    DealWithPic(lines.ToArray(), new double[] { });

                }
                else if (this.step == 14)
                {
                    var msg = "如图所示。氘和氚反应，犹如把两个球往上推。有一定能量后，其反应。释放出巨大的能量（如上图中的势能）";
                    DealWithMsg(msg);
                }
                else if (this.step == 15)
                {
                    var msg = "要运用$$_1^2H+_1^3H\\rightarrow _2^4He+_0^1n$$"; DealWithMsg(msg);
                }
                else if (this.step == 16)
                {
                    var msgRight = "由于氘分子和氚分子的重量大于氦原子和1个中子的重量";
                    var msgWrong = "由于氘分子和氚分子的重量小于氦原子和1个中子的重量";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 17)
                {
                    var msg = "我们依据$E=\\Delta m c^2$，可以轻易计算出该反应释放的能量。";
                    DealWithMsg(msg);
                }
                else if (this.step == 18)
                {
                    var msg = "那现在问题来了，我们如何得到氚？";
                    DealWithMsg(msg);
                }
                else if (this.step == 19)
                {
                    var msgRight = "氚的制取是利用中子轰击锂原子$$_3^6Li+_0^1n \\rightarrow _2^4He+_1^3H$$这个反应在常温就能进行。";
                    var msgWrong = "氚的制取是利用中子轰击锂原子$$_3^7Li + _0^1n \\rightarrow _2^4He + _1^3H$$这个反应在常温就能进行。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }

                else if (this.step == 20)
                {
                    var msg = "现在氘和氚都拥有了。";
                    DealWithMsg(msg);
                }
                else if (this.step == 21)
                {
                    var msg = "如何让氘和氚发生反应？这个门槛有点儿高了。";
                    DealWithMsg(msg);
                }
                else if (this.step == 22)
                {
                    var msg = "我们的太阳之所以能发出光和热，是因为太阳中心有1500万K的高温、2500亿个标准大气压的压强的环境。这个环境为持续的核聚变提供了条件。";
                    DealWithMsg(msg);
                }
                else if (this.step == 23)
                {
                    var msg = "我们如何制造这个种环境呢？。"; DealWithMsg(msg);
                }
                else if (this.step == 24)
                {
                    var msg = "托卡马克装置是将加热到1亿多开的氘粒子和氚粒子放在由磁场构成的“容器”中。";
                    DealWithMsg(msg);
                }
                else if (this.step == 25)
                {
                    var msg = "想要此反应持续运行，需要不断往容器中注入氘和氚，并且从反应中提取能量与反应的废料氦。";
                    DealWithMsg(msg);
                }
                else if (this.step == 26)
                {
                    var msg = "咱们还是先谈谈磁约束。带电粒子在磁场的运动分为两部分：在垂直于磁力线方向做拉莫尔进动；沿磁力线方向则可以自由运动（如果磁场是均匀的。）";
                    DealWithMsg(msg);
                }
                else if (this.step == 27)
                {
                    var msgRight = "因此，带电粒子不会离开磁力线";
                    var msgWrong = "因此，带电粒子有可能会离开磁力线";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 28)
                {
                    var msg = "所以，磁场可以将高温等粒子体与周围物质（真空室）隔开。";
                    DealWithMsg(msg);
                }
                else if (this.step == 29)
                {
                    var msg = "磁约束聚变研究近60年的历史表明，环形此约束在理论可以控制核聚变反应堆。我们叫这个装置为托卡马克装置。";
                    DealWithMsg(msg);
                }
                else if (this.step == 30)
                {
                    var msgRight = "托卡马克是一种环形系统。";
                    var msgWrong = "托卡马克是一种方形系统。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 31)
                {
                    var msg = "如何制造这种盛放1亿5千度粒子的磁“容器”呢？";
                    DealWithMsg(msg);
                }
                else if (this.step == 32)
                {
                    var msgRight = "我们需要超导体";
                    var msgWrong = "我们需要隐形材料";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 33)
                {
                    var msgRight = "当导体在一温度下，可使电阻为零。我们称之为超导。";
                    var msgWrong = "当导体在一温度以上，可使电阻为零。我们称之为超导。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 34)
                {
                    var msg = "电阻为零后，可产生强大的电流。产生强大的电流后就可以产生强大的磁场。";
                    DealWithMsg(msg);
                }
                else if (this.step == 35)
                {
                    var msgRight = "但是现在的最好的超导，也得在低于常温下才能表现出超导性能。";
                    var msgWrong = "但是现在的最好的超导，也得在高于常温下才能表现出超导性能。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 36)
                {
                    var msg = "所以托卡马克装置是科学家的“冰与火之歌”。";
                    DealWithMsg(msg);
                }
                else if (this.step == 37)
                {
                    var msg = "咱们再聊聊惯性约束核聚变。";
                    DealWithMsg(msg);
                }
                else if (this.step == 38)
                {
                    var msg = "其原理非常简单。例如将2mg的氘和3mg的氚放置于一个金属小球中。用激光，轰击这个小球（轰击过程相当于原子弹引爆氢弹）";
                    DealWithMsg(msg);
                }
                else if (this.step == 39)
                {
                    var msg = "由于当量非常小。这个小氢弹的破坏力不像武器级别的氢弹破坏力大。而且能量在容器可控范围内。";
                    DealWithMsg(msg);
                }
                else if (this.step == 40)
                {
                    var msg = "我们从这里看到，惯性约束的核心就是怎样获得均匀的高能激光作为点火装置。"; DealWithMsg(msg);
                }
                else if (this.step == 41)
                {
                    var msg = "如公开资料显示，美国2009年，将192束X射线激光在极短的时间内，聚焦在一个胡椒粒大小的燃料球上。在十亿分之三秒内，以近500万亿瓦的功率，输出约180万焦耳的能量，产生了一亿开的高温和亿亿帕的高压。";
                    DealWithMsg(msg);
                }
                else if (this.step == 42)
                {
                    var msgRight = "这项技术的难点是指挥若干激光在短时间内一起开火。";
                    var msgWrong = "这项技术的难点是指挥若干激光在长时间内一起协同工作。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 43)
                {
                    var msgRight = "我国上世纪60年代在王淦昌、王大珩的领导下开始研究惯性约束核聚变。";
                    var msgWrong = "我国上世纪70年代在李政道、杨振宁的领导下开始研究惯性约束核聚变。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 44)
                {
                    var msg = "通过比较，惯性约束耗能高、成本高。由于得到高能的激光要消耗大量的能量，但磁场的成本较低。此外微型氢弹（靶丸）的成本也难降不下来。惯性约束的持续性差。得到持续的高能粒子的难度比得到稳定磁场的难度要大得多。";
                    DealWithMsg(msg);
                }
                else if (this.step == 45)
                {
                    var msgRight = "所以说，磁约束比惯性约束更有商业价值。";
                    var msgWrong = "所以说，惯性约束比磁约束更有商业价值。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step == 46)
                {
                    var msg = "但是，人类通过惯性约束，可以制造更高的温度和更高压力。可以模拟太阳中心的高温高压环境。而且，美国已经用这个装置在不进行大能量的核爆的基础上来改进他的氢弹。";
                    DealWithMsg(msg);
                }
                else if (this.step == 47)
                {
                    var msgRight = "所以说，惯性约束比磁约束更有科研价值与军事价值。";
                    var msgWrong = "所以说，磁约束比惯性约束更有科研价值与军事价值。";
                    DealWithRightAndWrong(msgRight, msgWrong);
                }
                else if (this.step >= 48 && this.step < 100)
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
                else if (this.step > 100)
                {
                    CloseSocket();
                }

            }
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

        public string SaveAddress(string address)
        {
            if (pleaseInput)
            {
                if (this.score >= minScore)
                {
                    if (!this.getRewardSuccess)
                    {
                        decimal moneycountAddV;

                        MysqlCore.BaseItem b = new MysqlCore.BaseItem("kekonghejubian");
                        b.AddAddressValue(address, out moneycountAddV);
                        //MySql.kekonghejubian.AddAddressValue();
                        this.getRewardSuccess = true;
                        this.step = 100;
                        var msgPass = $"您好，您往<div>{address}</div>  添加了一枚“人造小太阳”勋章，并且获得了{moneycountAddV.ToString("f2")}金币！！<div> <a href=\"XunZhang.html\" style=\"color: whitesmoke;\">查看勋章</a></div>";
                        DealWithMsg(msgPass);
                    }

                }
                pleaseInput = false;
            }
            return this.msg;
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
    }
}
