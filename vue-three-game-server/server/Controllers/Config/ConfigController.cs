using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Models.Config.Earth;
using server.Models.Config.Mars;
using server.Models.Config.Universe;
using System.Reflection;
using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Authorization;


namespace server.Controllers.Config
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfigController : ControllerBase
    {

        [HttpGet("Universe/{lv}")]
        public ActionResult<Universe> GetUniverseConfig(int lv)
        {
            return Ok(new Universe(lv));
        }

        [HttpGet("Mars/{lv}")]
        public ActionResult<Mars> GetMarsConfig(int lv)
        {
            return Ok(new Mars(lv));
        }
    }
}
