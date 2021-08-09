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
                string[] ids;

                // Load Existing Data From Json
                //var blobClient = BlobContainer.GetBlobClient(this.HomeFileName);
                //var jsonData = new StreamReader(blobClient.OpenRead()).ReadToEnd();
                //List<InstagramPost> instagramPosts = JsonSerializer.Deserialize<List<InstagramPost>>(jsonData);

                // Load Data From Instagram
                string url = InstagramApi + this.LongLivedAccessToken;
                var response = await (new HttpClient()).GetStringAsync(url);
                List<InstagramPost> postsFromApi = JsonSerializer.Deserialize<InstagramResponse>(response).data.ToList();                

                #region Sort & Store Data
                #region Game
                string GameFileName = "game.json";
                List<InstagramPost> GamePosts = postsFromApi
                    .Where(item => item.caption.Contains("#GameA ") || item.caption.Contains("#GameN "))                    
                    .ToList();

                ids = GamePosts.Select(item => item.id).ToArray();
                postsFromApi = postsFromApi
                    .Where(item => !ids.Contains(item.id))
                    .ToList();

                // Upload data to files
                BlobContainer.DeleteBlobIfExists(GameFileName);
                BlobContainer.UploadBlob(GameFileName, BinaryData.FromString(JsonSerializer.Serialize(GamePosts)));
                #endregion

                #region Home
                string HomeFileName = "home.json";
                List<InstagramPost> HomePosts = postsFromApi
                    .Where(item => item.caption.Contains("#Home "))
                    .Select(item => { 
                        item.caption.Replace("#Home ", string.Empty); 
                        return item; 
                    })
                    .ToList();

                ids = HomePosts.Select(item => item.id).ToArray();
                postsFromApi = postsFromApi
                    .Where(item => !ids.Contains(item.id))
                    .ToList();

                // Upload data to files
                BlobContainer.DeleteBlobIfExists(HomeFileName);
                BlobContainer.UploadBlob(HomeFileName, BinaryData.FromString(JsonSerializer.Serialize(HomePosts)));
                #endregion
                #endregion                
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
