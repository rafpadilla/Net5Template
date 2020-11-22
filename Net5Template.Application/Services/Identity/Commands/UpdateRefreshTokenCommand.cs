using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Net5Template.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Identity.Commands
{
    public class UpdateRefreshTokenCommand : ICommand
    {
        public UpdateRefreshTokenCommand(Guid id, string token, string jwtId, DateTime creationDate, DateTime expiryDate, bool invalidated, string userId)
        {
            Id = id;
            Token = token;
            JwtId = jwtId;
            CreationDate = creationDate;
            ExpiryDate = expiryDate;
            Invalidated = invalidated;
            UserId = userId;
        }
        public Guid Id { get; }
        public string Token { get; }
        public string JwtId { get; }
        public DateTime CreationDate { get; }
        public DateTime ExpiryDate { get; }
        public bool Invalidated { get; }
        public string UserId { get; }
    }
    public class UpdateRefreshTokenCommandMapping : Profile
    {
        public UpdateRefreshTokenCommandMapping() => CreateMap<UpdateRefreshTokenCommand, RefreshToken>();
    }
    public class UpdateRefreshTokenCommandHandler : ICommandHandler<UpdateRefreshTokenCommand>
    {
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UpdateRefreshTokenCommandHandler(IMapper mapper, IRefreshTokenRepository refreshTokenRepository)
        {
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Unit> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenRefresh = _mapper.Map<RefreshToken>(request);

            await _refreshTokenRepository.Update(tokenRefresh);

            return new Unit();
        }
    }
}
