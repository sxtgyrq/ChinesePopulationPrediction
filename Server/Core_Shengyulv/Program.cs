using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Core_Shengyulv
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string msg = @"
这个程序的目的是模拟政府行为-企业行为-个人行为对整个社会生育率的影响。
debug
";

            Console.WriteLine("输入 “debug”，运行编程配置；输入其他任意内容，运行生产配置;");

            Thread.CurrentThread.Name = "MainOnly";
            var select = Console.ReadLine();
            MainThreadAsync(select);
            AssistThread(select);

            //  DataConmunicate.Do(select);

            //            Shengyulv syl = new Shengyulv();
            //            employee ee = null;
            //            employer er = null;
            //            string select = "";

            //            while (ee == null || er == null)
            //            {
            //                Console.WriteLine("ee雇员，er或其他为雇主");
            //                select = Console.ReadLine();
            //                if (select == "ee")
            //                {
            //                    ee = new employee();
            //                    var listAction = new byte[20]
            //                    {
            //                        0,0,0, 0,0,
            //                        0,0,0, 0,0,
            //                        0,0,0, 0,0,
            //                        0,0,0, 0,0
            //                    };

            //                    do
            //                    {
            //                        var msg = @"
            //1.恋爱
            //2.结婚
            //3.生一胎
            //4.生二胎
            //5.亲子
            //6.教育
            //7.奋斗
            //8.变为单收入家庭";
            //                        Console.WriteLine($"请输入你在今年运营策略:{msg}");
            //                        var result = byte.Parse(Console.ReadLine());
            //                        listAction[ee.Step] = result;
            //                        syl.DealWithEmployeeResult(result);
            //                    }
            //                    while (er.next()); 
            //                    er = null;
            //                }
            //                else
            //                {
            //                    er = new employer();
            //                    var listAction = new byte[20]
            //                    {
            //                        0,0,0, 0,0,
            //                        0,0,0, 0,0,
            //                        0,0,0, 0,0,
            //                        0,0,0, 0,0
            //                    };

            //                    do
            //                    {
            //                        var msg = @"
            //1.鼓吹996是福报
            //2.只招16-35岁年轻人
            //3.不招聘未婚未育女性
            //4.研发新技术
            //5.产业转移
            //6.提高福利";
            //                        Console.WriteLine($"请输入你在今年运营策略:{msg}");
            //                        var result = byte.Parse(Console.ReadLine());
            //                        listAction[er.Step] = result;
            //                        syl.DealWithEmployerResult(result);
            //                    }
            //                    while (er.next());
            //                    while (er.next())
            //                    {


            //                    }
            //                    er = null;
            //                }

            //            }




            //            Console.WriteLine("Hello World!");

        }

        private static void AssistThread(string select)
        {
            Thread ThMain = new Thread(() => Assist.Do(select));
            ThMain.Name = "Assist";
            ThMain.Start();
        }

        private static void MainThreadAsync(string select)
        {
            //_ = DataConmunicate.DoAsync(select);

            Thread ThMain = new Thread(() => DataConmunicate.DoAsync(select)  );
            ThMain.Name = "MainOnly";
            ThMain.Start();

            //throw new NotImplementedException();
        }
    }
}
