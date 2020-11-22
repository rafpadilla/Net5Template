using Net5Template.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Persistence.EF
{
    public static class SqlServerDbExtension
    {
        public static IServiceCollection AddSqlServerDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("AppSettings:UseEFIdentity"))
            {
                services.AddDbContext<Net5TemplateIdentityContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("sqlserver"),
                        options => options.MigrationsAssembly(typeof(Net5TemplateIdentityContext).Assembly.FullName));
                }).AddIdentity<AspNetUser, IdentityRole<Guid>>()
                 .AddEntityFrameworkStores<Net5TemplateIdentityContext>()
                 .AddDefaultTokenProviders();
            }
            else
            {
                services.AddDbContext<Net5TemplateContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("sqlserver"),
                        options => options.MigrationsAssembly(typeof(Net5TemplateContext).Assembly.FullName));
                });
            }
            return services;
        }
    }
}
