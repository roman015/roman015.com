using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using dotNetWorker.Services;


namespace dotNetWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private DiscordSocketClient _client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            // create the configuration
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");  

            // build the configuration and assign to _config          
            _config = _builder.Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // This method represents the background service we are running

            // call ConfigureServices to create the ServiceCollection/Provider for passing around the services
            using (var services = ConfigureServices())
            {
                // get the client and assign to client 
                // you get the services via GetRequiredService<T>
                var client = services.GetRequiredService<DiscordSocketClient>();
                _client = client;

                // setup logging and the ready event
                client.Log += LogAsync;
                client.Ready += ReadyAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                // this is where we get the Token, and start the bot
                var token = Environment.GetEnvironmentVariable("WORKER_DISCORD_BOT_TOKEN");
                await _client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(-1);
            }


            // old stuff ... 
            /*
            _logger.LogInformation("Starting worker service!");            

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100
            });

            //Hook into log event
            _client.Log += LogAsync;

            //Hook into the client ready event
            _client.Ready += ReadyAsync;

            //Hook into the message received event, this is how we handle the hello world example
            _client.MessageReceived += MessageReceivedAsync;

            var token = Environment.GetEnvironmentVariable("WORKER_DISCORD_BOT_TOKEN");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
            */
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            // This method is called to gracefully shut down the worker
            _logger.LogInformation("Gracefully stopping worker service.");

            await Task.CompletedTask;
        }

        private Task LogAsync(LogMessage message)
        {
            // Write logs to systemd/journalctl directly
            _logger.LogInformation(message.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            _logger.LogInformation($"Connected as -> [{_client.CurrentUser}] :)");
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {
            _logger.LogInformation(message.Content);

            //This ensures we don't loop things by responding to ourselves (as the bot)
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            if (message.Content == ".hello")
            {
                await message.Channel.SendMessageAsync("world!");
            }  
        }

        // this method handles the ServiceCollection creation/configuration, and builds out the service provider we can call on later
        private ServiceProvider ConfigureServices()
        {
            // this returns a ServiceProvider that is used later to call for those services
            // we can add types we have access to here, hence adding the new using statement:
            // using dotNetWorker.Services;
            // the config we build is also added, which comes in handy for setting the command prefix!
            return new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }

    }
}
