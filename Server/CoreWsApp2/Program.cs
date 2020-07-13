using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreWsApp2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(new string[] { "http://127.0.0.1:20639" }).Build().Run();
            //Console.WriteLine($"ÊÇ·ñ{"http://10.80.52.194:20630"}? Y/any key");
            //if (Console.ReadLine() == "Y")
            //{
            //    CreateWebHostBuilder(new string[] { "http://10.80.52.194:20630" }).Build().Run();
            //}
            //else
            //{
            //    Console.WriteLine($"ÊÇ·ñ{"http://10.80.52.194:20639"}? Y/any key");
            //    if (Console.ReadLine() == "Y")
            //    {
            //        CreateWebHostBuilder(new string[] { "http://10.80.52.194:20639" }).Build().Run();
            //    }
            //}



        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
              WebHost.CreateDefaultBuilder(args).Configure(item => item.UseForwardedHeaders(new ForwardedHeadersOptions
              {
                  ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
              }))
              .UseUrls(args[0])

                  .UseStartup<Startup>();
    }
}
