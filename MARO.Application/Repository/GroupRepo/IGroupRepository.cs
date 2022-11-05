using MARO.Application.Aggregate.Models.ResponseModels;

namespace MARO.Application.Repository.GroupRepo
{
    public interface IGroupRepository
    {
        Task<CreateGroupResponseModel> CreateGroup(Guid userId, string webRootPath, string host);
        Task JoinGroup(Guid groupId, Guid userId);
        Task DeleteGroup(Guid groupId, string webRootPath);
        Task<GroupDetailsResponseModel> GroupDetails(Guid groupId);
    }
}
