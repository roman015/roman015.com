using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace www.roman015.com.Pages
{
    public class QRCodeModel : PageModel
    {
        public string QRUrl {get;set;} = "";
        public string QRText {get;set;} = "";
        private readonly ILogger<QRCodeModel> _logger;

        public QRCodeModel(ILogger<QRCodeModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        private string GenerateQRcodeUrl(string input)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            return "/QRGenerator/Image/png?input="
            + System.Web.HttpUtility.UrlEncode(input);
        }

        public void OnPostGenerateCode()
        {
            QRUrl = GenerateQRcodeUrl(Request.Form["textdata"]);
            QRText = Request.Form["textdata"];
        }
    }
}
