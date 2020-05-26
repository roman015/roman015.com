using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace dotNetWorker.Services
{
    public class ExampleTaskBoss
    {
        private readonly IConfiguration _config;
        private readonly DiscordSocketClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public ExampleTaskBoss(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<IConfiguration>();            
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
        }
        public async void StartTask()
        {
            _logger.LogInformation("begin StartTask() ");
            await (_client
                .GetChannel(ulong.Parse(_config["channel-shell-output"])) as IMessageChannel)
                .SendMessageAsync("hello! from typecasting");

            var temp_socket_channel = _client
                .GetChannel(ulong.Parse(_config["channel-shell-output"]));
            
            if (temp_socket_channel == null) {
                _logger.LogInformation("received null, returning");
                return;
            }
            _logger.LogInformation("got something, attempting typecast");

            var channel = temp_socket_channel as IMessageChannel;
            // TODO: start a task, for now just write a message into discord
            await channel.SendMessageAsync("hello from object!");
            for (int i = 1; i <= 5; i++) { 
                await Task.Delay(2000);
                await channel.SendMessageAsync($"Loop iteration = {i}");
            }
            await channel.SendMessageAsync("Loop over");
        }
    }
}
