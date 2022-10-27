using MARO.Application.Models;
using MARO.Domain;

namespace MARO.Application.Interfaces
{
    public interface ITokenManager
    {
        Task<TokenResult> CreateAccessTokenAsync(User user, string role);
        Task<TokenResult> CreateRefreshTokenAsync();
    }
}
