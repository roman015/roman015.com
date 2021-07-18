using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<NotificationHub> logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            this.logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            logger.Log(LogLevel.Information, "OnConnectedAsync");
            return new Task(() => { }); // TODO : Cleanup
        }
        
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            logger.Log(LogLevel.Information, "OnDisconnectedAsync :" + exception?.Message);
            return new Task(() => { }); // TODO : Cleanup
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
