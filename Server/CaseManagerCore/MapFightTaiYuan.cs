using MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagerCore
{
    public class MapFightTaiYuan : CaseManager
    {
        public class postion
        {

            public string roadCode { get; private set; }
            public int roadOrder { get; private set; }
            public double percent { get; private set; }
            public double lon { get; private set; }
            public double lat { get; private set; }

            public void set(string r, int o, double p, double lon_, double lat_)
            {
                this.roadCode = r;
                this.roadOrder = o;
                this.percent = p;
                this.lon = lon_;
                this.lat = lat_;
            }
        }

        Dictionary<string, decimal> rewardOfPositions = new Dictionary<string, decimal>();
        postion positionNow = new postion();
        bool autoOnOff { get; set; }

        string selectState = "";
        double lon { get; set; }
        double lat { get; set; }
        int pathOrder { get; set; }
        string state { get; set; }

        SelectorOfPosition sp { get; set; }
        int FpCount { get; set; }

        int PositionFirst { get; set; }

        List<int> taskPoints { get; set; }
        Dictionary<string, int> fpCodes;

        decimal sumCostStepValue { get; set; }
        decimal costPerStepValue { get; set; }
        decimal sumRewardOfShop { get; set; }
        // decimal rewardPerShop { get; set; }
        //  const decimal rewardDeltaPerShop = 0.05m;

        object profit
        {
            get
            {
                return new
                {
                    sumCost = this.sumCostStepValue,
                    sumReward = this.sumRewardOfShop,
                    w = this.sp.l.Length > 0,
                    nW = this.sp.nl.Length > 0,
                    a = this.sp.a.Length > 0,
                    canGet = this.fpCodes.Count == 0 && this.sumCostStepValue < this.sumRewardOfShop,
                    over = this.fpCodes.Count == 0
                };
            }
        }

        List<FastonPosition> needPass;
        public MapFightTaiYuan()
        {
            getCostPerStep();
            getFPCount();
            this.sumCostStepValue = 0;
            this.sumRewardOfShop = 0;

            this.rm = new Random(DateTime.Now.GetHashCode());
            this.PositionFirst = this.getFirstPosition();// this.rm.Next(0, this.FpCount);

            this.taskPoints = new List<int>();
            this.rewardOfPositions = new Dictionary<string, decimal>();
            var usedIndex = new Dictionary<int, bool>();

            while (this.taskPoints.Count < 7)
            {
                var index = this.rm.Next(0, this.FpCount);
                if (usedIndex.ContainsKey(index) || index == this.PositionFirst)
                {
                    continue;
                }
                else
                {
                    usedIndex.Add(index, true);
                    this.taskPoints.Add(index);
                }
            }
            usedIndex = null;

            List<string> material = new List<string>();
            this.needPass = new List<FastonPosition>();
            this.fpCodes = new Dictionary<string, int>();
            for (int i = 0; i < this.taskPoints.Count; i++)
            {
                var indexValue = taskPoints[i];
                FastonPosition fp;
                decimal reward;
                getFpByIndex(indexValue, out fp, out reward);
                this.sumRewardOfShop += reward;
                needPass.Add(fp);
                fpCodes.Add(fp.FastenPositionID, taskPoints[i]);
                this.rewardOfPositions.Add(fp.FastenPositionID, reward);
            }


            //for (int i = 0; i < needPass.Count; i++)
            //{
            //    fpCodes.Add(needPass[i].FastenPositionID, needPass);
            //}
            FastonPosition firstFp;
            {

                //decimal reward;
                getFpByIndex(this.PositionFirst, out firstFp);

                this.lon = firstFp.Longitude;
                this.lat = firstFp.Latitde;

                //this.sumRewardOfShop += reward;
            }
            // this.positionNow.set(firstFPID)
            this.sp = getSelect(firstFp.RoadCode, firstFp.RoadOrder, firstFp.RoadPercent);
            this.positionNow.set(firstFp.RoadCode, firstFp.RoadOrder, firstFp.RoadPercent, firstFp.Longitude, firstFp.Latitde);

            //= select;
            var objs = new { needPass = needPass, firstFp = firstFp, lon = this.lon, lat = this.lat, p = this.profit };
            var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(objs);

            PassObj p = new PassObj()
            {
                ObjType = "initial",
                msg = passMsg,
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.state = "0";
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            this.autoOnOff = false;
            getLengthM(firstFp, needPass.ToArray().ToList());
        }

        public class firstPosition
        {
            public int v { get; set; }
        }
        private int getFirstPosition()
        {
            var msg = sendMsg("{\"Type\":\"FirstIndex\",\"JsonValue\":\"\"}");

            return Newtonsoft.Json.JsonConvert.DeserializeObject<firstPosition>(msg).v;
        }

        decimal rewardMeasure { get; set; }
        /// <summary>
        /// 衡量奖励的依据
        /// </summary>
        /// <param name="firstFp"></param>
        /// <param name="needPass"></param>
        private void getLengthM(FastonPosition firstFp, List<FastonPosition> needPass)
        {
            double result = 0;
            double minLon, minLat, maxLon, maxLat;


            minLon = firstFp.Longitude;
            minLat = firstFp.Latitde;
            maxLon = firstFp.Longitude;
            maxLat = firstFp.Latitde;

            while (needPass.Count > 0)
            {
                var x = (from item in needPass orderby getDistance(firstFp, item) ascending select item).ToList()[0];
                result += getDistance(firstFp, x);
                firstFp = x;
                needPass.Remove(x);
            }

            this.rewardMeasure = Math.Round(Convert.ToDecimal(result / 1000), 2);
            Console.WriteLine($"rewardMeasure:{this.rewardMeasure}km");
        }

        private double getDistance(FastonPosition firstFp, FastonPosition item)
        {
            return getLengthOfTwoPoint.GetDistance(firstFp.Latitde, firstFp.Longitude, item.Latitde, item.Longitude);
        }

        private SelectorOfPosition getSelect(string roadCode, int roadOrder, double roadPercent)
        {

            var objParameter = new { roadCode = roadCode, roadOrder = roadOrder, roadPercent = roadPercent };
            var jsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(objParameter);
            var passObj = new { Type = "GetNext", JsonValue = jsonValue };
            var command = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
            var result = sendMsg(command);
            // return ;
            SelectorOfPosition spResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectorOfPosition>(result);
            var usedFps = (from item in spResult.a where this.fpCodes.ContainsKey(item.l.id) select item).ToArray();
            // this.sp.a = usedFps;
            spResult.a = usedFps;
            return spResult;
        }

        class indexGotton
        {
            public FastonPosition fp { get; set; }
            public decimal reward { get; set; }
        }
        private void getFpByIndex(int i, out FastonPosition fp, out decimal reward)
        {
            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { IndexValule = i });
            var msgToSend = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "GetByIndex", JsonValue = JsonValue });

            var msgGotton = sendMsg(msgToSend);
            var objGotton = Newtonsoft.Json.JsonConvert.DeserializeObject<indexGotton>(msgGotton);
            fp = objGotton.fp;
            reward = objGotton.reward;
        }


        private void getFpByIndex(int i, out FastonPosition fp)
        {
            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { IndexValule = i });
            var msgToSend = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "GetFPOnlyByIndex", JsonValue = JsonValue });

            var msgGotton = sendMsg(msgToSend);
            var objGotton = Newtonsoft.Json.JsonConvert.DeserializeObject<indexGotton>(msgGotton);
            fp = objGotton.fp;
        }

        class FastonPosition
        {
            public string FastenPositionID { get; set; }
            public string FastenPositionName { get; set; }
            public string FastenPositionInfo { get; set; }
            public double Longitude { get; set; }
            public double Latitde { get; set; }
            public string RoadCode { get; set; }
            public double RoadPercent { get; set; }
            public int IsChanged { get; set; }
            public int FastenType { get; set; }
            public int RoadOrder { get; set; }
            public string UserName { get; set; }
            public double positionLongitudeOnRoad { get; set; }
            public double positionLatitudeOnRoad { get; set; }
            public int MacatuoX { get; set; }
            public int MacatuoY { get; set; }
        }



        class CountC
        {
            public int Count { get; set; }
        }
        void getFPCount()
        {
            var msg = sendMsg("{\"Type\":\"GetCountOfFP\",\"JsonValue\":\"\"}");
            var count = Newtonsoft.Json.JsonConvert.DeserializeObject<CountC>(msg);
            this.FpCount = count.Count;
        }

        class CostPerStep
        {
            public decimal result { get; set; }
        }
        void getCostPerStep()
        {
            this.costPerStepValue = 0.03m;
            //var msg = sendMsg("{\"Type\":\"GetCost\",\"JsonValue\":\"\"}");
            //var count = Newtonsoft.Json.JsonConvert.DeserializeObject<CostPerStep>(msg);
            //this.costPerStepValue = count.result;
            //Console.WriteLine($"获取的CostPerStepValue：{this.costPerStepValue}");
        }

        string sendMsg(string inputMsg)
        {
            return sendMsg(inputMsg, 20651);
        }

        string sendMsg(string inputMsg, int port)
        {
            try
            {
                String response = String.Empty;
                using (TcpClient client = new TcpClient("10.80.52.218", port))
                {
                    using (NetworkStream stream = client.GetStream())
                    {

                        Byte[] data = System.Text.Encoding.UTF8.GetBytes(inputMsg);
                        // Send the message to the connected TcpServer. 
                        stream.Write(data, 0, data.Length);
                        // Bytes Array to receive Server Response.
                        data = new Byte[1024 * 1024 * 2];//10M

                        // Read the Tcp Server Response Bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);
                        response = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                        stream.Close();

                    }
                    client.Close();
                }
                return response;
            }
            catch
            {
                return "";
            }
        }


        public class SelectorOfPosition
        {
            // public int wr { get; set; }//walkInTheMapRecord

            public fastonPositionSelector[] a { get; set; }//allowedfastonPositionSelectors
                                                           //public fastonPositionSelector[] na { get; set; }//notAllowedfastonPositionSelectors

            public positionSelector[] l { get; set; }//listOfAllowedRouteSelector
            public positionSelector[] nl { get; set; }//listOfNotAllowedRouteSelector

            public class fastonPositionSelector
            {
                public int sn { get; set; }//selectNum
                public List<position2_simple> r { get; set; }//route
                public position3_simple l { get; set; }//localPosition

            }
            public class positionSelector
            {
                public int s { get; set; }//selectNum
                public List<position2_simple> r { get; set; }//route
            }

            public class position2_simple
            {
                public double x { get; set; }//longitude
                public double y { get; set; }//latitude
                public string C { get; set; }//roadCode
                public int O { get; set; }//roadOrder
                public double p { get; set; }//percent
            }

            public class position3_simple
            {
                /// <summary>
                /// longitude
                /// </summary>
                public double x { get; set; }//longitude
                /// <summary>
                /// latitude
                /// </summary>
                public double y { get; set; }//latitude
                /// <summary>
                /// fastonPositionID
                /// </summary>
                public string id { get; set; }//fastonPositionID
                /// <summary>
                /// FastenPositionName
                /// </summary>
                public string N { get; set; }//FastenPositionName

                /// <summary>
                /// RoadOrder
                /// </summary>
                public int O { get; set; }//RoadOrder
                /// <summary>
                /// RoadCode
                /// </summary>
                public string C { get; set; }//RoadCode
                /// <summary>
                /// FastenType
                /// </summary>
                public int T { get; set; }//FastenType
            }
        }

        public void move(int v)
        {
            switch (this.state)
            {
                case "0":
                    {
                        switch (v)
                        {
                            case 0:
                                {
                                    {

                                        this.state = "selec_l_nl_a";
                                        var divInnerText = $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;";
                                        var obj = new { divInnerText = divInnerText, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                    }
                                }; break;
                        }
                    }; break;
                case "selec_l_nl_a":
                    {
                        switch (v)
                        {
                            case 0:
                                {
                                    if (this.sp.l.Length == 0)
                                    {
                                        this.state = "selec_l_nl_a";
                                        var divInnerText = getDiv(this.state, ";正路不通，可逆行");// $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:得分"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;正路不通，可逆行。";
                                        var obj = new { divInnerText = divInnerText, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);

                                    }
                                    else
                                    {

                                        if (this.sp.l.Length == 1 && this.sp.a.Length == 0 && this.autoOnOff)
                                        {
                                            this.state = "l_select_auto";
                                            this.pathOrder = 0;
                                            var divInnerText = $"<span style=\"background:#312f3d\">{"↑:执行"}</span>;<span style=\"background:#452f2f\">{"→:下一路径"}</span>;<span style=\"background:#3c420e\">{"↓:返回"}</span>;<span style=\"background:#062c03\">{"←:上一路径"}</span>;点此返回;";

                                            var obj = new { divInnerText = divInnerText, path = this.sp.l[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                            var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                            PassObj p = new PassObj()
                                            {
                                                ObjType = this.state,
                                                msg = passMsg,
                                                showContinue = true,
                                                showIsError = false,
                                                isEnd = false,
                                                ObjID = $"{idPrevious}{startId++}",
                                                styleStr = "msg"

                                            };
                                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                            this.pathOrder = 0;
                                        }
                                        else
                                        {
                                            if (this.sp.l.Length == 2 && this.sp.a.Length == 0 && this.autoOnOff)
                                            {
                                                if (getCarInOpposeDirection(this.sp.l[0].r[0].C) == 1)
                                                {
                                                    var selectStateOfPath = DateTime.Now.ToString("yyyyMMddHHssmmffff");
                                                    {
                                                        var firstPath = this.sp.l[0].r;
                                                        var firstPoint = firstPath[0];


                                                        for (int i = 1; i < firstPath.Count; i++)
                                                        {
                                                            var secondPoint = firstPath[i];
                                                            if (firstPoint.C != secondPoint.C)
                                                            {
                                                                break;
                                                            }
                                                            if ((firstPoint.O + firstPoint.p) > (secondPoint.O + secondPoint.p))
                                                            {
                                                                selectStateOfPath = $"{firstPoint.C}>";
                                                                break;
                                                            }
                                                            else if ((firstPoint.O + firstPoint.p) < (secondPoint.O + secondPoint.p))
                                                            {
                                                                selectStateOfPath = $"{firstPoint.C}<";
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                continue;
                                                            }
                                                        }

                                                        if (this.selectState == selectStateOfPath)
                                                        {
                                                            this.state = "l_select_auto";
                                                            this.pathOrder = 0;
                                                            var divInnerText = $"自动执行";
                                                            var obj = new { divInnerText = divInnerText, path = this.sp.l[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                                            var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                                            PassObj p = new PassObj()
                                                            {
                                                                ObjType = this.state,
                                                                msg = passMsg,
                                                                showContinue = true,
                                                                showIsError = false,
                                                                isEnd = false,
                                                                ObjID = $"{idPrevious}{startId++}",
                                                                styleStr = "msg"

                                                            };
                                                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                                            return;
                                                        }
                                                    }
                                                    {
                                                        var secondPath = this.sp.l[1].r;
                                                        var firstPoint = secondPath[0];


                                                        for (int i = 1; i < secondPath.Count; i++)
                                                        {
                                                            var secondPoint = secondPath[i];
                                                            if (firstPoint.C != secondPoint.C)
                                                            {
                                                                break;
                                                            }
                                                            if ((firstPoint.O + firstPoint.p) > (secondPoint.O + secondPoint.p))
                                                            {
                                                                selectStateOfPath = $"{firstPoint.C}>";
                                                                break;
                                                            }
                                                            else if ((firstPoint.O + firstPoint.p) < (secondPoint.O + secondPoint.p))
                                                            {
                                                                selectStateOfPath = $"{firstPoint.C}<";
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                continue;
                                                            }
                                                        }

                                                        // var selectStateOfPath = $"{secondPoint.C}{(((firstPoint.O + firstPoint.p) > (secondPoint.O + secondPoint.p)) ? ">" : "<")}";
                                                        if (this.selectState == selectStateOfPath)
                                                        {
                                                            this.state = "l_select_auto";
                                                            this.pathOrder = 1;
                                                            var divInnerText = $"自动执行";
                                                            var obj = new { divInnerText = divInnerText, path = this.sp.l[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                                            var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                                            PassObj p = new PassObj()
                                                            {
                                                                ObjType = this.state,
                                                                msg = passMsg,
                                                                showContinue = true,
                                                                showIsError = false,
                                                                isEnd = false,
                                                                ObjID = $"{idPrevious}{startId++}",
                                                                styleStr = "msg"

                                                            };
                                                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                            {
                                                this.state = "l_select";
                                                this.pathOrder = 0;
                                                for (int i = 0; i < this.sp.l.Length; i++)
                                                {
                                                    var firstPath = this.sp.l[i].r;
                                                    var firstPoint = firstPath[0];
                                                    var secondPoint = firstPath[1];

                                                    //var slast2 = this.sp.l[this.pathOrder].r[this.sp.l[this.pathOrder].r.Count - 2];
                                                    //var s = this.sp.l[this.pathOrder].r.Last();

                                                    var selectStateOfPath = $"{secondPoint.C}{(firstPoint.O + firstPoint.p > secondPoint.O + secondPoint.p ? ">" : "<")}";
                                                    if (selectStateOfPath == this.selectState)
                                                    {
                                                        this.pathOrder = i;
                                                        break;
                                                    }
                                                }
                                                // this.pathOrder = 0;
                                                var divInnerText = $"<span style=\"background:#312f3d\">{"↑:执行"}</span>;<span style=\"background:#452f2f\">{"→:下一路径"}</span>;<span style=\"background:#3c420e\">{"↓:返回"}</span>;<span style=\"background:#062c03\">{"←:上一路径"}</span>;点此返回;";

                                                var obj = new { divInnerText = divInnerText, path = this.sp.l[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                                var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                                PassObj p = new PassObj()
                                                {
                                                    ObjType = this.state,
                                                    msg = passMsg,
                                                    showContinue = true,
                                                    showIsError = false,
                                                    isEnd = false,
                                                    ObjID = $"{idPrevious}{startId++}",
                                                    styleStr = "msg"

                                                };
                                                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);

                                                return;
                                            }
                                        }
                                    }

                                }; break;
                            case 2:
                                {
                                    if (sp.nl.Length > 0)
                                    {
                                        this.state = "nl_select";
                                        //for (int i = 0; i < this.sp.nl.Length; i++)
                                        //{
                                        //    var firstPath = this.sp.nl[i].r;
                                        //    var firstPoint = firstPath[0];
                                        //    var secondPoint = firstPath[1];

                                        //    //var slast2 = this.sp.l[this.pathOrder].r[this.sp.l[this.pathOrder].r.Count - 2];
                                        //    //var s = this.sp.l[this.pathOrder].r.Last();

                                        //    var selectStateOfPath = $"{secondPoint.C}{(firstPoint.O + firstPoint.p > secondPoint.O + secondPoint.p ? ">" : "<")}";
                                        //    if (selectStateOfPath == this.selectState)
                                        //    {
                                        //        this.pathOrder = i;
                                        //        break;
                                        //    }
                                        //}
                                        // this.pathOrder = 0;
                                        this.pathOrder = 0;
                                        var divInnerText = $"<span style=\"background:#312f3d\">{"↑:执行"}</span>;<span style=\"background:#452f2f\">{"→:下一路径"}</span>;<span style=\"background:#3c420e\">{"↓:返回"}</span>;<span style=\"background:#062c03\">{"←:上一路径"}</span>;点此返回;";

                                        var obj = new { divInnerText = divInnerText, path = this.sp.nl[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                        return;
                                    }
                                    else
                                    {
                                        this.state = "selec_l_nl_a";
                                        var divInnerText = $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;无需逆行";
                                        var obj = new { divInnerText = divInnerText, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                    }
                                }; break;
                            case 1:
                                {
                                    if (sp.a.Length > 0)
                                    {
                                        this.pathOrder = 0;
                                        this.state = "selec_a";
                                        var divInnerText = $"({pathOrder + 1}/{this.sp.a.Length})<span style=\"background:#312f3d\">{"↑:完成"}</span>;<span style=\"background:#452f2f\">{"→:下一个"}</span>;<span style=\"background:#3c420e\">{"↓:返回"}</span>;<span style=\"background:#062c03\">{"←:上一个"}</span>;点此返回。";
                                        var obj = new { divInnerText = divInnerText, path = this.sp.a[this.pathOrder], lon = this.lon, lat = this.lat, fpLon = this.sp.a[this.pathOrder].l.x, fpLat = this.sp.a[this.pathOrder].l.y, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                    }
                                    else
                                    {
                                        this.state = "selec_l_nl_a";
                                        var divInnerText = $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;此处没任务。";
                                        var obj = new { divInnerText = divInnerText, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                    }
                                }; break;
                            case 3:
                                {
                                    this.state = "mapConfig";
                                    var obj = new { p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                            case 4:
                                {
                                    this.state = "0";
                                    var obj = new { p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                        }

                    }; break;
                case "selec_nl_a":
                    {
                        switch (v)
                        {
                            case 0:
                                {
                                    throw new Exception("selec_nl_a 应该返回的");
                                }; break;
                        }

                    }; break;
                case "l_select":
                    {
                        switch (v)
                        {
                            case 0:
                                {
                                    // var slast2 = this.sp.l[this.pathOrder].r[this.sp.l.Length - 2];

                                    this.pathOrder = this.pathOrder + this.sp.l.Length;
                                    this.pathOrder = this.pathOrder % this.sp.l.Length;

                                    var s = this.sp.l[this.pathOrder].r.Last();
                                    this.sumCostStepValue += this.costPerStepValue;
                                    //   this.selectState = $"{s.C}{((slast2.O + slast2.p) > (s.O + s.p) ? ">" : "<")}";
                                    this.selectState = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                    for (int i = this.sp.l[this.pathOrder].r.Count - 2; i >= 0; i--)
                                    {
                                        var slast2 = this.sp.l[this.pathOrder].r[i];
                                        if (slast2.C != s.C)
                                        {
                                            break;
                                        };
                                        if ((slast2.O + slast2.p) > (s.O + s.p))
                                        {
                                            this.selectState = $"{s.C}>";
                                            break;
                                        }
                                        else if ((slast2.O + slast2.p) < (s.O + s.p))
                                        {
                                            this.selectState = $"{s.C}<";
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                    this.autoOnOff = true;
                                    this.sp = this.getSelect(s.C, s.O, s.p);
                                    this.positionNow.set(s.C, s.O, s.p, s.x, s.y);
                                    this.state = "selec_l_nl_a";
                                    this.lon = s.x;
                                    this.lat = s.y;
                                    //  this.CurrentPosition = s;
                                    move(0);


                                }; break;
                            case 1:
                                {
                                    this.state = "l_select";
                                    this.pathOrder += 1;
                                    this.pathOrder %= this.sp.l.Length;
                                    var divInnerText = $"<span style=\"background:#312f3d\">{"↑:执行"}</span>;<span style=\"background:#452f2f\">{"→:下一路径"}</span>;<span style=\"background:#3c420e\">{"↓:返回"}</span>;<span style=\"background:#062c03\">{"←:上一路径"}</span>;点此返回;";

                                    var obj = new { divInnerText = divInnerText, path = this.sp.l[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                            case 3:
                                {
                                    this.state = "l_select";
                                    this.pathOrder += (this.sp.l.Length - 1);
                                    this.pathOrder %= this.sp.l.Length;
                                    var divInnerText = $"<span style=\"background:#312f3d\">{"↑:执行"}</span>;<span style=\"background:#452f2f\">{"→:下一路径"}</span>;<span style=\"background:#3c420e\">{"↓:返回"}</span>;<span style=\"background:#062c03\">{"←:上一路径"}</span>;点此返回;";

                                    var obj = new { divInnerText = divInnerText, path = this.sp.l[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                            case 2:
                                {
                                    this.state = "selec_l_nl_a";

                                    var divInnerText = getDiv(this.state);//$"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:得分"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;";
                                    var obj = new { divInnerText = divInnerText, p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                        };
                    }; break;
                case "l_select_auto":
                    {
                        switch (v)
                        {
                            case 0:
                                {


                                    var s = this.sp.l[this.pathOrder].r.Last();
                                    this.selectState = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                    for (int i = this.sp.l[this.pathOrder].r.Count - 2; i >= 0; i--)
                                    {
                                        var slast2 = this.sp.l[this.pathOrder].r[i];
                                        if (slast2.C != s.C)
                                        {
                                            break;
                                        };
                                        if ((slast2.O + slast2.p) > (s.O + s.p))
                                        {
                                            this.selectState = $"{s.C}>";
                                            break;
                                        }
                                        else if ((slast2.O + slast2.p) < (s.O + s.p))
                                        {
                                            this.selectState = $"{s.C}<";
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                    // this.selectState = $"{s.C}{(((slast2.O + slast2.p) > (s.O + s.p)) ? ">" : "<")}";


                                    this.sp = this.getSelect(s.C, s.O, s.p);
                                    this.positionNow.set(s.C, s.O, s.p, s.x, s.y);
                                    this.sumCostStepValue += this.costPerStepValue;
                                    this.state = "selec_l_nl_a";
                                    this.lon = s.x;
                                    this.lat = s.y;
                                    //  this.CurrentPosition = s;
                                    move(0);
                                }; break;
                        };
                    }; break;
                case "nl_select":
                    {
                        switch (v)
                        {
                            case 0:
                                {
                                    this.autoOnOff = false;
                                    this.selectState = DateTime.Now.ToString() + DateTime.Now.ToString().GetHashCode().ToString();
                                    var s = this.sp.nl[this.pathOrder].r.Last();
                                    this.sp = this.getSelect(s.C, s.O, s.p);
                                    this.positionNow.set(s.C, s.O, s.p, s.x, s.y);
                                    this.state = "selec_l_nl_a";

                                    if (this.rm.Next(10) < 2)
                                    {
                                        this.sumCostStepValue += this.costPerStepValue;
                                    }
                                    else
                                    {
                                        this.sumCostStepValue += this.costPerStepValue * 10;
                                    }
                                    this.sumCostStepValue += this.costPerStepValue;

                                    this.lon = s.x;
                                    this.lat = s.y;
                                    move(0);


                                }; break;
                            case 1:
                                {
                                    this.state = "nl_select";
                                    this.pathOrder += 1;
                                    this.pathOrder %= this.sp.nl.Length;
                                    var divInnerText = $"<span style=\"background:#312f3d\">{"↑:执行"}</span>;<span style=\"background:#452f2f\">{"→:下一路径"}</span>;<span style=\"background:#3c420e\">{"↓:返回"}</span>;<span style=\"background:#062c03\">{"←:上一路径"}</span>;点此返回;";

                                    var obj = new { divInnerText = divInnerText, path = this.sp.nl[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                            case 3:
                                {
                                    this.state = "nl_select";
                                    this.pathOrder += (this.sp.nl.Length - 1);
                                    this.pathOrder %= this.sp.nl.Length;
                                    var divInnerText = $"<span style=\"background:#312f3d\">{"↑:执行"}</span>;<span style=\"background:#452f2f\">{"→:下一路径"}</span>;<span style=\"background:#3c420e\">{"↓:返回"}</span>;<span style=\"background:#062c03\">{"←:上一路径"}</span>;点此返回;";

                                    var obj = new { divInnerText = divInnerText, path = this.sp.nl[this.pathOrder], lon = this.lon, lat = this.lat, p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                            case 2:
                                {
                                    this.state = "selec_l_nl_a";
                                    var divInnerText = $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回。";
                                    var obj = new { divInnerText = divInnerText, };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                        };
                    }; break;
                case "selec_a":
                    {
                        switch (v)
                        {
                            case 0:
                                {
                                    var codeSelect = this.sp.a[this.pathOrder].l.id;

                                    if (this.fpCodes.ContainsKey(codeSelect))
                                    {
                                        var indexGet = this.fpCodes[codeSelect];
                                        FastonPosition fp;
                                        getFpByIndex(indexGet, out fp);
                                        //    var fp = Newtonsoft.Json.JsonConvert.DeserializeObject<FastonPosition>(getFpByIndex(indexGet));
                                        this.fpCodes.Remove(codeSelect);

                                        var newA = new List<SelectorOfPosition.fastonPositionSelector>();
                                        for (int i = 0; i < this.sp.a.Length; i++)
                                        {
                                            if (i == this.pathOrder) { }
                                            else
                                            {
                                                newA.Add(this.sp.a[i]);
                                            }
                                        }
                                        this.sp.a = newA.ToArray();

                                        // this.sumRewardOfShop += this.rewardPerShop;
                                        //this.rewardPerShop += rewardDeltaPerShop;
                                        this.sumCostStepValue += this.costPerStepValue;
                                        this.selectState = DateTime.Now.GetHashCode().ToString() + DateTime.Now.ToString();
                                        this.state = "selec_l_nl_a_afterremovea";
                                        var divInnerText = $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;";
                                        var obj = new { divInnerText = divInnerText, removeObjCode = codeSelect, fp = fp, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                        this.state = "selec_l_nl_a";
                                        this.sp = getSelect(fp.RoadCode, fp.RoadOrder, fp.RoadPercent);
                                        this.positionNow.set(fp.RoadCode, fp.RoadOrder, fp.RoadPercent, fp.Longitude, fp.Latitde);
                                    }
                                    else
                                    {
                                        return;
                                    }

                                }; break;
                            case 2:
                                {
                                    this.state = "selec_l_nl_a";
                                    var divInnerText = $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;";
                                    var obj = new { divInnerText = divInnerText, p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                            case 1:
                                {
                                    this.pathOrder++;
                                    this.pathOrder = this.pathOrder % this.sp.a.Length;
                                    var codeSelect = this.sp.a[this.pathOrder].l.id;
                                    if (this.fpCodes.ContainsKey(codeSelect))
                                    {
                                        var indexGet = this.fpCodes[codeSelect];

                                        FastonPosition fp;
                                        getFpByIndex(indexGet, out fp);
                                        // var fp = Newtonsoft.Json.JsonConvert.DeserializeObject<FastonPosition>(getFpByIndex(indexGet));
                                        this.fpCodes.Remove(codeSelect);

                                        this.state = "selec_l_nl_a_afterremovea";
                                        var divInnerText = $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;";
                                        var obj = new { divInnerText = divInnerText, removeObjCode = codeSelect, fp = fp, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                        this.state = "selec_l_nl_a";
                                        this.sp = getSelect(fp.RoadCode, fp.RoadOrder, fp.RoadPercent);
                                        this.positionNow.set(fp.RoadCode, fp.RoadOrder, fp.RoadPercent, fp.Longitude, fp.Latitde);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }; break;
                            case 3:
                                {
                                    this.pathOrder--;
                                    this.pathOrder += this.sp.a.Length;
                                    this.pathOrder = this.pathOrder % this.sp.a.Length;
                                    var codeSelect = this.sp.a[this.pathOrder].l.id;
                                    if (this.fpCodes.ContainsKey(codeSelect))
                                    {
                                        var indexGet = this.fpCodes[codeSelect];

                                        FastonPosition fp;
                                        getFpByIndex(indexGet, out fp);
                                        this.fpCodes.Remove(codeSelect);

                                        this.state = "selec_l_nl_a_afterremovea";
                                        var divInnerText = $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回;";
                                        var obj = new { divInnerText = divInnerText, removeObjCode = codeSelect, fp = fp, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        PassObj p = new PassObj()
                                        {
                                            ObjType = this.state,
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                        this.state = "selec_l_nl_a";
                                        this.sp = getSelect(fp.RoadCode, fp.RoadOrder, fp.RoadPercent);
                                        this.positionNow.set(fp.RoadCode, fp.RoadOrder, fp.RoadPercent, fp.Longitude, fp.Latitde);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }; break;
                        }
                    }; break;
                case "mapConfig":
                    {
                        switch (v)
                        {
                            case 4:
                                {
                                    this.state = "selec_l_nl_a";
                                    var divInnerText = getDiv(this.state);
                                    var obj = new { divInnerText = divInnerText, p = this.profit };
                                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                    PassObj p = new PassObj()
                                    {
                                        ObjType = this.state,
                                        msg = passMsg,
                                        showContinue = true,
                                        showIsError = false,
                                        isEnd = false,
                                        ObjID = $"{idPrevious}{startId++}",
                                        styleStr = "msg"

                                    };
                                    this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                }; break;
                            case 2:
                                {
                                    if (this.fpCodes.Count == 0)
                                    {
                                        var path = new double[] { };
                                        var obj = new { path = path, count = this.fpCodes.Count, p = this.profit };
                                        var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                                        PassObj p = new PassObj()
                                        {
                                            ObjType = $"{this.state}_afterGetPath",
                                            msg = passMsg,
                                            showContinue = true,
                                            showIsError = false,
                                            isEnd = false,
                                            ObjID = $"{idPrevious}{startId++}",
                                            styleStr = "msg"

                                        };
                                        this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                    }
                                    else
                                    {
                                        if (this.sumCostStepValue >= this.sumRewardOfShop)
                                        {
                                            var path = new double[] { };
                                            var obj = new { path = path, count = this.fpCodes.Count, p = this.profit };
                                            var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                                            PassObj p = new PassObj()
                                            {
                                                ObjType = $"{this.state}_noNeed",
                                                msg = passMsg,
                                                showContinue = true,
                                                showIsError = false,
                                                isEnd = false,
                                                ObjID = $"{idPrevious}{startId++}",
                                                styleStr = "msg"

                                            };
                                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                        }
                                        else
                                        {
                                            this.sumCostStepValue += 0.1m;
                                            List<FastonPosition> fps = new List<FastonPosition>();
                                            var nearestFp = (from item in this.needPass where fpCodes.ContainsKey(item.FastenPositionID) orderby getLengthOfTwoPoint.GetDistance(this.positionNow.lat, this.positionNow.lon, item.Latitde, item.Longitude) ascending select item).First();
                                            var path = this.getPath(this.positionNow.roadCode, this.positionNow.roadOrder, this.positionNow.percent, nearestFp.FastenPositionID);
                                            var obj = new { path = path, count = this.fpCodes.Count, p = this.profit };
                                            var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                                            PassObj p = new PassObj()
                                            {
                                                ObjType = $"{this.state}_afterGetPath",
                                                msg = passMsg,
                                                showContinue = true,
                                                showIsError = false,
                                                isEnd = false,
                                                ObjID = $"{idPrevious}{startId++}",
                                                styleStr = "msg"

                                            };
                                            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                                        }
                                    }
                                }; break;
                        };
                    }; break;
            }
            if (this.state == "")
            {

            }


            //throw new NotImplementedException();
        }

        private double[] getPath(string roadCode, int roadOrder, double percent, string fastenPositionID)
        {
            var JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { roadCode = roadCode, roadOrder = roadOrder, percent = percent, targetFPID = fastenPositionID });
            var msgSent = Newtonsoft.Json.JsonConvert.SerializeObject(new { @Type = "CToT", JsonValue = JsonValue });
            var msg = sendMsg(msgSent, 20652);
            var path = Newtonsoft.Json.JsonConvert.DeserializeObject<double[]>(msg);
            return path;
        }

        private string getDiv(string state_Input, params string[] ps)
        {
            switch (state_Input)
            {
                case "selec_l_nl_a":
                    {
                        return $"<span style=\"background:#312f3d\">{"↑:行走"}</span>;<span style=\"background:#452f2f\">{"→:完成"}</span>;<span style=\"background:#3c420e\">{"↓:逆行"}</span>;<span style=\"background:#062c03\">{"←:移动地图"}</span>;点此返回{(ps.Length >= 1 ? ps[0] : "")}。";
                    }; break;
            }
            return "";
            //   throw new NotImplementedException();
        }

        private int getCarInOpposeDirection(string RoadCode)
        {

            // var jsonValue = $"{{\"RoadCode\":\"{RoadCode}\"}}";
            var msgMaterial = Newtonsoft.Json.JsonConvert.SerializeObject(new { Type = "GI", JsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { RoadCode = RoadCode }) });
            //var msg = sendMsg($"{{\"Type\":\"GI\",\"JsonValue\":\"\"}}");

            var gi = Newtonsoft.Json.JsonConvert.DeserializeObject<GIResult>(sendMsg(msgMaterial));
            return gi.result;
        }

        public class GIResult
        {
            public int result { get; set; }
        }

        public class Profit
        {
            public decimal sumCost { get; set; }
            public decimal sumReward { get; set; }
        }


        public string SaveAddress(string address)
        {
            // MySql.BaseItem b = new MySql.BaseItem("yilingersi");
            //b.AddAddressValue(address, out moneycountAddV);


            if (this.fpCodes.Count == 0)
            {
                var get = this.sumRewardOfShop - this.sumCostStepValue;
                if (get >= 0.01m)
                {
                    //if (this.fpCodes.Count == 0)
                    //{
                    //    return "";
                    //}
                    //else
                    {


                        var alertMsg = $"您往{address}存入{get.ToString("f2")}金币";
                        this.sumCostStepValue = this.sumCostStepValue + get;
                        this.setReward(get);

                        MysqlCore.BaseItem b = new MysqlCore.BaseItem("mapfighttaiyuan");
                        b.AddAddressValue(address, get);
                        //b.AddAddressValue(address, out moneycountAddV);
                        var msgPass = $"您好，您往<div>{address}</div>  添加了一枚{b.xunzhangName}勋章，并且获得了{get.ToString("f2")}金币！<div> <a href=\"https://www.nyrq123.com/XunZhang.html\" style=\"color: whitesmoke;\">查看勋章</a></div>";
                        DealWithMsg(msgPass);
                        return this.msg;

                        //var obj = new { address = address, p = this.profit, alertMsg = alertMsg };
                        //var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        //PassObj p = new PassObj()
                        //{
                        //    ObjType = "saveAddress",
                        //    msg = passMsg,
                        //    showContinue = true,
                        //    showIsError = false,
                        //    isEnd = false,
                        //    ObjID = $"{idPrevious}{startId++}",
                        //    styleStr = "msg"

                        //};
                        //return Newtonsoft.Json.JsonConvert.SerializeObject(p);
                    }
                }
                else
                {
                    var alertMsg = $"您现在消耗了{this.sumCostStepValue.ToString("f2")}金币，获得了{this.sumRewardOfShop.ToString("f2")}金币，还没有利润。";
                    var obj = new { address = address, p = this.profit, alertMsg = alertMsg };
                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    PassObj p = new PassObj()
                    {
                        ObjType = "saveAddress",
                        msg = passMsg,
                        showContinue = true,
                        showIsError = false,
                        isEnd = false,
                        ObjID = $"{idPrevious}{startId++}",
                        styleStr = "msg"

                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(p);
                }
            }
            else
            {
                var alertMsg = $"你现在还有{this.fpCodes.Count}项任务没有完成！！！不能领取奖励哦！";
                var obj = new { address = address, p = this.profit, alertMsg = alertMsg };
                var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                PassObj p = new PassObj()
                {
                    ObjType = "saveAddress",
                    msg = passMsg,
                    showContinue = true,
                    showIsError = false,
                    isEnd = false,
                    ObjID = $"{idPrevious}{startId++}",
                    styleStr = "msg"

                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(p);
            }

            if (this.fpCodes.Count == 0)
            {
                return "";
            }



            //return this.SaveAddress(address, b.xunzhangName, 0, b.AddAddressValue);
        }

        private void setReward(decimal get)
        {
            //List<string> positions = new List<string>();
            //for (int i = 0; i < this.needPass.Count; i++)
            //{
            //    positions.Add(this.needPass[i].FastenPositionID);
            //}
            var jsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(new { get = get, rewardOfPositions = this.rewardOfPositions });
            var passObj = new { Type = "SetReward", JsonValue = jsonValue };
            var command = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
            sendMsg(command);
        }

        public class getLengthOfTwoPoint
        {
            private const double EARTH_RADIUS = 6371393;
            private static double rad(double d)
            {
                return d * Math.PI / 180.0;
            }
            public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
            {
                double radLat1 = rad(lat1);
                double radLat2 = rad(lat2);
                double a = radLat1 - radLat2;
                double b = rad(lng1) - rad(lng2);

                double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
                 Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
                s = s * EARTH_RADIUS;
                return s;
            }
        }

        string firstFPID { get; set; }
        string secondFPID { get; set; }
        string pathID
        {
            get { return $"{this.firstFPID}{this.secondFPID}"; }
        }
    }
}
