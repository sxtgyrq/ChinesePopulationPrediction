using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CaseManager = CaseManagerCore;

namespace CoreWsApp2
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        static long sumVisitor = 0;
        static long sumLeaver = 0;
        static Dictionary<string, long> detail = new Dictionary<string, long>();

        [Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseFileServer(enableDirectoryBrowsing: true);
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60 * 1000),
                ReceiveBufferSize = size
            };
            //webSocketOptions.AllowedOrigins.Add("https://client.com");
            //webSocketOptions.AllowedOrigins.Add("https://www.client.com");
            app.Map("/state", builder =>
            {
                builder.Use(async (context, next) =>
                {
                    // Console.WriteLine("访问了state");
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { sumVisitor = sumVisitor, onLine = sumVisitor - sumLeaver, detail = detail });
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(msg, Encoding.UTF8);
                });
            });


            app.UseWebSockets(webSocketOptions); // Only for Kestrel

            app.Map("", builder =>
            {
                builder.Use(async (context, next) =>
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        sumVisitor++;
                        //   context.
                        Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--累计登陆{sumVisitor},当前在线{sumVisitor - sumLeaver}");
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await Echo(webSocket);
                        sumLeaver++;
                        //var text = $"{DateTime.Now.ToString("yyyyMMddHHmmss")},{sumVisitor},{sumVisitor - sumLeaver},";
                        //foreach (var item in detail)
                        //{
                        //    text += $"{item.Key},{item.Value},";
                        //}
                        //File.AppendText()

                        return;
                    }

                    await next();
                });
            });



        }


        const int size = 1024;
        private async Task Echo(WebSocket webSocket)
        {

            //webSocket.
            //string bitcoinAddress = "1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr";
            byte[] buffer = new byte[size];
            // websocket
            WebSocketReceiveResult result;


            // Dictionary<string, string> state = new Dictionary<string, string>();
            //   Object obj = null;
            CaseManager.BitCoinQuestionPage QuestionPageObjs = null; //Dictionary<string, CaseManager.BitCoinQuestionPage> QuestionPageObjs = new Dictionary<string, CaseManager.BitCoinQuestionPage>();
            CaseManager.KekongHejubian KekongHejubians = null; // Dictionary<string, CaseManager.KekongHejubian> KekongHejubians = new Dictionary<string, CaseManager.KekongHejubian>();
            CaseManager.Morsecode Morsecodes = null;// Dictionary<string, CaseManager.Morsecode> Morsecodes = new Dictionary<string, CaseManager.Morsecode>();
            CaseManager.Periodictable Periodictables = null;//  Dictionary<string, CaseManager.Periodictable> Periodictables = new Dictionary<string, CaseManager.Periodictable>();
            CaseManager.QianAndYang QianAndYangs = null; //Dictionary<string, CaseManager.QianAndYang> QianAndYangs = new Dictionary<string, CaseManager.QianAndYang>();

            CaseManager.Kobe Kobes = null;
            CaseManager.Identitycard IdCards = null;
            CaseManager.SelfSoftware SelfSoftwares = null;
            CaseManager.Yilingersi Yilingersis = null;
            CaseManager.MapFightTaiYuan MapFightTaiYuans = null;
            CaseManager.Tuokamake Tuokamakes = null;
            CaseManager.Population Populations = null;

            string state = "";
            string msg = "";
            //Dictionary<string, CaseManager.Kobe> Kobes = new Dictionary<string, CaseManager.Kobe>();
            //Dictionary<string, CaseManager.Identitycard> IdCards = new Dictionary<string, CaseManager.Identitycard>();
            //Dictionary<string, CaseManager.SelfSoftware> SelfSoftwares = new Dictionary<string, CaseManager.SelfSoftware>();
            //Dictionary<string, CaseManager.Yilingersi> Yilingersis = new Dictionary<string, CaseManager.Yilingersi>();
            //Dictionary<string, CaseManager.MapFightTaiYuan> MapFightTaiYuans = new Dictionary<string, CaseManager.MapFightTaiYuan>();
            //Dictionary<string, CaseManager.Tuokamake> Tuokamakes = new Dictionary<string, CaseManager.Tuokamake>();

            try
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var str = getStrFromByte(ref buffer);
                var v = str.Split('_');
                if (v.Length != 3)
                {
                    return;
                }
                else
                {
                    if (v[0].Length != 14)
                    {
                        return;
                    }
                    else
                    {
                        bool verifyOk = false;
                        try
                        {
                            if (true)
                            {
                                verifyOk = true;
                            }
                        }
                        catch
                        {
                            return;
                        }

                        if (verifyOk)
                        {
                            var year = int.Parse(v[0].Substring(0, 4));
                            var mounth = int.Parse(v[0].Substring(4, 2));
                            var day = int.Parse(v[0].Substring(6, 2));
                            var hour = int.Parse(v[0].Substring(8, 2));
                            var minute = int.Parse(v[0].Substring(10, 2));
                            var second = int.Parse(v[0].Substring(12, 2));
                            var time = new DateTime(year, mounth, day, hour, minute, second);
                            //if ((DateTime.Now - time).TotalSeconds < 15)
                            if (true)
                            {
                                state = v[1];

                                string e = state;
                                try
                                {
                                    switch (e)
                                    {
                                        case "BitCoinQuestionPage":
                                            {
                                                QuestionPageObjs = new CaseManager.BitCoinQuestionPage();
                                                msg = QuestionPageObjs.GetMsg();
                                            }; break;
                                        case "KekongHejubian":
                                            {
                                                var obj = new CaseManager.KekongHejubian();
                                                KekongHejubians = obj;
                                                msg = KekongHejubians.GetMsg();
                                            }; break;
                                        case "BlockChain":
                                            {
                                                return;
                                            }; break;
                                        case "Morsecode":
                                            {
                                                var obj = new CaseManager.Morsecode();
                                                Morsecodes = obj;
                                                msg = Morsecodes.GetMsg();
                                            }; break;
                                        case "Periodictable":
                                            {
                                                var obj = new CaseManager.Periodictable();
                                                Periodictables = obj;
                                                msg = Periodictables.GetMsg();
                                            }; break;
                                        case "QianHeYang":
                                            {
                                                var obj = new CaseManager.QianAndYang();
                                                QianAndYangs = obj;
                                                msg = QianAndYangs.GetMsg();
                                            }; break;
                                        case "Kobe":
                                            {
                                                var obj = new CaseManager.Kobe();
                                                Kobes = obj;
                                                msg = Kobes.GetMsg();
                                            }; break;
                                        case "Identitycard":
                                            {
                                                var obj = new CaseManager.Identitycard();
                                                IdCards = obj;
                                                msg = IdCards.GetMsg();
                                            }; break;
                                        case "SelfSoftware":
                                            {
                                                var obj = new CaseManager.SelfSoftware();
                                                SelfSoftwares = obj;
                                                msg = SelfSoftwares.GetMsg();
                                            }; break;
                                        case "Yilingersi":
                                            {
                                                var obj = new CaseManager.Yilingersi();
                                                Yilingersis = obj;
                                                msg = Yilingersis.GetMsg();
                                            }; break;
                                        case "MapFightTaiYuan":
                                            {
                                                var obj = new CaseManager.MapFightTaiYuan();
                                                MapFightTaiYuans = obj;
                                                msg = MapFightTaiYuans.GetMsg();
                                            }; break;
                                        case "TKMK":
                                            {
                                                return;
                                                //if (this.state.ContainsKey(session.SessionID)) { }
                                                //else
                                                //{
                                                //    this.state.Add(session.SessionID, e);
                                                //    var obj = new CaseManager.MapFightTaiYuan();
                                                //    MapFightTaiYuans.Add(session.SessionID, obj);
                                                //    string msg = MapFightTaiYuans[session.SessionID].GetMsg();
                                                //    session.Send(msg);
                                                //}
                                            }; break;
                                        case "Population":
                                            {
                                                var obj = new CaseManager.Population();
                                                Populations = obj;
                                                msg = Populations.GetMsg();
                                            }; break;
                                        default:
                                            {
                                                return;
                                            }
                                    }

                                    if (detail.ContainsKey(e))
                                    {
                                        detail[e]++;
                                    }
                                    else
                                    {
                                        detail.Add(e, 1);
                                    }
                                }
                                catch (Exception ee)
                                {
                                    var content = Newtonsoft.Json.JsonConvert.SerializeObject(ee);
                                    File.WriteAllText(DateTime.Now.ToString("yyyyMMddHHmmssff"), content);
                                    throw ee;
                                }
                                var sendData = Encoding.UTF8.GetBytes(msg);
                                try
                                {
                                    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

                                }
                                catch
                                {
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                }
                //var str = File.ReadAllText(path);
                //idata = Newtonsoft.Json.JsonConvert.DeserializeObject<IndexData>(str);
            }
            catch
            {
                return;
            }

            while (!result.CloseStatus.HasValue)
            {
                try
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var str = getStrFromByte(ref buffer);
                    try
                    {
                        switch (str)
                        {

                            case "1":
                                {

                                    switch (state)
                                    {
                                        case "BitCoinQuestionPage":
                                            {
                                                if (QuestionPageObjs != null)
                                                {
                                                    QuestionPageObjs.Continue();
                                                    msg = QuestionPageObjs.GetMsg();
                                                }
                                            }; break;
                                        case "KekongHejubian":
                                            {
                                                if (KekongHejubians != null)
                                                {
                                                    KekongHejubians.Continue();
                                                    msg = KekongHejubians.GetMsg();

                                                }
                                            }; break;
                                        case "Morsecode":
                                            {
                                                if (Morsecodes != null)
                                                {
                                                    Morsecodes.Continue();
                                                    msg = Morsecodes.GetMsg();
                                                }
                                            }; break;
                                        case "Periodictable":
                                            {
                                                if (Periodictables != null)
                                                {
                                                    Periodictables.Continue();
                                                    msg = Periodictables.GetMsg();
                                                }
                                            }; break;
                                        case "QianHeYang":
                                            {
                                                if (QianAndYangs != null)
                                                {
                                                    QianAndYangs.Continue();
                                                    msg = QianAndYangs.GetMsg();
                                                }
                                            }; break;
                                        case "Kobe":
                                            {
                                                if (Kobes != null)
                                                {
                                                    Kobes.Continue();
                                                    msg = Kobes.GetMsg();
                                                }
                                            }; break;
                                        case "Identitycard":
                                            {
                                                if (IdCards != null)
                                                {
                                                    IdCards.Continue();
                                                    msg = IdCards.GetMsg();

                                                }
                                            }; break;
                                        case "SelfSoftware":
                                            {
                                                if (SelfSoftwares != null)
                                                {
                                                    SelfSoftwares.Continue();
                                                    msg = SelfSoftwares.GetMsg();

                                                }
                                            }; break;
                                        case "Population":
                                            {
                                                if (Populations != null)
                                                {
                                                    Populations.setAsEmployee();
                                                    msg = Populations.GetMsg();
                                                }
                                            }; break;
                                    }

                                }; break;
                            case "2":
                                {

                                    switch (state)
                                    {
                                        case "BitCoinQuestionPage":
                                            {
                                                if (QuestionPageObjs != null)
                                                {
                                                    QuestionPageObjs.errorRecovery();
                                                    msg = QuestionPageObjs.GetMsg();

                                                }
                                            }; break;
                                        case "KekongHejubian":
                                            {
                                                if (KekongHejubians != null)
                                                {
                                                    KekongHejubians.errorRecovery();
                                                    msg = KekongHejubians.GetMsg();

                                                }
                                            }; break;
                                        case "Morsecode":
                                            {
                                                if (Morsecodes != null)
                                                {
                                                    Morsecodes.errorRecovery();
                                                    msg = Morsecodes.GetMsg();

                                                }
                                            }; break;
                                        case "Periodictable":
                                            {
                                                if (Periodictables != null)
                                                {
                                                    Periodictables.errorRecovery();
                                                    msg = Periodictables.GetMsg();

                                                }
                                            }; break;
                                        case "QianHeYang":
                                            {
                                                if (QianAndYangs != null)
                                                {
                                                    QianAndYangs.errorRecovery();
                                                    msg = QianAndYangs.GetMsg();

                                                }
                                            }; break;
                                        case "Kobe":
                                            {
                                                if (Kobes != null)
                                                {
                                                    Kobes.errorRecovery();
                                                    msg = Kobes.GetMsg();

                                                }
                                            }; break;
                                        case "Identitycard":
                                            {
                                                if (IdCards != null)
                                                {
                                                    IdCards.errorRecovery();
                                                    msg = IdCards.GetMsg();

                                                }
                                            }; break;
                                        case "SelfSoftware":
                                            {
                                                if (SelfSoftwares != null)
                                                {
                                                    SelfSoftwares.errorRecovery();
                                                    msg = SelfSoftwares.GetMsg();

                                                }
                                            }; break;
                                        case "Yilingersi":
                                            {
                                                if (Yilingersis != null)
                                                {
                                                    Yilingersis.errorRecovery();
                                                    msg = Yilingersis.GetMsg();

                                                }
                                            }; break;
                                    }

                                }; break;
                            case "score":
                                {
                                    switch (state)
                                    {
                                        case "BitCoinQuestionPage":
                                            {
                                                if (QuestionPageObjs != null)
                                                {
                                                    msg = QuestionPageObjs.GetScore();

                                                }
                                            }; break;
                                        case "KekongHejubian":
                                            {
                                                if (KekongHejubians != null)
                                                {
                                                    msg = KekongHejubians.GetScore();

                                                }
                                            }; break;
                                        case "Morsecode":
                                            {
                                                if (Morsecodes != null)
                                                {
                                                    msg = Morsecodes.GetScore();

                                                }
                                            }; break;
                                        case "Periodictable":
                                            {
                                                if (Periodictables != null)
                                                {
                                                    msg = Periodictables.GetScore();

                                                }
                                            }; break;
                                        case "QianHeYang":
                                            {
                                                if (QianAndYangs != null)
                                                {
                                                    msg = QianAndYangs.GetScore();

                                                }
                                            }; break;
                                        case "Kobe":
                                            {
                                                if (Kobes != null)
                                                {
                                                    msg = Kobes.GetScore();

                                                }
                                            }; break;
                                        case "SelfSoftware":
                                            {
                                                if (SelfSoftwares != null)
                                                {
                                                    msg = SelfSoftwares.GetScore();

                                                }
                                            }; break;
                                    }
                                }; break;
                            case "top":
                                {
                                    switch (state)
                                    {
                                        case "Yilingersi":
                                            {
                                                if (Yilingersis != null)
                                                {
                                                    Yilingersis.move(0);
                                                    msg = Yilingersis.GetMsg();

                                                }
                                            }
                                            break;
                                        case "MapFightTaiYuan":
                                            {
                                                if (MapFightTaiYuans != null)
                                                {
                                                    MapFightTaiYuans.move(0);
                                                    msg = MapFightTaiYuans.GetMsg();

                                                }
                                            }; break;
                                    }
                                }; break;
                            case "left":
                                {
                                    switch (state)
                                    {
                                        case "Yilingersi":
                                            {
                                                if (Yilingersis != null)
                                                {
                                                    Yilingersis.move(3);
                                                    msg = Yilingersis.GetMsg();

                                                }
                                            }
                                            break;
                                        case "MapFightTaiYuan":
                                            {
                                                if (MapFightTaiYuans != null)
                                                {
                                                    MapFightTaiYuans.move(3);
                                                    msg = MapFightTaiYuans.GetMsg();

                                                }
                                            }; break;
                                    }
                                }; break;
                            case "bottom":
                                {
                                    switch (state)
                                    {
                                        case "Yilingersi":
                                            {
                                                if (Yilingersis != null)
                                                {
                                                    Yilingersis.move(2);
                                                    msg = Yilingersis.GetMsg();

                                                }
                                            }
                                            break;
                                        case "MapFightTaiYuan":
                                            {
                                                if (MapFightTaiYuans != null)
                                                {
                                                    MapFightTaiYuans.move(2);
                                                    msg = MapFightTaiYuans.GetMsg();

                                                }
                                            }; break;
                                    }
                                }; break;
                            case "right":
                                {
                                    switch (state)
                                    {
                                        case "Yilingersi":
                                            {
                                                if (Yilingersis != null)
                                                {
                                                    Yilingersis.move(1);
                                                    msg = Yilingersis.GetMsg();

                                                }
                                            }
                                            break;
                                        case "MapFightTaiYuan":
                                            {
                                                if (MapFightTaiYuans != null)
                                                {
                                                    MapFightTaiYuans.move(1);
                                                    msg = MapFightTaiYuans.GetMsg();

                                                }
                                            }; break;
                                    }
                                }; break;
                            case "back":
                                {
                                    switch (state)
                                    {
                                        case "MapFightTaiYuan":
                                            {
                                                if (MapFightTaiYuans != null)
                                                {
                                                    MapFightTaiYuans.move(4);
                                                    msg = MapFightTaiYuans.GetMsg();

                                                }
                                            }; break;
                                    }
                                }; break;

                            case "A":
                                {
                                    switch (state)
                                    {
                                        case "Population":
                                            {
                                                if (Populations != null)
                                                {
                                                    Populations.Action("A");
                                                    msg = Populations.GetMsg();
                                                }

                                            }; break;
                                    }

                                }; break;
                            case "I":
                                {
                                    switch (state)
                                    {
                                        case "Population":
                                            {
                                                if (Populations != null)
                                                {
                                                    Populations.Action("I");
                                                    msg = Populations.GetMsg();
                                                }

                                            }; break;
                                    }
                                }; break;
                            case "B":
                                {
                                    switch (state)
                                    {
                                        case "Population":
                                            {
                                                if (Populations != null)
                                                {
                                                    Populations.Action("B");
                                                    msg = Populations.GetMsg();
                                                }

                                            }; break;
                                    }
                                }; break;
                            case "Next":
                                {
                                    if (Populations != null)
                                    {
                                        Populations.NextYear();
                                        msg = Populations.GetMsg();
                                    }
                                }; break;

                            default:
                                {
                                    if (EccCore.Address.ValidateBitcoinAddress(str))
                                    {


                                        switch (state)
                                        {
                                            case "BitCoinQuestionPage":
                                                {
                                                    if (QuestionPageObjs != null)
                                                    {
                                                        msg = QuestionPageObjs.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "KekongHejubian":
                                                {
                                                    if (KekongHejubians != null)
                                                    {
                                                        msg = KekongHejubians.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "Morsecode":
                                                {
                                                    if (Morsecodes != null)
                                                    {
                                                        msg = Morsecodes.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "Periodictable":
                                                {
                                                    if (Periodictables != null)
                                                    {
                                                        msg = Periodictables.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "QianHeYang":
                                                {
                                                    if (QianAndYangs != null)
                                                    {
                                                        msg = QianAndYangs.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "Kobe":
                                                {
                                                    if (Kobes != null)
                                                    {
                                                        msg = Kobes.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "Identitycard":
                                                {
                                                    if (IdCards != null)
                                                    {
                                                        msg = IdCards.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "SelfSoftware":
                                                {
                                                    if (SelfSoftwares != null)
                                                    {
                                                        msg = SelfSoftwares.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "Yilingersi":
                                                {
                                                    if (Yilingersis != null)
                                                    {
                                                        msg = Yilingersis.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                            case "MapFightTaiYuan":
                                                {
                                                    if (MapFightTaiYuans != null)
                                                    {
                                                        msg = MapFightTaiYuans.SaveAddress(str.Trim());

                                                    }
                                                }; break;
                                        }


                                    }
                                    else
                                    {
                                        break;
                                    }
                                }; break;
                        }
                    }
                    catch (Exception ee)
                    {
                        var content = Newtonsoft.Json.JsonConvert.SerializeObject(ee);
                        File.WriteAllText(DateTime.Now.ToString("yyyyMMddHHmmssff"), content);
                        throw ee;
                    }
                    if (msg == "closeXXXX")
                    {
                        break;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(msg)) { }
                        else
                        {
                            var sendData = Encoding.UTF8.GetBytes(msg);
                            try
                            {
                                await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

                            }
                            catch
                            {
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    return;
                }
            }

            try
            {
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            catch
            {
                return;
            }

        }

        private string getStrFromByte(ref byte[] buffer)
        {
            string str = "";

            for (var i = buffer.Length - 1; i >= 0; i--)
            {
                if (buffer[i] != 0)
                {
                    str = Encoding.UTF8.GetString(buffer.Take(i + 1).ToArray()).Trim();
                    break;
                }

            }
            buffer = new byte[size];
            return str;

        }

        private string Find(DateTime dateNow, string problem)
        {
            for (int i = 1; i < 100; i++)
            {
                var path = $"{dateNow.AddDays(-i).ToString("yyyyMMdd")}.txt";
                if (File.Exists(path))
                {
                    var str = File.ReadAllText(path);
                    var idata = Newtonsoft.Json.JsonConvert.DeserializeObject<IndexData>(str);
                    if (idata.problems.Contains(problem))
                    {
                        var index = idata.problems.FindIndex(item => item == problem);
                        if (string.IsNullOrEmpty(idata.answers[index]))
                        {
                            continue;
                        }
                        else
                        {
                            return idata.answers[index];
                        }
                    }
                }
            }
            return "没有找到答案";
            // throw new NotImplementedException();
        }

        private objReceived getObj(string resultGet)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<objReceived>(resultGet);
            }
            catch
            {
                return null;
            }
            //throw new NotImplementedException();
        }

        public class IndexData
        {
            public string tag { get; set; }
            public List<string> problems { get; set; }
            public List<string> answers { get; set; }
        }
        public class objReceived
        {
            public string t { get; set; }
            public string msg { get; set; }
        }


    }
}
