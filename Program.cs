using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjetoMvc.Models.Enum;
using ProjetoMvc.ORM.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adicionando cookie para autenticação
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    // Define o tempo de expiração do cookie (ex: 30 minutos)
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    // Se o SlidingExpiration estiver como false, mesmo com o usuário ativo na seção será redirecionado para o login.
    options.SlidingExpiration = false;
});

builder.Services.AddAuthorization(options =>
{
    // adicionando a politica de segurança e adicionando quais permissões são "acessiveis"
    options.AddPolicy("AdminOrDeveloper", policy =>
            policy.RequireRole(UserPermissionEnum.Admin.ToString(), UserPermissionEnum.Developer.ToString()));
});

// DbContext da aplicação
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
