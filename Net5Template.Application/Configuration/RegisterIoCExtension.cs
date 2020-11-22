using Net5Template.Application.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Application.Configuration
{
    public static class RegisterIoCExtension
    {
        public static IServiceCollection RegisterIoCApplicationDependencies(this IServiceCollection services)
        {
            return services
               //jobs
               .AddTransient<ISystemJobHostedService, SystemJobHostedService>()
               //.AddTransient<IMailingJobAppService, MailingJobAppService>()
               //.AddTransient<IGeoIPJobAppService, GeoIPJobAppService>()
               //.AddTransient<INotificationJobAppService, NotificationJobAppService>()
               ;
        }
    }
}
