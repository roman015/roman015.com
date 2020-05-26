using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Diagnostics;
using Discord;
using Discord.Commands;
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

        // ref: https://docs.microsoft.com/en-us/dotnet/api/system.threading.monitor.tryenter?view=netcore-3.1
        // ref: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1,1);

        public ExampleTaskBoss(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<IConfiguration>();            
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
        }
        public async Task ManageRemoteServer(String command)
        {
            var channel_general = (_client
                .GetChannel(ulong.Parse(_config["channel-general"])) as IMessageChannel);
            
            _logger.LogInformation($"ManageRemoteServer({command}) was called");

            if (semaphoreSlim.CurrentCount == 0) {
                await channel_general.SendMessageAsync($"Task in-progress, please wait for it to complete.");
                return;
            }

            //Asynchronously wait to enter the Semaphore. If no-one has been granted access to the Semaphore, 
            // code execution will proceed, otherwise this thread waits here until the semaphore is released 
            await semaphoreSlim.WaitAsync();
            try
            {
                // we can start long running task here
                await RunExternalCommand(command);
            }
            finally
            {
                // When the task is ready, release the semaphore. It is vital to ALWAYS release the 
                // semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                // This is why it is important to do the Release within a try...finally clause; program execution 
                // may crash or take a different path, this way you are guaranteed execution
                semaphoreSlim.Release();
            }

        }

        private async Task RunExternalCommand(String command)
        {
            String process_arguments = "";
            switch (command)
            {
                case "start-terraria":
                    // TODO: fill values
                    process_arguments = @" start-terraria.yml";
                    break;
                case "stop-terraria":
                    // TODO: fill values
                    process_arguments = @" stop-terraria.yml";
                    break;                
                default:
                    return; // nothing to do
            }

            var channel = (_client
                .GetChannel(ulong.Parse(_config["channel-shell-output"])) as IMessageChannel);
            await channel.SendMessageAsync($"Received ({command}) request");
            
            Process external_cli = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ansible-playbook",
                    Arguments = process_arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = @"/home/sammy/terraria_on_digital_ocean"
                }
            };
            // start external process
            external_cli.Start();
            StreamReader reader = external_cli.StandardOutput;
            String line;
            while((line = reader.ReadLine()) != null) {
                _logger.LogInformation($"data = {line}");
                if(string.IsNullOrEmpty(line))
                {
                    line = "- - -";  // something to indicate a blank line
                }
                await channel.SendMessageAsync(line);
            }
            await channel.SendMessageAsync($"({command}) request has been completed.");
        }
    }
}
