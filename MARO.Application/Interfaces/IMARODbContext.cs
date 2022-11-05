using MARO.Domain;
using Microsoft.EntityFrameworkCore;

namespace MARO.Application.Interfaces
{
    public interface IMARODbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Criterion> Criteria { get; set; }
        DbSet<UserCriteria> UserCriteria { get; set; }
        DbSet<UserItem> UserItems { get; set; }
        DbSet<CriterionItem> CriterionItems { get; set; }
        DbSet<Group> Groups { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
