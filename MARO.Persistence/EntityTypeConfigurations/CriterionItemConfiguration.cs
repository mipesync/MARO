using MARO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARO.Persistence.EntityTypeConfigurations
{
    public class CriterionItemConfiguration : IEntityTypeConfiguration<CriterionItem>
    {
        public void Configure(EntityTypeBuilder<CriterionItem> builder)
        {
            builder.ToTable("CriterionItems");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(32)
                .IsRequired();
        }
    }
}
