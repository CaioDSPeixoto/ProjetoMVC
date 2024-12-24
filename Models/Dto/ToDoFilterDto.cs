namespace ProjetoMvc.Models.Dto
{
    public class ToDoFilterDto
    {
        public string SearchTitle { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsFinished { get; set; } = false; // Definindo "não finalizado" como padrão
        public int? CategoryFilter { get; set; }
        public int? CreatedBy { get; set; }
        public int? AssignedTo { get; set; }
    }
}
