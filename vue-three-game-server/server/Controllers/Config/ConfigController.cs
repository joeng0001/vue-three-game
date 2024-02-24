﻿using Microsoft.AspNetCore.Http;
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
    //[Authorize]
    public class ConfigController : ControllerBase
    {


        private readonly IConfigRepository _configRepository;
        private readonly IUserRepository _userRepository;

        public ConfigController(IUserRepository userRepository, IConfigRepository configRepository)
        {
            _configRepository = configRepository;
            _userRepository = userRepository;
        }

        [HttpGet("Universe/{lv}/{userID}/{profileID}")]
        public async Task<ActionResult<Universe>> GetUniverseConfig(int lv,int userID,int profileID)
        {
            User user = await _userRepository.Get(userID);
            if (user == null)
            {
                return NotFound("User not exist");
            }

            SpaceShipProfileRes profile = _configRepository.GetSpaceShipProfileByID(user, profileID);
            if (profile == null)
            {
                return NotFound("Profile not exist");
            }
            return Ok(new Universe(profile,lv));
        }

        [HttpGet("Mars/{lv}/{userID}/{profileID}")]
        public async Task<ActionResult<Mars>> GetMarsConfig(int lv,int userID,int profileID)
        {
            User user = await _userRepository.Get(userID);
            if (user == null)
            {
                return NotFound("User not exist");
            }

            MarsRoverProfileRes profile = _configRepository.GetMarsRoverProfileByID(user, profileID);
            if (profile == null)
            {
                return NotFound("Profile not exist");
            }
            return Ok(new Mars(profile,lv));
        }

        [HttpGet("test/{userID}/{profile_id}")]
        public async Task<ActionResult<SpaceShipProfileRes>> getSpaceShipProfile(int userID, int profile_id)
        {
            User user = await _userRepository.Get(userID);

            SpaceShipProfileRes s = _configRepository.GetSpaceShipProfileByID(user, profile_id);

            return Ok(s);

        }

        [HttpPost("addSpaceShipProfile/{UserId}")]
        public async Task<ActionResult> addSpaceShipProfile(int UserId, SpaceShipProfileReq s)
        {
            if(s.ammo > 50 || s.ammo < 0 ||
                s.energy>50 || s.energy<0||
                s.life>60 || s.life<0 ||
                s.energyConsume > 0.01 || s.energyConsume< 0 ||
                s.lifeConsume > 0.001 || s.lifeConsume< 0)
            {
                return BadRequest("Value out of range");
            }

            User user = await _userRepository.Get(UserId);

            if (user == null)
            {
                return NotFound("User not exist");
            }

            var success = await _configRepository.AddSpaceShipProfile(user, s);

            if (success)
            {
                return Ok("profile added");
            }
            else
            {
                return StatusCode(403, "Only 5 profiles are allowed.");
            }

        }

        [HttpPost("addMarsRoverProfile/{UserId}")]
        public async Task<ActionResult> addMarsRoverProfile( int UserId,  MarsRoverProfileReq s)
        {
            if(s.energy>10 || s.energy<0 ||
                s.oil > 10 || s.oil < 0)
            {
                return BadRequest("Value out of range");
            }

            User user = await _userRepository.Get(UserId);

            if (user == null)
            {
                return NotFound("User not exist");
            }

            var success = await _configRepository.AddMarsRoverProfile(user, s);

            if (success)
            {
                return Ok("profile added");
            }
            else
            {
                return StatusCode(403, "Only 5 profiles are allowed.");
            }

        }

        [HttpPut("updateSpaceShipProfile/{UserId}/{ProfileId}")]
        public async Task<ActionResult> updateSpaceShipProfile(int UserId, int ProfileId,SpaceShipProfileReq s)
        {
            if (s.ammo > 50 || s.ammo < 0 ||
                s.energy > 50 || s.energy < 0 ||
                s.life > 60 || s.life < 0 ||
                s.energyConsume > 0.01 || s.energyConsume < 0 ||
                s.lifeConsume > 0.001 || s.lifeConsume < 0)
            {
                return BadRequest("Value out of range");
            }

            User user = await _userRepository.Get(UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            Boolean success = await _configRepository.UpdateSpaceShipProfile(user, ProfileId, s);

            if (success)
            {
                return Ok("profile updated");
            }
            else
            {
                return StatusCode(403,"update fail");
            }

        }

        [HttpPut("updateMarsRoverProfile/{UserId}/{ProfileId}")]
        public async Task<ActionResult> updateMarsRoverProfile(int UserId, int ProfileId, MarsRoverProfileReq s)
        {
            if (s.energy > 10 || s.energy < 0 ||
                s.oil > 10 || s.oil < 0)
            {
                return BadRequest("Value out of range");
            }

            User user = await _userRepository.Get(UserId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var success = await _configRepository.UpdateMarsRoverProfile(user, ProfileId, s);

            if (success)
            {
                return Ok("profile updated");
            }
            else
            {
                return StatusCode(403, "update fail");
            }

        }

        [HttpGet("getSpaceShipProfile/{UserId}")]
        public async Task<ActionResult> getSpaceShipProfile(int UserId)
        {

            User user = await _userRepository.Get(UserId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var collection = _configRepository.GetSpaceShipProfiles(user);

            if (collection != null)
            {
                return Ok(collection);
            }
            else
            {
                return StatusCode(403, "fail to get Profile");
            }

        }

        [HttpGet("getMarsRoverProfile/{UserId}")]
        public async Task<ActionResult> getMarsRoverProfile(int UserId)
        {
            
            User user = await _userRepository.Get(UserId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var collection=_configRepository.GetMarsRoverProfiles(user);

            if (collection !=null)
            {
                return Ok(collection);
            }
            else
            {
                return StatusCode(403, "fail to get Profile");
            }

        }
    }
}
