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

            #region Данные пожеланий
            builder.HasData(
                new CriterionItem
                {
                    Id = 1,
                    Name = "free",
                    CriterionId = 1
                },
                new CriterionItem
                {
                    Id = 2,
                    Name = "fee",
                    CriterionId = 1
                },
                new CriterionItem
                {
                    Id = 3,
                    Name = "walking",
                    CriterionId = 1
                },
                new CriterionItem
                {
                    Id = 4,
                    Name = "electrobus",
                    CriterionId = 1
                },
                new CriterionItem
                {
                    Id = 5,
                    Name = "more_people",
                    CriterionId = 1
                },
                new CriterionItem
                {
                    Id = 6,
                    Name = "less_people",
                    CriterionId = 1
                },
                new CriterionItem
                {
                    Id = 7,
                    Name = "street",
                    CriterionId = 1
                },
                new CriterionItem
                {
                    Id = 8,
                    Name = "room",
                    CriterionId = 1
                },
                new CriterionItem
                {
                    Id = 9,
                    Name = "limited_health",
                    CriterionId = 1
                });
            #endregion

            #region Данные времяпрепровождения
            builder.HasData(
                new CriterionItem
                {
                    Id = 10,
                    Name = "museus_permanentExhibits",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 11,
                    Name = "museums_temporaryExhibits",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 12,
                    Name = "museums_excursions",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 13,
                    Name = "entertaining_festivals",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 14,
                    Name = "entertaining_concerts",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 15,
                    Name = "entertaining_attractions",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 16,
                    Name = "educational_masterClasses",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 17,
                    Name = "educational_lectures",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 18,
                    Name = "sports_masterClasses",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 19,
                    Name = "sports_races",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 20,
                    Name = "gastronomic_festivals",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 21,
                    Name = "business_expo",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 22,
                    Name = "eating_cafe",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 23,
                    Name = "eating_restaurant",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 24,
                    Name = "eating_streetfood",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 25,
                    Name = "walking_fountains",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 26,
                    Name = "walking_rocket",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 27,
                    Name = "walking_architecture",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 28,
                    Name = "walking_ponds",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 29,
                    Name = "walking_botanicalGarden",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 30,
                    Name = "walking_infoCenter",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 31,
                    Name = "walking_mothersRoom",
                    CriterionId = 2
                },
                new CriterionItem
                {
                    Id = 32,
                    Name = "walking_toilets",
                    CriterionId = 2
                });
            #endregion
        }
    }
}
