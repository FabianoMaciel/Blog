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

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _postHandler.GetAll());
        }

        // GET: Posts/Details/5
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

        // GET: Posts/Create
        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["AutorId"] = new SelectList(await _userHandler.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CreatedAt,AutorId")] PostInsertModel model)
        {
            if (ModelState.IsValid)
            {
                await _postHandler.Add(model, string.Empty);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AutorId"] = new SelectList(await _userHandler.GetAll(), "Id", "Name", model.AutorId);
            return View(model);
        }

        // GET: Posts/Edit/5
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

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    await _postHandler.Edit(id, post, string.Empty);//to do fabiano passar logged user
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
            //ViewData["AutorId"] = new SelectList(await _userHandler.GetAll(), "Id", "Name", post.AutorId);
            return View(post);
        }

        // GET: Posts/Delete/5
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

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postHandler.Delete(id, string.Empty);// to do Fabiano logged user
            return RedirectToAction(nameof(Index));
        }
    }
}
