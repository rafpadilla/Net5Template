using Net5Template.Core.Entities;
using Net5Template.Infrastructure.EntityMapping;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Persistence.EF
{
    /// <summary>
    /// Add Migration -> from src folder
    /// dotnet ef migrations add InitialMigration -s .\Net5Template.WebAPI -p .\Net5Template.Infrastructure -o .\Persistence\Ef\Migrations -c Net5TemplateIdentityContext
    /// 
    /// Add Migration MySql
    /// dotnet ef migrations add InitialMySql -p ..\Net5Template.Infrastructure.EfCore
    /// 
    /// Remove Migration
    /// dotnet ef migrations remove -s .\Net5Template.WebAPI -p .\Net5Template.Infrastructure -c Net5TemplateIdentityContext
    /// 
    /// Update Database
    /// dotnet ef database update -s .\Net5Template.WebAPI
    /// dotnet ef database update -s .\Net5Template.WebAPI <TargetMigrationName>
    /// </summary>
    public class Net5TemplateIdentityContext
        : IdentityDataContext<Net5TemplateIdentityContext,
        IdentityUser<Guid>,
        IdentityRole<Guid>,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        //public virtual DbSet<Country> Countries { get; set; }
        //public virtual DbSet<Event> Events { get; set; }
        //public virtual DbSet<EventSubscriptor> EventSubscriptors { get; set; }
        //public virtual DbSet<Organization> Organizations { get; set; }
        //public virtual DbSet<OrganizationSubscriptor> OrganizationSubscriptors { get; set; }
        //public virtual DbSet<OrganizationTeam> OrganizationTeams { get; set; }
        //public virtual DbSet<Subscriptor> Subscriptors { get; set; }

        public Net5TemplateIdentityContext(DbContextOptions<Net5TemplateIdentityContext> options, IMediator mediator)
            : base(options, mediator)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //register types
            builder.ApplyConfiguration(new AspNetUserMap());
            //builder.ApplyConfiguration(new CountryMap());
            //builder.ApplyConfiguration(new EventMap());
            //builder.ApplyConfiguration(new EventSubscriptorMap());
            builder.ApplyConfiguration(new LogMap());
            //builder.ApplyConfiguration(new OrganizationMap());
            //builder.ApplyConfiguration(new OrganizationSubscriptorMap());
            //builder.ApplyConfiguration(new OrganizationTeamMap());
            builder.ApplyConfiguration(new RefreshTokenMap());
            //builder.ApplyConfiguration(new SubscriptorMap());
        }
    }
}
