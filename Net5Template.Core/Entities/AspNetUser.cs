using Net5Template.Core.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Entities
{
    public class AspNetUser : IdentityUser<Guid>, IEntity<Guid>
    {
        public override Guid Id { get => base.Id; set => base.Id = value; }
    }
}
