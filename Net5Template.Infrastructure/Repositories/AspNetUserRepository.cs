using Net5Template.Core.Entities;
using Net5Template.Core.Repository;
using Net5Template.Infrastructure.DataContext;
using Net5Template.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Repositories
{
    public class AspNetUserRepository : Repository<AspNetUser, Guid>, IAspNetUserRepository
    {
        public AspNetUserRepository(IDataContext context)
               : base(context)
        {
        }
    }
}
