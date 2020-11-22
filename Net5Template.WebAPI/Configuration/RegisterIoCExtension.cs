using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Configuration
{
    public static class RegisterIoCExtension
    {
        public static IServiceCollection RegisterIoCWebAPIDependencies(this IServiceCollection services)
        {
            //Los servicios de duración con ámbito (AddScoped) se crean una vez por solicitud del cliente (conexión).
            //Los servicios de duración transitoria (AddTransient) se crean cada vez que el contenedor del servicio los solicita. Esta duración funciona mejor para servicios sin estado ligeros.
            //Los servicios con duración Singleton (AddSingleton) se crean la primera vez que se solicitan, o bien al ejecutar Startup.ConfigureServices y especificar una instancia con el registro del servicio. Cada solicitud posterior usa la misma instancia
            return services
               // .AddTransient<IIdentityService, IdentityService>()
               //user notification
               ;
        }
    }
}
