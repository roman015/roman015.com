using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace dotNetWorker.Services
{
    public class ExampleTaskBoss
    {
        private readonly IConfiguration _config;
        private readonly DiscordSocketClient _client;
        private readonly IMessageChannel _channel;

        public ExampleTaskBoss(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<IConfiguration>();
            _channel = (IMessageChannel)_client
                .GetChannel(ulong.Parse(_config["channel-shell-output"]));
        }
        public async void StartTask()
        {
            // TODO: start a task, for now just write a message into discord
            await _channel.SendMessageAsync("Message from ExampleTaskBoss - StartTask!");
            for (int i = 1; i <= 5; i++) { 
                await Task.Delay(2000);
                await _channel.SendMessageAsync($"Loop iteration = {i}");
            }
            await _channel.SendMessageAsync("Loop over");
        }
    }
}
