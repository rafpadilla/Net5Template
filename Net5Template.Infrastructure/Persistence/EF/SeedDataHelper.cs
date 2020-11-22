using Net5Template.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Persistence.EF
{
    internal class SeedDataHelper
    {
        public static IdentityRole<Guid>[] GetRoles()
        {
            return new[] {new IdentityRole<Guid>() { Id = new Guid("eae096b8-cfd5-47bd-a860-474c04f932ab"), ConcurrencyStamp = "ff3946c3-799e-4531-9492-f8631b585f7a", NormalizedName = UserRoleEnum.Administrador.ToString().ToUpper(), Name = UserRoleEnum.Administrador.ToString() },
                new IdentityRole<Guid>() { Id = new Guid("5185af78-dd09-43be-9994-09b0829840fc"), ConcurrencyStamp = "1d7ea43c-d97c-4aca-9990-607bdbe27dee", NormalizedName = UserRoleEnum.Colaborador.ToString().ToUpper(), Name = UserRoleEnum.Colaborador.ToString() },
                new IdentityRole<Guid>() { Id = new Guid("0c946d83-83d7-4649-b94d-5e4278371d7c"), ConcurrencyStamp = "daa2f9c3-e0e0-4813-83a3-88279c874ce5", NormalizedName = UserRoleEnum.Desarrollador.ToString().ToUpper(), Name = UserRoleEnum.Desarrollador.ToString() },
                new IdentityRole<Guid>() { Id = new Guid("ba7ac007-3fd3-4256-8a4e-d0fe60afcf15"), ConcurrencyStamp = "72125014-5d4e-418b-b3da-9b23a0757faf", NormalizedName = UserRoleEnum.Editor.ToString().ToUpper(), Name = UserRoleEnum.Editor.ToString() },
                new IdentityRole<Guid>() { Id = new Guid("05dba86c-d15d-432b-9135-047c3a1cf183"), ConcurrencyStamp = "89382bf4-5b6d-4356-abcd-ff91b708909e", NormalizedName = UserRoleEnum.Lector.ToString().ToUpper(), Name = UserRoleEnum.Lector.ToString() },
                new IdentityRole<Guid>() { Id = new Guid("c384342d-7b3f-4cd0-8e31-2605b3dd26fc"), ConcurrencyStamp = "926584ec-872d-4e73-a560-d17b4de72a4c", NormalizedName = UserRoleEnum.Soporte.ToString().ToUpper(), Name = UserRoleEnum.Soporte.ToString() }};
        }
    }
}
