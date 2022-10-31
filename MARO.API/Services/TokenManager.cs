using MARO.API;
using MARO.Application.Aggregate.Models;
using MARO.Application.Interfaces;
using MARO.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MARO.Application.Services
{
    public class TokenManager : ITokenManager
    {
        public Task<TokenResult> CreateAccessTokenAsync(User user, string role)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id), new Claim(ClaimTypes.Role, role) };

            var expires = new JwtOptions().EXPIRES;

            var jwt = new JwtSecurityToken(
                issuer: JwtOptions.ISSUER,
                audience: JwtOptions.AUDIENCE,
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Task.FromResult(new TokenResult { Token = encodedJwt, Expires = expires });
        }

        public Task<TokenResult> CreateRefreshTokenAsync()
        {
            var randomNumber = new byte[64];

            using var numberGenerator = RandomNumberGenerator.Create();
            numberGenerator.GetBytes(randomNumber);

            return Task.FromResult(new TokenResult { Token = Convert.ToBase64String(randomNumber) });
        }

        public Task<ClaimsPrincipal>? GetPrincipalFromExpiredTokenAsync(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return Task.FromResult(principal);
        }
    }
}
