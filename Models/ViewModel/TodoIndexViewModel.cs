using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoMvc.Models.Dto;
using ProjetoMvc.Models.Entities.ToDo;

namespace ProjetoMvc.Models.ViewModel
{
    public class TodoIndexViewModel : ToDoFilterDto
    {
        public List<Todo> Todos { get; set; }
        public SelectList IsFinishedSelectList { get; set; }
        public SelectList Categories { get; set; }
        public bool FiltersApplied { get; set; }
    }
}
