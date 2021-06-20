﻿using Azure.Storage.Blobs;
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

namespace Roman015API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogViewerController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private BlobContainerClient BlogContainer { get; }

        public BlogViewerController(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
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
        [Route("GetAllPostsWithTag")]
        public IActionResult GetPostsWithTag([FromQuery]List<string> selectedTags)
        {
            return Ok(
                GetAllPosts()
                    .Where(item => item.Tags.Any(tag => selectedTags.Contains(tag)))
                );
        }    
        
        private List<Post> GetAllPosts()
        {
            return GetPostsFromServer();
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
