using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjModeloIdentity.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AspNetCoreIdentityContextConnection") ?? 
    throw new InvalidOperationException("Connection string 'AspNetCoreIdentityContextConnection' not found.");


//Est� se adicionando o IdentityFramework e apontando para um contexto do AspNetCoreIdentityContext
builder.Services.AddDbContext<AspNetCoreIdentityContext>(options =>
    options.UseSqlServer(connectionString));

//Est� se adicionando o Default Identity relacionando o IdentityUSer
//IdentityUSer � uma biblioteca � uma classe do identity, onde o mesmo trabalha como se fosse o usu�rio
//conectado na aplica��o. 
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //Est� se falando - Adicione camada de apresenta��o(camada visual) do Identity
    .AddDefaultUI()
    //Est� trabalhando com o Stores para trabalhar com EntityFramework, se estivesse se trabalhando com MongoDb
    //Adicionaria-se .AddMongoDbStore
    .AddEntityFrameworkStores<AspNetCoreIdentityContext>();

// Add services to the container.
builder.Services.AddRazorPages();

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

//Precisa estar configurado para trabalhar com autentica��o
app.UseAuthentication();;

app.UseAuthorization();

// Rota padrão
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Mapeando componentes Razor Pages (ex: Identity)
app.MapRazorPages();

app.Run();
