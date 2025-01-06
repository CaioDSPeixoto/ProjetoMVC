using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Enum
{
    public enum TransactionTypeEnum
    {
        [Display(Name = "Crédito (entrar)")]
        Credit = 1,

        [Display(Name = "Débito (sair)")]
        Debit = 2
    }
}
