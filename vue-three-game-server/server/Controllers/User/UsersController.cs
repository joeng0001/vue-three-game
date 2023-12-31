using server.Model;
using server.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Web;
using Microsoft.AspNetCore.DataProtection;

namespace server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UsersController(IUserRepository userRepository,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration=configuration;

        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUsers(int id)
        {
            return await _userRepository.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult<User>> PostUsers([FromBody] User user)
        {
            var newUser = await _userRepository.Create(user);
            return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser);
        }

        [HttpPut]
        public async Task<ActionResult> PutUsers(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userRepository.Update(user);
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var userToDelete = await _userRepository.Get(id);
            if (userToDelete == null)
            {
                return NotFound();
            }

            await _userRepository.Delete(userToDelete.Id);
            return NoContent();
        }



        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody]  UserAuth req)
        {

            var user = await _userRepository.GetUserByName(req.Username);
            if (user is not null) // userName in used
            {
                return BadRequest("userName in used");
            }
            createHashPassword(req.Password, out byte[] hashPassword, out byte[] salt);
            var User = new User();
            User.Name = req.Username;
            User.password = hashPassword;
            User.salt = salt;
            User=await _userRepository.Create(User);
            //return CreatedAtAction(nameof(GetUsers), new { id = User.Id }, User.Name);
            return Ok("register success");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody]  UserAuth req)
        {
            var user=await _userRepository.GetUserByName(req.Username);
            if (user is null) // userName not found
            {
                return BadRequest("Incorrect Login Info");
            }
            if (!verifyPassword(req.Password, user.password, user.salt)) // incorrect password
            {
                return BadRequest("Incorrect Login Info");
            }
            string token = createToken(req);
            var cookieOptions = new CookieOptions {
                Expires = DateTime.Now.AddDays(1),
                Path = "/",
                HttpOnly = true,
                Secure = true,
                Domain = "localhost",
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("token", token, cookieOptions);
            return Ok("login success");
        }

        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                Path = "/",
                HttpOnly = true,
                Secure = true,
                Domain = "localhost",
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("token", "", cookieOptions);
            return Ok("Logout success");
        }


        [HttpGet("test")]
        public async Task<ActionResult<string>> test()
        {
            System.Console.WriteLine("receive test request");
            string cookieValue = Request.Cookies["token"];
            System.Console.WriteLine(cookieValue);
            return Ok("Logout success");
        }

        private void createHashPassword(string password,out byte[] hashPassword,out byte[] salt)
        {
            using(var hmac=new HMACSHA512())
            {
                salt = hmac.Key;
                hashPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool verifyPassword(string password, byte[] hashPassword, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHashPW = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHashPW.SequenceEqual(hashPassword);
            }
        }

        private string createToken(UserAuth user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:TokenKey").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials:cred
                    );
            var jwt=new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}
