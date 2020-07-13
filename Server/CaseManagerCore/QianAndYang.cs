using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CaseManagerCore
{
    public class QianAndYang : CaseManager
    {
        //protected bool error = false;
        //protected Random rm;
        //protected int step = 0;
        //protected string msg = "";
        string filePath = System.Configuration.ConfigurationManager.AppSettings["ProblemPath"] + "YangAndQian.xml";// System.Configuration. @"Xmlfolder\";
        string HashCode = "";
        List<int> orderGet = new List<int>();
        List<string> Problems = new List<string>();
        const int minScore = 98;
        public QianAndYang()
        {
            this.idPrevious = "qhy";
            this.rm = new Random(DateTime.Now.GetHashCode());
            this.rm.Next();
            this.rm.Next();
            this.step = 0;
            PassObj p = new PassObj()
            {
                ObjType = "html",
                msg = $"老师您好，现在是{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")}。我给您讲讲杨振宁和钱学森，你看看对不对，不对您要及时指出我的错误哦！关注要瑞卿@知乎，了解更多。",
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);

            MD5 md5Hash = MD5.Create();
            //using (XElement booksFromFile = XElement.Load(@"books.xml")) { }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            for (int i = 0; i < xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes.Count; i++)
            {
                this.orderGet.Add(i);
            }
            xmlDoc = null;
            var nowT = DateTime.Now;
            orderGet = (from item in orderGet orderby dataToStr(md5Hash.ComputeHash(Encoding.UTF8.GetBytes(item + nowT.ToString("yyyyMMddHHssmm")))) descending select item).ToList();
        }

        private string dataToStr(byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串 
            return sBuilder.ToString();
        }

        //public string GetMsg()
        //{
        //    throw new NotImplementedException();
        //}

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
                if (this.step <= this.orderGet.Count)
                {
                    var indexValue = this.orderGet[this.step - 1];
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);
                    var rightMsg = string.Format(xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes[indexValue]["prmblem"].InnerText,
                      styleText(xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes[indexValue]["answer"]["right"].InnerText));
                    var wrongMsg = string.Format(xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes[indexValue]["prmblem"].InnerText,
                                      styleText(xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes[indexValue]["answer"]["wrong"].InnerText));

                    DealWithRightAndWrong(rightMsg, wrongMsg);
                    xmlDoc = null;

                }
                else if (this.step == this.orderGet.Count + 1)
                {
                    DealWithMsg($"你的最终得分是{this.score}.");
                }
                else if (this.step > this.orderGet.Count + 1 && this.step < 500)
                {
                    if (this.getRewardSuccess)
                    {
                        this.step = 500;
                    }
                    else
                    {
                        if (this.score >= minScore)
                        {
                            InputAddress();
                            this.step = 500;
                        }
                        else
                        {
                            var msg = $"您的得分为{this.score}。小于{minScore}将不能获得勋章，继续努力吧！少年！！！";
                            DealWithMsg(msg);
                            this.step = 500;
                        }
                    }
                }
                else if (this.step >= 500)
                {

                    CloseSocket();
                }
                //if ((this.step - 1) % 2 == 0)
                //{
                //    var indexValue= orderGet
                //}
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(filePath);
                ////xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes

                //XmlDocument

                //if (this.step == 1)
                //{
                //    DealWithMsg($"我先给您讲讲摩尔斯电码，更多信息关注知乎@要瑞卿。请您翻译我的电码信息哈。如果做的不对，我得重新回到知乎，访问知乎@要瑞卿的文章，重新进行学习。");
                //}
                //else if (this.step == 2)
                //{
                //    DealWithMsgSheet("MorseCode");
                //}
                //else if (this.step >= 3 && this.step < 45 && this.step % 2 == 1)
                //{
                //    do
                //    {
                //        this.CurrentIndex = this.rm.Next(0, this.Material.Length);
                //    }
                //    while (this.usedIndex.ContainsKey(this.CurrentIndex));
                //    this.usedIndex.Add(this.CurrentIndex, true);
                //    DealWithMsg2();
                //}
                //else if (this.step >= 3 && this.step < 45 && this.step % 2 == 0)
                //{
                //    var wrongIndex = this.CurrentIndex;
                //    do
                //    {
                //        wrongIndex = this.rm.Next(0, this.Material.Length);
                //    }
                //    while (wrongIndex == this.CurrentIndex);
                //    var rightMsg = $"以上电文翻译成英语，再翻译成汉语，是“{this.Result[this.CurrentIndex]}”。";
                //    var wrongMsg = $"以上电文翻译成英语，再翻译成汉语，是“{this.Result[wrongIndex]}”。";
                //    //  var wrongMsg = this.Result[wrongIndex];
                //    DealWithRightAndWrong(rightMsg, wrongMsg);
                //}
                //else if (this.step == 45)
                //{
                //    DealWithMsg($"你的最终得分是{this.score}.");
                //}
                //else if (this.step > 45 && this.step < 100)
                //{
                //    if (this.getRewardSuccess)
                //    {
                //        this.step = 100;
                //    }
                //    else
                //    {
                //        if (this.score >= minScore)
                //        {
                //            InputAddress();
                //            this.step = 67;
                //        }
                //        else
                //        {
                //            var msg = $"您的得分为{this.score}。小于{minScore}将不能获得勋章，继续努力吧！少年！！！";
                //            DealWithMsg(msg);
                //            this.step = 100;
                //        }
                //    }

                //}
                //else if (this.step >= 100)
                //{

                //    CloseSocket();
                //}
            }
        }

        private string styleText(string innerText)
        {
            return $"<strong  style=\"color:yellow;\">{innerText}</strong>";
        }

        //public void errorRecovery()
        //{
        //    throw new NotImplementedException();
        //}

        public string SaveAddress(string address)
        {
            MysqlCore.BaseItem b = new MysqlCore.BaseItem("qianandyang");
            return this.SaveAddress(address, b.xunzhangName, minScore, b.AddAddressValue);
        }
    }
}
