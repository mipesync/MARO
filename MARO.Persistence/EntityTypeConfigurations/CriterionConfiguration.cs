using MARO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARO.Persistence.EntityTypeConfigurations
{
    public class CriterionConfiguration : IEntityTypeConfiguration<Criterion>
    {
        public void Configure(EntityTypeBuilder<Criterion> builder)
        {
            builder.ToTable("Criteria");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(32)
                .IsRequired();
        }
    }
}
