using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysqlCore
{
    class Connection
    {
        static string _ConnectionStr = "";
        //private static readonly IConfigurationBuilder ConfigurationBuilder = new ConfigurationBuilder();
        public static string ConnectionStr
        {
            get
            {
                if (string.IsNullOrEmpty(_ConnectionStr))
                {
                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd")}读取了配置文件Connection.json");
                    var jsonStr = File.ReadAllText("Connection.json");
                    var connecttionObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Obj>(jsonStr);
                    _ConnectionStr = connecttionObj.connectStr; 
                }
                else
                {

                }
                return _ConnectionStr;
            }
        }


        class Obj
        {
            public string connectStr { get; set; }
        }
    }
}
