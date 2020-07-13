using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CaseManagerCore
{
    public class Population : CaseManager
    {
        bool hasRole = false;
        public Population()
        {
            PassObj p = new PassObj()
            {
                ObjType = "selectRole",
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
        }

        string role = "";

        Employee emplyee = null;
        public void setAsEmployee()
        {
            if (hasRole)
            { }
            else
            {
                hasRole = true;
                this.msg = "";
                this.role = "employee";
                this.emplyee = new Employee();

                List<string> msgs = new List<string>();

                msgs.Add(this.emplyee.ageDisplay);
                msgs.Add(this.emplyee.yearDisplay);
                //msgs.Add(this)

                PassObj p = new PassObj()
                {
                    ObjType = $"{this.role}-notify",
                    showContinue = true,
                    showIsError = true,
                    isEnd = false,
                    ObjID = $"{idPrevious}{startId++}",
                    styleStr = "msg",
                    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        ageDisplay = this.emplyee.ageDisplay,
                        yearDisplay = this.emplyee.yearDisplay,
                        childrenInfo = this.emplyee.childrenInfo,
                        state = this.emplyee.state,
                        actions = this.emplyee.actions,
                        educateAction = this.emplyee.educateAction
                    })

                };
                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            }
        }

        public class Employee
        {
            public class State
            {
                public bool canLove { get; set; }
                public bool canBeMarried { get; set; }
                public bool canGetFirstBaby { get; set; }
                public bool canGetSecondBaby { get; set; }
                public bool canGetThirdBaby { get; set; }
                public bool canGetFourthBaby { get; set; }
                public bool canEducate { get; set; }
                public bool canSingleWork { get; set; }
                public bool canPlayWithChildren { get; set; }
                public bool canStrugle { get; set; }



                public int? firstBabyAge = null;
                public int? secondBabyAge = null;
                public int? thirdBabyAge = null;
                public int? fourthBabyAge = null;

                public double sumSave = 0;

                public int age = 22;

                public int year { get; set; }

                public Dictionary<int, List<double>> educationCost { get; set; }

                public Dictionary<int, List<int>> educationScore { get; set; }

                public bool gameOver = false;


            }

            public State state { get; set; }

            public string ageSumSave
            {
                get { return $"你现在有{this.state.sumSave.ToString("f2")}金币的储蓄！"; }
            }

            public List<string> actions = new List<string>();

            public string educateAction
            {
                get
                {
                    var result = new List<string>();
                    if (actions.Contains("Employee-Educate-Perfect"))
                    {
                        return "Employee-Educate-Perfect";
                    }
                    else if (actions.Contains("Employee-Educate-Good"))
                    {
                        return "Employee-Educate-Good";
                    }
                    else
                    {
                        return "Employee-Educate-Common";
                    }
                }
            }

            internal List<string> childrenInfo
            {
                get
                {
                    var msg = new List<string>();
                    if (this.state.firstBabyAge != null)
                    {
                        if (this.state.firstBabyAge == 0)
                        {
                            msg.Add("您第一个孩子刚刚出生。");
                        }
                        else
                        {
                            msg.Add($"您第一个个孩子{this.state.firstBabyAge}岁了！");
                        }
                    }
                    if (this.state.secondBabyAge != null)
                    {
                        if (this.state.secondBabyAge == 0)
                        {
                            msg.Add("您第二个孩子刚刚出生。");
                        }
                        else
                        {
                            msg.Add($"您第二个孩子{this.state.secondBabyAge}岁了！");
                        }
                    }
                    if (this.state.thirdBabyAge != null)
                    {
                        if (this.state.thirdBabyAge == 0)
                        {
                            msg.Add("您第三个孩子刚刚出生。");
                        }
                        else
                        {
                            msg.Add($"您第三个孩子{this.state.thirdBabyAge}岁了！");
                        }
                    }
                    if (this.state.fourthBabyAge != null)
                    {
                        if (this.state.fourthBabyAge == 0)
                        {
                            msg.Add("您第四个孩子刚刚出生。");
                        }
                        else
                        {
                            msg.Add($"您第四个孩子{this.state.fourthBabyAge}岁了！");
                        }
                    }
                    return msg;
                }
            }

            public string yearDisplay { get { return $"这是你打工第{this.state.year + 1}年"; } }

            public string ageDisplay
            {
                get { return $"你现在{this.state.age}岁了。"; }
            }

            public Employee()
            {
                this.state = new State();
                this.actions = new List<string>();
                this.state.year = 0;
                this.state.canLove = true;
                this.state.canBeMarried = false;
                this.state.canGetFirstBaby = false;
                this.state.canGetSecondBaby = false;
                this.state.canGetThirdBaby = false;
                this.state.canGetFourthBaby = false;
                this.state.canEducate = false;
                this.state.canSingleWork = false;
                this.state.canPlayWithChildren = false;
                this.state.canStrugle = true;
                this.state.gameOver = false;

                var rm = Math.Abs(DateTime.Now.GetHashCode()) % 3;
                this.state.age = 22 + rm;

                this.state.firstBabyAge = null;
                this.state.secondBabyAge = null;
                this.state.thirdBabyAge = null;
                this.state.fourthBabyAge = null;

                this.state.educationCost = new Dictionary<int, List<double>>();
                this.state.educationScore = new Dictionary<int, List<int>>();
            }

            public override string ToString()
            {
                var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    state = this.state,
                    actions = this.actions,
                    notifyMsgs = new List<string>()
                });
                return Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Type = "Employee",
                    JsonValue = JsonValue
                });
            }

            internal static string DoAction(ref Random rm, Data data, Server.Command c)
            {
                PassObjToServer passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObjToServer>(c.JsonValue);
                passObj.state.age++;
                passObj.state.year++;

                DealWithWork(ref rm, data, c, ref passObj);

                if (passObj.state.fourthBabyAge != null)
                {
                    passObj.state.fourthBabyAge++;
                    if (passObj.state.fourthBabyAge > 3 && passObj.state.fourthBabyAge < passObj.state.age)
                    {
                        setChildEducate(passObj.state.fourthBabyAge.Value, 4, ref passObj, ref rm, data);
                    }
                }

                if (passObj.state.thirdBabyAge != null)
                {
                    passObj.state.thirdBabyAge++;
                    if (passObj.state.thirdBabyAge > 3 && passObj.state.fourthBabyAge < passObj.state.age)
                    {
                        setChildEducate(passObj.state.thirdBabyAge.Value, 3, ref passObj, data);
                    }
                }

                if (passObj.state.secondBabyAge != null)
                {
                    passObj.state.secondBabyAge++;

                    if (passObj.state.secondBabyAge > 3 && passObj.state.fourthBabyAge < passObj.state.age)
                    {
                        setChildEducate(passObj.state.secondBabyAge.Value, 2, ref passObj, data);
                    }
                }

                if (passObj.state.firstBabyAge != null)
                {
                    passObj.state.firstBabyAge++;

                    if (passObj.state.firstBabyAge > 2)
                    {
                        if (!passObj.state.canEducate)
                        {
                            passObj.notifyMsgs.Add("您获得了新技能：教育。");
                            passObj.state.canEducate = true;
                        }

                    }
                    if (passObj.state.firstBabyAge > 3 && passObj.state.fourthBabyAge < passObj.state.age)
                    {
                        setChildEducate(passObj.state.firstBabyAge.Value, 1, ref passObj, data);
                    }
                }
                //工作

                if (passObj.actions.Contains("Employee-Love"))
                {
                    return Love(ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-Marry"))
                {
                    return Marry(ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-BirthFirstBaby"))
                {
                    return BirthhBaby(1, ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-BirthSecondBaby"))
                {
                    return BirthhBaby(2, ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-BirthThirdBaby"))
                {
                    return BirthhBaby(3, ref rm, data, c, ref passObj);
                }
                else if (passObj.actions.Contains("Employee-BirthFourthBaby"))
                {
                    return BirthhBaby(4, ref rm, data, c, ref passObj);
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
            }

            private static void setChildEducate(int babyAgeValue, int babyIndex, ref PassObjToServer passObj, ref Random rm, Data data)
            {
                var selfPay = Math.Round(1.0 / 50 * data.govementPosition.Last(), 2);
                var govementPay = Math.Round(1 - selfPay, 2);
                //bool moneyIsNotEnought = false;
                if (babyAgeValue > 3)
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
                        price = Math.Round(price, 2);
                        if (passObj.state.sumSave - (price * selfPay) >= 0)
                        {
                            educationPay(price, babyIndex, selfPay, ref passObj);
                            addNotifyMsgOfEdution(govementPay, selfPay, babyIndex, price, "贵族教育", ref passObj);
                            setEducateResult(0, babyIndex, ref rm, ref passObj);

                            return;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (passObj.actions.Contains("Employee-Educate-Good") || passObj.actions.Contains("Employee-Educate-Perfect"))
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
                        price = Math.Round(price, 2);

                        if (passObj.state.sumSave - (price * selfPay) >= 0)
                        {
                            educationPay(price, babyIndex, selfPay, ref passObj);

                            if (passObj.actions.Contains("Employee-Educate-Perfect"))
                            {
                                addNotifyMsgOfEduReplaceBy(govementPay, selfPay, babyIndex, price, "贵族教育", "优质教育", ref passObj);
                            }
                            else
                            {
                                addNotifyMsgOfEdution(govementPay, selfPay, babyIndex, price, "优质教育", ref passObj);
                            }
                            setEducateResult(1, babyIndex, ref rm, ref passObj);
                            return;
                        }
                        else
                        {

                        }
                    }


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
                            price = data.govementPosition.Last() + 0.01;
                        }
                        educationPay(price, babyIndex, selfPay, ref passObj);

                        if (passObj.actions.Contains("Employee-Educate-Perfect"))
                        {
                            addNotifyMsgOfEduReplaceBy(govementPay, selfPay, babyIndex, price, "贵族教育", "普通教育", ref passObj);

                        }
                        else if (passObj.actions.Contains("Employee-Educate-Good"))
                        {
                            addNotifyMsgOfEduReplaceBy(govementPay, selfPay, babyIndex, price, "优质教育", "普通教育", ref passObj);
                        }
                        else
                        {
                            addNotifyMsgOfEdution(govementPay, selfPay, babyIndex, price, "普通教育", ref passObj);
                        }
                        setEducateResult(2, babyIndex, ref rm, ref passObj);
                        return;
                    }
                }
            }

            private static void setEducateResult(int selectCondition, int childIndex, ref Random rm, ref PassObjToServer passObj)
            {
                if (selectCondition == 0)
                {
                    //贵族教育
                    var score = 99;
                    for (int i = 0; i < 2; i++)
                    {
                        var s = rm.Next(90, 100);
                        score = Math.Min(score, s);
                    }
                    passObj.state.educationScore[childIndex].Add(score);
                    passObj.notifyMsgs.Add($"你的第{getBabyIndex(childIndex)}个孩子获取了{score}分。");
                }
                else if (selectCondition == 1)
                {
                    //贵族教育
                    var score = 99;
                    for (int i = 0; i < 2; i++)
                    {
                        var s = rm.Next(70, 100);
                        score = Math.Min(score, s);
                    }
                    passObj.state.educationScore[childIndex].Add(score);
                    passObj.notifyMsgs.Add($"你的第{getBabyIndex(childIndex)}个孩子获取了{score}分。");
                }
                else if (selectCondition == 2)
                {
                    //贵族教育
                    var score = 99;
                    for (int i = 0; i < 2; i++)
                    {
                        var s = rm.Next(50, 100);
                        score = Math.Min(score, s);
                    }
                    passObj.state.educationScore[childIndex].Add(score);
                    passObj.notifyMsgs.Add($"你的第{getBabyIndex(childIndex)}个孩子获取了{score}分。");
                }

            }

            private static void educationPay(double price, int babyIndex, double selfPay, ref PassObjToServer passObj)
            {
                price = Math.Round(price, 2);
                passObj.state.educationCost[babyIndex].Add(price);
                passObj.state.sumSave -= (price * selfPay);
                passObj.state.sumSave = Math.Round(passObj.state.sumSave);
            }

            private static void addNotifyMsgOfEdution(double govementPay, double selfPay, int babyIndex, double price, string edu, ref PassObjToServer passObj)
            {
                string msg;
                if (govementPay > 0)
                {
                    msg = $"您的第{getBabyIndex(babyIndex)}个孩子接受到了{edu}，总共需要{price.ToString("f2")}金币，个人花费{(price * selfPay).ToString("f2")}金币，政府补贴{(price * govementPay).ToString("f2")}金币。";
                }
                else if (govementPay == 0)
                {
                    msg = $"您的第{getBabyIndex(babyIndex)}个孩子接受到了{edu}，总共需要{price.ToString("f2")}金币，个人花费{(price * selfPay).ToString("f2")}金币。";
                }
                else
                {
                    msg = $"您的第{getBabyIndex(babyIndex)}个孩子接受到了{edu}，总共需要{price.ToString("f2")}金币，个人花费{(price * selfPay).ToString("f2")}金币，其中{(price * (-govementPay)).ToString("f2")}金币作为教育附加税上缴给了政府！";
                }
                passObj.notifyMsgs.Add(msg);
            }

            private static void addNotifyMsgOfEduReplaceBy(double govementPay, double selfPay, int babyIndex, double price, string replaceItem, string replaceBy, ref PassObjToServer passObj)
            {
                //  addNotifyMsgOfReplaceBy(govementPay, selfPay,babyIndex, price,"贵族教育", "普通教育",ref passObj);
                string msg;
                if (govementPay > 0)
                {
                    msg = $"您的第{getBabyIndex(babyIndex)}个孩子未能接受{replaceItem}，但接受到了{replaceBy}，总共需要{price.ToString("f2")}金币，个人花费{(price * selfPay).ToString("f2")}金币，政府补贴{(price * govementPay).ToString("f2")}金币。";
                }
                else if (govementPay == 0)
                {
                    msg = $"您的第{getBabyIndex(babyIndex)}个孩子未能接受{replaceItem}，但接受到了{replaceBy}，总共需要{price.ToString("f2")}金币，个人花费{(price * selfPay).ToString("f2")}金币。";
                }
                else
                {
                    msg = $"您的第{getBabyIndex(babyIndex)}个孩子未能接受{replaceItem}，但接受到了{replaceBy}，总共需要{price.ToString("f2")}金币，个人花费{(price * selfPay).ToString("f2")}金币，其中{(price * (-govementPay)).ToString("f2")}金币作为教育附加税上缴给了政府！";
                }
                passObj.notifyMsgs.Add(msg);
            }

            private static string getBabyIndex(int babyIndex)
            {
                switch (babyIndex)
                {
                    case 1: { return "一"; };
                    case 2: { return "二"; };
                    case 3: { return "三"; };
                    case 4: { return "四"; };
                    default: { return ""; }
                }
                // throw new NotImplementedException();
            }


            private static string BirthhBaby(int babyIndex, ref Random rm, Data data, Server.Command c, ref PassObjToServer passObj)
            {
                var govementPosition = data.govementPosition.Last();
                var successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.375 + 0.425;

                double cost;
                switch (babyIndex)
                {
                    case 1:
                        {
                            cost = data.housePrice * 0.5;
                        }; break;
                    case 2:
                        {
                            cost = data.housePrice * 0.4;
                        }; break;
                    case 3:
                        {
                            cost = data.housePrice * 0.3;
                        }; break;
                    case 4:
                        {
                            cost = data.housePrice * 0.2;
                        }; break;
                    default:
                        {
                            cost = data.housePrice * 0.2;
                        }; break;
                }
                cost = Math.Round(cost, 2);

                var sumActionCount = data.employerActions.Sum(item => item.Count);




                short employerAction = -1;

                //遇到的雇主，会对生孩几率的影响。
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

                if (rm.NextDouble() < successLimet)
                {
                    passObj.notifyMsgs.Add($"你生第{getBabyIndex(babyIndex)}个孩子的时候，花费了{cost.ToString("f2")}金币。");
                    if (babyIndex == 1) passObj.notifyMsgs.Add("您获得了新技能：陪孩子玩耍--亲子。");
                    passObj.state.sumSave -= cost;
                    passObj.state.sumSave = Math.Round(passObj.state.sumSave, 2);

                    switch (babyIndex)
                    {
                        case 1:
                            {
                                passObj.state.firstBabyAge = 0;
                                passObj.state.canGetSecondBaby = true;
                                passObj.state.canGetFirstBaby = false;
                            }; break;
                        case 2:
                            {
                                passObj.state.secondBabyAge = 0;
                                passObj.state.canGetThirdBaby = true;
                                passObj.state.canGetSecondBaby = false;
                            }; break;
                        case 3:
                            {
                                passObj.state.thirdBabyAge = 0;
                                passObj.state.canGetFourthBaby = true;
                                passObj.state.canGetThirdBaby = false;
                            }; break;
                        case 4:
                            {
                                passObj.state.fourthBabyAge = 0;
                                passObj.state.canGetFourthBaby = false;
                            }; break;

                    }

                    passObj.state.educationCost.Add(1, new List<double>());
                    passObj.state.educationScore.Add(babyIndex, new List<int>());
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
                else
                {
                    passObj.notifyMsgs.Add($"你生第一个孩子的时候，受孕不成功！！");
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
            }


            private static void DealWithWork(ref Random rm, Data data, Server.Command c, ref PassObjToServer passObj)
            {
                var govementPosition = data.govementPosition.Last();
                double salary = Math.Cos(govementPosition / 100 * Math.PI) * 0.5 + 1;//
                salary = Math.Round(salary, 2);
                passObj.state.sumSave += salary;
                {
                    //恋爱、政府、企业主对打工收入的影响。
                }
                passObj.notifyMsgs.Add($"你去年在打工中，获得了{salary.ToString("f2")}金币。");
            }

            private static string Marry(ref Random rm, Data data, Server.Command c, ref PassObjToServer passObj)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 打工者的恋爱成功率
            /// </summary>
            /// <param name="rm"></param>
            /// <param name="data"></param>
            /// <param name="c"></param>
            /// <param name="passObj"></param>
            /// <returns></returns>
            private static string Love(ref Random rm, Data data, Server.Command c, ref PassObjToServer passObj)
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
        }

        public class Server
        {
            public class Command
            {
                public string Type { get; set; }
                public string JsonValue { get; set; }
            }
            public static string dealWithMsg(ref Random rm, string ss, ref Data data, int minitues, int seconds)
            {
                var c = Newtonsoft.Json.JsonConvert.DeserializeObject<Command>(ss);
                switch (c.Type)
                {
                    case "Employee":
                        {
                            return Employee.DoAction(ref rm, data, c);
                        }; break;
                    case "Refresh":
                        {
                            return Refresh(ref rm, ref data, minitues, seconds);
                        }; break;
                    default:
                        {
                            return "";
                        }

                }
                var passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObjToServer>(c.JsonValue);
                throw new NotImplementedException();
            }

            private static string Refresh(ref Random rm, ref Data data, int minitues, int seconds)
            {
                var operateTime = DateTime.Now.AddMinutes(-minitues).AddSeconds(-seconds);
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
                    File.WriteAllText("Data.dt", json);
                }

                return $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--Refrest OK";
            }
        }

        public class Data
        {
            /// <summary>
            /// 24小时内成功的企业
            /// 依据此参数*15为企业能养活的人数
            /// </summary>
            public List<int> employerCountLast24Hour { get; set; }

            /// <summary>
            /// 教育参数1
            /// 此参数受24小时内打工者获得的奖金是否大于24×60进行调节
            /// </summary>
            //public double educationK1 { get; set; }

            /// <summary>
            /// 教育参数2
            /// 此参数受govementPosition影响
            /// </summary>
            //public double educationK2 { get; set; }


            /// <summary>
            /// 过去24小时教育花费记录表
            /// 用于计算教育花费
            /// 24小时内10-1名，接受极好教育
            /// 这个存储的是比例。真实的价格为value*Base
            /// </summary>
            public List<List<double>> educationCost { get; set; }
            /// <summary>
            /// 政府资本倾向,过去24小时内的数据
            /// 0-99
            /// 政府资本走向，决定情商-智商比例，决定人才评分
            /// </summary>
            public List<double> govementPosition { get; set; }

            public List<double> employerSumEarn { get; set; }

            public List<double> employteenSumEarn { get; set; }

            /// <summary>
            /// 人才等级，用于决定什么是合格人才的质数，第一个是数量，第二是平均值。新加入，在取新平均
            /// </summary>
            public List<long[]> rencaiLevel { get; set; }
            //public double employerK1 = 0.1853;
            //public double employerK2 = 1;

            public List<List<Int16>> employerActions { get; set; }
            public List<List<Int16>> employteenActions { get; set; }

            public string state { get; set; }

            /// <summary>
            /// 企业的年营业率，取值范围在1.05~1.20之间
            /// 这个参数受employerSumEarn影响。
            /// </summary>
            public double businessRate { get; set; }

            ///// <summary>
            ///// 教育基本支出
            ///// 当24小时内，依据打工者收入进行调整
            ///// </summary>
            //public double educationBasePercent { get; set; }

            /// <summary>
            /// 房价
            /// </summary>
            public double housePrice { get; set; }

            /// <summary>
            /// 教育基本价格
            /// </summary>
            //public double educateBasePrice { get; set; }



        }

        public class PassObjToServer
        {
            public Employee.State state { get; set; }
            public List<string> actions { get; set; }

            public List<string> notifyMsgs { get; set; }
        }

        public static class Config
        {
            //public const string ServerIP = "10.80.52.218";
            //public const int ServerPort = 20702;
        }

        public void Action(string action)
        {
            switch (action)
            {
                case "A":
                    {
                        if (emplyee != null)
                        {
                            //A对于打工者来说，只是恋爱动作
                            if (this.emplyee.actions.Contains("Employee-Love"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Love");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.emplyee.state,
                                    actions = this.emplyee.actions,
                                    educateAction = this.emplyee.educateAction

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                this.emplyee.actions.Add("Employee-Love");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.emplyee.state,
                                    actions = this.emplyee.actions,
                                    educateAction = this.emplyee.educateAction

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;
                case "I":
                    {
                        if (emplyee != null)
                        {
                            //A对于打工者来说，只是恋爱动作
                            if (this.emplyee.actions.Contains("Employee-Strive"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Strive");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.emplyee.state,
                                    actions = this.emplyee.actions,
                                    educateAction = this.emplyee.educateAction

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                this.emplyee.actions.Add("Employee-Strive");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.emplyee.state,
                                    actions = this.emplyee.actions,
                                    educateAction = this.emplyee.educateAction

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;
                case "B":
                    {
                        if (emplyee != null)
                        {
                            //A对于打工者来说，只是恋爱动作
                            if (this.emplyee.actions.Contains("Employee-Marry"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Marry");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.emplyee.state,
                                    actions = this.emplyee.actions,
                                    educateAction = this.emplyee.educateAction

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                this.emplyee.actions.Add("Employee-Marry");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.emplyee.state,
                                    actions = this.emplyee.actions,
                                    educateAction = this.emplyee.educateAction

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;


            }

        }

        public void NextYear()
        {
            //var msg=
            //msg = this.ToString();
            if (this.emplyee != null)
            {
                var ip = IpAndPortManager.Population_Ip;
                int port = IpAndPortManager.Population_Port;
                using (TcpClient client = new TcpClient(ip, port))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        //var msg = "{\"Type\":\"Refresh\"}";
                        Byte[] msgData = System.Text.Encoding.UTF8.GetBytes(this.emplyee.ToString());
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
                        //  Console.WriteLine("{0}Received: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), response);
                        stream.Close();

                        if (string.IsNullOrEmpty(response)) { }
                        else
                        {

                            var PassObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObjToServer>(response);

                            this.emplyee.actions = new List<string>();
                            this.emplyee.state = PassObj.state;

                            for (int i = 0; i < PassObj.notifyMsgs.Count; i++)
                            {
                                //Console.WriteLine(PassObj.notifyMsgs[i]);
                            }

                            PassObj p = new PassObj()
                            {
                                ObjType = $"{this.role}-notify-next",
                                showContinue = true,
                                showIsError = true,
                                isEnd = false,
                                ObjID = $"{idPrevious}{startId++}",
                                styleStr = "msg",
                                msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                                {
                                    ageDisplay = this.emplyee.ageDisplay,
                                    yearDisplay = this.emplyee.yearDisplay,
                                    childrenInfo = this.emplyee.childrenInfo,
                                    state = this.emplyee.state,
                                    actions = this.emplyee.actions,
                                    educateAction = this.emplyee.educateAction,
                                    notifyMsgs = PassObj.notifyMsgs
                                })

                            };
                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                        }
                    }
                    client.Close();
                }
            }


        }
    }
}
