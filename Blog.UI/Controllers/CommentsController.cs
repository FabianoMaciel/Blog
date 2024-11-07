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

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            return View(await _commentHandler.GetAll());
        }

        // GET: Comments/Details/5
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

        // GET: Comments/Create
        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["PostId"] = new SelectList(await _postHandler.GetAll(), "Id", "Title");
            //ViewData["UserId"] = new SelectList(await _userHandler.GetAll(), "Id", "User.UserName");
            return View();
        }

        [Authorize]
        [HttpGet("create/{id:int}")]
        // GET: Comments/Create
        public async Task<IActionResult> CreateAsync(int id)
        {
            ViewData["PostId"] = new SelectList(await _postHandler.GetAll(), "Id", "Title", id);
           // ViewData["UserId"] = new SelectList(await _userHandler.GetAll(), "Id", "User.UserName");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            //ViewData["UserId"] = new SelectList(await _userHandler.GetAll(), "Id", "User.UserName", comment.AuthorId);
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
           // ViewData["UserId"] = new SelectList(await _userHandler.GetAll(), "Id", "User.UserName", comment.AuthorId);
            return View(comment);
        }

        // GET: Comments/Edit/5
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
            //ViewData["UserId"] = new SelectList(await _userHandler.GetAll(), "Id", "User.UserName", comment.AuthorId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
           // ViewData["UserId"] = new SelectList(await _userHandler.GetAll(), "Id", "User.UserName", comment.AuthorId);
            return View(comment);
        }

        // GET: Comments/Delete/5
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

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commentHandler.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
