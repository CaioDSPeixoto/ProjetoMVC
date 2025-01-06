using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models.Entities.Payment;
using ProjetoMvc.Models.Enum;
using ProjetoMvc.Models.Helper;
using ProjetoMvc.Models.ViewModel;
using ProjetoMvc.ORM.Contexts;
using System.Security.Claims;

namespace ProjetoMvc.Controllers
{
    [Authorize]
    public class FinanceController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        public IActionResult IndexCard()
        {
            return View();
        }

        #region Geral
        public async Task<IActionResult> Index()
        {
            int userId = ObterIdUsuarioLogado();

            var dataAtual = DateTime.Now;

            var accounts = await _context.Accounts
                .Where(w => w.UserId == null || w.UserId == userId)
                .Include(i => i.Transactions.Where(t => t.UserId == userId && (t.TransactionDate.Month == dataAtual.Month && t.TransactionDate.Year == dataAtual.Year || t.IsMonthly)))
                .ToListAsync();

            var viewModel = new FinanceViewModel
            {
                Income = accounts.SelectMany(s => s.Transactions).Where(t => t.Type == TransactionTypeEnum.Credit).Sum(t => t.Amount),
                Expenses = accounts.SelectMany(s => s.Transactions).Where(t => t.Type == TransactionTypeEnum.Debit).Sum(t => t.Amount),
                UserId = userId,
                Accounts = accounts
            };

            return View(viewModel);
        }
        #endregion Fim Geral

        #region Accounts
        public async Task<IActionResult> IndexAccount()
        {
            int userId = ObterIdUsuarioLogado();

            var accounts = await _context.Accounts
                .Where(w => w.UserId == null || w.UserId == userId)
                .Include(i => i.Transactions.Where(t => t.UserId == userId))
                .ToListAsync();

            var viewModel = new FinanceViewModel
            {
                Accounts = accounts
            };

            return View("Account/IndexAccount", viewModel);
        }

        public IActionResult CreateAccount()
        {
            return View("Account/CreateAccount", new Account());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(Account account)
        {
            if (ModelState.IsValid)
            {
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Account/CreateAccount", account);
        }

        public async Task<IActionResult> AccountDetails(int id)
        {
            int userId = ObterIdUsuarioLogado();

            var account = await _context.Accounts
                .Where(w => w.UserId == null || w.UserId == userId)
                .Include(i => i.Transactions.Where(t => t.AccountId == id && t.UserId == userId))
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return View("Account/AccountDetails", account);
        }
        #endregion Fim Accounts

        #region Transactions
        // Action para a tela de Transações
        public async Task<IActionResult> IndexTransaction()
        {
            int userId = ObterIdUsuarioLogado();

            var transactions = await _context.Transactions
                .Where(w => w.UserId == userId)
                .ToListAsync();

            var viewModel = new FinanceViewModel
            {
                Transactions = transactions
            };

            return View("Transaction/IndexTransaction", viewModel);
        }

        public async Task<IActionResult> CreateTransaction()
        {
            int userId = ObterIdUsuarioLogado();

            // Buscar contas do banco
            var accounts = await _context.Accounts.Where(w => w.UserId == null || w.UserId == userId).ToListAsync();

            if (accounts == null || accounts.Count == 0)
            {
                MessageHelper.Error(TempData, $"Nenhuma conta foi encontrada. Crie uma conta antes de adicionar um lançamento.");
                return RedirectToAction(nameof(Index));
            }

            AdicionarViewBagDeTipo();
            await AdicionarViewBagDeAccounts(accounts);

            return View("Transaction/CreateTransaction", new Transaction());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                int userId = ObterIdUsuarioLogado();

                try
                {
                    var account = await _context.Accounts.Where(w => w.UserId == null || w.UserId == userId).FirstOrDefaultAsync(a => a.Id == transaction.AccountId);

                    if (account != null && userId > 0)
                    {

                        transaction.Create(userId);

                        // Adicionar a transação no banco de dados
                        await _context.Transactions.AddAsync(transaction);

                        // Atualizar a conta com o novo saldo
                        _context.Accounts.Update(account);

                        // Salvar todas as alterações no banco
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {

                    MessageHelper.Error(TempData, $"Ocorreu um erro: {ex.Message}");
                }
            }

            await AdicionarViewBagDeAccounts();
            AdicionarViewBagDeTipo();

            return View("Transaction/CreateTransaction", transaction);
        }
        #endregion Fim Transactions

        #region Métodos privados
        private async Task AdicionarViewBagDeAccounts(List<Account>? accounts = null)
        {
            int userId = ObterIdUsuarioLogado();

            var accountsSelect = accounts ?? await _context.Accounts.Where(w => w.UserId == null || w.UserId == userId).ToListAsync();

            var accountSelectList = accountsSelect.Select(account => new SelectListItem
            {
                Value = account.Id.ToString(),
                Text = account.Name
            }).ToList();

            ViewBag.Accounts = accountSelectList;
        }

        private void AdicionarViewBagDeTipo()
        {
            ViewBag.TransactionTypes = Enum.GetValues(typeof(TransactionTypeEnum))
                .Cast<TransactionTypeEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = new EnumHelper().ObterDisplay(e)
                })
                .ToList();
        }

        private int ObterIdUsuarioLogado()
        {
            // Capturar o ID do usuário autenticado
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _ = int.TryParse(userIdClaim, out int userId);

            return userId;
        }
        #endregion Fim metodos privados
    }
}
