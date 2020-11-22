using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.DataContext
{
    public interface IDataContext
    {
        void Commit();
        void Rollback();
        int SaveChanges();
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync();
        Task RollbackAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
