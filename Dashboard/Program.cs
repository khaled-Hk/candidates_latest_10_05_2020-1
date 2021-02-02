using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PrintePdf;

namespace Dashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        var prc = new ProcManager();
                        prc.KillByPort(5000);
                        prc.KillByPort(5001);
                        prc.KillByPort(8080);
                        options.Listen(IPAddress.Loopback, 5000);
                        options.Listen(IPAddress.Loopback, 5001);
                        //options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                        //{
                        //    listenOptions.UseHttps("localhost.pfx", "Tuk6puPb5pZ87JCX");
                        //});
                    });
                    webBuilder.UseStartup<Startup>();

                    //.UseUrls("https://localhost:5001");
                });
    }
}
