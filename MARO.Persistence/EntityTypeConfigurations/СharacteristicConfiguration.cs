using MARO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARO.Persistence.EntityTypeConfigurations
{
    public class СharacteristicConfiguration : IEntityTypeConfiguration<Сharacteristic>
    {
        public void Configure(EntityTypeBuilder<Сharacteristic> builder)
        {
            builder.ToTable("Сharacteristics");

            builder.HasKey(x => x.Id);
        }
    }
}
