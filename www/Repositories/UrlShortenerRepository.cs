using System;
using System.Collections.Generic;
using System.Linq;

public class UrlShortenerRepository
{
    private readonly int MinUrlSize = 6;
    private readonly int MaxUrlToStore = 32767; //Hard limit to avoid maxing out storage
    private UrlShortenerContext UrlShortenerContext {get;set;}
    public UrlShortenerRepository(UrlShortenerContext UrlShortenerContext)
    {
        this.UrlShortenerContext = UrlShortenerContext;

        this.UrlShortenerContext.Database.EnsureCreated();
    }

    public string SetShortenedUrl(string originalUrl, string customUrl = "")
    {
        if(string.IsNullOrWhiteSpace(originalUrl))
        {
            throw new ArgumentException("Invalid Argument for originalUrl : '" + originalUrl + "'");
        }

        if(!string.IsNullOrWhiteSpace(customUrl) && IsUrlPresent(customUrl))
        {         
            throw new ArgumentException("Invalid Argument for customUrl, value already in use : '" + customUrl + "'");            
        }
        else if(!IsFreeSpaceAvailable())
        {
            throw new ApplicationException("Unable to add urlmapping, no more space available, hardcoded limit reached");
        }
        else
        {
            List<string> invalidStrings = new List<string>();

            var tempGuid = Guid.NewGuid().ToString("D");
            customUrl = tempGuid.Substring(tempGuid.Length - MinUrlSize).ToUpper();
            while(invalidStrings.Contains(customUrl) || IsUrlPresent(customUrl))
            {
                if(!invalidStrings.Contains(customUrl))
                {
                    invalidStrings.Add(customUrl);
                }                

                tempGuid = Guid.NewGuid().ToString("D");
                customUrl = tempGuid.Substring(tempGuid.Length - MinUrlSize).ToUpper();
            }
        }

        UrlShortenerContext.ShortenedUrls.Add(new ShortenedUrl(){
          SourceUrl = customUrl,
          DestinaltionUrl =  originalUrl,
          Timestamp = DateTime.Now 
        });
        UrlShortenerContext.SaveChanges();

        return customUrl;
    }

    public string GetDestinationUrl(string customUrl)
    {
        Console.WriteLine("GetDestinationUrl - " + customUrl);
        if(string.IsNullOrWhiteSpace(customUrl))
        {
            throw new ArgumentException("Invalid Argument for customUrl : '" + customUrl + "'");
        }

        if(!IsUrlPresent(customUrl))
        {
            throw new ApplicationException("Unable to find an entry for customUrl : '" + customUrl + "'");
        }

        return UrlShortenerContext.ShortenedUrls
            .Where(item => item.SourceUrl.Equals(customUrl))
            .First()
            .DestinaltionUrl;
    }

    private bool IsUrlPresent(string url)
    {
        // Check if customUrl does not exist
        int count = UrlShortenerContext.ShortenedUrls
            .Where(item => item.SourceUrl.Equals(url))
            .Count();

        return (count > 0);
    }

    private bool IsFreeSpaceAvailable()
    {
        int count = UrlShortenerContext.ShortenedUrls
            .Count();

        return (count <= MaxUrlToStore);
    }
}