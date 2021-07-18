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
        public async Task SendTestMessage(string message)
        {
            await Clients.All.TestMessage(message);
        }
    }
}
