﻿using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string msg = @"
这个程序的目的是模拟政府行为-企业行为-个人行为对整个社会生育率的影响。
debug
";
            Console.WriteLine("输入 “debug”，运行编程配置；输入其他任意内容，运行生产配置;");
            var select = Console.ReadLine();
            MainThread(select);
            AssistThread(select);
        }

        private static void AssistThread(string select)
        {
            throw new NotImplementedException();
        }

        private static void MainThread(string select)
        {
            throw new NotImplementedException();
        }
    }
}
