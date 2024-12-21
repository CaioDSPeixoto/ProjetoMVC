using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, digite um e-mail válido.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
