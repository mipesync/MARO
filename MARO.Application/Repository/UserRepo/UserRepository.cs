using AutoMapper;
using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Aggregate.Models.ResponseModels;
using MARO.Application.Common.Exceptions;
using MARO.Application.Interfaces;
using MARO.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MARO.Application.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMARODbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<User> userManager, IMARODbContext dbContext, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task AddUserDetails(UserDetailsDto model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) throw new NotFoundException(nameof(User), userId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = $"{model.LastName} {model.FirstName}";
            user.Age = model.Age;

            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateUserCriteria(Guid userId, long itemIds)
        {
            var user = await _dbContext.Users.Include(u => u.UserItems).FirstOrDefaultAsync(u => u.Id == userId.ToString());

            if (user == null) throw new NotFoundException(nameof(User), userId);

            var items = await GetItemsFromBinary(itemIds);

            var userItems = new List<UserItem>();

            foreach (var item in items)
            {
                userItems.Add(new UserItem { CriterionItem = item, User = user });
            }

            _dbContext.UserItems.RemoveRange(user.UserItems!);
            user.UserItems = userItems;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        private async Task<List<CriterionItem>> GetItemsFromBinary(long itemIds)
        {
            var binary = Convert.ToString(itemIds, 2).Reverse();

            var items = new List<CriterionItem>();

            int iter = 1;

            foreach (var elem in binary)
            {
                if (elem.Equals('1'))
                {
                    var criterionItem = await _dbContext.CriterionItems.FirstOrDefaultAsync(u => u.Id == iter)!;
                    items.Add(criterionItem!);
                }
                iter++;
            }

            return items;
        }

        public async Task<UserDetailsResponseModel> UserDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) throw new NotFoundException(nameof(User), userId);

            var role = await _userManager.GetRolesAsync(user);

            if (role.Count() == 0) throw new NotFoundException(nameof(IdentityRole), userId);

            user.Role.Name = role.FirstOrDefault();

            var responseModel = _mapper.Map<UserDetailsResponseModel>(user);

            return responseModel;
        }
    }
}
