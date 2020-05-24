using System;
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

        public ExampleTaskBoss(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<IConfiguration>();
        }
        public void StartTask()
        {
            // TODO: start a task, for now just write a message into discord
            (_client
                .GetChannel(ulong.Parse(_config["channel-shell-output"])) as IMessageChannel)
                .SendMessageAsync("Message from ExampleTaskBoss - StartTask!");
        }
    }
}
