using MARO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MARO.Persistence.EntityTypeConfigurations
{
    public class PlaceRatingConfiguration : IEntityTypeConfiguration<PlaceRating>
    {
        public void Configure(EntityTypeBuilder<PlaceRating> builder)
        {
            builder.ToTable("PlaceRatings");

            builder.HasKey(new string[] { "UserId", "PlaceId" });
        }
    }
}
