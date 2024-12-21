using ProjetoMvc.Models.Enum;
using ProjetoMvc.ORM.Entitie;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Entities.ToDo
{
    public class Category : EntityBase
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Status")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public AtivoInativoEnum Status { get; set; } = AtivoInativoEnum.Ativo;
    }
}
