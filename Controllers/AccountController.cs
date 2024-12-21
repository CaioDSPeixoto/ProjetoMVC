using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models;
using ProjetoMvc.Models.Entities.User;
using ProjetoMvc.ORM.Contexts;
using System.Security.Claims;

namespace ProjetoMvc.Controllers
{
    public class AccountController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        [Authorize(Policy = "AdminOrDeveloper")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Editar Usuário";

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser == null)
                    return NotFound();

                AtualizaOsCamposDoUsuario(user, existingUser);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Editar Tarefa";

            return View(user);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel registration)
        {
            if (ModelState.IsValid)
            {
                var passwordHasher = new PasswordHasher<UserAccount>();

                UserAccount account = new()
                {
                    Email = registration.Email,
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    Password = passwordHasher.HashPassword(null, registration.Password)
                };

                try
                {
                    await _context.Users.AddAsync(account);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();
                    TempData["SuccessMessage"] = $"O usuário {account.FirstName} {account.LastName} foi registrado com sucesso! Por favor, faça o login.";
                    return RedirectToAction(nameof(Login));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Por favor, use um email e senha válidos.");
                    return View(registration);
                }
            }
            return View(registration);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            string mensagemGenericaDeErro = "Email ou senha estão incorretos! Tente novamente.";

            if (ModelState.IsValid)
            {
                var user = await _context.Users.Where(w => w.Email == login.Email).FirstOrDefaultAsync();

                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<UserAccount>();
                    var verificationResult = passwordHasher.VerifyHashedPassword(null, user.Password, login.Password);

                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim>()
                        {
                            new(ClaimTypes.Name, user.FirstName), // salvando para acessar através do Context.User.Identity.Name
                            new(ClaimTypes.Role, user.Permission.ToString()), // " Context.User.Claims.First(c => c.Type == ClaimTypes.Role)
                            new(ClaimTypes.NameIdentifier, user.Id.ToString()) // " Context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home"); // manda para a tela inicial em caso de sucesso do login
                    }
                    else
                    {
                        ModelState.AddModelError("", mensagemGenericaDeErro);
                    }
                }
                else
                {
                    ModelState.AddModelError("", mensagemGenericaDeErro);
                }
            }

            return View(login);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void AtualizaOsCamposDoUsuario(UserAccount user, UserAccount? existingUser)
        {
            _context.Entry(existingUser).CurrentValues.SetValues(user);

            if (!string.IsNullOrEmpty(user.Password))
            {
                var passwordHasher = new PasswordHasher<UserAccount>();
                existingUser.Password = passwordHasher.HashPassword(null, user.Password);
            }
            else
            {
                // Marca o campo da senha como "Não modificado" para não alterar
                _context.Entry(existingUser).Property(u => u.Password).IsModified = false;
            }
        }
    }
}
