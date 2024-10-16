using Blog.Core.Models;
using Core.Handlers;
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

        // GET: api/<UsersController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserModel>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<UserModel>> GetAsync()
        {
            return await _userHandler.GetAll();
        }

        // GET api/<UsersController>/5
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

        // POST api/<UsersController>
        [HttpPost]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] UserModel model)
        {
            var newUser = await _userHandler.Add(model);
            return CreatedAtAction("Get", new { id = newUser.Id }, newUser);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutAsync(int id, [FromBody] UserModel model)
        {
            if (!id.Equals(model.Id)) return BadRequest();

             await _userHandler.Edit(model);

            return NoContent();
        }

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
