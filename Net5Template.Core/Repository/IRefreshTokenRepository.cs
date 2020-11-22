using Net5Template.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Repository
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
    {
        Task<RefreshToken> GetTokenByToken(string token);
        Task<IEnumerable<RefreshToken>> GetTokensForUser(string userId);
        Task InvalidateToken(string token);
        Task InvalidateTokenById(Guid tokenId);
    }
}
