using Net5Template.Core.Domain;
using Net5Template.Core.Repository;
using Net5Template.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net5Template.Infrastructure.Persistence.Repository
{
    public class RepositoryValueObject<TEntityValueObject> : IRepositoryValueObject<TEntityValueObject> where TEntityValueObject : class, IValueObject
    {
        protected readonly IDataContext _context;
        protected readonly DbSet<TEntityValueObject> _dbSet;

        public RepositoryValueObject(IDataContext context)
        {
            _context = context;
            if (context is DbContext dbContext)
            {
                _dbSet = dbContext.Set<TEntityValueObject>();
            }
        }
        public IQueryable<TEntityValueObject> Queryable()
        {
            return _dbSet;
        }
    }
}
