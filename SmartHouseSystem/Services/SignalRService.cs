using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

            connection.On<bool>("CheckStatus", async data=>
            {
                Debug.WriteLine("Cheking status in uwp app");
                var isOn= await wiFiService.CheckStatusOfLight();
                await InvokeSendMethod(isOn);
            });

            await connection.StartAsync();
        }

        public async Task InvokeSendMethod(string isOn)
        {
            var lightStatus = JsonConvert.DeserializeObject<LightModel>(isOn);
            await connection.InvokeAsync("Send", lightStatus.state);
        }
      
    }
}
    

