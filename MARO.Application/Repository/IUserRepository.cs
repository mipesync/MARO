using MARO.Application.Aggregate.Models.DTOs;

namespace MARO.Application.Repository
{
    public interface IUserRepository
    {
        Task AddUserDetails(UserDetailsDto model, string userId);
        Task UpdateUserCriteria(Guid userId, long itemIds);
    }
}
