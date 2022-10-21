using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjModeloIdentity.Areas.Identity.Data;
using ProjModeloIdentity.Extensions;

namespace ProjModeloIdentity.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddAuthorizationConfig(this IServiceCollection services)
        {
            // Adicionando Autorizações personalizadas por policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PodeExcluir", policy => policy.RequireClaim("PodeExcluir"));

                options.AddPolicy("PodeLer", policy => policy.Requirements.Add(new PermissaoNecessaria("PodeLer")));
                options.AddPolicy("PodeEscrever", policy => policy.Requirements.Add(new PermissaoNecessaria("PodeEscrever")));
            });

            return services;
        }

        public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AspNetCoreIdentityContextConnection") ??
                throw new InvalidOperationException("Connection string 'AspNetCoreIdentityContextConnection' not found.");


            //Chega se é necessário algum consetimento para acessar o site etc
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Est� se adicionando o IdentityFramework e apontando para um contexto do AspNetCoreIdentityContext
            services.AddDbContext<AspNetCoreIdentityContext>(options =>
                options.UseSqlServer(connectionString));

            //Est� se adicionando o Default Identity relacionando o IdentityUSer
            //IdentityUSer � uma biblioteca � uma classe do identity, onde o mesmo trabalha como se fosse o usu�rio
            //conectado na aplica��o. 
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //Dizendo que suportes roles
                .AddRoles<IdentityRole>()
                //Est� se falando - Adicione camada de apresenta��o(camada visual) do Identity
                .AddDefaultUI()
                //Est� trabalhando com o Stores para trabalhar com EntityFramework, se estivesse se trabalhando com MongoDb
                //Adicionaria-se .AddMongoDbStore
                .AddEntityFrameworkStores<AspNetCoreIdentityContext>();

            return services;
        }
    }
}
