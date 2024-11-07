using Blog.UI.Models;
using Core.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostHandler _postHandler;

        public HomeController(IPostHandler postHandler)
        {
            _postHandler = postHandler;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _postHandler.GetAll());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
