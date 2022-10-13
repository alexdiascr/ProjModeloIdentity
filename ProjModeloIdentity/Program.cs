using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjModeloIdentity.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AspNetCoreIdentityContextConnection") ?? 
    throw new InvalidOperationException("Connection string 'AspNetCoreIdentityContextConnection' not found.");


//Está se adicionando o IdentityFramework e apontando para um contexto do AspNetCoreIdentityContext
builder.Services.AddDbContext<AspNetCoreIdentityContext>(options =>
    options.UseSqlServer(connectionString));

//Está se adicionando o Default Identity relacionando o IdentityUSer
//IdentityUSer é uma biblioteca é uma classe do identity, onde o mesmo trabalha como se fosse o usuário
//conectado na aplicação. 
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //Está se falando - Adicione camada de apresentação(camada visual) do Identity
    .AddDefaultUI()
    //Está trabalhando com o Stores para trabalhar com EntityFramework, se estivesse se trabalhando com MongoDb
    //Adicionaria-se .AddMongoDbStore
    .AddEntityFrameworkStores<AspNetCoreIdentityContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
