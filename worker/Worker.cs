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
            _logger.LogInformation("Creating Discord client object");
            _client = new DiscordSocketClient();
            var token = Environment.GetEnvironmentVariable("WORKER_DISCORD_BOT_TOKEN");
            _logger.LogInformation("Attempting to login as bot");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            _logger.LogInformation("Hopefully logged in now!");

            // Block this task until the program is closed.
            await Task.Delay(-1);


            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Information - Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogWarning("Warning - Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogError("Error - Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogCritical("Critical - Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
