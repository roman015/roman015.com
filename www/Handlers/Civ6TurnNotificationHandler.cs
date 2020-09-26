using System;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace www.roman015.com
{
    public class Civ6TurnNotificationHandler
    {
        private readonly IConfiguration _config;
        private readonly DiscordSocketClient _client;
        private readonly IMessageChannel _messageChannel;
        private readonly ILogger<Civ6TurnNotificationHandler> _logger;
        
        public Civ6TurnNotificationHandler(ILogger<Civ6TurnNotificationHandler> logger, IServiceProvider services)
        {
            _logger = logger;  

            #region Setup config.json
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");  
            _config = _builder.Build();
            #endregion

            #region Setup Discord
            var client = services.GetRequiredService<DiscordSocketClient>();
            _client = client;

            _logger.LogInformation("Starting the CIV6 NOTI BOT!");

            var token = Environment.GetEnvironmentVariable("CIV6NOTIBOT_DISCORD_BOT_TOKEN");
            client.LoginAsync(TokenType.Bot, token).Wait();
            client.StartAsync().Wait();
            var channelGeneral = client.GetChannel(ulong.Parse(_config["channel-general"]));
            var textChannel = channelGeneral as IMessageChannel;
            _messageChannel = textChannel;
            
            _logger.LogInformation("CIV6 NOTI BOT Started!");
            #endregion
        }

        public void HandleNotification(Civ6TurnModel data)
        {
            _messageChannel.SendMessageAsync(data.ToString());
        }
    }
}