using Blog.Core.Models;
using Core.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostHandler _postHandler;
        private readonly IUserHandler _userHandler;

        public PostsController(IPostHandler postHandler, IUserHandler userHandler)
        {
            _postHandler = postHandler;
            _userHandler = userHandler;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _postHandler.GetAll());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postHandler.Get(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["AutorId"] = new SelectList(await _userHandler.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CreatedAt,AutorId")] PostInsertModel model)
        {
            if (ModelState.IsValid)
            {
                await _postHandler.Add(model, await _userHandler.GetUserIdAsync());
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postHandler.Get(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Content")] PostInsertModel post)
        {
            if (id < 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _postHandler.Edit(id, post, await _userHandler.GetUserIdAsync());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_postHandler.Exists(id))
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

            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postHandler.Get(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postHandler.Delete(id, await _userHandler.GetUserIdAsync());
            return RedirectToAction(nameof(Index));
        }
    }
}
