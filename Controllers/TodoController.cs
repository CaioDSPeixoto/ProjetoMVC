using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models;
using ProjetoMvc.Models.Dto;
using ProjetoMvc.Models.Entities.ToDo;
using ProjetoMvc.Models.ViewModel;
using ProjetoMvc.ORM.Contexts;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace ProjetoMvc.Controllers
{
    [Authorize]
    public class TodoController(ILogger<TodoController> logger, AppDbContext todoContext) : Controller
    {
        private readonly AppDbContext _context = todoContext;
        private readonly ILogger<TodoController> _logger = logger;

        #region Métodos Publicos
        public async Task<IActionResult> Index(ToDoFilterDto filtro)
        {
            var tarefasQuery = _context.Todos.Include(i => i.Category).AsQueryable();

            tarefasQuery = AplicandoFiltrosParaBuscaDasTarefas(filtro, tarefasQuery);

            var tarefas = await tarefasQuery.ToListAsync();

            var viewModel = new TodoIndexViewModel
            {
                Todos = tarefas,
                SearchTitle = filtro.SearchTitle,
                StartDate = filtro.StartDate,
                EndDate = filtro.EndDate,
                IsFinished = filtro.IsFinished,
                CategoryFilter = filtro.CategoryFilter,
                IsFinishedSelectList = new SelectList(new[] { new { Value = "true", Text = "Finalizado" }, new { Value = "false", Text = "Não Finalizado" } }, "Value", "Text", filtro.IsFinished?.ToString()),
                Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title", filtro.CategoryFilter),
                FiltersApplied = VerificaSePossuiFiltroAplicado(filtro)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Criar Tarefa";

            // Buscar categorias do banco
            var categories = await _context.Categories.ToListAsync();

            if (categories == null || categories.Count == 0)
            {
                TempData["Error"] = "Nenhuma categoria encontrada. Crie uma categoria antes de adicionar uma tarefa.";
                return RedirectToAction("Create", "Category");
            }

            var users = await _context.Users.ToListAsync();

            AdicionarViewBagDeCategorias(categories);
            AdicionarViewBagDeUsuarios(users);

            return View("Form");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Todo todo)
        {
            if (ModelState.IsValid)
            {
                // Capturar o ID do usuário autenticado
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    TempData["Error"] = "Usuário não autenticado. Faça login antes de criar uma tarefa.";
                    return RedirectToAction("Index");
                }

                todo.Create(userId); // atribuindo data da criação e o usuário que cadastrou a tarefa

                await _context.Todos.AddAsync(todo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // view que será feito o redirect
            }

            ViewData["Title"] = "Criar Tarefa";

            // Recarregar categorias em caso de erro
            var categories = await _context.Categories.ToListAsync();
            var users = await _context.Users.ToListAsync();

            AdicionarViewBagDeCategorias(categories);
            AdicionarViewBagDeUsuarios(users);

            return View("Form", todo); // passando na mão a view "Form" por ser uma view para vários usos (criação e edição)
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Editar Tarefa";

            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
                return NotFound();

            // Buscar categorias do banco
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View("Form", todo); // passando na mão a view "Form" por ser uma view para vários usos (criação e edição)
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Todo todo)
        {
            if (ModelState.IsValid)
            {
                _context.Todos.Update(todo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Editar Tarefa";

            // Recarregar categorias em caso de erro
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View("Form", todo); // passando na mão a view "Form" por ser uma view para vários usos (criação e edição)
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewData["Title"] = "Excluir Tarefa";

            var todo = await _context.Todos.FindAsync(id);
            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Todo todo)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Finish(int id)
        {
            ViewData["Title"] = "Finalizar Tarefa";

            var todo = await _context.Todos.FindAsync(id);

            if (todo is null)
                return NotFound();

            todo.Finish(); // atribuindo data da finalização da tarefa

            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region Métodos Privados

        private static IQueryable<Todo> AplicandoFiltrosParaBuscaDasTarefas(ToDoFilterDto filtro, IQueryable<Todo> tarefasQuery)
        {
            // Filtro por título
            if (!string.IsNullOrEmpty(filtro.SearchTitle))
            {
                tarefasQuery = tarefasQuery.Where(t => t.Title.Contains(filtro.SearchTitle));
            }

            // Filtro por categoria
            if (filtro.CategoryFilter.HasValue)
            {
                tarefasQuery = tarefasQuery.Where(t => t.CategoryId == filtro.CategoryFilter);
            }

            // Filtro por data
            if (filtro.StartDate.HasValue)
            {
                tarefasQuery = tarefasQuery.Where(t => t.CreateAt.Date >= filtro.StartDate.Value.Date);
            }
            if (filtro.EndDate.HasValue)
            {
                tarefasQuery = tarefasQuery.Where(t => t.CreateAt.Date <= filtro.EndDate.Value.Date);
            }

            // Filtro por tarefa finalizada
            if (filtro.IsFinished.HasValue)
            {
                if (filtro.IsFinished.Value)
                {
                    tarefasQuery = tarefasQuery.Where(t => t.FinishedAt.HasValue);
                }
                else
                {
                    tarefasQuery = tarefasQuery.Where(t => !t.FinishedAt.HasValue);
                }
            }

            return tarefasQuery;
        }

        private static bool VerificaSePossuiFiltroAplicado(ToDoFilterDto filtro)
        {
            return filtro.SearchTitle != null || filtro.StartDate.HasValue || filtro.EndDate.HasValue || filtro.IsFinished.HasValue || filtro.CategoryFilter.HasValue;
        }

        private void AdicionarViewBagDeUsuarios(List<Models.Entities.User.UserAccount> users)
        {
            ViewBag.Users = new SelectList(users.Select(u => new
            {
                u.Id,
                FullName = $"{u.FirstName} {u.LastName} - [{u.Email}]"
            }), "Id", "FullName");
        }

        private void AdicionarViewBagDeCategorias(List<Category> categories)
        {
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
        }

        #endregion
    }
}
