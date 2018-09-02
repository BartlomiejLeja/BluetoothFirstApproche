using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NewestSignalRService.Hubs
{
    public class Broadcaster : Hub
    {

        //public override  Task OnConnectedAsync()
        //{
        //    return Clients.Client(Context.ConnectionId).InvokeAsync("Send", message);
        //}
        public Task Subscribe(string clientName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, clientName);
        }

        public Task Unsubscribe(string clientName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, clientName);
        }

        public Task Send(string message)
        {
            Console.WriteLine("TestTestTEstETSTETSTETST");
            return Clients.All.SendAsync("Send", message);
        }

        public Task ChangeLightState(bool isLightOn)
        {
            Console.WriteLine("TrigerLightSevice");
            return Clients.All.SendAsync("ChangeLightState", isLightOn);
        }

        public Task CheckStatusOfLight()
        {
            Console.WriteLine("CheckingStatusOfLight");
            return Clients.Others.SendAsync("CheckStatusOfLight", true);
        }

        public Task SendLightState(bool isLightOn)
        {
            Console.WriteLine("SendingLightState");
            return Clients.Others.SendAsync("SendLightState", isLightOn);
        }
        public Task SendLightStatisticData(int timeOn, int timeOff)
        {
            Console.WriteLine("SendingStatisticData");
            return Clients.Others.SendAsync("StatisticData", timeOn, timeOff);
        }
    }
}
