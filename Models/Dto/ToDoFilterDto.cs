namespace ProjetoMvc.Models.Dto
{
    public class ToDoFilterDto
    {
        public string SearchTitle { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsFinished { get; set; }
        public int? CategoryFilter { get; set; }
    }
}
