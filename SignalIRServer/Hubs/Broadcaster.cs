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
            Console.WriteLine("Send Mmethod invoke");
            return Clients.All.InvokeAsync("Send", message);
        }
    }

    public interface IBroadcaster
    {

    }
}
