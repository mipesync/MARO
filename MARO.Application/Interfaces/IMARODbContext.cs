using MARO.Domain;
using Microsoft.EntityFrameworkCore;

namespace MARO.Application.Common.Interfaces
{
    public interface IMARODbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Criterion> Criteria { get; set; }
        DbSet<UserCriteria> UserCriteria { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
