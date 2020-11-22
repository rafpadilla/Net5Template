using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Application.Configuration
{
    public static class MapperExtension
    {
        public static IServiceCollection RegisterMapperDomainDTODependencies(this IServiceCollection services)
        {
            return services.AddAutoMapper(//cfg => { cfg.AddExpressionMapping(); }, 
                                          //typeof(AutoMapperProfile).Assembly,
                typeof(MapperExtension).Assembly);
        }
    }
}
