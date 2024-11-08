using Blog.Core.Models;
using Core.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentHandler _commentHandler;
        private readonly IPostHandler _postHandler;
        private readonly IUserHandler _userHandler;

        public CommentsController(ICommentHandler commentHandler, IUserHandler userHandler, IPostHandler postHandler)
        {
            _commentHandler = commentHandler;
            _postHandler = postHandler;
            _userHandler = userHandler;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _commentHandler.GetAll());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentHandler.Get(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["PostId"] = new SelectList(await _postHandler.GetAll(), "Id", "Title");
            return View();
        }

        [Authorize]
        [HttpGet("create/{id:int}")]
        public async Task<IActionResult> CreateAsync(int id)
        {
            ViewData["PostId"] = new SelectList(await _postHandler.GetAll(), "Id", "Title", id);
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,CreatedAt,UserId,PostId")] CommentModel comment)
        {
            if (ModelState.IsValid)
            {
                await _commentHandler.Add(comment);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(await _postHandler.GetAll(), "Id", "Title", comment.PostId);
            return View(comment);
        }

        [Authorize]
        [HttpPost("create/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Content,CreatedAt,UserId,PostId")] CommentModel comment)
        {
            if (ModelState.IsValid)
            {
                await _commentHandler.Add(comment);
                return RedirectToAction("Index", "Home");
            }
            ViewData["PostId"] = new SelectList(await _postHandler.GetAll(), "Id", "Title", comment.PostId);
            return View(comment);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentHandler.Get(id.Value);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(await _postHandler.GetAll(), "Id", "Content", comment.PostId);
            return View(comment);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,CreatedAt,UserId,PostId")] CommentModel comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _commentHandler.Edit(comment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_commentHandler.Exists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(await _postHandler.GetAll(), "Id", "Title", comment.PostId);
            return View(comment);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentHandler.Get(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commentHandler.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
