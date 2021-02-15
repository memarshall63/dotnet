using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));
            Serilog.Debugging.SelfLog.Enable(Console.Error);    
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                    webBuilder.UseStartup<Startup>()
                        .UseSerilog((hostingContext, loggerConfiguration) => {
                            loggerConfiguration
                                .ReadFrom.Configuration(hostingContext.Configuration)
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                                .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment)
                                .Enrich.AtLevel(Serilog.Events.LogEventLevel.Error, e => e.WithProperty("Version", typeof(Program).Assembly.ImageRuntimeVersion));
                        });
                    }
                );
    }
}
