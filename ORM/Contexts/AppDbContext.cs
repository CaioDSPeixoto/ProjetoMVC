using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models.Entities;
using ProjetoMvc.Models.Entities.Payment;
using ProjetoMvc.Models.Entities.ToDo;
using ProjetoMvc.Models.Entities.User;
using ProjetoMvc.Models.Enum;
using ProjetoMvc.ORM.Extension;

namespace ProjetoMvc.ORM.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed();

            DefinindoNomeDasTabelas(modelBuilder);

            DefinindoTabelasParaNaoDeletarEmCasoDeRelacionamento(modelBuilder);

            ConfiguracoesPersonalizadasParaTabelas(modelBuilder);
        }

        private static void DefinindoNomeDasTabelas(ModelBuilder modelBuilder)
        {
            // se não definir, irá pegar conforme está setado nos DbSet anteriormente
            modelBuilder.Entity<User>().ToTable("Todo");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
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
            modelBuilder.Entity<User>()
                .Property(user => user.Permission)
                .HasConversion<string>(); // Quando precisamos armazenar o valor do enum em um campo string no banco

            // Configuração para Status
            modelBuilder.Entity<Category>()
                .Property(category => category.Status)
                .HasConversion<string>(); // Quando precisamos armazenar o valor do enum em um campo string no banco

            // Configuração para o relacionamento entre Account e Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(transaction => transaction.Account) // Uma transação pertence a uma conta
                .WithMany(account => account.Transactions)  // Uma conta pode ter várias transações
                .HasForeignKey(transaction => transaction.AccountId) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Definir comportamento de exclusão
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // cria esses valores de forma automática ao iniciar um banco de dados vazio
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 101,
                    Name = "Contas Fixas",
                    Status = AtivoInativoEnum.Active,
                    UserId = null
                },
                new Account
                {
                    Id = 102,
                    Name = "Contas Variáveis",
                    Status = AtivoInativoEnum.Active,
                    UserId = null
                },
                new Account
                {
                    Id = 103,
                    Name = "Investimentos",
                    Status = AtivoInativoEnum.Active,
                    UserId = null
                }
            );
        }
    }
}
