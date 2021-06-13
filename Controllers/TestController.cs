using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [Authorize(Roles = "BlogAdministrator")]
        public string GetBlogAdministrator()
        {            
            return "You are Authorized as Blog Administrator";
        }

        [HttpGet]
        [Route("BlogAdministrator2")]
        public string GetBlogAdministrator2()
        {            
            return User.HasClaim(ClaimTypes.Role, "BlogAdministrator") ?
                "You are Authorized as Blog Administrator"
                : "NOT a Blog Administrator";
        }
    }
}
