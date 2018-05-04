using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace LightControlerWebApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
       // public string BulbStatus;
       // private HubConnection connection
       //= new HubConnectionBuilder()
       //        .WithUrl("http://localhost:51691/message")
       //        .WithConsoleLogger(LogLevel.Trace)
       //        .Build();

       // [HttpGet("[action]")]
       // public IActionResult ConnectToSignalR()
       // {
       //     connection = new HubConnectionBuilder()
       //         .WithUrl("http://localhost:51691/message")
       //         .WithConsoleLogger()
       //         .Build();

       //     connection.On<string>("Send", data =>
       //     {
       //         Console.BackgroundColor = ConsoleColor.Blue;
       //         Console.ForegroundColor = ConsoleColor.White;
       //         BulbStatus = data;
       //         Console.WriteLine($"Received: {data}");
       //     });

       //     connection.StartAsync();
       //     // await connection.InvokeAsync("Send", "TestTestTestKurwaMacTest");
       //     return Ok();
       // }

       // [HttpGet("[action]")]
       // public async Task<IActionResult> InvokeSendMethod()
       // {
       //     await connection.StartAsync();
       //     await connection.InvokeAsync("Send", "InvokeSendMethod from sp.net core client");
       //     return Ok();
       // }

       // [HttpGet("[action]")]
       // public async Task<IActionResult> LightUp()
       // {
       //     await connection.StartAsync();
       //     await connection.InvokeAsync("ChangeLightState", true);
       //     return Ok();
       // }

       // [HttpGet("[action]")]
       // public async Task<IActionResult> CheckStatus()
       // {
       //     await connection.StartAsync();
       //     await connection.InvokeAsync("CheckStatusOfLight");
       //     return Ok();
       // }

       // [HttpGet("[action]")]
       // public IEnumerable<string> GetBulbStatus()
       // {
       //     var tab = new string[1];
       //     tab[0] = BulbStatus;
       //     return tab;
       // }

    }
}
