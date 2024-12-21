using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models
{
    public class RegistrationViewModel
    {
        [Display(Name = "Primeiro Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, digite um e-mail válido.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Por favor, confirme a sua senha.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
