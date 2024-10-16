using Blog.Core.Models;
using Core.Handlers;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserHandler _userHandler;
        public UsersController(IUserHandler userHandler)
        { 
            _userHandler = userHandler;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserModel registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            string token = await _userHandler.Register(registerUser);
            if (!string.IsNullOrWhiteSpace(token))
                return Ok(token);

            return Problem("Error registering the user.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([Bind("Email, Password")] LoginModel login)
        {
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }

            string token = await _userHandler.Login(login);
            if (!string.IsNullOrWhiteSpace(token))
                return Ok(token);

            return Problem($"User or password incorrect.");
        }

        // GET: api/<UsersController>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserModel>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<UserModel>> GetAsync()
        {
            return await _userHandler.GetAll();
        }

        // GET api/<UsersController>/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(string id)
        {
            var user = await _userHandler.Get(id);
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();
            else
                return Ok(user);
        }


        // PUT api/<UsersController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutAsync(int id, [FromBody] UserModel model)
        {
            if (!id.Equals(model.Id)) return BadRequest();

             await _userHandler.Edit(model);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (!_userHandler.Exists(id))
                return NotFound();

           await  _userHandler.Delete(id);

            return NoContent();
        }
    }
}
