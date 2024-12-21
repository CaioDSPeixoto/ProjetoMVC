using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models;
using ProjetoMvc.Models.Entities.ToDo;
using ProjetoMvc.ORM.Contexts;
using System.Diagnostics;

namespace ProjetoMvc.Controllers
{
    [Authorize]
    public class CategoryController(ILogger<CategoryController> logger, AppDbContext todoContext) : Controller
    {
        private readonly AppDbContext _context = todoContext;
        private readonly ILogger<CategoryController> _logger = logger;

        public async Task<IActionResult> Index()
        {
            var tarefas = await _context.Categories.ToListAsync();
            return View(tarefas);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Criar Categoria";

            return View("Form");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Criar Categoria";

            return View("Form", category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Editar Categoria";

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View("Form", category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Editar Categoria";

            return View("Form", category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewData["Title"] = "Excluir Categoria";

            var categoria = await _context.Categories.FindAsync(id);
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Category category)
        {
            // Verifica se existem tarefas associadas a esta categoria
            bool hasTodos = await _context.Todos.AnyAsync(todo => todo.CategoryId == category.Id);

            if (hasTodos)
            {
                ModelState.AddModelError(string.Empty, "Não é possível excluir a categoria, pois existem tarefas associadas a ela.");

                ViewData["Title"] = "Excluir Categoria";
                return View(category);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
