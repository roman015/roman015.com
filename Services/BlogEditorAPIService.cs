using HomePage.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomePage.Services
{
    public class BlogEditorAPIService
    {
        private readonly HttpClient Http;
        private readonly IAccessTokenProvider TokenProvider;

        public BlogEditorAPIService(HttpClient Http, IAccessTokenProvider TokenProvider)
        {
            this.Http = Http;
            this.TokenProvider = TokenProvider;
        }

        public async Task<PostList> GetBlogPosts(int pageIdx = 0, int pageSize = 10)
        {
            string requestUrl = "https://api.roman015.com/BlogViewer/GetPostsForPreview?"
            + "pageSize=" + pageSize + "&"
            + "pageIdx=" + pageIdx;

            PostList result = await Http.GetFromJsonAsync<PostList>(requestUrl);

            return result;
        }
        
        public async Task<Post> GetSelectedBlogPost(string postId)
        {
            string FileName = postId + ".md";
            string requestUrl = "https://content.roman015.com/blog/" + FileName;

            string postMarkDown = await Http.GetStringAsync(requestUrl);

            return new Post(postId, postMarkDown);
        }

        public async Task<bool> UploadFile(string FileName, byte[] FileBytes)
        {
            bool result = false;

            UploadedFile uploadedFile = new UploadedFile()
            {
                FileName = FileName,
                Bytes = FileBytes
            };

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.roman015.com/BlogEditor/UploadFile"))
            {
                var tokenResult = await TokenProvider.RequestAccessToken();

                if (tokenResult.TryGetToken(out var token))
                {
                    requestMessage.Headers.Authorization =
                      new AuthenticationHeaderValue("Bearer", token.Value);
                    requestMessage.Content = new StringContent(
                        JsonSerializer.Serialize<UploadedFile>(uploadedFile), 
                        Encoding.UTF8, 
                        "application/json");

                    var response = await Http.SendAsync(requestMessage);
                    await response.Content.ReadAsStringAsync().ContinueWith((val) =>
                    {
                        result = (val.Status == TaskStatus.RanToCompletion);
                    });
                }
            }

            return result;
        }

        public async Task<bool> UploadPost(string PostId, string PostMarkDown)
        {
            bool result = false;

            UploadedPost uploadedPost = new UploadedPost()
            {
                PostId = PostId,
                PostMarkDown = PostMarkDown
            };

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.roman015.com/BlogEditor/UploadPost"))
            {
                var tokenResult = await TokenProvider.RequestAccessToken();

                if (tokenResult.TryGetToken(out var token))
                {
                    requestMessage.Headers.Authorization =
                      new AuthenticationHeaderValue("Bearer", token.Value);
                    requestMessage.Content = new StringContent(
                        JsonSerializer.Serialize<UploadedPost>(uploadedPost),
                        Encoding.UTF8,
                        "application/json");

                    var response = await Http.SendAsync(requestMessage);
                    await response.Content.ReadAsStringAsync().ContinueWith((val) =>
                    {
                        result = (val.Status == TaskStatus.RanToCompletion);
                    });
                }
            }

            return result;
        }

        // TODO : Clean this up
        public async Task<string> TriggerMessage(string message)
        {
            string result = string.Empty;

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.roman015.com/Test/SignalRTest?message=" + message))
            {
                var tokenResult = await TokenProvider.RequestAccessToken();

                if (tokenResult.TryGetToken(out var token))
                {
                    requestMessage.Headers.Authorization =
                      new AuthenticationHeaderValue("Bearer", token.Value);                    

                    var response = await Http.SendAsync(requestMessage);
                    await response.Content.ReadAsStringAsync().ContinueWith((val) =>
                    {
                        result = val.Result;
                    });
                }
            }

            return result;
        }
    }

}
