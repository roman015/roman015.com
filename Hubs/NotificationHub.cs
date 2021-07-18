using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roman015API.Hubs
{
    public interface INotificationHub
    {
        Task TestMessage(string message);
    }

    public class NotificationHub : Hub<INotificationHub>
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("OnConnectedAsync");
            return null;
        }
        
        public override Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine("OnDisconnectedAsync :" + e.Message);
            return null;
        }

        public async Task SendTestMessage(string message)
        {
            Console.WriteLine("SendTestMessage");
            try
            {
                await Clients.All.TestMessage(message);
            }
            catch(Exception e)
            {
                Console.WriteLine("SendTestMessage : " + e.Message);
            }
        }
    }
}
