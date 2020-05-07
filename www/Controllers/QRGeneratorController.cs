using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;

namespace www.roman015.com
{
    [ApiController]
    [Route("[controller]")]
    public class QRGeneratorController : ControllerBase
    {
        private readonly ILogger<QRGeneratorController> _logger;

        public QRGeneratorController(ILogger<QRGeneratorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return BadRequest();
        }

        [HttpGet("Text")]
        public IActionResult GetText([FromQuery]string input)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                return BadRequest();
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q);
            AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);
            string qrCodeAsAsciiArt = qrCode.GetGraphic(1);

            string result = string.Empty;
            string[] strings = qrCodeAsAsciiArt.Split("\n");
            int whiteSpaceBefore = strings[0].Length - strings[0].TrimStart().Length;
            int whiteSpaceAfter = strings[0].Length - strings[0].TrimEnd().Length;
            foreach(string line in strings)
            {
                result += line.Substring(whiteSpaceBefore, line.Length - whiteSpaceAfter - whiteSpaceBefore) + "\n";
            }

            return Ok(result);
        }

        [HttpPost("Text")]
        public IActionResult PostText([FromForm]string input)
        {
            // This is the Post version of the same API
            return GetText(input);
        }        
    
        [HttpGet("Image/{type}")]
        public IActionResult GetImage(string type, [FromQuery]string input, [FromQuery]int bitsPerPixel = 10)
        {
            if(string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(type))
            {
                return BadRequest();
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q);

            switch(type)
            {
                case "png":
                    PngByteQRCode pngQrCode = new PngByteQRCode(qrCodeData);
                    return File(pngQrCode.GetGraphic(bitsPerPixel), "image/png");                 

                case "bmp":
                    BitmapByteQRCode bmpQrCode = new BitmapByteQRCode(qrCodeData);
                    return File(bmpQrCode.GetGraphic(bitsPerPixel), "image/bmp");                    

                // TODO : return svg
                // case "svg":
                //     SvgQRCode svgQrCode = new SvgQRCode(qrCodeData);
                //     return File(svgQrCode.GetGraphic(bitsPerPixel), "image/svg+xml");

                default:
                    return BadRequest("Unable to generate code in image format : " + type);
            }
        }

        [HttpPost("Image/{type}")]
        public IActionResult PostImage(string type, [FromForm]string input, [FromForm]int bitsPerPixel = 10)
        {
            // This is the Post version of the same API
            return GetImage(type, input, bitsPerPixel);
        }
    }
}
