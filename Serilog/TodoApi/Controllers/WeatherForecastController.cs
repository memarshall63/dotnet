using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var rtn = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            _logger.LogWarning("Warning: Hello, World!");
            _logger.LogInformation("TimeSpan:{ts}",TimeSpan.FromDays(1));

            int a = 10, b = 0;
            try
            {
                _logger.LogDebug("Dividing {@A} by {@B}", a, b);
                Console.WriteLine(a / b);
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong: Exception={@ex}", ex);
            }
            finally
            {
                //_logger.CloseAndFlush();
            }

            return(rtn);
        }
    }
}
