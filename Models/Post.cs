using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Roman015API.Models
{
    public class Post
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public List<string> Tags { get; set; }
        public string PostMarkDown { get; set; }

        public Post() { }

        public Post(string PostID, string MarkDownFileContent, DateTime? LastModifiedOn = null)
        {
            var result = new Post();

            var data = MarkDownFileContent
                        .Split(Environment.NewLine)
                        .Take(3)
                        .Select(item => item.Substring(item.IndexOf(":") + 1).Trim())
                        .ToList();

            result.ID = PostID;
            result.Title = data[0];
            result.PublishedOn = DateTime.ParseExact(data[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            result.LastModifiedOn = LastModifiedOn.HasValue ? LastModifiedOn.Value : result.PublishedOn;
            result.Tags = data[2]
                    .Substring(1, data[2].Length - 2) // Remove []
                    .Trim()
                    .Split(",")
                    .Select(item => item.Trim().Replace("\"", string.Empty)) // To remove the double quotes from each entry
                    .ToList();
            result.PostMarkDown = MarkDownFileContent
                    .Substring(MarkDownFileContent.IndexOf("---") + "---".Length)
                    .Split(Environment.NewLine)
                    .First(); // Only the first line of the markdown
        }
    }
}
