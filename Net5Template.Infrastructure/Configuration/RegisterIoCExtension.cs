using Net5Template.Core.Bus;
using Net5Template.Core.Repository;
using Net5Template.Core.Services;
using Net5Template.Infrastructure.Bus;
using Net5Template.Infrastructure.Caching;
using Net5Template.Infrastructure.DataContext;
using Net5Template.Infrastructure.Email;
using Net5Template.Infrastructure.Formatters;
using Net5Template.Infrastructure.ImageProcessing;
using Net5Template.Infrastructure.Persistence.EF;
using Net5Template.Infrastructure.Repositories;
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
            switch (configuration["AppSettings:UseDatabase"])
            {
                //case "mongodb":
                //    //mongo
                //    services
                //        .AddTransient<ILogRepository, Repositories.Mongo.LogRepository>()
                //        //cached repository
                //        //.AddTransient<ICountryRepository, Repositories.Mongo.CountryRepository>();
                //        ;
                //    break;
                case "sqlserver":
                default:
                    services
                        .AddTransient<IRefreshTokenRepository, RefreshTokenRepository>()
                        .AddTransient<IAspNetUserRepository, AspNetUserRepository>()
                        .AddTransient<ILogRepository, LogRepository>()
                        //cached repository
                        //.AddTransient<ICountryRepository, Repositories.EfCore.CountryRepository>();
                        ;
                    if (configuration.GetValue<bool>("AppSettings:UseEFIdentity"))
                    {
                        services.AddScoped<IDataContext, Net5TemplateIdentityContext>();
                    }
                    else
                    {
                        services.AddScoped<IDataContext, Net5TemplateContext>();
                    }
                    break;
            }

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });

            //CQRS
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();

            return services
                //infrastructure services
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IEmailService, EmailService>()
                .AddSingleton<ICSVService, CSVService>()
                //.AddSingleton<IGeoAPIAppService, GeoAPIService>()
                .AddSingleton<IImageService, ImageService>()
                //cache service
                .AddTransient<IDataCacheService, DataCacheService>()
                ;
        }
    }
}
