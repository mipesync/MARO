using MARO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARO.Persistence.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(u => u.FirstName)
                .HasMaxLength(32);

            builder.Property(u => u.LastName)
                .HasMaxLength(32);

            builder.Property(u => u.FullName)
                .HasMaxLength(64);

            builder.HasMany(u => u.Criteria)
                .WithMany(u => u.Users)
                .UsingEntity<UserCriteria>(
                u => u
                    .HasOne(x => x.Criterion)
                    .WithMany(x => x.UserCriteria)
                    .HasForeignKey(x => x.CriterionId),
                o => o
                    .HasOne(x => x.User)
                    .WithMany(x => x.UserCriteria)
                    .HasForeignKey(x => x.UserId),
                p =>
                {
                    p.HasKey(new string[] { nameof(UserCriteria.UserId), nameof(UserCriteria.CriterionId) });
                    p.ToTable("UserCriteria");
                });
        }
    }
}
