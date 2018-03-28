using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace IoTHomeAutomationWebApplication.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private HubConnection connection
        = new HubConnectionBuilder()
                .WithUrl("http://localhost:51691/message")
                .WithConsoleLogger(LogLevel.Trace)
                .Build();

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        public IActionResult ConnectToSignalR()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:51691/message")
                .WithConsoleLogger()
                .Build();

            connection.On<string>("Send", data =>
            {
                Debug.WriteLine($"Received: {data}");
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Received: {data}");
            });

            connection.StartAsync();
            

         // await connection.InvokeAsync("Send", "TestTestTestKurwaMacTest");
           return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> InvokeSendMethod()
        {
           await connection.StartAsync();
            await connection.InvokeAsync("Send", "InvokeSendMethod from sp.net core client");
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> LightUp()
        {
            await connection.StartAsync();
            await connection.InvokeAsync("ChangeLightState", true);
            return Ok();
        }


        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

    }
}
