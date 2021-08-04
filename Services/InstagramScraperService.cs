using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Roman015API.Services
{
    public class InstagramScraperService : IHostedService, IDisposable
    {
        private Timer timer;

        private readonly string InstagramApi = "https://graph.instagram.com/me/media" 
            + "?fields=id,caption,media_type,media_url,thumbnail_url,timestamp,permalink" 
            + "&access_token=";
        private readonly string HomeFileName = "home.json";

        private string LongLivedAccessToken { get; } // These last for 60 days
        private IConfiguration Configuration { get; }
        private BlobContainerClient BlobContainer { get; }

        public InstagramScraperService(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            this.BlobContainer = new BlobContainerClient(Configuration["AzureBlobConnectionString"], "instagram-data");
            this.LongLivedAccessToken = System.Environment.GetEnvironmentVariable("LongLivedAccessToken");
        }        

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(async (state) =>
            {
                // Load Existing Data From Json
                //var blobClient = BlobContainer.GetBlobClient(this.HomeFileName);
                //var jsonData = new StreamReader(blobClient.OpenRead()).ReadToEnd();
                //List<InstagramPost> instagramPosts = JsonSerializer.Deserialize<List<InstagramPost>>(jsonData);

                // Load Data From Instagram
                string url = InstagramApi + this.LongLivedAccessToken;
                var response = await (new HttpClient()).GetStringAsync(url);
                List<InstagramPost> postsFromApi = JsonSerializer.Deserialize<InstagramResponse>(response).data.ToList();

                // Setup css
                for(int i = 0; i < postsFromApi.Count; i++)
                {
                    postsFromApi[i].css = i % 7 == 0 ? "big" : "small";
                }

                // Upload data to file
                BlobContainer.DeleteBlobIfExists(this.HomeFileName);
                BlobContainer.UploadBlob(this.HomeFileName, BinaryData.FromString(JsonSerializer.Serialize(postsFromApi)));
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(2));

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

    public class InstagramPost
    {
        public String id { get; set; }
        public String caption { get; set; }
        public String media_type{ get; set; }
        public String media_url{ get; set; }
        public String thumbnail_url{ get; set; }
        public String timestamp { get; set; }
        public String permalink{ get; set; }
        public String css{ get; set; }
    }

    public class InstagramResponse
    {
        public InstagramPost[] data { get; set; }

        // TODO : Check For Paging afterwards
    }
}
