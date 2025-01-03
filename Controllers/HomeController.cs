using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models;
using ProjetoMvc.Models.Dto;
using ProjetoMvc.ORM.Contexts;
using System.Diagnostics;

namespace ProjetoMvc.Controllers
{
    [Authorize]
    public class HomeController(ILogger<HomeController> logger, AppDbContext todoContext) : Controller
    {
        private readonly AppDbContext _context = todoContext;
        private readonly ILogger<HomeController> _logger = logger;

        public async Task<IActionResult> Index()
        {
            return View();
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
