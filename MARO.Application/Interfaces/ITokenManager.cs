using MARO.Application.Aggregate.Models;
using MARO.Domain;
using System.Security.Claims;

namespace MARO.Application.Interfaces
{
    public interface ITokenManager
    {
        Task<TokenResult> CreateAccessTokenAsync(User user, string role);
        Task<TokenResult> CreateRefreshTokenAsync();
        public Task<ClaimsPrincipal>? GetPrincipalFromExpiredTokenAsync(string? token);
    }
}
