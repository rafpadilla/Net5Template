using Net5Template.Application.Services.Identity.Queries;
using Net5Template.Core;
using Net5Template.Core.Bus;
using Net5Template.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.Services.Identity.Commands
{
    public class RefreshTokenCommand : ICommand<RefreshTokenCommandDTO>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string FingerPrint { get; set; }
    }
    public class RefreshTokenCommandDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset Exp { get; set; }
    }
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenCommandDTO>
    {
        private readonly IConfiguration _configuration;
        private readonly IQueryBus _queryBus;
        private readonly ICommandBus _commandBus;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParams;

        public RefreshTokenCommandHandler(IConfiguration configuration, IQueryBus queryBus, ICommandBus commandBus, UserManager<AspNetUser> userManager,
            TokenValidationParameters tokenValidationParams)
        {
            _configuration = configuration;
            _queryBus = queryBus;
            _commandBus = commandBus;
            _userManager = userManager;
            _tokenValidationParams = tokenValidationParams;
        }
        public async Task<RefreshTokenCommandDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var validatedToken = GetPrincipalFromToken(request.AccessToken);

            if (validatedToken == null)
                throw new ArgumentException("Invalid Token");

            //BEGIN - Check token expiry date
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(a => a.Type == JwtRegisteredClaimNames.Exp).Value);

            var allowRefreshTokenBeforeExpiry = _configuration.GetValue<TimeSpan>("JWT:AllowRefreshTokenBeforeExpiry");

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix).Subtract(allowRefreshTokenBeforeExpiry);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                throw new ArgumentException("Token hasn't expired yet");
            }
            //END - Check token expiry date

            var jti = validatedToken.Claims.Single(a => a.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _queryBus.Send(new GetRefreshTokenQuery(request.RefreshToken));

            if (storedRefreshToken == null)
                throw new ArgumentException("Token does not exists");

            if (!VerifyHash(GetStringForHash(storedRefreshToken.UserId, request.FingerPrint), storedRefreshToken.Token))
                throw new ArgumentException("Fingerprint does not match");

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                throw new ArgumentException("Token expired");

            if (storedRefreshToken.Invalidated)
                throw new ArgumentException("Token is invalidated");

            if (!storedRefreshToken.JwtId.Equals(jti))
                throw new ArgumentException("Token does not match access token");

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(a => a.Type == JwtRegisteredClaimNames.NameId).Value);

            var newToken = await _commandBus.Send(new GenerateTokenJWTCommand(user.Id, request.FingerPrint, true, jti));

            return new RefreshTokenCommandDTO() { AccessToken = newToken.AccessToken, Exp = newToken.Exp, RefreshToken = newToken.RefreshToken };
        }
        private string GetStringForHash(string userId, string fingerprint)
        {
            return $"{userId}-{fingerprint}";
        }
        private ClaimsPrincipal GetPrincipalFromToken(string accessToken)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //var principal = tokenHandler.ValidateToken(token, _tokenValidationParams, out var validatedToken);
                var tokenValidationParameters = _tokenValidationParams.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
        private string GetHash(string input)
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

        // Verify a hash against a string.
        private bool VerifyHash(/*HashAlgorithm hashAlgorithm,*/ string input, string hash)
        {
            using var hashAlgorithm = SHA256.Create();
            // Hash the input.
            var hashOfInput = GetHash(/*hashAlgorithm, */input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
