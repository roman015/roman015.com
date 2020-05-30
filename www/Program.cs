using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;

namespace www.roman015.com
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SentrySdk.Init(o =>
            {
                o.Dsn = new Dsn("https://d257da19dc22412fa4bc587737dbd577@o400378.ingest.sentry.io/5258767");
                o.Debug = true;
                o.Environment = "production";
                o.AttachStacktrace = true;
                o.SendDefaultPii = true;
            }))
            {
                CreateHostBuilder(args).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
