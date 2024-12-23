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
                var existingUser = await _context.Users.Include(i => i.BlockedBy).FirstOrDefaultAsync(f => f.Id == user.Id);
                if (existingUser == null)
                    return NotFound();

                if (user.BlockedUntil.HasValue && existingUser.BlockedBy == null)
                {
                    if (!ValidarUsuarioLogado(user.Id, out string errorMessage))
                    {
                        TempData["ErrorMessage"] = errorMessage;
                        return RedirectToAction(nameof(Index));
                    }

                    existingUser.BlockedBy = await _context.Users.FirstOrDefaultAsync(f => f.Id == user.Id);
                }

                AtualizaOsCamposDoUsuario(user, existingUser);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Usuário alterado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Editar Tarefa";

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(int id, DateTime? blockedUntil)
        {
            var user = await _context.Users.Include(u => u.BlockedBy).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Validar se o usuário logado pode bloquear
            if (!ValidarUsuarioLogado(id, out string errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction(nameof(Index));
            }

            var idUsuarioLogadoString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioLogadoString) || !int.TryParse(idUsuarioLogadoString, out int idUsuarioLogado))
            {
                TempData["ErrorMessage"] = "Usuário logado inválido.";
                return RedirectToAction(nameof(Index));
            }

            user.BlockedBy = await _context.Users.FirstOrDefaultAsync(u => u.Id == idUsuarioLogado);
            user.BlockedUntil = blockedUntil.HasValue ? blockedUntil : null;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(int id)
        {
            var user = await _context.Users.Include(i => i.BlockedBy).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Validar se o usuário logado pode desbloquear
            if (!ValidarUsuarioLogado(id, out string errorMessage))
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction(nameof(Index));
            }

            // Remover as informações de bloqueio
            user.BlockedBy = null;
            user.BlockedUntil = null;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
                var user = await _context.Users
                    .Include(i => i.BlockedBy)
                    .Where(w => w.Email == login.Email)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    // Verificar se o usuário está bloqueado
                    if (user.BlockedBy != null) // Verifica se há um usuário que realizou o bloqueio
                    {
                        if (user.BlockedUntil.HasValue && user.BlockedUntil > DateTime.Now)
                        {
                            // Bloqueio temporário com data futura
                            string mensagemBloqueio = $"Sua conta está bloqueada até {user.BlockedUntil.Value:dd/MM/yyyy HH:mm}.";
                            ModelState.AddModelError("", mensagemBloqueio);
                        }

                        // Bloqueio permanente (sem data de desbloqueio)
                        if (!user.BlockedUntil.HasValue)
                        {
                            string mensagemBloqueioPermanente = "Sua conta está bloqueada permanentemente. Entre em contato com o suporte.";
                            ModelState.AddModelError("", mensagemBloqueioPermanente);
                        }

                        return View(login);
                    }

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

        private bool ValidarUsuarioLogado(int id, out string errorMessage)
        {
            // Obter o ID do usuário logado
            var idUsuarioLogadoStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Validar se o ID é nulo ou inválido
            if (string.IsNullOrEmpty(idUsuarioLogadoStr) || !int.TryParse(idUsuarioLogadoStr, out int idUsuarioLogado))
            {
                errorMessage = "Usuário logado inválido.";
                return false;
            }

            // Verificar se está tentando bloquear ou desbloquear a si mesmo
            if (id == idUsuarioLogado)
            {
                errorMessage = "Você não pode bloquear/desbloquear a si mesmo.";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
