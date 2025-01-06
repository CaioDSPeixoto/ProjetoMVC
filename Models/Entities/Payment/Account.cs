using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProjetoMvc.Models.Enum;
using ProjetoMvc.ORM.Entitie;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Entities.Payment
{
    public class Account : EntityBase
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Status")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public AtivoInativoEnum Status { get; set; } = AtivoInativoEnum.Active;

        #region Relacionamento com o usuário
        public int? UserId { get; set; }  // Vinculação ao usuário

        [ValidateNever]
        public User.User? User { get; set; }  // Propriedade de navegação para o usuário
        #endregion

        // Propriedade de navegação para as transações associadas
        public ICollection<Transaction> Transactions { get; set; } = [];

        public decimal GetBalanceForUser(int userId)
        {
            // Filtra transações do usuário e calcula o saldo
            var creditAmount = Transactions
                .Where(t => t.Type == TransactionTypeEnum.Credit && t.UserId == userId && t.AccountId == Id)
                .Sum(t => t.Amount);

            var debitAmount = Transactions
                .Where(t => t.Type == TransactionTypeEnum.Debit && t.UserId == userId && t.AccountId == Id)
                .Sum(t => t.Amount);

            // Retorna o saldo como a diferença entre créditos e débitos
            return creditAmount - debitAmount;
        }
    }
}
