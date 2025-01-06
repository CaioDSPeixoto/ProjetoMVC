using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models.Enum;
using ProjetoMvc.ORM.Entitie;
using ProjetoMvc.Validators;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Entities.User
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : EntityBase
    {
        #region Atributos normais do UserAccount
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
        #endregion

        #region Bloqueio de login
        [Display(Name = "Bloqueado Por")]
        public User? BlockedBy { get; set; }

        [Display(Name = "Bloqueado Até")]
        [FututeOrPresent]
        public DateTime? BlockedUntil { get; set; }
        #endregion
    }
}
