using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SignalIRServer.Services;

namespace SignalIRServer.Hubs
{
    public class Broadcaster : Hub
    {
        private readonly ILightsService _lightsService;
        private readonly ILogger _logger;

        public Broadcaster(ILightsService lightsService, ILogger<Broadcaster> logger)
        {
            _logger = logger;
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
         //   _lightsService.SetNewBulbStatus(lightNumber, isLightOn, DateTime.Now);
            return Clients.All.SendAsync("ChangeLightState", isLightOn, lightNumber);
        }
        
        public Task SendLightState(int lightID, bool lightStatus, DateTime dateTime,string serializedLightBulbModel)
        {
            _logger.LogInformation($"SendLightState {dateTime}");
            _lightsService.SetNewBulbStatus(lightID, lightStatus, dateTime);
            var jsonConvertetLightBulbModel = JsonConvert.SerializeObject(_lightsService.GetLightModel(lightID));
            return Clients.Others.SendAsync("SendLightState", lightID,lightStatus, dateTime, jsonConvertetLightBulbModel);
        }
        
        public Task SendInitialLightCollection(string lightsCollection)
        {
            Console.WriteLine("SendInitialLightCollection");
            return Clients.Others.SendAsync("SendInitialLightCollection", lightsCollection);
        }
    } 
}
