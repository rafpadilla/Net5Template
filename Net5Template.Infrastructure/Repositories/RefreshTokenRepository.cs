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
    public class RefreshTokenRepository : Repository<RefreshToken, Guid>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IDataContext context)
               : base(context)
        {
        }

        public Task<RefreshToken> GetTokenByToken(string token)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Token.Equals(token));
        }

        public async Task<IEnumerable<RefreshToken>> GetTokensForUser(string userId)
        {
            return await _dbSet.AsNoTracking().Where(a => a.UserId.Equals(userId)).ToListAsync();
        }

        public async Task InvalidateToken(string token)
        {
            var t = await _dbSet.FirstOrDefaultAsync(a => a.Token.Equals(token));
            t.Invalidated = true;

            await Update(t);
        }

        public async Task InvalidateTokenById(Guid tokenId)
        {
            var t = await _dbSet.FirstOrDefaultAsync(a => a.Id.Equals(tokenId));
            t.Invalidated = true;

            await Update(t);
        }
    }
}
