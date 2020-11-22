using Net5Template.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Repository
{
    public interface ILogRepository : IRepository<Log, int>
    {
        Task<IEnumerable<Log>> GetLogs();
    }
}
