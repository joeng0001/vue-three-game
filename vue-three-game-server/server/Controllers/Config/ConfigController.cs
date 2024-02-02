using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Models.Config.Earth;
using server.Models.Config.Mars;
using server.Models.Config.Universe;
using System.Reflection;
using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Authorization;
using server.Repositories;
using server.Model;

namespace server.Controllers.Config
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfigController : ControllerBase
    {


        private readonly IConfigRepository _configRepository;
        private readonly IUserRepository _userRepository;

        public ConfigController(IUserRepository userRepository, IConfigRepository configRepository)
        {
            _configRepository = configRepository;
            _userRepository = userRepository;
        }

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

        [HttpGet("test/{userID}/{profile_id}")]
        public async Task<ActionResult<SpaceShipProfileRes>> getSpaceShipProfile(int userID, int profile_id)
        {
            User user = await _userRepository.Get(userID);

            SpaceShipProfileRes s = _configRepository.GetSpaceShipProfileByID(user, profile_id);

            return Ok(s);

        }
    }
}
