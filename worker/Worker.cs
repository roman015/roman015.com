using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using dotNetWorker.Services;
using Serilog;


namespace dotNetWorker
{
    public class Worker : BackgroundService
    {
        private Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IConfiguration _config;
        private DiscordSocketClient _client;

        public Worker(ILogger<Worker> logger)
        {

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
                _logger = services.GetRequiredService<ILogger<CommandHandler>>();
                _logger.LogInformation("Starting the worker service!");

                // setup logging and the ready event
                services.GetRequiredService<LoggingService>();

                // this is where we get the Token, and start the bot
                var token = Environment.GetEnvironmentVariable("WORKER_DISCORD_BOT_TOKEN");
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(-1);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            // This method is called to gracefully shut down the worker
            _logger.LogInformation("Gracefully stopping worker service.");

            await Task.CompletedTask;
        }

        // this method handles the ServiceCollection creation/configuration, and builds out the service provider we can call on later
        private ServiceProvider ConfigureServices()
        {
            // this returns a ServiceProvider that is used later to call for those services
            // we can add types we have access to here, hence adding the new using statement:
            // using dotNetWorker.Services;
            // the config we build is also added, which comes in handy for setting the command prefix!
            var services = new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<ExampleTaskBoss>()
                .AddSingleton<LoggingService>()
                .AddLogging(configure => configure.AddSerilog());
            
            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

    }
}
