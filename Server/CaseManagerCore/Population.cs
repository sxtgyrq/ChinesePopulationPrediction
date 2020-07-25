using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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


        public enum empoyerAction
        {
            _996,
            onlyYounger,
            discriminateWomen,
            newTecnology_Success,
            newTecnology_Failure,
            goAway,
            improveWelfare,
            stopBusiness,
            @null
        }

        public enum emploeeAction
        {
            normal,
            strive,
            enjoyTime,
            singleWork
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

                public int ageOfStartingGame = 22;

                public int year { get; set; }

                public Dictionary<int, List<double>> educationCost { get; set; }

                public Dictionary<int, List<int>> educationScore { get; set; }
                public Dictionary<int, List<int>> spiritScore { get; set; }

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
                this.state.ageOfStartingGame = this.state.age;

                this.state.firstBabyAge = null;
                this.state.secondBabyAge = null;
                this.state.thirdBabyAge = null;
                this.state.fourthBabyAge = null;

                this.state.educationCost = new Dictionary<int, List<double>>();
                this.state.educationScore = new Dictionary<int, List<int>>();
                this.state.spiritScore = new Dictionary<int, List<int>>();
            }

            public override string ToString()
            {
                var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new PassObjToServer_Employee()
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
                PassObjToServer_Employee passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObjToServer_Employee>(c.JsonValue);

                if (passObj.state.gameOver)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }

                passObj.state.age++;
                passObj.state.year++;

                var condition = getWorkCondition(ref rm, data);
                DealWithWork(condition, ref rm, data, c, ref passObj);

                if (passObj.state.fourthBabyAge != null)
                {
                    passObj.state.fourthBabyAge++;
                    if (passObj.state.fourthBabyAge > 3 && passObj.state.fourthBabyAge <= passObj.state.ageOfStartingGame)
                    {
                        setChildEducate(passObj.state.fourthBabyAge.Value, 4, ref passObj, ref rm, data);
                    }


                }

                if (passObj.state.thirdBabyAge != null)
                {
                    passObj.state.thirdBabyAge++;
                    if (passObj.state.thirdBabyAge > 3 && passObj.state.thirdBabyAge <= passObj.state.ageOfStartingGame)
                    {
                        setChildEducate(passObj.state.thirdBabyAge.Value, 3, ref passObj, ref rm, data);
                    }
                }

                if (passObj.state.secondBabyAge != null)
                {
                    passObj.state.secondBabyAge++;

                    if (passObj.state.secondBabyAge > 3 && passObj.state.secondBabyAge <= passObj.state.ageOfStartingGame)
                    {
                        setChildEducate(passObj.state.secondBabyAge.Value, 2, ref passObj, ref rm, data);
                    }
                }

                if (passObj.state.firstBabyAge != null)
                {

                    passObj.state.firstBabyAge++;

                    if (passObj.state.firstBabyAge > 2)
                    {
                        if (!passObj.state.canEducate)
                        {
                            //  passObj.notifyMsgs.Add("您获得了新技能：教育。");
                            passObj.state.canEducate = true;
                        }

                    }
                    if (passObj.state.firstBabyAge > 3 && passObj.state.firstBabyAge <= passObj.state.ageOfStartingGame)
                    {
                        setChildEducate(passObj.state.firstBabyAge.Value, 1, ref passObj, ref rm, data);
                    }
                }

                setChildrenScript(condition, ref passObj, ref rm, data);

                setCanPlayWithChildren(ref passObj);
                setCanEducate(ref passObj);
                //工作

                setBirthState(ref passObj);

                if (passObj.actions.Contains("Employee-Love"))
                {
                    return Love(condition, ref rm, data, c, ref passObj);
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
                    if (passObj.state.age > 60)
                    {
                        if (passObj.state.firstBabyAge != null && passObj.state.firstBabyAge <= passObj.state.ageOfStartingGame + 1)
                        {
                            passObj.state.gameOver = false;
                        }
                        else if (passObj.state.secondBabyAge != null && passObj.state.secondBabyAge <= passObj.state.ageOfStartingGame + 1)
                        {
                            passObj.state.gameOver = false;
                        }
                        else if (passObj.state.thirdBabyAge != null && passObj.state.thirdBabyAge <= passObj.state.ageOfStartingGame + 1)
                        {
                            passObj.state.gameOver = false;
                        }
                        else if (passObj.state.fourthBabyAge != null && passObj.state.fourthBabyAge <= passObj.state.ageOfStartingGame + 1)
                        {
                            passObj.state.gameOver = false;
                        }
                        else
                        {
                            passObj.state.gameOver = true;
                        }
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
            }

            private static void setBirthState(ref PassObjToServer_Employee passObj)
            {
                if (passObj.state.age >= 42)
                {
                    passObj.state.canGetFirstBaby = false;
                    passObj.state.canGetSecondBaby = false;
                    passObj.state.canGetThirdBaby = false;
                    passObj.state.canGetFourthBaby = false;
                }
            }

            private static void setCanEducate(ref PassObjToServer_Employee passObj)
            {
                //throw new Exception("写到这里不想写了");

                if ((passObj.state.firstBabyAge != null && passObj.state.firstBabyAge >= 3 && passObj.state.firstBabyAge < passObj.state.ageOfStartingGame) ||
                    (passObj.state.secondBabyAge != null && passObj.state.secondBabyAge >= 3 && passObj.state.secondBabyAge < passObj.state.ageOfStartingGame) ||
                    (passObj.state.thirdBabyAge != null && passObj.state.thirdBabyAge >= 3 && passObj.state.thirdBabyAge < passObj.state.ageOfStartingGame) ||
                    (passObj.state.fourthBabyAge != null && passObj.state.fourthBabyAge >= 3 && passObj.state.fourthBabyAge < passObj.state.ageOfStartingGame)
                    )
                {
                    if (passObj.state.canEducate) { }
                    else
                    {
                        passObj.state.canEducate = true;
                        //  passObj.notifyMsgs.Add("您获得了新技能教育孩子！");
                    }
                }
                else
                {
                    passObj.state.canEducate = false;
                }
            }

            private static empoyerAction getWorkCondition(ref Random rm, Data data)
            {
                var employerActions = new List<short>();
                for (var i = 0; i < data.employerActions.Count; i++)
                {
                    for (var j = 0; j < data.employerActions[i].Count; j++)
                    {
                        employerActions.Add(data.employerActions[i][j]);
                    }
                }
                if (employerActions.Count == 0)
                {
                    return empoyerAction.@null;
                }
                else
                {
                    var employerActionNumber = employerActions[rm.Next(0, employerActions.Count)];
                    return getEmployerActionByNum(employerActionNumber);
                }
            }

            private static void setChildrenScript(empoyerAction condition, ref PassObjToServer_Employee passObj, ref Random rm, Data data)
            {
                if (
                    (passObj.state.firstBabyAge.HasValue && passObj.state.firstBabyAge <= 18) &&
                    (passObj.state.secondBabyAge.HasValue && passObj.state.secondBabyAge <= 18) &&
                    (passObj.state.thirdBabyAge.HasValue && passObj.state.thirdBabyAge <= 18) &&
                    (passObj.state.fourthBabyAge.HasValue && passObj.state.fourthBabyAge <= 18)
                    )
                {
                    var employerAction = condition;
                    if (passObj.state.firstBabyAge.HasValue && passObj.state.firstBabyAge <= 18)
                    {
                        setChildScript(employerAction, 1, passObj.state.firstBabyAge.Value, ref passObj, ref rm, data);
                    }
                    if (passObj.state.secondBabyAge.HasValue && passObj.state.secondBabyAge <= 18)
                    {
                        setChildScript(employerAction, 2, passObj.state.secondBabyAge.Value, ref passObj, ref rm, data);
                    }
                    if (passObj.state.thirdBabyAge.HasValue && passObj.state.thirdBabyAge <= 18)
                    {
                        setChildScript(employerAction, 3, passObj.state.thirdBabyAge.Value, ref passObj, ref rm, data);
                    }
                    if (passObj.state.fourthBabyAge.HasValue && passObj.state.fourthBabyAge <= 18)
                    {
                        setChildScript(employerAction, 4, passObj.state.fourthBabyAge.Value, ref passObj, ref rm, data);
                    }

                }
                //var baseScript = rm.Next(90, 100);
                //int addValue = 0;
                ////for()



                //else
                //{

                //    switch (employerAction)
                //    {
                //        case empoyerAction._996:
                //            {
                //                // addValue=-rm.Next()
                //            }; break;
                //    }
                //}
            }



            private static void setChildScript(empoyerAction employerAction, int babyIndex, int babyAgeValue, ref PassObjToServer_Employee passObj, ref Random rm, Data data)
            {
                var strive = passObj.actions.Contains("Employee-Strive");
                var singleWork = passObj.actions.Contains("Employee-SingleWork");
                var playWithChildren = passObj.actions.Contains("Employee-PlayWithChildren");

                double harm;
                switch (employerAction)
                {
                    case empoyerAction._996:
                        {
                            harm = (50.0 + (strive ? 10.0 : 0.0) - (playWithChildren ? 10 : 0)) * (singleWork ? 0.5 : 1);
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "在职场里，资本家鼓吹996是福报，你个人还打了鸡血似的奋斗。但在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家鼓吹996是福报，你个人还打了鸡血似的奋斗。而且家长只顾上班，无人照顾孩子，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家鼓吹996是福报，而你却抽时间陪伴孩子。而且在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家鼓吹996是福报，而你却抽时间陪伴孩子。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家鼓吹996是福报。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家鼓吹996是福报。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。"
                        };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);
                        }; break;
                    case empoyerAction.onlyYounger:
                        {
                            harm = (40.0 + (strive ? 10.0 : 0.0) - (playWithChildren ? 10 : 0)) * (singleWork ? 0.5 : 1);
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "在职场里，资本家只招年轻人，恶性竞争，你个人还打了鸡血似的奋斗。但在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家只招年轻人，恶性竞争，你个人还打了鸡血似的奋斗。而且家长只顾上班，无人照顾孩子，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家只招年轻人，恶性竞争，而你却抽时间陪伴孩子。而且在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家只招年轻人，恶性竞争，而你却抽时间陪伴孩子。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家只招年轻人，恶性竞争。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家只招年轻人，恶性竞争。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。"
                            };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);

                        }; break;
                    case empoyerAction.discriminateWomen:
                        {
                            harm = (40.0 + (strive ? 10.0 : 0.0) - (playWithChildren ? 10 : 0)) * (singleWork ? 0.1 : 1);
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "在职场里，资本家歧视就业的女性，就业环境恶略，你个人还打了鸡血似的奋斗。但在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歧视就业的女性，就业环境恶略，你个人还打了鸡血似的奋斗。而且家长只顾上班，无人照顾孩子，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歧视就业的女性，就业环境恶略，而你却抽时间陪伴孩子。而且在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歧视就业的女性，就业环境恶略，而你却抽时间陪伴孩子。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歧视就业的女性，就业环境恶略。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歧视就业的女性，就业环境恶略。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。"
                            };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);
                        }; break;
                    case empoyerAction.newTecnology_Success:
                        {
                            harm = (20.0 + (strive ? 10.0 : 0.0) - (playWithChildren ? 10 : 0)) * (singleWork ? 0.5 : 1);
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "在职场里，资本家采用了新技术并成功了，机会遇上你的奋斗。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并成功了，机会遇上你的奋斗。家长只顾上班，照顾孩子有不周的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并成功了，你抽时间陪伴孩子。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并成功了，你抽时间陪伴孩子。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并成功了。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并成功了。由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。"
                            };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);
                        }; break;
                    case empoyerAction.newTecnology_Failure:
                        {
                            harm = (30.0 + (strive ? 10.0 : 0.0) - (playWithChildren ? 10 : 0)) * (singleWork ? 0.3 : 1);
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "在职场里，资本家采用了新技术并失败了，你的奋斗没有遇到机会。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并失败了，你的奋斗没有遇到机会。家长只顾上班，照顾孩子有不周的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并失败了，但你抽时间陪伴了孩子。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并失败了，你抽时间陪伴了孩子。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并失败了。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家采用了新技术并失败了。由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。"
                            };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);
                        }; break;
                    case empoyerAction.goAway:
                        {
                            harm = (20.0 + (strive ? 10.0 : 0.0) - (playWithChildren ? 10 : 0)) * (singleWork ? 0.6 : 1);
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "在职场里，资本家进行了产业转移，你的奋斗没有遇到机会。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家进行了产业转移，你的奋斗没有遇到机会。家长只顾上班，照顾孩子有不周的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家进行了产业转移，但你抽时间陪伴了孩子。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家进行了产业转移，你抽时间陪伴了孩子。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家进行了产业转移。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家进行了产业转移。由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。"
                            };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);
                        }; break;
                    case empoyerAction.improveWelfare:
                        {

                            harm = (10.0 + (strive ? 5.0 : 0.0) - (playWithChildren ? 5 : 0)) * (singleWork ? 0.5 : 1);
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "在职场里，资本家提高了福利，但你仍在奋斗。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家提高了福利，但你仍在奋斗。家长只顾上班，照顾孩子有不周的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家提高了福利，同时你抽时间陪伴了孩子。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家提高了福利，同时你抽时间陪伴了孩子。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家提高了福利。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家提高了福利。由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。"
                            };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);

                        }; break;
                    case empoyerAction.stopBusiness:
                        {
                            harm = (15.0 + (strive ? 10.0 : 0.0) - (playWithChildren ? 10 : 0)) * (singleWork ? 0.6 : 1);
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "在职场里，资本家都歇业了，你的奋斗给谁看？在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家都歇业了，你的奋斗给谁看？。家长只顾上班，照顾孩子有不周的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歇业了，你抽时间陪伴了孩子。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歇业了，你抽时间陪伴了孩子。但由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歇业了。在家庭里，一个工作一个顾家，您的第{0}个孩子获得了{1}的情操值。",
                                "在职场里，资本家歇业了。由于夫妻双方都上班，孩子还是有照顾不到的地方，您的第{0}个孩子获得了{1}的情操值。"
                            };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);
                        }; break;
                    case empoyerAction.@null:
                        {
                            harm = (18.0 + (strive ? 10.0 : 0.0) - (playWithChildren ? 10 : 0)) * 1;
                            var scriptValue = getScriptResult(harm, ref rm);
                            var materrial = new string[]
                            {
                                "现在连工作都找不到，你的奋斗给谁看？您的第{0}个孩子获得了{1}的情操值。",
                                "现在连工作都找不到，你的奋斗给谁看？。您的第{0}个孩子获得了{1}的情操值。",
                                "现在连工作都找不到，你花时间陪伴了孩子。您的第{0}个孩子获得了{1}的情操值。",
                                "现在连工作都找不到，你花时间陪伴了孩子。您的第{0}个孩子获得了{1}的情操值。",
                                "现在连工作都找不到。您的第{0}个孩子获得了{1}的情操值。",
                                "现在连工作都找不到。您的第{0}个孩子获得了{1}的情操值。"
                            };
                            var msg = getChildScriptFormat(materrial, strive, singleWork, playWithChildren, babyIndex, scriptValue);
                            passObj.notifyMsgs.Add(msg);
                            passObj.state.spiritScore[babyIndex].Add(scriptValue);
                        }; break;

                }
                //// if()
                //var baseScript = rm.Next(90, 100);
                //int addValue = 0;
                ////for()
                //var employerActions = new List<short>();
                //for (var i = 0; i < data.employerActions.Count; i++)
                //{
                //    for (var j = 0; j < data.employerActions[i].Count; j++)
                //    {
                //        employerActions.Add(data.employerActions[i][j]);
                //    }
                //}
                //if (employerActions.Count == 0)
                //{

                //}
                //else
                //{
                //    var employerActionNumber = employerActions[rm.Next(0, employerActions.Count)];
                //    var employerAction = getEmployerActionByNum(employerActionNumber);
                //    switch (employerAction)
                //    {
                //        case empoyerAction._996:
                //            {
                //                // passObj.notifyMsgs.Add("在你打工过程中，遇到了黑心老板鼓吹996是福报，没有陪伴");


                //            }; break;
                //    }
                //}
                //{


                //    //职业状况对孩子情操的影响
                //}
                //{
                //    //单职 +10
                //    //如果为负，伤害减半
                //}
                //{
                //    //亲子活动
                //    //+10
                //}
                ////throw new NotImplementedException();
            }

            private static string getChildScriptFormat(string[] format, bool strive, bool singleWork, bool playWithChildren, int babyIndex, int scriptValue)
            {
                if (strive)
                {
                    if (singleWork)
                    {
                        return string.Format(format[0], getBabyIndex(babyIndex), scriptValue);
                    }
                    else
                    {
                        return string.Format(format[1], getBabyIndex(babyIndex), scriptValue);
                    }
                }
                else if (playWithChildren)
                {
                    if (singleWork)
                    {
                        return string.Format(format[2], getBabyIndex(babyIndex), scriptValue);
                    }
                    else
                    {
                        return string.Format(format[3], getBabyIndex(babyIndex), scriptValue);
                    }
                }
                else
                {
                    if (singleWork)
                    {
                        return string.Format(format[4], getBabyIndex(babyIndex), scriptValue);
                    }
                    else
                    {
                        return string.Format(format[5], getBabyIndex(babyIndex), scriptValue);
                    }
                }
            }

            private static int getScriptResult(double harm, ref Random rm)
            {
                var score = 99;
                for (int i = 0; i < 2; i++)
                {
                    var s = rm.Next(Convert.ToInt32(100 - harm), 100);
                    score = Math.Min(score, s);
                }
                return score;
            }

            private static empoyerAction getEmployerActionByNum(short num)
            {
                switch (num)
                {
                    case 1:
                        {
                            return empoyerAction._996;
                        };
                    case 2:
                        {
                            return empoyerAction.onlyYounger;
                        };
                    case 3:
                        {
                            return empoyerAction.discriminateWomen;
                        };
                    case 4:
                        {
                            return empoyerAction.newTecnology_Success;
                        };
                    case 5:
                        {
                            return empoyerAction.newTecnology_Failure;
                        };
                    case 6:
                        {
                            return empoyerAction.goAway;
                        };
                    case 7:
                        {
                            return empoyerAction.improveWelfare;
                        };
                    case 8:
                        {
                            return empoyerAction.stopBusiness;
                        };
                    default:
                        {
                            return empoyerAction.@null;
                        }
                }
            }

            private static void setChildEducate(int babyAgeValue, int babyIndex, ref PassObjToServer_Employee passObj, ref Random rm, Data data)
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

            private static void setEducateResult(int selectCondition, int childIndex, ref Random rm, ref PassObjToServer_Employee passObj)
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

            private static void educationPay(double price, int babyIndex, double selfPay, ref PassObjToServer_Employee passObj)
            {
                price = Math.Round(price, 2);
                passObj.state.educationCost[babyIndex].Add(price);
                passObj.state.sumSave -= (price * selfPay);
                passObj.state.sumSave = Math.Round(passObj.state.sumSave);
            }

            private static void addNotifyMsgOfEdution(double govementPay, double selfPay, int babyIndex, double price, string edu, ref PassObjToServer_Employee passObj)
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

            private static void addNotifyMsgOfEduReplaceBy(double govementPay, double selfPay, int babyIndex, double price, string replaceItem, string replaceBy, ref PassObjToServer_Employee passObj)
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


            private static string BirthhBaby(int babyIndex, ref Random rm, Data data, Server.Command c, ref PassObjToServer_Employee passObj)
            {
                var govementPosition = data.govementPosition.Last();
                var successLimet = 2;

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

                    setCanPlayWithChildren(ref passObj);

                    passObj.state.educationCost.Add(babyIndex, new List<double>());
                    passObj.state.educationScore.Add(babyIndex, new List<int>());
                    passObj.state.spiritScore.Add(babyIndex, new List<int>());
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
                else
                {
                    passObj.notifyMsgs.Add($"你生第一个孩子的时候，受孕不成功！！");
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
            }

            private static void setCanPlayWithChildren(ref PassObjToServer_Employee passObj)
            {
                if (passObj.state.firstBabyAge != null && passObj.state.firstBabyAge < 18)
                {
                    passObj.state.canPlayWithChildren = true;
                }
                else if (passObj.state.secondBabyAge != null && passObj.state.secondBabyAge < 18)
                {
                    passObj.state.canPlayWithChildren = true;
                }
                else if (passObj.state.thirdBabyAge != null && passObj.state.thirdBabyAge < 18)
                {
                    passObj.state.canPlayWithChildren = true;
                }
                else if (passObj.state.fourthBabyAge != null && passObj.state.fourthBabyAge < 18)
                {
                    passObj.state.canPlayWithChildren = true;
                }
                else
                {
                    passObj.state.canPlayWithChildren = false;
                }
            }

            private static void DealWithWork(empoyerAction condition, ref Random rm, Data data, Server.Command c, ref PassObjToServer_Employee passObj)
            {
                var govementPosition = data.govementPosition.Last();
                double salary = Math.Cos(govementPosition / 100 * Math.PI) * 0.5 + 1;//
                var singleWork = passObj.actions.Contains("Employee-SingleWork");
                switch (condition)
                {
                    case empoyerAction._996:
                        {
                            if (passObj.actions.Contains("Employee-Strive"))
                            {
                                salary *= (singleWork ? 1.2 : 2.3);
                                string msgFomat = "你去年在打工中，老板说：“996是福报！”。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-Love"))
                            {
                                salary *= 0.8;
                                string msgFomat = "你去年在打工中，老板特别对你说：“996是福报！年轻人应该把重点放在事业上！”。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-PlayWithChildren"))
                            {
                                salary *= (singleWork ? 0.8 : 1.4);
                                string msgFomat = "你去年在打工中，老板特别对你说：“996是福报！作为员工，应该把注意力放在事业上！”。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else
                            {
                                salary *= (singleWork ? 0.9 : 1.5);
                                string msgFomat = "你去年在打工中，老板特别对你说：“996是福报！不奋斗怎么获得福报”。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                        }; break;
                    case empoyerAction.onlyYounger:
                        {
                            if (passObj.actions.Contains("Employee-Strive"))
                            {
                                salary *= (singleWork ? 1.3 : 2.5);
                                string msgFomat = "你去年在打工中，老板只招年轻人干活。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-Love"))
                            {
                                salary *= 0.7;
                                string msgFomat = "你去年在打工中，老板只招年轻人干活，你因为谈恋爱活没干好，被老板借故骂了一顿。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-PlayWithChildren"))
                            {
                                salary *= (singleWork ? 0.7 : 1.2);
                                string msgFomat = "你去年在打工中，老板只招年轻人干活，你不加班，陪伴家人，被老板借故骂了一顿。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else
                            {
                                salary *= (singleWork ? 0.9 : 1.6);
                                string msgFomat = "你去年在打工中，老板只招年轻人，还歧视大龄员工。老板还对大家说“混日子的不是我兄弟！”。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                        }; break;
                    case empoyerAction.discriminateWomen:
                        {
                            if (passObj.actions.Contains("Employee-Strive"))
                            {
                                salary *= (singleWork ? 1.4 : 2.4);
                                string msgFomat = "你去年在打工中，公司文化里歧视女性员工。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-Love"))
                            {
                                salary *= 0.6;
                                string msgFomat = "你去年在打工中，公司文化里歧视女性员工，招工一般只招。老板还教导你说“年轻人应该把重点放在事业上！”。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-PlayWithChildren"))
                            {
                                salary *= (singleWork ? 0.9 : 1.3);
                                string msgFomat = "你去年在打工中，老板很少招女性员工，只招男性！老板还教导你说“员工应该把重点放在事业上！”。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else
                            {
                                salary *= (singleWork ? 0.9 : 1.4);
                                string msgFomat = "你去年在打工中，老板很少招女性员工，只招男性。老板还对大家说“混日子的不是我兄弟！”。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }

                        }; break;
                    case empoyerAction.newTecnology_Success:
                        {
                            if (passObj.actions.Contains("Employee-Strive"))
                            {
                                salary *= (singleWork ? 2 : 3.8);
                                string msgFomat = "所谓成功，就是努力的你遇到了好运气，去年的奋斗和公司新技术的成功使你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-Love"))
                            {
                                salary *= 1.2;
                                string msgFomat = "去年，公司新技术的研发成功使你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-PlayWithChildren"))
                            {
                                salary *= (singleWork ? 1.2 : 2.3);
                                string msgFomat = "去年，公司新技术的研发成功使你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else
                            {
                                salary *= (singleWork ? 1.2 : 2.3);
                                string msgFomat = "去年，公司新技术的研发成功使你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                        }; break;
                    case empoyerAction.newTecnology_Failure:
                        {
                            if (passObj.actions.Contains("Employee-Strive"))
                            {
                                salary *= (singleWork ? 1.5 : 2.8);
                                string msgFomat = "去年的你奋斗了，但公司新技术的研发没能成功。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-Love"))
                            {
                                salary *= 1;
                                string msgFomat = "去年，公司新技术的研发没能成功。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-PlayWithChildren"))
                            {
                                salary *= (singleWork ? 1 : 1.8);
                                string msgFomat = "去年，公司新技术的研发没能成功。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else
                            {
                                salary *= (singleWork ? 1 : 1.8);
                                string msgFomat = "去年，公司新技术的研发没能成功。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                        }; break;
                    case empoyerAction.goAway:
                        {
                            if (passObj.actions.Contains("Employee-Strive"))
                            {
                                salary *= (singleWork ? 1 : 1.8);
                                string msgFomat = "老板正在转移产业。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-Love"))
                            {
                                salary *= 1;
                                string msgFomat = "老板正在转移产业。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-PlayWithChildren"))
                            {
                                salary *= (singleWork ? 1 : 1.8);
                                string msgFomat = "老板正在转移产业。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else
                            {
                                salary *= (singleWork ? 1 : 1.8);
                                string msgFomat = "老板正在转移产业。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                        }; break;
                    case empoyerAction.improveWelfare:
                        {
                            if (passObj.actions.Contains("Employee-Strive"))
                            {
                                salary *= (singleWork ? 1.6 : 3);
                                string msgFomat = "老板提高了福利。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-Love"))
                            {
                                salary *= 1.4;
                                string msgFomat = "老板提高了福利。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-PlayWithChildren"))
                            {
                                salary *= (singleWork ? 1.3 : 2.5);
                                string msgFomat = "老板提高了福利。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else
                            {
                                salary *= (singleWork ? 1.3 : 2.4);
                                string msgFomat = "老板提高了福利。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                        }; break;
                    case empoyerAction.stopBusiness:
                        {
                            if (passObj.actions.Contains("Employee-Strive"))
                            {
                                salary *= (singleWork ? 0.6 : 1);
                                string msgFomat = "老板都歇业了，你只能打零工，你奋斗顶啥用？你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-Love"))
                            {
                                salary *= 0.6;
                                string msgFomat = "老板歇业了，你只能打零工。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else if (passObj.actions.Contains("Employee-PlayWithChildren"))
                            {
                                salary *= (singleWork ? 0.6 : 1);
                                string msgFomat = "老板歇业了，你只能打零工。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                            else
                            {
                                salary *= (singleWork ? 0.6 : 1);
                                string msgFomat = "老板歇业了，你只能打零工。你获得了{0}金币。";
                                setDealWithWorkResult(salary, ref passObj, msgFomat);
                            }
                        }; break;
                    case empoyerAction.@null:
                        {
                            salary *= 0;
                            string msgFomat = "现在都找不到工作。你获得了{0}金币。";
                            setDealWithWorkResult(salary, ref passObj, msgFomat);
                        }; break;

                }




                {
                    //恋爱、政府、企业主对打工收入的影响。
                }
                //passObj.notifyMsgs.Add($"你去年在打工中，获得了{salary.ToString("f2")}金币。");
            }

            private static void setDealWithWorkResult(double salary, ref PassObjToServer_Employee passObj, string msgFomat)
            {
                salary = Math.Round(salary, 2);
                passObj.state.sumSave += salary;
                passObj.state.sumSave = Math.Round(passObj.state.sumSave, 2);
                passObj.notifyMsgs.Add(string.Format(msgFomat, salary.ToString("f2")));
                //passObj.notifyMsgs.Add($"你去年在打工中，老板说：“996是福报！”。你获得了{salary.ToString("f2")}金币。");
            }

            private static string Marry(ref Random rm, Data data, Server.Command c, ref PassObjToServer_Employee passObj)
            {
                if (passObj.state.canBeMarried)
                {
                    var govementPosition = data.govementPosition.Last();
                    double dowry;
                    if (govementPosition < 50)
                    {
                        dowry = (Math.Cos(govementPosition / 100 * Math.PI) * 2 + 1) * data.housePrice;

                    }
                    else
                    {
                        dowry = (Math.Cos(govementPosition / 100 * Math.PI) * 1 + 1) * data.housePrice;
                    }
                    dowry = Math.Round(dowry, 2);
                    var cost = data.housePrice * 2;
                    cost = Math.Round(cost, 2);
                    passObj.state.sumSave -= cost;
                    passObj.notifyMsgs.Add($"彩礼和房价总共花费{cost.ToString("f2")}金币！");


                    passObj.state.sumSave += dowry;
                    passObj.notifyMsgs.Add($"你获得了{dowry.ToString("f2")}金币作为长辈对你结婚的祝福！");



                    passObj.state.canBeMarried = false;
                    passObj.state.canGetFirstBaby = true;
                    passObj.state.canSingleWork = true;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
                else
                {
                    return "";
                }
            }

            /// <summary>
            /// 打工者的恋爱成功率
            /// </summary>
            /// <param name="rm"></param>
            /// <param name="data"></param>
            /// <param name="c"></param>
            /// <param name="passObj"></param>
            /// <returns></returns>
            private static string Love(empoyerAction condition, ref Random rm, Data data, Server.Command c, ref PassObjToServer_Employee passObj)
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
                // var successLimet = Math.Cos(govementPosition / 100 * Math.PI) * 0.375 + 0.425;
                double A = 200, B = 0, C = 0.375, D = 0.425;
                switch (condition)
                {
                    case empoyerAction._996:
                        {
                            A = 200;
                            B = 0;
                            C = 0.6;
                            D = 0.1;
                        }; break;
                    case empoyerAction.onlyYounger:
                        {
                            A = 200;
                            B = 31.83099;
                            C = 0.385;
                            D = 0.425;
                        }; break;
                    case empoyerAction.discriminateWomen:
                        {
                            A = 200;
                            B = 0;
                            C = 0.5;
                            D = 0.1;
                        }; break;
                    case empoyerAction.newTecnology_Success:
                        {
                            A = 200;
                            B = 0;
                            C = 0.375;
                            D = 0.475;
                        }; break;
                    case empoyerAction.newTecnology_Failure:
                        {
                            A = 200;
                            B = 0;
                            C = 0.375;
                            D = 0.375;
                        }; break;
                    case empoyerAction.goAway:
                        {
                            A = 200;
                            B = 0;
                            C = 0.475;
                            D = 0.375;
                        }; break;
                    case empoyerAction.improveWelfare:
                        {
                            A = 200;
                            B = 0;
                            C = 0.375;
                            D = 0.525;
                        }; break;
                    case empoyerAction.stopBusiness:
                        {
                            A = 200;
                            B = 0;
                            C = 0.375;
                            D = 0.275;
                        }; break;
                    case empoyerAction.@null:
                        {
                            A = 200;
                            B = 0;
                            C = 0.375;
                            D = 0.425;
                        }; break;
                }
                var successLimet = Math.Cos(govementPosition / A * Math.PI + B) * C + D;
                var sumActionCount = data.employerActions.Sum(item => item.Count);
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



        public void setAsEmployer()
        {
            if (hasRole)
            { }
            else
            {
                hasRole = true;
                this.msg = "";
                this.role = "employer";
                this.employerObj = new Employer();

                List<string> msgs = new List<string>();

                //msgs.Add(this.employerObj.ageDisplay);
                msgs.Add(this.employerObj.yearDisplay);
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
                        yearDisplay = this.employerObj.yearDisplay,
                        state = this.employerObj.state,
                        actions = this.employerObj.actions,
                    })

                };
                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            }
        }
        Employer employerObj = null;
        public class Employer
        {
            public class State
            {
                internal int newTecnologySuccessProtectCount;

                public bool can996 { get; set; }
                public bool onlyYounger { get; set; }
                public bool discriminateWomen { get; set; }
                public bool newTecnology { get; set; }
                public bool goAway { get; set; }
                public bool improveWelfare { get; set; }
                public bool stopBusiness { get; set; }

                public double sumSave { get; set; }

                public int year { get; set; }

                public bool gameOver { get; set; }


            }

            internal State state { get; set; }
            public List<string> actions { get; set; }

            public int year { get { return this.state.year; } }
            public Employer()
            {
                this.state = new State()
                {
                    year = 0,
                    can996 = true,
                    discriminateWomen = true,
                    onlyYounger = true,
                    newTecnology = true,
                    goAway = false,
                    gameOver = false,
                    improveWelfare = false,
                    stopBusiness = false,
                    sumSave = 1
                };
                this.actions = new List<string>();
            }

            internal string yearDisplay
            {
                get
                { return $"你有幸成为了资本家！这是你成为资本家的第{(this.year + 1)}年"; }
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
                    Type = "Employer",
                    JsonValue = JsonValue
                });
            }

            internal static string DoAction(ref Random rm, Data data, Server.Command c)
            {
                PassObjToServer_Employer passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObjToServer_Employer>(c.JsonValue);

                if (passObj.state.gameOver)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                }
                passObj.state.year++;

                var condition = getWorkCondition(ref rm, data);

                //  double addPercent = 1;


                var checkOrder = new List<string>()
                {
                    "Employer-996",
                    "Employer-OnlyYounger",
                    "Employer-DiscriminateWomen",
                  //  "Employer-NewTecnology",
                    "Employer-GoAway",
                    "Employer-ImproveWelfare",
                    "Employer-StopBusiness"
                };
                var operateT = DateTime.Now.GetHashCode().ToString();
                checkOrder = checkOrder.OrderBy(item => (item + operateT).GetHashCode()).ToList();
                checkOrder.Insert(0, "Employer-NewTecnology");

                var govPosition = data.govementPosition.Last();


                double addValue = 0;
                double lossValue = 0;
                for (int i = 0; i < checkOrder.Count; i++)
                {
                    switch (checkOrder[i])
                    {
                        case "Employer-996":
                            {
                                if (passObj.actions.Contains(checkOrder[i]))
                                {
                                    var successCount = 10 - 0.091 * govPosition;
                                    if (condition.Count(item => item == emploeeAction.strive) > successCount)
                                    {
                                        var itemAdd = Math.Round(passObj.state.sumSave * (data.businessRate - 1), 2);
                                        itemAdd = Math.Max(itemAdd, 0.01);
                                        passObj.notifyMsgs.Add($"你实行了996，员工没有带头造反的，你获得了{itemAdd.ToString("f2")}金币。");
                                        addValue += itemAdd;
                                    }
                                    else
                                    {
                                        if (passObj.state.newTecnologySuccessProtectCount > 0)
                                        {
                                            passObj.notifyMsgs.Add($"你实行了996，有部分员工走了。");
                                            passObj.state.newTecnologySuccessProtectCount--;
                                            passObj.notifyMsgs.Add($"你的明星企业家光环保护-1，变为{passObj.state.newTecnologySuccessProtectCount}！");
                                        }
                                        else
                                        {
                                            var lossItem = Math.Round(passObj.state.sumSave * (1 - 1 / data.businessRate), 2);
                                            passObj.notifyMsgs.Add($"你实行了996，员工对开始造反了，你损失了{lossItem.ToString("f2")}金币。");
                                            lossValue -= lossItem;
                                        }
                                    }
                                }
                            }; break;
                        case "Employer-OnlyYounger":
                            {
                                if (passObj.actions.Contains(checkOrder[i]))
                                {
                                    var successCount = 13 - 0.081 * govPosition;
                                    if (condition.Count(item => item == emploeeAction.enjoyTime) > successCount)
                                    {
                                        var itemAdd = Math.Round(passObj.state.sumSave * (data.businessRate - 1), 2);
                                        itemAdd = Math.Max(itemAdd, 0.01);
                                        passObj.notifyMsgs.Add($"你排挤大龄员工，没人搞你，你获得了{itemAdd.ToString("f2")}金币。");
                                        addValue += itemAdd;
                                    }
                                    else
                                    {
                                        if (passObj.state.newTecnologySuccessProtectCount > 0)
                                        {
                                            passObj.notifyMsgs.Add($"有屁民诽谤你：“排挤大龄员工。”");
                                            passObj.state.newTecnologySuccessProtectCount--;
                                            passObj.notifyMsgs.Add($"你的明星企业家光环保护-1，变为{passObj.state.newTecnologySuccessProtectCount}！");
                                        }
                                        else
                                        {
                                            var lossItem = Math.Round(passObj.state.sumSave * (1 - 1 / data.businessRate), 2);
                                            passObj.notifyMsgs.Add($"你排挤大龄员工，部分人去劳动局告你了，你损失了{lossItem.ToString("f2")}金币。");
                                            lossValue -= lossItem;
                                        }
                                    }
                                }
                            }; break;
                        case "Employer-DiscriminateWomen":
                            {
                                if (passObj.actions.Contains(checkOrder[i]))
                                {
                                    var successCount = 12 - 0.111 * govPosition;
                                    if (condition.Count(item => item == emploeeAction.singleWork) > successCount)
                                    {
                                        var itemAdd = Math.Round(passObj.state.sumSave * (data.businessRate - 1), 2);
                                        itemAdd = Math.Max(itemAdd, 0.01);
                                        passObj.notifyMsgs.Add($"你招工时，只招男的，没人举报你，你获得了{itemAdd.ToString("f2")}金币。");
                                        addValue += itemAdd;
                                    }
                                    else
                                    {
                                        if (passObj.state.newTecnologySuccessProtectCount > 0)
                                        {
                                            passObj.notifyMsgs.Add($"有屁民流传你：“你招工时，只招男员工。”");
                                            passObj.state.newTecnologySuccessProtectCount--;
                                            passObj.notifyMsgs.Add($"你的明星企业家光环保护-1，变为{passObj.state.newTecnologySuccessProtectCount}！");
                                        }
                                        else
                                        {
                                            var lossItem = Math.Round(passObj.state.sumSave * (1 - 1 / data.businessRate), 2);
                                            passObj.notifyMsgs.Add($"你招工时，只招男的，你被人举报歧视女性员工，你损失了{lossItem.ToString("f2")}金币。");
                                            lossValue -= lossItem;
                                        }
                                    }
                                }
                            }; break;
                        case "Employer-NewTecnology":
                            {
                                if (passObj.actions.Contains(checkOrder[i]))
                                {
                                    //  var successCount = 12 - 0.111 * govPosition;
                                    var succss = 0.35 - 0.0015 * govPosition;
                                    if (rm.NextDouble() < succss)
                                    {
                                        var itemAdd = Math.Round(passObj.state.sumSave * (data.businessRate - 1) * 2, 2);
                                        passObj.notifyMsgs.Add($"你研发新技术成功了,获得了{itemAdd.ToString("f2")}金币！");
                                        passObj.state.newTecnologySuccessProtectCount = 3;
                                        passObj.notifyMsgs.Add($"您获得了明星企业家光环保护{passObj.state.newTecnologySuccessProtectCount}！");
                                    }
                                    else
                                    {
                                        var itemLoss = Math.Round(passObj.state.sumSave * (1 - 1 / data.businessRate), 2);
                                        passObj.notifyMsgs.Add($"你研发新技术失败了，损失了{itemLoss.ToString("f2")}金币！");
                                    }
                                }
                            }; break;


                    }
                }

                if (passObj.state.newTecnologySuccessProtectCount > 0)
                {
                    passObj.state.newTecnologySuccessProtectCount--;
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(passObj);


            }

            private static List<emploeeAction> getWorkCondition(ref Random rm, Data data)
            {
                var employeeActions = new List<short>();
                for (var i = 0; i < data.employteenActions.Count; i++)
                {
                    for (var j = 0; j < data.employteenActions[i].Count; j++)
                    {
                        employeeActions.Add(data.employteenActions[i][j]);
                    }
                }

                while (employeeActions.Count < 100)
                {
                    employeeActions.Add(Convert.ToInt16(rm.Next(0, 3)));
                }

                Dictionary<int, bool> indexSelected = new Dictionary<int, bool>();

                var employeeActionsOfSelect = new List<emploeeAction>();
                while (employeeActionsOfSelect.Count < 15)
                {
                    var indexRm = rm.Next(employeeActions.Count);
                    if (indexSelected.ContainsKey(indexRm))
                    {
                        continue;
                    }
                    else
                    {
                        indexSelected.Add(indexRm, true);
                        switch (employeeActions[indexRm])
                        {
                            case 0:
                                {
                                    employeeActionsOfSelect.Add(emploeeAction.normal);
                                }; break;
                            case 1:
                                {
                                    employeeActionsOfSelect.Add(emploeeAction.strive);
                                }; break;
                            case 2:
                                {
                                    employeeActionsOfSelect.Add(emploeeAction.enjoyTime);
                                }; break;
                        }

                    }
                }
                return employeeActionsOfSelect;
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
                    case "Employer":
                        {
                            return Employer.DoAction(ref rm, data, c);
                        }; break;
                    default:
                        {
                            return "";
                        }

                }
                //var passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObjToServer_Employee>(c.JsonValue);
                //throw new NotImplementedException();
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

        public class PassObjToServer_Employee
        {
            public Employee.State state { get; set; }
            public List<string> actions { get; set; }

            public List<string> notifyMsgs { get; set; }
        }

        public class PassObjToServer_Employer
        {
            public Employer.State state { get; set; }
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
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Strive");
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
                        else if (this.employerObj != null)
                        {
                            //A对于资本家来说，这是996
                            if (this.employerObj.actions.Contains("Employer-996"))
                            {
                                this.employerObj.actions.RemoveAll(item => item == "Employer-996");
                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions,

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                var canExitAtSameTime = new string[] { "Employer-ImproveWelfare", "Employer-GoAway", "Employer-StopBusiness" };
                                this.employerObj.actions.RemoveAll(item => canExitAtSameTime.Contains(item));
                                this.employerObj.actions.Add("Employer-996");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions

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
                        else if (employerObj != null)
                        {
                            if (this.employerObj.actions.Contains("Employer-OnlyYounger"))
                            {
                                this.employerObj.actions.RemoveAll(item => item == "Employer-OnlyYounger");
                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions,

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                var canExitAtSameTime = new string[] { "Employer-ImproveWelfare", "Employer-StopBusiness", "Employer-GoAway" };
                                this.employerObj.actions.RemoveAll(item => canExitAtSameTime.Contains(item));
                                this.employerObj.actions.Add("Employer-OnlyYounger");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;
                case "C":
                    {
                        if (emplyee != null)
                        {
                            if (this.emplyee.actions.Contains("Employee-BirthFirstBaby"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-BirthFirstBaby");
                            }
                            else
                            {
                                this.emplyee.actions.Add("Employee-BirthFirstBaby");
                            }
                            var p = new
                            {
                                ObjType = $"{this.role}-action",
                                state = this.emplyee.state,
                                actions = this.emplyee.actions,
                                educateAction = this.emplyee.educateAction

                            };
                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                        }
                        else if (employerObj != null)
                        {
                            if (this.employerObj.actions.Contains("Employer-DiscriminateWomen"))
                            {
                                this.employerObj.actions.RemoveAll(item => item == "Employer-DiscriminateWomen");
                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions,

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                var canExitAtSameTime = new string[] { "Employer-GoAway", "Employer-StopBusiness" };
                                this.employerObj.actions.RemoveAll(item => canExitAtSameTime.Contains(item));
                                this.employerObj.actions.Add("Employer-DiscriminateWomen");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;
                case "D":
                    {
                        if (emplyee != null)
                        {
                            if (this.emplyee.actions.Contains("Employee-BirthSecondBaby"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-BirthSecondBaby");
                            }
                            else
                            {
                                this.emplyee.actions.Add("Employee-BirthSecondBaby");
                            }
                            var p = new
                            {
                                ObjType = $"{this.role}-action",
                                state = this.emplyee.state,
                                actions = this.emplyee.actions,
                                educateAction = this.emplyee.educateAction

                            };
                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                        }
                        else if (employerObj != null)
                        {
                            if (this.employerObj.actions.Contains("Employer-NewTecnology"))
                            {
                                this.employerObj.actions.RemoveAll(item => item == "Employer-NewTecnology");
                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions,

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                var canExitAtSameTime = new string[] { "Employer-GoAway", "Employer-StopBusiness" };
                                this.employerObj.actions.RemoveAll(item => canExitAtSameTime.Contains(item));
                                this.employerObj.actions.Add("Employer-NewTecnology");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;
                case "E":
                    {
                        if (emplyee != null)
                        {
                            if (this.emplyee.actions.Contains("Employee-BirthThirdBaby"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-BirthThirdBaby");
                            }
                            else
                            {
                                this.emplyee.actions.Add("Employee-BirthThirdBaby");
                            }
                            var p = new
                            {
                                ObjType = $"{this.role}-action",
                                state = this.emplyee.state,
                                actions = this.emplyee.actions,
                                educateAction = this.emplyee.educateAction

                            };
                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                        }
                        else if (employerObj != null)
                        {
                            if (this.employerObj.actions.Contains("Employer-GoAway"))
                            {
                                this.employerObj.actions.RemoveAll(item => item == "Employer-GoAway");
                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions,

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                var canExitAtSameTime = new string[] { "Employer-996", "Employer-OnlyYounger", "Employer-DiscriminateWomen", "Employer-NewTecnology", "Employer-StopBusiness", "Employer-ImproveWelfare" };
                                this.employerObj.actions.RemoveAll(item => canExitAtSameTime.Contains(item));
                                this.employerObj.actions.Add("Employer-GoAway");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;
                case "F":
                    {
                        if (emplyee != null)
                        {
                            if (this.emplyee.actions.Contains("Employee-BirthFourthBaby"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-BirthFourthBaby");
                            }
                            else
                            {
                                this.emplyee.actions.Add("Employee-BirthFourthBaby");
                            }
                            var p = new
                            {
                                ObjType = $"{this.role}-action",
                                state = this.emplyee.state,
                                actions = this.emplyee.actions,
                                educateAction = this.emplyee.educateAction

                            };
                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                        }
                        else if (employerObj != null)
                        {
                            if (this.employerObj.actions.Contains("Employer-ImproveWelfare"))
                            {
                                this.employerObj.actions.RemoveAll(item => item == "Employer-ImproveWelfare");
                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions,

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                var canExitAtSameTime = new string[] { "Employer-996", "Employer-OnlyYounger", "Employer-DiscriminateWomen", "Employer-GoAway", "Employer-StopBusiness" };
                                this.employerObj.actions.RemoveAll(item => canExitAtSameTime.Contains(item));
                                this.employerObj.actions.Add("Employer-ImproveWelfare");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;
                case "G":
                    {
                        if (emplyee != null)
                        {

                            //A对于打工者来说，只是亲子行为
                            if (this.emplyee.actions.Contains("Employee-PlayWithChildren"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-PlayWithChildren");

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
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Strive");
                                this.emplyee.actions.Add("Employee-PlayWithChildren");

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
                        else if (employerObj != null)
                        {
                            if (this.employerObj.actions.Contains("Employer-StopBusiness"))
                            {
                                this.employerObj.actions.RemoveAll(item => item == "Employer-StopBusiness");
                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions,

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                            else
                            {
                                var canExitAtSameTime = new string[] { "Employer-996", "Employer-OnlyYounger", "Employer-DiscriminateWomen", "Employer-NewTecnology", "Employer-GoAway", "Employer-ImproveWelfare" };
                                this.employerObj.actions.RemoveAll(item => canExitAtSameTime.Contains(item));
                                this.employerObj.actions.Add("Employer-StopBusiness");

                                var p = new
                                {
                                    ObjType = $"{this.role}-action",
                                    state = this.employerObj.state,
                                    actions = this.employerObj.actions

                                };
                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                            }
                        }
                    }; break;
                case "H":
                    {
                        if (emplyee != null)
                        {

                            //A对于打工者来说，只是亲子行为
                            if (this.emplyee.actions.Contains("Employee-SingleWork"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-SingleWork");

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
                                this.emplyee.actions.Add("Employee-SingleWork");

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
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Love" || item == "Employee-PlayWithChildren");
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
                case "J":
                    {
                        if (emplyee != null)
                        {
                            //A对于打工者来说，是使子女得到顶级教育
                            //
                            if (this.emplyee.actions.Contains("Employee-Educate-Perfect"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Educate-Perfect" || item == "Employee-Educate-Good" || item == "Employee-Educate-Common");
                                this.emplyee.actions.Add("Employee-Educate-Common");
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
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Educate-Perfect" || item == "Employee-Educate-Good" || item == "Employee-Educate-Common");
                                this.emplyee.actions.Add("Employee-Educate-Perfect");

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
                case "K":
                    {
                        if (emplyee != null)
                        {
                            //A对于打工者来说，只是恋爱动作
                            if (this.emplyee.actions.Contains("Employee-Educate-Good"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Educate-Perfect" || item == "Employee-Educate-Good" || item == "Employee-Educate-Common");
                                this.emplyee.actions.Add("Employee-Educate-Perfect");
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
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Educate-Perfect" || item == "Employee-Educate-Good" || item == "Employee-Educate-Common");
                                this.emplyee.actions.Add("Employee-Educate-Good");

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
                case "L":
                    {
                        if (emplyee != null)
                        {
                            //A对于打工者来说，只是恋爱动作
                            if (this.emplyee.actions.Contains("Employee-Educate-Common"))
                            {
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Educate-Perfect" || item == "Employee-Educate-Good" || item == "Employee-Educate-Common");
                                this.emplyee.actions.Add("Employee-Educate-Good");
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
                                this.emplyee.actions.RemoveAll(item => item == "Employee-Educate-Perfect" || item == "Employee-Educate-Good" || item == "Employee-Educate-Common");
                                this.emplyee.actions.Add("Employee-Educate-Common");

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
            // bool canGetResult = false;
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
                        // stream.Close();

                        if (string.IsNullOrEmpty(response)) { }
                        else
                        {

                            var PassObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObjToServer_Employee>(response);

                            this.emplyee.actions = new List<string>();
                            this.emplyee.state = PassObj.state;

                            for (int i = 0; i < PassObj.notifyMsgs.Count; i++)
                            {
                                //Console.WriteLine(PassObj.notifyMsgs[i]);
                            }
                            if (PassObj.state.gameOver)
                            {
                                //canGetResult = true;
                                PassObj p = new PassObj()
                                {
                                    ObjType = $"{this.role}-notify-finish",
                                    showContinue = true,
                                    showIsError = true,
                                    //showIsFinished = true,
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
                            else
                            {
                                PassObj p = new PassObj()
                                {
                                    ObjType = $"{this.role}-notify-next",
                                    showContinue = true,
                                    showIsError = true,
                                    //showIsFinished = true,
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
                    }
                    client.Close();
                }
            }
            else if (this.employerObj != null)
            {
                var ip = IpAndPortManager.Population_Ip;
                int port = IpAndPortManager.Population_Port;
                using (TcpClient client = new TcpClient(ip, port))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        //var msg = "{\"Type\":\"Refresh\"}";
                        Byte[] msgData = System.Text.Encoding.UTF8.GetBytes(this.employerObj.ToString());
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
                        // stream.Close();

                        if (string.IsNullOrEmpty(response)) { }
                        else
                        {

                            var PassObj = Newtonsoft.Json.JsonConvert.DeserializeObject<PassObjToServer_Employee>(response);

                            this.emplyee.actions = new List<string>();
                            this.emplyee.state = PassObj.state;

                            for (int i = 0; i < PassObj.notifyMsgs.Count; i++)
                            {
                                //Console.WriteLine(PassObj.notifyMsgs[i]);
                            }
                            if (PassObj.state.gameOver)
                            {
                                PassObj p = new PassObj()
                                {
                                    ObjType = $"{this.role}-notify-finish",
                                    showContinue = true,
                                    showIsError = true,
                                    //showIsFinished = true,
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
                            else
                            {
                                PassObj p = new PassObj()
                                {
                                    ObjType = $"{this.role}-notify-next",
                                    showContinue = true,
                                    showIsError = true,
                                    //showIsFinished = true,
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
                    }
                    client.Close();
                }
            }

        }
    }
}
