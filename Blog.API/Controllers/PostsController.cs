using Blog.Core.Models;
using Core.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostHandler _postHandler;
        public PostsController(IPostHandler postHandler)
        {
            _postHandler = postHandler;
        }

        // GET: api/<UsersController>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PostModel>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PostModel>> GetAsync()
        {
            return await _postHandler.GetAll(true);
        }

        // GET api/<UsersController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await _postHandler.Get(id);
            if (user == null || user.Id == 0)
                return NotFound();
            else
                return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] PostModel model)
        {
            var newUser = await _postHandler.Add(model);
            return CreatedAtAction("Get", new { id = newUser.Id }, newUser);
        }

        // PUT api/<UsersController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutAsync(int id, [FromBody] PostModel model)
        {
            if (id != model.Id) return BadRequest();

            await _postHandler.Edit(model);

            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (!_postHandler.Exists(id))
                return NotFound();

            await _postHandler.Delete(id);

            return NoContent();
        }
    }
}
