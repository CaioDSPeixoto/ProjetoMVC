using Microsoft.EntityFrameworkCore;

namespace ProjetoMvc.ORM.Extension
{
    public static class ModelBuilderExtensions
    {
        public static void AplicandoValorParaDeleteBehavior(this ModelBuilder modelBuilder, DeleteBehavior deleteBehavior)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = deleteBehavior;
            }
        }
    }
}
