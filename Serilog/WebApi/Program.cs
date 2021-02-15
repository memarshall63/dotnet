using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Debugging;

namespace WebApi
{
    public class Program
    {
        public class LoginData
        {
            public string Username;
            public string Password;
        }

        public static void Main(string[] args)
        {
        var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration, sectionName: "Serilog")
            .CreateLogger();

        logger.Information("Args: {a}", args);
        do
        {
            logger.ForContext<Program>().Information("Hello, world!");
            logger.ForContext<Program>().Error("Hello, world!");
            logger.ForContext(Constants.SourceContextPropertyName, "Microsoft").Warning("Hello, world!");
            logger.ForContext(Constants.SourceContextPropertyName, "Microsoft").Error("Hello, world!");
            logger.ForContext(Constants.SourceContextPropertyName, "MyApp.Something.Tricky").Verbose("Hello, world!");

            logger.Information("Destructure with max object nesting depth:\n{@NestedObject}",
                new { FiveDeep = new { Two = new { Three = new { Four = new { Five = "the end" } } } } });

            logger.Information("Destructure with max string length:\n{@LongString}",
                new { TwentyChars = "0123456789abcdefghij" });

            logger.Information("Destructure with max collection count:\n{@BigData}",
                new { TenItems = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" } });

            logger.Information("Destructure with policy to strip password:\n{@LoginData}",
                new LoginData { Username = "BGates", Password = "isityearoflinuxyet" });

            Console.WriteLine("\nPress \"q\" to quit, or any other key to run again.\n");
        }
        while (!args.Contains("--run-once") && (Console.ReadKey().KeyChar != 'q'));


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseSerilog((hostingContext, loggerConfiguration) => {
                            loggerConfiguration
                                .ReadFrom.Configuration(hostingContext.Configuration);
                        });
                    }
                );
    }
}
