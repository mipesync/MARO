using MARO.Application.Common.Interfaces;
using MARO.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MARO.Persistence
{
    public class MARODbContext : IdentityDbContext<User>, IMARODbContext 
    {
        public MARODbContext(DbContextOptions<MARODbContext> options) : base(options) { }

        public DbSet<Criterion> Categories { get; set; } = null!;
        public DbSet<Criterion> Criteria { get; set; } = null!;
        public DbSet<UserCriteria> UserCriteria { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
