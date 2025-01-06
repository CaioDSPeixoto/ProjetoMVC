using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Enum
{
    public enum UserPermissionEnum
    {
        [Display(Name = "Administrador")]
        Admin,

        [Display(Name = "Desenvolvedor")]
        Developer,

        [Display(Name = "Usuário Padrão")]
        Default
    }
}
