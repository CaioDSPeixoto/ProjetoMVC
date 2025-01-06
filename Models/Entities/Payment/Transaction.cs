using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProjetoMvc.Models.Enum;
using ProjetoMvc.ORM.Entitie;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Entities.Payment
{
    public class Transaction : EntityBase
    {
        #region Dados da transação

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        public string Description { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O campo {0} precisa ter um valor positivo.")]
        public decimal Amount { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TransactionTypeEnum Type { get; set; }

        [Display(Name = "Data de lançamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Data de Vencimento")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Lançamento Mensal")]
        public bool IsMonthly { get; set; } = false; // Flag que indica se a transação é mensal
        #endregion

        #region Dados das contas
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int AccountId { get; set; }

        [ValidateNever]
        public Account Account { get; set; }  // Propriedade de navegação para a conta associada
        #endregion

        #region Relacionamento com o usuário
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int UserId { get; set; }  // Vinculação ao usuário

        [ValidateNever]
        public User.User User { get; set; }  // Propriedade de navegação para o usuário
        #endregion

        public void Create(int userId)
        {
            TransactionDate = DateTime.Now;
            UserId = userId;
        }
    }
}
