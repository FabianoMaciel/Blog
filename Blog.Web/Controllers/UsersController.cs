using Blog.Core.Models;
using Blog.Data;
using Core.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserHandler _userHandler;
        private readonly AppDbContext _context;

        public UsersController(IUserHandler userHandler, AppDbContext context)
        {
            _userHandler = userHandler;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _userHandler.GetAll());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userHandler.Get(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Occupation,IsAdmin,DateOfBirth,CreatedAt")] UserModel model)
        {
            if (ModelState.IsValid)
            {
                model = await _userHandler.Add(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userHandler.Get(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Occupation,IsAdmin,DateOfBirth,CreatedAt")] UserModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userHandler.Edit(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userHandler.Exists(model.Id))
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
            return View(model);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userHandler.Get(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userHandler.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
