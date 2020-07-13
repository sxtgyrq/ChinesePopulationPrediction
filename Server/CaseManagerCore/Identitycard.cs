using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using MysqlCore;

namespace CaseManagerCore
{
    public class Identitycard : CaseManager
    {
        class ConfigObj
        {
            public string dic { get; set; }
        }

        enum Sex { boy, girl }
        //Sex sex;
        int[] IDResult;
        int[] IDResultError;
        int current;

        public Identitycard()
        {
            this.current = 0;
            this.IDResult = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            this.IDResultError = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<string> result = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            string configValue;
            configValue = File.ReadAllText("Identitycard.json"); 
            var configObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigObj>(configValue);


            string filePath = configObj.dic + "Identitycard.xml";// System.Configuration. @"Xmlfolder\";
            xmlDoc.Load(filePath);


            this.rm = new Random(DateTime.Now.GetHashCode());
            var v1 = this.rm.Next(2);
            Sex sex;
            string name;
            if (v1 == 0)
            {
                var c = this.rm.Next(0, xmlDoc.ChildNodes[2]["name"]["boy"].ChildNodes.Count);
                var boyName = xmlDoc.ChildNodes[2]["name"]["boy"].ChildNodes[c].InnerText;
                var addMsg = $"男孩的名字叫{boyName}";
                Console.WriteLine(addMsg);
                result.Add(addMsg);
                sex = Sex.boy;
                name = boyName;
                result.Add($"这些信息来自于{boyName}。");
                result.Add($"{boyName}是一名男婴。");
            }
            else
            {
                var c = this.rm.Next(0, xmlDoc.ChildNodes[2]["name"]["girl"].ChildNodes.Count);
                var girlName = xmlDoc.ChildNodes[2]["name"]["girl"].ChildNodes[c].InnerText;
                var addMsg = $"女孩的名字叫{girlName}";
                Console.WriteLine(addMsg);
                result.Add(addMsg);
                sex = Sex.girl;
                result.Add($"这些信息来自于{girlName}。");
                result.Add($"{girlName}是一名女婴。");
                name = girlName;
            }
            XmlNode provinceNode;
            string provinceName;
            string provinceCode;
            {
                var c = this.rm.Next(0, xmlDoc.ChildNodes[2]["province"].ChildNodes.Count);
                provinceNode = xmlDoc.ChildNodes[2]["province"].ChildNodes[c];
                provinceName = provinceNode.Attributes["v"].Value;
                provinceCode = provinceNode.Attributes["c"].Value;
                //result.Add($"出生的省份或直辖市为{provinceName}");
                //result.Add($"{provinceName}行政代码为{code}");
                //Console.WriteLine($"为{provinceName}");
                //Console.WriteLine($"省份或直辖市代码为{code}");
                this.IDResult[0] = int.Parse(provinceCode) / 10;
                this.IDResult[1] = int.Parse(provinceCode) % 10;
            }

            XmlNode region;
            string regionName;
            string regionCode;
            {
                var c = this.rm.Next(0, provinceNode.ChildNodes.Count);
                region = provinceNode.ChildNodes[c];
                regionName = region.Attributes["v"].Value;
                regionCode = region.Attributes["c"].Value;

                this.IDResult[2] = int.Parse(regionCode) / 10;
                this.IDResult[3] = int.Parse(regionCode) % 10;
            }
            {
                result.Add($"{provinceName}{regionName}的行政编号是{provinceCode}{regionCode}");
            }
            XmlNode xian;
            string xianName;
            {
                var c = this.rm.Next(0, region.ChildNodes.Count);
                xian = region.ChildNodes[c];
                xianName = xian.Attributes["v"].Value;
                var xianCode = xian.Attributes["c"].Value;

                Console.WriteLine($"县区为{xianName}");
                Console.WriteLine($"县区代码{xianCode}");

                result.Add($"{xianName}隶属于{provinceName}{regionName}");
                result.Add($"{xianName}的行政编号是{xianCode}");

                this.IDResult[4] = int.Parse(xianCode) / 10;
                this.IDResult[5] = int.Parse(xianCode) % 10;
            }

            XmlNode pcs;
            {
                var c = this.rm.Next(0, xian.ChildNodes.Count);
                pcs = xian.ChildNodes[c];
                var pcsName = pcs.Attributes["v"].Value;
                var pcsCode = pcs.Attributes["c"].Value;

                //Console.WriteLine($"县区为{pcsName}");
                //Console.WriteLine($"县区代码{pcsCode}");
                result.Add($"{name}的出生地归{pcsName}所管辖");
                if (pcsCode == "xx")
                {
                    pcsCode = rm.Next(0, 100).ToString("D2");
                }
                result.Add($"{pcsName}的编码为{pcsCode}");
                result.Add($"{pcsName}属于{xianName}公安局的派出机构。");

                this.IDResult[14] = int.Parse(pcsCode) / 10; ;
                this.IDResult[15] = int.Parse(pcsCode) % 10;
            }
            int days = this.rm.Next(1, 5);
            DateTime birthDay = DateTime.Now.AddDays(-days);
            {

                this.IDResult[6] = (birthDay.Year % 10000) / 1000;
                this.IDResult[7] = (birthDay.Year % 1000) / 100;
                this.IDResult[8] = (birthDay.Year % 100) / 10;
                this.IDResult[9] = (birthDay.Year % 10) / 1;

                this.IDResult[10] = birthDay.Month / 10;
                this.IDResult[11] = birthDay.Month % 10;

                this.IDResult[12] = birthDay.Day / 10;
                this.IDResult[13] = birthDay.Day % 10;
            }
            {
                result.Add($"{name}出生于{birthDay.ToString("yyyy年MM月dd日HH点mm分ss秒")}");
            }
            int order = this.rm.Next(1, 6);
            {
                result.Add($"{name}是今天第{order}个登记的{(sex == Sex.boy ? "男婴" : "女婴")}");

                if (sex == Sex.boy)
                {
                    this.IDResult[16] = order * 2 - 1;
                }
                else
                {
                    this.IDResult[16] = (order * 2) % 10;
                }
            }

            this.IDResult[17] = GetCheckCode();

            result.Add("x代表罗马数字X，代表阿拉伯数字10");
            result.Add("关注要瑞卿@知乎，了解更多");
            result.Add("派出所对于上户口的男婴，一般按照1,3,5,7,9进行排序；女婴，一般按照2,4,6,8,0进行排序");
            result.Add("最后一位是校验码，其应该满足最后一行的结果是1。");

            result.Add("乘积=号码×权重，然后对11进行取余");
            result.Add("纠错，可以改换号码");
            result.Add("继续按钮，可以切换位数；当你修改无误时，点击继续按钮会校验你修改的身份证是否正确。");
            result = (from item in result orderby (item + item.GetHashCode()).GetHashCode() select item).ToList();

            Console.WriteLine($"身份证{this.IDResult[0]}{this.IDResult[1]}{this.IDResult[2]}{this.IDResult[3]}{this.IDResult[4]}{this.IDResult[5]}{this.IDResult[6]}{this.IDResult[7]}{this.IDResult[8]}{this.IDResult[9]}{this.IDResult[10]}{this.IDResult[11]}{this.IDResult[12]}{this.IDResult[13]}{this.IDResult[14]}{this.IDResult[15]}{this.IDResult[16]}{this.IDResult[17]}");
            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine($"{i}_提示消息_{result[i]}");
            }


            // this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { infomations = result });

            PassObj p = new PassObj()
            {
                ObjType = "html_json",
                msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { infomations = result }),
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
        }

        private int GetCheckCode()
        {
            int start = 2;
            var sum = 0;
            for (int i = 16; i >= 0; i--)
            {
                sum += (start * this.IDResult[i]) % 11;
                sum = sum % 11;
                start = start * 2;
                start = start % 11;
            }
            return (12 - sum) % 11;
        }

        public void Continue()
        {
            if (isRight())
            {
                InputAddress();
            }
            else
            {
                current = (current + 1) % 18;
                bool notify = false;
                if (current >= 18)
                {
                    notify = true;
                }
                string currentId = $"r{current / 18}c{current % 18}";
                PassObj p = new PassObj()
                {
                    ObjType = "html_ID",
                    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { id = currentId, n = notify })

                };
                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            }
        }
        public new void errorRecovery()
        {

            {
                if (current >= 17)
                {
                    this.IDResultError[current]++;
                    this.IDResultError[current] = this.IDResultError[current] % 11;
                }
                else
                {
                    this.IDResultError[current]++;
                    this.IDResultError[current] = this.IDResultError[current] % 10;
                }
                string currentId = $"r{current / 18}c{current % 18}";
                PassObj p = new PassObj()
                {
                    ObjType = "html_SET",
                    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { id = currentId, v = (this.IDResultError[current] == 10 ? "x" : this.IDResultError[current].ToString()) })

                };
                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            }
        }

        private bool isRight()
        {
            if (this.isEnd)
            {
                return true;
            }
            for (int i = 0; i < 18; i++)
            {
                if (this.IDResult[i] == this.IDResultError[i]) { }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public string SaveAddress(string address)
        {
            //MysqlCore.
            MysqlCore.BaseItem b = new MysqlCore.BaseItem("identitycard");
            return this.SaveAddress(address, b.xunzhangName, 0, b.AddAddressValue);
        }
    }


}
