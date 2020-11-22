using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Net5Template.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Identity.Queries
{
    public class GetRefreshTokenQuery : IQuery<GetRefreshTokenDTO>
    {
        public GetRefreshTokenQuery(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
        public string RefreshToken { get; set; }
    }
    public class GetRefreshTokenDTO
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Invalidated { get; set; }
        public string UserId { get; set; }
    }
    public class GetRefreshTokenMapping : Profile
    {
        public GetRefreshTokenMapping() => CreateMap<RefreshToken, GetRefreshTokenDTO>();
    }
    public class GetRefreshTokenHandler : IQueryHandler<GetRefreshTokenQuery, GetRefreshTokenDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public GetRefreshTokenHandler(IMapper mapper, IRefreshTokenRepository refreshTokenRepository)
        {
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<GetRefreshTokenDTO> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var results = await _refreshTokenRepository.GetTokenByToken(request.RefreshToken);
            
            return _mapper.Map<GetRefreshTokenDTO>(results);
        }
    }
}
