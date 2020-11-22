using Net5Template.Application.Configuration;
using Net5Template.Infrastructure.Configuration;
using Net5Template.Infrastructure.Email;
using Net5Template.Infrastructure.Health;
using Net5Template.Infrastructure.Logging;
using Net5Template.WebAPI.Configuration;
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
            services.AddMyHealthChecks(Configuration);

            //MediatR
            services.AddCQRS(Configuration);

            services.AddEventBusMQ(Configuration, typeof(Startup).Assembly);

            //register dependencies
            services.RegisterIoCWebAPIDependencies()
                .RegisterIoCInfrastructureDependencies(Configuration)
                .RegisterIoCApplicationDependencies();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            if (Environment.GetEnvironmentVariable("USE_HTTP") != "true")
                app.UseHttpsRedirection();

            app.UseHealthChecks();
        }
    }
}
