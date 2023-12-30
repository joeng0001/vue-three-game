using server.Model;
using server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
        public async Task<ActionResult<User>> Register()
        {
            System.Console.WriteLine("receive register request");
            return NoContent();
        }


    }
}
