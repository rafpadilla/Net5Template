using Net5Template.Core.Bus;
using Net5Template.Core.Services;
using Net5Template.Infrastructure.Bus;
using Net5Template.Infrastructure.ImageProcessing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Configuration
{
    public static class ResiterIoCExtension
    {
        public static IServiceCollection RegisterIoCInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //CQRS
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();

            return services.AddSingleton<IImageService, ImageService>();
        }
    }
}
