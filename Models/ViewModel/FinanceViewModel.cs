using ProjetoMvc.Models.Entities.Payment;

namespace ProjetoMvc.Models.ViewModel
{
    public class FinanceViewModel
    {
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public int UserId { get; set; }
        public List<Account> Accounts { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
