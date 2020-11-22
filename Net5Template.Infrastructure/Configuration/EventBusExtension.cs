using Net5Template.Core.Bus.RabbitMQ;
using Net5Template.Infrastructure.Bus.RabbitMQ;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Configuration
{
    public static class EventBusExtension
    {
        public static IServiceCollection AddEventBusMQ(this IServiceCollection services, IConfiguration configuration, params Assembly[] consumerAssemblies)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                if (consumerAssemblies != null)
                    x.AddConsumers(consumerAssemblies);

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], c =>
                    {
                        c.Username(configuration["RabbitMQ:UserName"]);
                        c.Password(configuration["RabbitMQ:Password"]);
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddMassTransitHostedService();

            services.AddTransient<IMessagePublish, MessagePublish>();

            return services;
        }
    }
}
