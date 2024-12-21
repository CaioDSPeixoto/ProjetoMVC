using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models.Enum;
using ProjetoMvc.ORM.Entitie;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Entities.User
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserAccount : EntityBase
    {
        [Display(Name = "Primeiro Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = string.Empty;

        [Display(Name = "Permissão")]
        public UserPermissionEnum Permission { get; set; } = UserPermissionEnum.Default;
    }
}
