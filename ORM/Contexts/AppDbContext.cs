using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models.Entities.ToDo;
using ProjetoMvc.Models.Entities.User;

namespace ProjetoMvc.ORM.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<UserAccount> Users => Set<UserAccount>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DefinindoNomeDasTabelas(modelBuilder);

            ConfiguracoesPersonalizadasParaTabelas(modelBuilder);
        }

        private static void ConfiguracoesPersonalizadasParaTabelas(ModelBuilder modelBuilder)
        {
            // Configuração para evitar a exclusão de categorias com tarefas relacionadas
            modelBuilder.Entity<Todo>()
                .HasOne(todo => todo.Category)
                .WithMany()
                .HasForeignKey(todo => todo.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração para Permission
            modelBuilder.Entity<UserAccount>()
                .Property(user => user.Permission)
                .HasConversion<string>(); // Caso use enum para Permission

            // Configuração para Status
            modelBuilder.Entity<Category>()
                .Property(category => category.Status)
                .HasConversion<string>(); // Caso use enum para Status
        }

        private static void DefinindoNomeDasTabelas(ModelBuilder modelBuilder)
        {
            // se não definir, irá pegar conforme está setado nos DbSet anteriormente
            modelBuilder.Entity<UserAccount>().ToTable("Todo");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<UserAccount>().ToTable("User");
        }
    }
}
