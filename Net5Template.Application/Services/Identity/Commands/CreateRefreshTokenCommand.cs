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
    public class CreateRefreshTokenCommand : ICommand
    {
        public CreateRefreshTokenCommand(Guid id, string token, string jwtId, DateTime creationDate, DateTime expiryDate, bool invalidated, string userId)
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
    public class CreateRefreshTokenCommandMapping : Profile
    {
        public CreateRefreshTokenCommandMapping() => CreateMap<CreateRefreshTokenCommand, RefreshToken>();
    }
    public class CreateRefreshTokenCommandHandler : ICommandHandler<CreateRefreshTokenCommand>
    {
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public CreateRefreshTokenCommandHandler(IMapper mapper, IRefreshTokenRepository refreshTokenRepository)
        {
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Unit> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenRefresh = _mapper.Map<RefreshToken>(request);

            await _refreshTokenRepository.Add(tokenRefresh);

            return new Unit();
        }
    }
}
