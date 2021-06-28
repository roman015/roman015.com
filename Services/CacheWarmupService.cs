using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Roman015API.Services
{
    public class CacheWarmupService : IHostedService, IDisposable
    {
        private Timer timer;       

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(async (state) =>
            {
                // Just call Any BlogViewerController API to keep the cache warm
                var result = await new HttpClient().GetStringAsync(@"http://localhost/BlogViewer/GetAllTags");
                Console.WriteLine(result);
            }, 
            null, 
            TimeSpan.Zero,
            TimeSpan.FromHours(3));

            return Task.CompletedTask;
        }        

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
