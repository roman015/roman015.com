using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Roman015API.Models;
using System.IO;
using Microsoft.Extensions.Caching.Memory;

namespace Roman015API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogViewerController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private BlobContainerClient BlogContainer { get; }
        private IMemoryCache MemoryCache { get; }

        private readonly string AllPostsCacheKey = "AllPosts";

        public BlogViewerController(IConfiguration Configuration, IMemoryCache MemoryCache)
        {
            this.Configuration = Configuration;
            this.MemoryCache = MemoryCache;
            this.BlogContainer = new BlobContainerClient(Configuration["AzureBlobConnectionString"], "blog");           
        }

        [HttpGet]
        [Route("GetAllTags")]
        public IActionResult GetTags()
        {
            List<string> tags = new List<string>();

            GetAllPosts().ForEach(item => tags.AddRange(item.Tags));

            return Ok(tags);
        }

        [HttpGet]
        [Route("GetAllPostsWithTags")]
        public IActionResult GetPostsWithTags([FromQuery]string tags)
        {
            if(string.IsNullOrWhiteSpace(tags))
            {
                return BadRequest(
                    "Use Query String Argument 'tags' to enter tags to search for. "
                    + "Separate multiple tags using comma");
            }

            List<string> selectedTags = tags
                .Split(",")
                .Select(item => item.Trim())
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .Distinct()
                .ToList();

            if(selectedTags.Count == 0)
            {
                return BadRequest("No valid tag in found in Query String Argument 'tags'");
            }

            // Get Posts where the number of tags that match the list of selectedTags EQUALS the entire count of selectedTags
            return Ok(
                GetAllPosts()
                    .Where(item => selectedTags.Count(selectedItem => item.Tags.Contains(selectedItem)) == selectedTags.Count)
                );
        }

        [HttpGet]
        [Route("GetPostsForPreview")]
        public IActionResult GetPostsForPreview([FromQuery]int pageIdx = 0, [FromQuery]int pageSize = 10, [FromQuery]String searchQuery = "")
        {
            if(pageIdx < 0)
            {
                return BadRequest("Invalid PageIdx Value");
            }

            if(pageSize <= 0)
            {
                return BadRequest("Invalid PageSize Value");
            }

            var allPosts = GetAllPosts()
                    .Where(post => string.IsNullOrWhiteSpace(searchQuery)
                        || post.Title.Contains(searchQuery)
                        || post.Tags.Any(tag => tag.Contains(searchQuery)))
                    .OrderByDescending(item => item.PublishedOn);

            var totalPostsCount = allPosts.Count();
            var posts = allPosts
                    .Skip(pageIdx * pageSize)
                    .Take(pageSize);

            return Ok(new {                                
                CurrentPage = pageIdx,
                PageSize = pageSize,
                TotalPages = totalPostsCount / pageSize,
                TotalPosts = totalPostsCount,
                Posts = posts
            });
        }

        private List<Post> GetAllPosts()
        {
            List<Post> result;

            // Ensure Cache is good
            if (!MemoryCache.TryGetValue<List<Post>>(AllPostsCacheKey, out result))
            {
                // Set Cache if Empty
                MemoryCache.Set<List<Post>>(AllPostsCacheKey, GetPostsFromServer(), GetPostMemoryCacheEntryOptions());
                
                // Read from cache again
                MemoryCache.TryGetValue<List<Post>>(AllPostsCacheKey, out result);
            }
            //else
            //{                
            //    var lastModifiedOn_InStorage = BlogContainer.GetBlobs(BlobTraits.Metadata, BlobStates.None, "post_")
            //        .Where(item => item.Name.EndsWith(".md"))
            //        .OrderByDescending(item => item.Properties.LastModified)
            //        .Select(item => item.Properties.LastModified)
            //        .First();

            //    var lastModifiedOn_InCache = result.OrderByDescending(item => item.LastModifiedOn).First().LastModifiedOn;

            //    if(lastModifiedOn_InStorage.Value.Date > lastModifiedOn_InCache.Date)
            //    {
            //        // Reload Cache if Rotten Data (Check the element with the latest ModifiedOn)
            //        MemoryCache.Set<List<Post>>(AllPostsCacheKey, GetPostsFromServer(), GetPostMemoryCacheEntryOptions());
            //    }
            //}            

            return result;
        }

        private MemoryCacheEntryOptions GetPostMemoryCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
            {                
                AbsoluteExpiration = DateTime.Now.AddDays(1),
                SlidingExpiration = TimeSpan.FromDays(1)
            };
        }

        private List<Post> GetPostsFromServer()
        {
            List<Post> posts = new List<Post>();
            var blobList = BlogContainer.GetBlobs(BlobTraits.Metadata, BlobStates.None, "post_")
                    .Where(item => item.Name.EndsWith(".md"));

            foreach (var item in blobList)
            {
                BlobClient postClient = BlogContainer.GetBlobClient(item.Name);

                posts.Add(new Post(
                    item.Name.Replace(".md", string.Empty),
                    new StreamReader(postClient.OpenRead()).ReadToEnd(),
                    item.Properties?.LastModified?.UtcDateTime
                    ));
            }

            return posts;
        }
    }
}
