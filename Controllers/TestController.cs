using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roman015API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "You are Authorized";
        }

        [HttpGet]
        [Route("BlogAdministrator")]
        [Authorize(Roles = "Blog Administrator")]
        public string GetBlogAdministrator()
        {
            return "You are Authorized as Blog Administrator";
        }
    }
}
