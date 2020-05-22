using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.WebSocket;

namespace dotNetWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private DiscordSocketClient _client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting worker service!");

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });
            _client.Log += Log;

            var token = Environment.GetEnvironmentVariable("WORKER_DISCORD_BOT_TOKEN");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _client.Ready += () => 
            {
                _logger.LogInformation("Bot is connected!");
                return Task.CompletedTask;
            };

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage message)
        {
            // Write logs to systemd/journalctl directly
            _logger.LogInformation(message.ToString());
            return Task.CompletedTask;
        }
    }
}
