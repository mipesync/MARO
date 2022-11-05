using AutoMapper;
using MARO.Application.Aggregate.Models.ResponseModels;
using MARO.Application.Common.Exceptions;
using MARO.Application.Interfaces;
using MARO.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MARO.Application.Repository.GroupRepo
{
    public class GroupRepository : IGroupRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMARODbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IQRGenerator _qRGenerator;

        public GroupRepository(UserManager<User> userManager, IMARODbContext dbContext, RoleManager<IdentityRole> roleManager, IMapper mapper, IQRGenerator qRGenerator)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _mapper = mapper;
            _qRGenerator = qRGenerator;
        }

        public async Task JoinGroup(Guid groupId, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) throw new NotFoundException(nameof(User), userId);

            var group = await _dbContext.Groups.FirstOrDefaultAsync(u => u.Id == groupId.ToString());

            if (group == null) throw new NotFoundException(nameof(Group), groupId);

            var role = await _roleManager.FindByNameAsync("group_admin");

            if (role == null) throw new NotFoundException(nameof(IdentityRole), "group_admin");

            user.GroupId = group.Id;
            user.GroupRoleId = role.Id;

            await _userManager.UpdateAsync(user);
        }

        public async Task<CreateGroupResponseModel> CreateGroup(Guid userId, string webRootPath, string host)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) throw new NotFoundException(nameof(User), userId);

            if (user.GroupId != null) throw new Exception("У пользователя уже есть группа");

            var role = await _roleManager.FindByNameAsync("group_admin");

            if (role == null) throw new NotFoundException(nameof(IdentityRole), "group_admin");

            var group = new Group
            {
                Id = Guid.NewGuid().ToString()
            };

            var path = await GenerateQR(group.Id, webRootPath, host);

            group.QRLink = path;

            user.GroupRoleId = role.Id;
            user.Group = group;

            await _userManager.UpdateAsync(user);

            return new CreateGroupResponseModel
            {
                GroupId = group.Id,
                QRLink = path
            };
        }

        private async Task<string> GenerateQR(string groupId, string webRootPath, string host)
        {
            return await _qRGenerator.GenerateAsync($"{host}/join-group?groupId={groupId}", webRootPath);
        }

        public async Task DeleteGroup(Guid groupId, string webRootPath)
        {
            var group = await _dbContext.Groups.Include(u => u.Users).FirstOrDefaultAsync(u => u.Id == groupId.ToString());

            if (group == null) throw new NotFoundException(nameof(Group), groupId);
            
            foreach(var user in group.Users)
            {
                user.GroupId = null;
            }

            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            File.Delete(webRootPath + group.QRLink);
        }

        public async Task<GroupDetailsResponseModel> GroupDetails(Guid groupId)
        {
            var group = await _dbContext.Groups.Include(u => u.Users).FirstOrDefaultAsync(u => u.Id == groupId.ToString());

            if (group == null) throw new NotFoundException(nameof(Group), groupId);

            return _mapper.Map<GroupDetailsResponseModel>(group);
        }
    }
}
