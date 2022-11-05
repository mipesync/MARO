using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Aggregate.Models.ResponseModels;

namespace MARO.Application.Repository.UserRepo
{
    public interface IUserRepository
    {
        Task AddUserDetails(UserDetailsDto model, string userId);
        Task UpdateUserCriteria(Guid userId, long itemIds);
        Task<UserDetailsResponseModel> UserDetails(string userId);
    }
}
