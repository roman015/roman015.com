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

        public URLShortenerController(ILogger<URLShortenerController> logger)
        {
            _logger = logger;
        }

        //[Authorize]
        [HttpPost("generate/{inputCode}")]
        public IActionResult GetText([FromForm]string inputURL, string inputCode = "")
        {
            if(string.IsNullOrWhiteSpace(inputURL))
            {
                return BadRequest();
            }
            
            // TODO : Generate Shortened url using input code, if present
            var result = "https://www.roman015.com/urls/codeHere";

            return Ok(result);
        }                
    
        [HttpGet("{inputCode}")]
        public IActionResult GetUrl(string inputCode)
        {
            if(string.IsNullOrWhiteSpace(inputCode))
            {
                return BadRequest();
            }

            // TODO : Rediret Shortened url, if present
            var resultUrl = @"https://www.google.com";

            return Redirect(resultUrl);
        }        
    }
}
