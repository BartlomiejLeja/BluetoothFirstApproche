using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalIRServer.Model;

namespace SignalIRServer.Hubs
{
    public class Broadcaster : Hub
    {
        //public override  Task OnConnectedAsync()
        //{
        //    return Clients.Client(Context.ConnectionId).InvokeAsync("Send", message);
        //}
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
            return Clients.All.SendAsync("ChangeLightState", isLightOn, lightNumber);
        }
        
        public Task CheckStatusOfLights(bool x)
        {
            Console.WriteLine("CheckStatusOfLights");
            return Clients.Others.SendAsync("CheckStatusOfLights", x);
        }

        public Task SendLightState(int lightID, bool lightStatus, string name)
        {
            Console.WriteLine("SendLightState");
            return Clients.Others.SendAsync("SendLightState", lightID,lightStatus,name);
        }
        public Task InvokeStatisticsService(bool isStatisticsServiceOn)
        {
            Console.WriteLine("InvokeStatisticsService");
            return Clients.Others.SendAsync("InvokeStatisticsService", isStatisticsServiceOn);
        }
    }     
}
