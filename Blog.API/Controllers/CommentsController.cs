using Blog.Core.Models;
using Core.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IUserHandler _userHandler;
        private readonly ICommentHandler _commentHandler;
        private readonly IPostHandler _postHandler;

        public CommentsController(ICommentHandler commentHandler, IUserHandler userHandler, IPostHandler postHandler)
        {
            _userHandler = userHandler;
            _commentHandler = commentHandler;
            _postHandler = postHandler;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(AuthorModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int postId)
        {
            if (!_postHandler.Exists(postId))
                return NotFound();

            var comments = await _commentHandler.GetCommentsByPost(postId);
            return Ok(comments);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthorModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post([FromBody] CommentInsertModel model)
        {
            if (!_postHandler.Exists(model.PostId))
                return NotFound();

            string authorId = await _userHandler.GetUserIdAsync();
            var newPost = await _commentHandler.Add(model, authorId);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(AuthorModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            if (!_commentHandler.Exists(id))
                return NotFound();

            var isDeleted = await _commentHandler.Delete(id);

            if (isDeleted)
                return NoContent();
            else
                return Forbid();
        }
    }
}
