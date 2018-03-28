using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTHomeAutomationWebApplication.Controllers
{
    [Route("api/[controller]")]
    public class CounterController : Controller
    {
        [HttpGet("[action]")]
        public async Task SiglnalRTestAsync()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chat")
                .WithConsoleLogger()
                .Build();

            connection.On<string>("Send", data =>
            {
                Console.WriteLine($"Received: {data}");
            });

            await connection.StartAsync();
            
            await connection.InvokeAsync("Send", "TestTestTestKurwaMacTest");
        }

    }
}
