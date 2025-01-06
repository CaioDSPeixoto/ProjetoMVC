using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Enum
{
    public enum AtivoInativoEnum
    {
        [Display(Name = "Inativo")]
        Inactive,

        [Display(Name = "Ativo")]
        Active
    }
}
