using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalIRServer.Hubs
{
   
    public class Broadcaster :Hub
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

        public Task Send(string message)
        {
            Console.WriteLine("TestTestTEstETSTETSTETST");
            return Clients.All.SendAsync("Send", message);
        }

        public Task ChangeLightState(bool isLightOn)
        {
            Console.WriteLine("TrigerLightSevice");
            return Clients.Others.SendAsync("TurnOnLight",isLightOn);
        }

        public Task CheckStatusOfLight()
        {
            Console.WriteLine("CheckingStatusOfLight");
            return Clients.Others.SendAsync("CheckStatus",true);
        }
    }

    public interface IBroadcaster
    {

    }
}
