using Net5Template.Application.Services.Identity.Queries;
using Net5Template.Core;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Identity.Commands
{
    public class GenerateTokenJWTCommand : ICommand<GenerateTokenJWTDTO>
    {
        public GenerateTokenJWTCommand(Guid userId, string fingerprint, bool isRefreshOperation = false, string refreshJwtId = null)
        {
            UserId = userId;
            Fingerprint = fingerprint;
            IsRefreshOperation = isRefreshOperation;
            RefreshJwtId = refreshJwtId;
        }
        public Guid UserId { get; }
        public string Fingerprint { get; }
        public bool IsRefreshOperation { get; }
        public string RefreshJwtId { get; }
    }
    public class GenerateTokenJWTDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset Exp { get; set; }
    }
    public class GenerateTokenJWTCommandHandler : ICommandHandler<GenerateTokenJWTCommand, GenerateTokenJWTDTO>
    {
        private readonly IConfiguration _configuration;
        private readonly IQueryBus _queryBus;
        private readonly ICommandBus _commandBus;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParams;

        public GenerateTokenJWTCommandHandler(IConfiguration configuration, IQueryBus queryBus, ICommandBus commandBus, UserManager<AspNetUser> userManager,
            TokenValidationParameters tokenValidationParams)
        {
            _configuration = configuration;
            _queryBus = queryBus;
            _commandBus = commandBus;
            _userManager = userManager;
            _tokenValidationParams = tokenValidationParams;
        }
        public async Task<GenerateTokenJWTDTO> Handle(GenerateTokenJWTCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToStringUser());
            if (user == null)
                throw new ArgumentException();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            //headers
            var header = new JwtHeader(cred);

            var jwtid = !request.IsRefreshOperation && string.IsNullOrEmpty(request.RefreshJwtId) ? Guid.NewGuid().ToString() : request.RefreshJwtId;

            //Expires 24h after creation
            var accessTokenLifeTimeHours = _configuration.GetValue<int>("JWT:AccessTokenLifeTimeHours");
            var refreshTokenLifeTimeDays = _configuration.GetValue<int>("JWT:RefreshTokenLifeTimeDays");
            var expiration = DateTimeOffset.UtcNow.AddHours(accessTokenLifeTimeHours);

            //claims
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, jwtid),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,expiration.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Prn, request.Fingerprint)//principal -> fingerprint
            };

            foreach (var r in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            //payload
            var accessPayload = new JwtPayload(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expiration.UtcDateTime
                );


            //token generation
            var accessToken = new JwtSecurityToken(header, accessPayload);
            var refreshToken = GetHash(GetStringForHash(user.Id.ToStringUser(), request.Fingerprint));

            if (!request.IsRefreshOperation)
            {
                //check for stored token (maybe expired)
                var currentToken = await _queryBus.Send(new GetRefreshTokenQuery(refreshToken));

                //save token
                if (currentToken == null)
                    await _commandBus.Send(new CreateRefreshTokenCommand(Guid.NewGuid(), refreshToken, jwtid, DateTime.UtcNow, DateTime.UtcNow.AddDays(refreshTokenLifeTimeDays), false, user.Id.ToStringUser()));
                else
                    await _commandBus.Send(new UpdateRefreshTokenCommand(currentToken.Id, refreshToken, jwtid, DateTime.UtcNow, DateTime.UtcNow.AddDays(refreshTokenLifeTimeDays), false, user.Id.ToStringUser()));
            }

            //important
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            return new GenerateTokenJWTDTO()
            {
                RefreshToken = refreshToken,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                Exp = expiration
            };
        }
        private string GetStringForHash(string userId, string fingerprint)
        {
            return $"{userId}-{fingerprint}";
        }
        private string GetHash(/*HashAlgorithm hashAlgorithm, */string input)
        {
            using var hashAlgorithm = SHA256.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
