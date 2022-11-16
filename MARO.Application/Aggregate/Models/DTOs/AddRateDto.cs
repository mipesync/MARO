using AutoMapper;
using MARO.Application.Common.Mappings;
using MARO.Domain;

namespace MARO.Application.Aggregate.Models.DTOs
{
    public class AddRateDto : IMapWith<PlaceRating>
    {
        public string UserId { get; set; } = null!;
        public string PlaceId { get; set; } = null!;

        private int rate = 0;
        public int Rate
        {
            get
            {
                return rate;
            }
            set
            {
                if (value > 0 && value <= 5)
                {
                    rate = value;
                }
                else rate = 0;
            }
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddRateDto, PlaceRating>()
                .ForMember(u => u.UserId, opt => opt.MapFrom(u => u.UserId))
                .ForMember(u => u.PlaceId, opt => opt.MapFrom(u => u.PlaceId))
                .ForMember(u => u.Rate, opt => opt.MapFrom(u => u.Rate));
        }
    }
}
