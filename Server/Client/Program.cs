using System;
using System.Runtime.CompilerServices;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // string ip = "10.80.52.218";

            Employee ee = null;
            while (true)
            {
                if (ee == null)
                {
                    Console.WriteLine("输入命令");

                    var command = Console.ReadLine();
                    if (command.ToUpper() == "EXIT")
                    {
                        break;
                    }
                    else if (command.ToUpper() == "EE")
                    {
                        ee = new Employee();
                    }
                }
                if (ee != null)
                {
                    Console.WriteLine(ee.ageDisplay);
                    Console.WriteLine(ee.yearDisplay);
                    Console.WriteLine(ee.ageSumSave);
                    Console.WriteLine(ee.childrenInfo);
                    Console.WriteLine(ee.selection);

                    var command = Console.ReadLine();
                    ee.DealWithCommand(command);
                }

            }
            Console.WriteLine("Hello World!");
        }
    }
}
