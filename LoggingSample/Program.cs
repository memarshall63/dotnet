using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace LoggingSample
{
    public class Account
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingSample.Program", LogLevel.Trace)
                    // .AddConsole();
                    // .AddJsonConsole()
                    .AddSimpleConsole()
                    ;
            }
            );

            ILogger logger = loggerFactory.CreateLogger<Program>();
            ILogger logger2 = loggerFactory.CreateLogger("CategoryX");
            using (logger.BeginScope("[enumerate]"))
            {
                logger.LogCritical("This is critical message.");
                logger.LogError("This is error message.");
                logger.LogWarning("This is warning message.");
                logger.LogInformation("This is log message.");
                logger.LogDebug("This is debug message.");
                logger.LogTrace("This is trace message.");
            }
            //logger.Log
            // logger.Log("This is log message.");
            // logger.LogInformation("This is log message.");
            // logger.LogInformation("This is log message.");
            // logger.LogInformation("This is log message.");

            Account account = new Account
            {
                Name = "John Doe",
                Email = "john@nuget.org",
                DOB = new DateTime(1980, 2, 20, 0, 0, 0, DateTimeKind.Utc),
            };

            string json = JsonConvert.SerializeObject(account, Formatting.Indented);
            //Console.WriteLine(json);
            logger.LogWarning("Object {json}", account);

            logger2.LogWarning("This is a category X message {json}", json);

        }
    }
}
