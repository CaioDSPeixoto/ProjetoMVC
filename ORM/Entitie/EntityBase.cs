using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.ORM.Entitie
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
