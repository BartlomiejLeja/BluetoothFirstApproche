using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace SignalIRServer.Services
{
    public class SignalRClientService : ISignalRClientService
    {
        private HubConnection _connection;

        public async Task Connect()
        {
            _connection = new HubConnectionBuilder()
                  //  .WithUrl("https://signalirserver20181021093049.azurewebsites.net/LightApp")
                .WithUrl("http://localhost:51690/LightApp")
                .WithConsoleLogger(LogLevel.Trace)
                .Build();

            await _connection.StartAsync();
        }

        public async Task InvokeSendSchedulePlan(string schedulePlanJson)
        {
            await Connect();
            Debug.WriteLine("schedulePlanJson");
            await _connection.InvokeAsync("SendSchedulePlan", schedulePlanJson);
        }
    }
}
