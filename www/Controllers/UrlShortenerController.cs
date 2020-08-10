using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;

namespace www.roman015.com
{
    [ApiController]
    [Route("urls")]
    public class URLShortenerController : ControllerBase
    {
        private readonly ILogger<URLShortenerController> _logger;
        private readonly UrlShortenerRepository _urlShortener;

        public URLShortenerController(ILogger<URLShortenerController> logger, UrlShortenerRepository urlShortener)
        {
            _logger = logger;
            _urlShortener = urlShortener;
        }

        //[Authorize]
        [HttpPost("generate/{inputCode}")]
        public IActionResult GetText([FromForm]string inputURL, string inputCode = "")
        {
            if(string.IsNullOrWhiteSpace(inputURL))
            {
                return BadRequest();
            }
            
            //"https://www.roman015.com/urls/codeHere"
            var shortenedValue = _urlShortener.SetShortenedUrl(inputURL, inputCode);            

            return Ok("https://www.roman015.com/urls/" + shortenedValue);
        }                
    
        [HttpGet("{inputCode}")]
        public IActionResult GetUrl(string inputCode)
        {
            if(string.IsNullOrWhiteSpace(inputCode))
            {
                return BadRequest();
            }

            var resultUrl = _urlShortener.GetDestinationUrl(inputCode);

            if(!resultUrl.StartsWith("http:"))
            {
                resultUrl = "http://" + resultUrl;
            }

            return Redirect(resultUrl);
        }        
    }
}
