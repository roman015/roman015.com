using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage.Models
{
    public class Post
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public List<string> Tags { get; set; }
        public string PostMarkDown { get; set; }
        public string PostHTML => Markdig.Markdown.ToHtml(PostMarkDown);

        public Post() { }

        public Post(string PostID, string MarkDownFileContent, DateTime? LastModifiedOn = null)
        {
            var data = MarkDownFileContent
                        .Split(Environment.NewLine)
                        .Take(3)
                        .Select(item => item.Substring(item.IndexOf(":") + 1).Trim())
                        .ToList();

            this.ID = PostID;
            this.Title = data[0];
            this.PublishedOn = DateTime.ParseExact(data[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            this.LastModifiedOn = LastModifiedOn.HasValue ? LastModifiedOn.Value : this.PublishedOn;
            this.Tags = data[2]
                    .Substring(1, data[2].Length - 2) // Remove []
                    .Trim()
                    .Split(",")
                    .Select(item => item.Trim().Replace("\"", string.Empty)) // To remove the double quotes from each entry
                    .Distinct()
                    .ToList();
            this.PostMarkDown = MarkDownFileContent
                    .Substring(MarkDownFileContent.IndexOf("---") + "---".Length)
                    .Replace("\"/assets/img/", "\"https://content.roman015.com/blog/"); // To remove old image references
        }
    }

    public class PostList
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalPosts { get; set; }        
        public List<Post> posts { get; set; }
    }
}
