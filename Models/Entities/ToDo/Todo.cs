using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProjetoMvc.Models.Entities.User;
using ProjetoMvc.ORM.Entitie;
using ProjetoMvc.Validators;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Entities.ToDo
{
    public class Todo : EntityBase
    {
        #region Dados das tarefas
        [Display(Name = "Título")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Descrição")]
        public string? Description { get; set; }

        public DateTime CreateAt { get; set; }

        [Display(Name = "Data de Entrega")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [FututeOrPresent]
        public DateTime DeadLine { get; set; }

        public DateTime? FinishedAt { get; set; }
        #endregion

        #region Dados da Categoria
        [Required(ErrorMessage = "Selecione uma categoria.")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
        #endregion

        #region Dados de Usuário
        [Display(Name = "Criado por")]
        [Required(ErrorMessage = "O criador da tarefa é obrigatório.")]
        public int CreatedByUserId { get; set; }

        [ValidateNever]
        public UserAccount CreatedByUser { get; set; }

        [Display(Name = "Atribuído a")]
        public int? AssignedToUserId { get; set; }

        [ValidateNever]
        public UserAccount? AssignedToUser { get; set; }
        #endregion

        public void Create(int createdByUserId)
        {
            CreateAt = DateTime.Now;
            CreatedByUserId = createdByUserId;
        }

        public void Finish()
        {
            FinishedAt = DateTime.Now;
        }
    }
}
