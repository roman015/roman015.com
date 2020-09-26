using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace www.roman015.com
{
    [ApiController]
    [Route("[controller]")]
    public class Civ6WebHookController : ControllerBase
    {
        private readonly ILogger<Civ6WebHookController> _logger;
        private readonly Civ6TurnNotificationHandler _civ6TurnNotificationHandler;

        public Civ6WebHookController(ILogger<Civ6WebHookController> logger, IServiceProvider services)
        {
            _logger = logger;
            _civ6TurnNotificationHandler = services.GetService<Civ6TurnNotificationHandler>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            _civ6TurnNotificationHandler.HandleNotification(new Civ6TurnModel()
            {
                value1 = "FAKE",
                value2 = "FAKE2",
                value3 = "FAKE3"
            });

            return BadRequest(
                "No, use a POST Request pls. "
                + "If you can read this, then ur bot should've sent a message by now");
        }        

        [HttpPost("NotifyTurn")]
        public IActionResult PostText([FromBody] Civ6TurnModel turndata)
        {
            // CIV6 Webhook returns the following data as json:
            // { "value1":"[game name]", "value2":"[player name]", "value3":"[game turn number]" }
            _logger.Log(LogLevel.Error, "TEST : " + turndata.ToString());
            Console.WriteLine("TEST 2 : " + turndata.ToString());

            _civ6TurnNotificationHandler.HandleNotification(turndata);
            return Ok();
        }                  
    }
}
