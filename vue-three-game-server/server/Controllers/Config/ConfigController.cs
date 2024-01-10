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

        [HttpGet("Universe")]
        public async Task<ActionResult<Universe>> GetUniverseConfig(int lv)
        {
            Universe res = new Universe(lv);
            return Ok(res);
        }

        [HttpGet("Mars")]
        public async Task<ActionResult<Mars>> GetMarsConfig(int lv)
        {
            Mars res = new Mars(lv);
            return Ok(res);
        }
    }
}
