using KissLog;
using KissLog.AspNetCore;
using KissLog.Formatters;
using Microsoft.AspNetCore.Authorization;
using ProjModeloIdentity.Extensions;

namespace ProjModeloIdentity.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection services)
        {
            //Serve para pegar em qualquer momento o contexto httpText
            services.AddHttpContextAccessor();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //Registrando uma injeção de dependência no formato Singleton(Porque vai valer para todo mundo)
            services.AddSingleton<IAuthorizationHandler, PermissaoNecessariaHandler>();

            // Optional. Register IKLogger if you use KissLog.IKLogger instead of Microsoft.Extensions.Logging.ILogger<>
            services.AddScoped<IKLogger>((provider) => Logger.Factory.Get());

            services.AddScoped<AuditoriaFilter>();

            return services;
        }       

        public static IServiceCollection AddKissLog(this IServiceCollection services)
        {            

            services.AddLogging(logging =>
            {
                logging.AddKissLog(options =>
                {
                    options.Formatter = (FormatterArgs args) =>
                    {
                        if (args.Exception == null)
                            return args.DefaultValue;

                        string exceptionStr = new ExceptionFormatter().Format(args.Exception, args.Logger);

                        return string.Join(Environment.NewLine, new[] { args.DefaultValue, exceptionStr });
                    };
                });
            });

            return services;
        }
    }
}
