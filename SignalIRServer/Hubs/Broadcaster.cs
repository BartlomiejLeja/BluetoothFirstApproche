using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalIRServer.Services;

namespace SignalIRServer.Hubs
{
    public class Broadcaster : Hub
    {
        private ILightsService _lightsService;

        public Broadcaster(ILightsService lightsService)
        {
            _lightsService = lightsService;
        }

        public override Task OnConnectedAsync()
        {
            var lightsCollection = _lightsService.GetListOfLightBullbs();
            var jsonConvertetLightsCollection = JsonConvert.SerializeObject(lightsCollection);
            return Clients.Client(Context.ConnectionId).SendAsync("SendInitialLightCollection", jsonConvertetLightsCollection);
        }

        public Task Subscribe(string clientName)
        {
            return Groups.AddAsync(Context.ConnectionId, clientName);
        }

        public Task Unsubscribe(string clientName)
        {
            return Groups.RemoveAsync(Context.ConnectionId, clientName);
        }
        
        public Task ChangeLightState(bool isLightOn, int lightNumber)
        {
            Console.WriteLine("TrigerLightSevice");
            _lightsService.SetNewBulbStatus(lightNumber, isLightOn);
            return Clients.All.SendAsync("ChangeLightState", isLightOn, lightNumber);
        }
        
        public Task CheckStatusOfLights(bool x)
        {
            Console.WriteLine("CheckStatusOfLights");
            return Clients.Others.SendAsync("CheckStatusOfLights", x);
        }

        public Task SendLightState(int lightID, bool lightStatus)
        {
            Console.WriteLine("SendLightState");
            _lightsService.SetNewBulbStatus(lightID, lightStatus);
            return Clients.Others.SendAsync("SendLightState", lightID,lightStatus);
        }
        public Task InvokeStatisticsService(bool isStatisticsServiceOn)
        {
            Console.WriteLine("InvokeStatisticsService");
            return Clients.Others.SendAsync("InvokeStatisticsService", isStatisticsServiceOn);
        }

        public Task SendInitialLightCollection(string lightsCollection)
        {
            Console.WriteLine("SendInitialLightCollection");
            return Clients.Others.SendAsync("SendInitialLightCollection", lightsCollection);
        }
    } 
}
