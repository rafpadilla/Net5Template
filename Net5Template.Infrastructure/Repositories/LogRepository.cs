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
    public class LogRepository : Repository<Log, int>, ILogRepository
    {
        public LogRepository(IDataContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Log>> GetLogs()
        {
            return await _dbSet.Take(50).ToListAsync();
        }
    }
}
