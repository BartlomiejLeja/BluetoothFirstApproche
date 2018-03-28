using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SmartHouseSystem.Services
{
    public class SignalRService : ISignalRService
    {
        private HubConnection connection;
        public async Task Connect(IWiFiService wiFiService)
        {
             connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:51691/message")
                .WithConsoleLogger(LogLevel.Trace)
                .Build();

            connection.On<string>("Send", data =>
            {
                Debug.WriteLine($"Received: {data}");
            });

            connection.On<bool>("TurnOnLight", async data =>
            {
                Debug.WriteLine("You light up light");
                await wiFiService.SendHttpRequestAsync(data);
            }
            );

            await connection.StartAsync();           
        }

        public async Task InvokeSendMethod()
        {
            await connection.InvokeAsync("Send", "Test method from UWP client");
        }
      
    }
}
    

