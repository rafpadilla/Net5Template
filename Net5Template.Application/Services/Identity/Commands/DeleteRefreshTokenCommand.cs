using AutoMapper;
using Net5Template.Core.Bus;
using Net5Template.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Identity.Commands
{
    public class DeleteRefreshTokenCommand : ICommand
    {
        public DeleteRefreshTokenCommand(string accessToken) => AccessToken = accessToken;
        public string AccessToken { get; }
    }
    public class DeleteRefreshTokenCommandHandler : ICommandHandler<DeleteRefreshTokenCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public DeleteRefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository) => _refreshTokenRepository = refreshTokenRepository;

        public async Task<Unit> Handle(DeleteRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenRefresh = await _refreshTokenRepository.GetTokenByToken(request.AccessToken);

            if (tokenRefresh == null)
                throw new ArgumentException();

            await _refreshTokenRepository.Remove(tokenRefresh);

            return new Unit();
        }
    }
}
