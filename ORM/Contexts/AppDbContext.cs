using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models.Entities.ToDo;
using ProjetoMvc.Models.Entities.User;
using ProjetoMvc.ORM.Extension;

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

            DefinindoTabelasParaNaoDeletarEmCasoDeRelacionamento(modelBuilder);

            ConfiguracoesPersonalizadasParaTabelas(modelBuilder);
        }

        private static void DefinindoNomeDasTabelas(ModelBuilder modelBuilder)
        {
            // se não definir, irá pegar conforme está setado nos DbSet anteriormente
            modelBuilder.Entity<UserAccount>().ToTable("Todo");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<UserAccount>().ToTable("User");
        }

        private static void DefinindoTabelasParaNaoDeletarEmCasoDeRelacionamento(ModelBuilder modelBuilder)
        {
            modelBuilder.AplicandoValorParaDeleteBehavior(DeleteBehavior.Restrict);
        }

        private static void ConfiguracoesPersonalizadasParaTabelas(ModelBuilder modelBuilder)
        {
            // Configuração para evitar a exclusão de categorias com tarefas relacionadas
            modelBuilder.Entity<Todo>()
                .HasOne(todo => todo.Category)
                .WithMany()
                .HasForeignKey(todo => todo.CategoryId);

            // Configuração para o campo CreatedByUser
            modelBuilder.Entity<Todo>()
                .HasOne(todo => todo.CreatedByUser)
                .WithMany()
                .HasForeignKey(todo => todo.CreatedByUserId);

            // Configuração para o campo AssignedUser
            modelBuilder.Entity<Todo>()
                .HasOne(todo => todo.AssignedToUser)
                .WithMany()
                .HasForeignKey(todo => todo.AssignedToUserId);

            // Configuração para Permission
            modelBuilder.Entity<UserAccount>()
                .Property(user => user.Permission)
                .HasConversion<string>(); // Quando precisamos armazenar o valor do enum em um campo string no banco

            // Configuração para Status
            modelBuilder.Entity<Category>()
                .Property(category => category.Status)
                .HasConversion<string>(); // Quando precisamos armazenar o valor do enum em um campo string no banco
        }
    }
}
