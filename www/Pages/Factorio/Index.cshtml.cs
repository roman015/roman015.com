using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace www.roman015.com.Pages
{
    public class FactorioModel : PageModel
    {
        public string StatusMessage {get;set;} = "Press a Button to Begin";
        private readonly ILogger<FactorioModel> _logger;

        public FactorioModel(ILogger<FactorioModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnPostServerStop()
        {
            StatusMessage = "Stop Pressed";
        }

        public void OnPostServerStart()
        {
            StatusMessage = "Start Pressed";
        }

        public void OnPostServerStatus()
        {
            StatusMessage = "Status Pressed";
        }
    }
}
