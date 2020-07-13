using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagerCore
{
    public abstract class PeriodictableA : CaseManager
    {

    }

    public class Periodictable : PeriodictableA
    {
        const int minScore = 98;
        //const Dictionary<int, string> PeriodictSimple =
        Dictionary<int, bool> usedIndex = new Dictionary<int, bool>();
        int currentIndex = 0;
        public Periodictable()
        {
            this.usedIndex = new Dictionary<int, bool>();
            this.idPrevious = "pt";
            this.rm = new Random(DateTime.Now.GetHashCode());
            rm.Next();
            rm.Next();
            this.currentIndex = this.rm.Next(1, 112);
            this.usedIndex.Add(this.currentIndex, true);
            this.step = 0;
            DealWithRightAndWrong2();
        }

        public void Continue()
        {
            if (error)
            {
                this.msg = this.rightMsg;
                this.error = !error;
                if (this.score > 0)
                    this.score -= 30;
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
                //this.step++;
                if (this.usedIndex.Count < 112)
                {
                    do
                    {
                        this.currentIndex = this.rm.Next(1, 113);
                    }
                    while (this.usedIndex.ContainsKey(this.currentIndex));
                    this.usedIndex.Add(this.currentIndex, true);
                    DealWithRightAndWrong2();
                }
                else if (this.step < 100)
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
                            var msg = $"您的得分为{this.score}。小于{minScore}将不能获得勋章，继续努力吧！少年！！！<a href=\"index.html\" style=\"color: green;\">返回</a>";
                            DealWithMsg(msg);
                            this.step = 100;
                        }
                    }
                }

            }
        }
        public string SaveAddress(string address)
        {
            //throw new Exception("");
            MysqlCore.BaseItem b = new MysqlCore.BaseItem("periodictable");
            return this.SaveAddress(address, b.xunzhangName, minScore, b.AddAddressValue);
        }

        static void GetPeriodic(int indexValue, out string simple, out string chineseStr, out bool radioactivity)
        {
            switch (indexValue)
            {
                case 1: { simple = "H"; chineseStr = "氢"; radioactivity = false; }; return;
                case 2: { simple = "He"; chineseStr = "氦"; radioactivity = false; }; return;
                case 3: { simple = "Li"; chineseStr = "锂"; radioactivity = false; }; return;
                case 4: { simple = "Be"; chineseStr = "铍"; radioactivity = false; }; return;
                case 5: { simple = "B"; chineseStr = "硼"; radioactivity = false; }; return;
                case 6: { simple = "C"; chineseStr = "碳"; radioactivity = false; }; return;
                case 7: { simple = "N"; chineseStr = "氮"; radioactivity = false; }; return;
                case 8: { simple = "O"; chineseStr = "氧"; radioactivity = false; }; return;
                case 9: { simple = "F"; chineseStr = "氟"; radioactivity = false; }; return;
                case 10: { simple = "Ne"; chineseStr = "氖"; radioactivity = false; }; return;
                case 11: { simple = "Na"; chineseStr = "钠"; radioactivity = false; }; return;
                case 12: { simple = "Mg"; chineseStr = "镁"; radioactivity = false; }; return;
                case 13: { simple = "Al"; chineseStr = "铝"; radioactivity = false; }; return;
                case 14: { simple = "Si"; chineseStr = "硅"; radioactivity = false; }; return;
                case 15: { simple = "P"; chineseStr = "磷"; radioactivity = false; }; return;
                case 16: { simple = "S"; chineseStr = "硫"; radioactivity = false; }; return;
                case 17: { simple = "Cl"; chineseStr = "氯"; radioactivity = false; }; return;
                case 18: { simple = "Ar"; chineseStr = "氩"; radioactivity = false; }; return;
                case 19: { simple = "K"; chineseStr = "钾"; radioactivity = false; }; return;
                case 20: { simple = "Ca"; chineseStr = "钙"; radioactivity = false; }; return;
                case 21: { simple = "Sc"; chineseStr = "钪"; radioactivity = false; }; return;
                case 22: { simple = "Ti"; chineseStr = "钛"; radioactivity = false; }; return;
                case 23: { simple = "V"; chineseStr = "钒"; radioactivity = false; }; return;
                case 24: { simple = "Cr"; chineseStr = "铬"; radioactivity = false; }; return;
                case 25: { simple = "Mn"; chineseStr = "锰"; radioactivity = false; }; return;
                case 26: { simple = "Fe"; chineseStr = "铁"; radioactivity = false; }; return;
                case 27: { simple = "Co"; chineseStr = "钴"; radioactivity = false; }; return;
                case 28: { simple = "Ni"; chineseStr = "镍"; radioactivity = false; }; return;
                case 29: { simple = "Cu"; chineseStr = "铜"; radioactivity = false; }; return;
                case 30: { simple = "Zn"; chineseStr = "锌"; radioactivity = false; }; return;
                case 31: { simple = "Ga"; chineseStr = "镓"; radioactivity = false; }; return;
                case 32: { simple = "Ge"; chineseStr = "锗"; radioactivity = false; }; return;
                case 33: { simple = "As"; chineseStr = "砷"; radioactivity = false; }; return;
                case 34: { simple = "Se"; chineseStr = "硒"; radioactivity = false; }; return;
                case 35: { simple = "Br"; chineseStr = "溴"; radioactivity = false; }; return;
                case 36: { simple = "Kr"; chineseStr = "氪"; radioactivity = false; }; return;
                case 37: { simple = "Rb"; chineseStr = "铷"; radioactivity = false; }; return;
                case 38: { simple = "Sr"; chineseStr = "锶"; radioactivity = false; }; return;
                case 39: { simple = "Y"; chineseStr = "钇"; radioactivity = false; }; return;
                case 40: { simple = "Zr"; chineseStr = "锆"; radioactivity = false; }; return;
                case 41: { simple = "Nb"; chineseStr = "铌"; radioactivity = false; }; return;
                case 42: { simple = "Mo"; chineseStr = "钼"; radioactivity = false; }; return;
                case 43: { simple = "Tc"; chineseStr = "锝"; radioactivity = true; }; return;
                case 44: { simple = "Ru"; chineseStr = "钌"; radioactivity = false; }; return;
                case 45: { simple = "Rh"; chineseStr = "铑"; radioactivity = false; }; return;
                case 46: { simple = "Pd"; chineseStr = "钯"; radioactivity = false; }; return;
                case 47: { simple = "Ag"; chineseStr = "银"; radioactivity = false; }; return;
                case 48: { simple = "Cd"; chineseStr = "镉"; radioactivity = false; }; return;
                case 49: { simple = "In"; chineseStr = "铟"; radioactivity = false; }; return;
                case 50: { simple = "Sn"; chineseStr = "锡"; radioactivity = false; }; return;
                case 51: { simple = "Sb"; chineseStr = "锑"; radioactivity = false; }; return;
                case 52: { simple = "Te"; chineseStr = "碲"; radioactivity = false; }; return;
                case 53: { simple = "I"; chineseStr = "碘"; radioactivity = false; }; return;
                case 54: { simple = "Xe"; chineseStr = "氙"; radioactivity = false; }; return;
                case 55: { simple = "Cs"; chineseStr = "铯"; radioactivity = false; }; return;
                case 56: { simple = "Ba"; chineseStr = "钡"; radioactivity = false; }; return;
                case 57: { simple = "La"; chineseStr = "镧"; radioactivity = false; }; return;
                case 58: { simple = "Ce"; chineseStr = "铈"; radioactivity = false; }; return;
                case 59: { simple = "Pr"; chineseStr = "镨"; radioactivity = false; }; return;
                case 60: { simple = "Nd"; chineseStr = "钕"; radioactivity = false; }; return;
                case 61: { simple = "Pm"; chineseStr = "钷"; radioactivity = true; }; return;
                case 62: { simple = "Sm"; chineseStr = "钐"; radioactivity = false; }; return;
                case 63: { simple = "Eu"; chineseStr = "铕"; radioactivity = false; }; return;
                case 64: { simple = "Gd"; chineseStr = "钆"; radioactivity = false; }; return;
                case 65: { simple = "Tb"; chineseStr = "铽"; radioactivity = false; }; return;
                case 66: { simple = "Dy"; chineseStr = "镝"; radioactivity = false; }; return;
                case 67: { simple = "Ho"; chineseStr = "钬"; radioactivity = false; }; return;
                case 68: { simple = "Er"; chineseStr = "铒"; radioactivity = false; }; return;
                case 69: { simple = "Tm"; chineseStr = "铥"; radioactivity = false; }; return;
                case 70: { simple = "Yb"; chineseStr = "镱"; radioactivity = false; }; return;
                case 71: { simple = "Lu"; chineseStr = "镥"; radioactivity = false; }; return;
                case 72: { simple = "Hf"; chineseStr = "铪"; radioactivity = false; }; return;
                case 73: { simple = "Ta"; chineseStr = "钽"; radioactivity = false; }; return;
                case 74: { simple = "W"; chineseStr = "钨"; radioactivity = false; }; return;
                case 75: { simple = "Re"; chineseStr = "铼"; radioactivity = false; }; return;
                case 76: { simple = "Os"; chineseStr = "锇"; radioactivity = false; }; return;
                case 77: { simple = "Ir"; chineseStr = "铱"; radioactivity = false; }; return;
                case 78: { simple = "Pt"; chineseStr = "铂"; radioactivity = false; }; return;
                case 79: { simple = "Au"; chineseStr = "金"; radioactivity = false; }; return;
                case 80: { simple = "Hg"; chineseStr = "汞"; radioactivity = false; }; return;
                case 81: { simple = "Tl"; chineseStr = "铊"; radioactivity = false; }; return;
                case 82: { simple = "Pb"; chineseStr = "铅"; radioactivity = false; }; return;
                case 83: { simple = "Bi"; chineseStr = "铋"; radioactivity = false; }; return;
                case 84: { simple = "Po"; chineseStr = "钋"; radioactivity = true; }; return;
                case 85: { simple = "At"; chineseStr = "砹"; radioactivity = true; }; return;
                case 86: { simple = "Rn"; chineseStr = "氡"; radioactivity = true; }; return;
                case 87: { simple = "Fr"; chineseStr = "钫"; radioactivity = true; }; return;
                case 88: { simple = "Ra"; chineseStr = "镭"; radioactivity = true; }; return;
                case 89: { simple = "Ac"; chineseStr = "锕"; radioactivity = true; }; return;
                case 90: { simple = "Th"; chineseStr = "钍"; radioactivity = true; }; return;
                case 91: { simple = "Pa"; chineseStr = "镤"; radioactivity = true; }; return;
                case 92: { simple = "U"; chineseStr = "铀"; radioactivity = true; }; return;
                case 93: { simple = "Np"; chineseStr = "镎"; radioactivity = true; }; return;
                case 94: { simple = "Pu"; chineseStr = "钚"; radioactivity = true; }; return;
                case 95: { simple = "Am"; chineseStr = "镅"; radioactivity = true; }; return;
                case 96: { simple = "Cm"; chineseStr = "锔"; radioactivity = true; }; return;
                case 97: { simple = "Bk"; chineseStr = "锫"; radioactivity = true; }; return;
                case 98: { simple = "Cf"; chineseStr = "锎"; radioactivity = true; }; return;
                case 99: { simple = "Es"; chineseStr = "锿"; radioactivity = true; }; return;
                case 100: { simple = "Fm"; chineseStr = "镄"; radioactivity = true; }; return;
                case 101: { simple = "Md"; chineseStr = "钔"; radioactivity = true; }; return;
                case 102: { simple = "No"; chineseStr = "锘"; radioactivity = true; }; return;
                case 103: { simple = "Lr"; chineseStr = "铹"; radioactivity = true; }; return;
                case 104: { simple = "Rf"; chineseStr = "钅卢"; radioactivity = true; }; return;
                case 105: { simple = "Db"; chineseStr = "钅杜"; radioactivity = true; }; return;
                case 106: { simple = "Sg"; chineseStr = "钅喜"; radioactivity = true; }; return;

                case 107: { simple = "Bh"; chineseStr = "钅波"; radioactivity = true; }; return;
                case 108: { simple = "Hs"; chineseStr = "钅黑"; radioactivity = true; }; return;
                case 109: { simple = "Mt"; chineseStr = "钅麦"; radioactivity = true; }; return;
                case 110: { simple = "Ds"; chineseStr = "钅达"; radioactivity = true; }; return;
                case 111: { simple = "Rg"; chineseStr = "钅仑"; radioactivity = true; }; return;
                case 112: { simple = "Cn"; chineseStr = "钅哥"; radioactivity = true; }; return;

            };
            simple = "";
            chineseStr = "";
            radioactivity = false;
            return;
        }

        //public override void DealWithRightAndWrong
        public void DealWithRightAndWrong2()
        {
            if (this.usedIndex.Count < 112)
            {
                int rmV = this.rm.Next(0, 2);
                int errorIndex = this.currentIndex;
                do
                {
                    errorIndex = this.rm.Next(1, 113);
                }
                while (errorIndex == this.currentIndex || this.usedIndex.ContainsKey(errorIndex));

                string simple_Right, chineseStr_Right;
                bool radioActivity_Right;

                string simple_Wrong, chineseStr_Wrong;
                bool radioActivity_Wrong;

                GetPeriodic(this.currentIndex, out simple_Right, out chineseStr_Right, out radioActivity_Right);
                GetPeriodic(errorIndex, out simple_Wrong, out chineseStr_Wrong, out radioActivity_Wrong);

                this.errorMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    ObjType = "Change",
                    indexV = this.currentIndex,
                    InnerHtml = $"<div style=\"height:22px;width:64px;\"><span style=\"height:14px;font-size:10px;\">{this.currentIndex}</span><span style=\"height:22px;font-size:20px;float:right\">{simple_Wrong}</span></div><div style=\"height:42px;\"><span style=\"height:42px;font-size:22px;\">{chineseStr_Wrong}</span></div>",
                    showContinue = true,
                    showIsError = true,
                });
                this.rightMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    ObjType = "Change",
                    indexV = this.currentIndex,
                    InnerHtml = $"<div style=\"height:22px;width:64px;\"><span style=\"height:14px;font-size:10px;\">{this.currentIndex}</span><span style=\"height:22px;font-size:20px;float:right\">{simple_Right}</span></div><div style=\"height:42px;\"><span style=\"height:42px;font-size:22px;\">{chineseStr_Right}</span></div>",
                    showContinue = true,
                    showIsError = true,
                });
                if (rmV == 0)
                {
                    this.error = true;
                    this.msg = this.errorMsg;
                }
                else
                {
                    this.error = false;
                    this.msg = this.rightMsg;
                }
                this.AddScore = 1;
            }
            else if (this.usedIndex.Count == 112)
            {
                string simple_Right, chineseStr_Right;
                bool radioActivity_Right;

                GetPeriodic(this.currentIndex, out simple_Right, out chineseStr_Right, out radioActivity_Right);

                this.rightMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    ObjType = "Change",
                    indexV = this.currentIndex,
                    InnerHtml = $"<div style=\"height:22px;width:64px;\"><span style=\"height:14px;font-size:10px;\">{this.currentIndex}</span><span style=\"height:22px;font-size:20px;float:right\">{simple_Right}</span></div><div style=\"height:42px;\"><span style=\"height:42px;font-size:22px;\">{chineseStr_Right}</span></div>",
                    showContinue = true,
                    showIsError = false,
                });
                this.msg = this.rightMsg;
            }
            else
            {
                throw new Exception("");
            }
        }
    }
}
