using Net5Template.Application.Configuration;
using Net5Template.Infrastructure.Configuration;
using Net5Template.Infrastructure.Health;
using Net5Template.Infrastructure.Logging;
using Net5Template.WebAPI.Configuration;
using Net5Template.WebAPI.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Net5Template.WebAPI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables()
              .Build();

            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin",
                    options => options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
            });

            services.AddMyHealthChecks(Configuration);

            //MediatR
            services.AddCQRS(Configuration);

            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JWT:Issuer"],
                ValidAudience = Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"])
                        ),
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParams);

            //JWT configuration
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParams;
                });

            services.AddApiVersioning();

            services.AddControllers(options =>
            {
                options.Filters.Add<Net5TemplateExceptionFilterAttribute>();
                options.Filters.Add<Net5TemplateValidationHandler>();
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            IMvcBuilder builder = services.AddRazorPages();

            if (Configuration.GetValue<bool>("AppSettings:EnableSwagger"))
                services.AddSwaggerDocumentation(typeof(Startup));

            //register dependencies
            services.RegisterIoCWebAPIDependencies()
                .RegisterIoCInfrastructureDependencies(Configuration)
                .RegisterIoCApplicationDependencies();

            //register domain mapper
            services.RegisterMapperDomainDTODependencies();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (Configuration.GetValue<bool>("AppSettings:EnableSwagger"))
            {
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Net5Template.API v1"));
                app.UseSwaggerDocumentation();
            }

            //CORS
            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseCors(options => options.SetIsOriginAllowed(_ => true)//para signalr
            //.AllowAnyOrigin()// -> no funciona con signalr
            .AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseSerilogRequestLogging();

            if (Environment.GetEnvironmentVariable("USE_HTTP") != "true")
                app.UseHttpsRedirection();

            app.UseResponseCompression();

            app.UseRouting();

            app.UseHealthChecks();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
