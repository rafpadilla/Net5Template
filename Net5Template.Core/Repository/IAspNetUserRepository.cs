using Net5Template.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Repository
{
    public interface IAspNetUserRepository : IRepository<AspNetUser, Guid>
    {
    }
}
