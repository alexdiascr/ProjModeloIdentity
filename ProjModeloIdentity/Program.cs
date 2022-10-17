using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjModeloIdentity.Areas.Identity.Data;
using ProjModeloIdentity.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AspNetCoreIdentityContextConnection") ?? 
    throw new InvalidOperationException("Connection string 'AspNetCoreIdentityContextConnection' not found.");

// Politica de cookies (LGPD)
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

//Est� se adicionando o IdentityFramework e apontando para um contexto do AspNetCoreIdentityContext
builder.Services.AddDbContext<AspNetCoreIdentityContext>(options =>
    options.UseSqlServer(connectionString));

//Est� se adicionando o Default Identity relacionando o IdentityUSer
//IdentityUSer � uma biblioteca � uma classe do identity, onde o mesmo trabalha como se fosse o usu�rio
//conectado na aplica��o. 
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //Dizendo que suportes roles
    .AddRoles<IdentityRole>()
    //Est� se falando - Adicione camada de apresenta��o(camada visual) do Identity
    .AddDefaultUI()
    //Est� trabalhando com o Stores para trabalhar com EntityFramework, se estivesse se trabalhando com MongoDb
    //Adicionaria-se .AddMongoDbStore
    .AddEntityFrameworkStores<AspNetCoreIdentityContext>();

// Adicionando Autorizações personalizadas por policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PodeExcluir", policy => policy.RequireClaim("PodeExcluir"));

    options.AddPolicy("PodeLer", policy => policy.Requirements.Add(new PermissaoNecessaria("PodeLer")));
    options.AddPolicy("PodeEscrever", policy => policy.Requirements.Add(new PermissaoNecessaria("PodeEscrever")));
});

//Registrando uma injeção de dependência no formato Singleton(Porque vai valer para todo mundo)
builder.Services.AddSingleton<IAuthorizationHandler, PermissaoNecessariaHandler>();

// Adicionando suporte a componentes Razor (ex. Telas do Identity)
builder.Services.AddRazorPages();

var app = builder.Build();

// *** Configurando o resquest dos serviços no pipeline *** 

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/erro/500");
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
    app.UseHsts();
}


//Redirecionamento para HTTPs
app.UseHttpsRedirection();
//Uso de arquivos estáticos (ex. CSS, JS)
app.UseStaticFiles();

//Adicionando suporte a rota
app.UseRouting();

//Autenticacao e autorização (Identity)
app.UseAuthentication();
app.UseAuthorization();

//Rota padrão
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

//Mapeando componentes Razor Pages (ex: Identity)
app.MapRazorPages();

app.Run();
