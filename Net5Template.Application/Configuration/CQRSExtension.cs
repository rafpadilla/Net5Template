using Net5Template.Core.Bus;
using Net5Template.Core.Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Application.Configuration
{
    public static class CQRSExtension
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(IQueryBus).Assembly, typeof(CQRSExtension).Assembly);
            
            return services;
        }
    }
}
