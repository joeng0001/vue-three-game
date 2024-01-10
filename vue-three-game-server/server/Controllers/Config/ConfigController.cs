using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Model;
using server.Models.Config.Universe;
using server.Models.Config.Mars;
using System.Reflection;
using System;
using System.Runtime.ConstrainedExecution;

namespace server.Controllers.Config
{
    [Route("api/[controller]")]
    [ApiController]
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
