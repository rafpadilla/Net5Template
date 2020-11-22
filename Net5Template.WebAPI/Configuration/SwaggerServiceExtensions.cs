using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Configuration
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, Type assemblyDocType)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Net5Template API",
                    Version = "v1",
                    Description = "API de Net5Template en .NET Core"//,
                    //TermsOfService = new Uri("https://yourdomain.com"),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Contact YourCompany",
                    //    Email = "contact@yourdomain.com",
                    //    Url = new Uri("https://yourdomain.com"),
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Licencia de uso",
                    //    Url = new Uri("https://yourdomain.com"),
                    //}
                });

                c.DocInclusionPredicate((_, a) => true);// !string.IsNullOrWhiteSpace(a.GroupName));

                c.TagActionsBy(a => new List<string> { a.GroupName });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{assemblyDocType.Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Net5Template API v1");

                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });

            return app;
        }
    }
}
