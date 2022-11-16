using AutoMapper;
using MARO.Application.Common.Mappings;
using MARO.Domain;

namespace MARO.Application.Aggregate.Models.ResponseModels
{
    public class UserDetailsResponseModel : IMapWith<User>
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int Age { get; set; }
        public string Contact { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? GroupId { get; set; }
        public string? GroupRole { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDetailsResponseModel>()
                .ForMember(detail => detail.LastName, opt => opt.MapFrom(user => user.LastName))
                .ForMember(detail => detail.FirstName, opt => opt.MapFrom(user => user.FirstName))
                .ForMember(detail => detail.LastName, opt => opt.MapFrom(user => user.LastName))
                .ForMember(detail => detail.FullName, opt => opt.MapFrom(user => user.FullName))
                .ForMember(detail => detail.Age, opt => opt.MapFrom(user => user.Age))
                .ForMember(detail => detail.Contact, opt => opt.MapFrom(user => user.UserName))
                .ForMember(detail => detail.GroupId, opt => opt.MapFrom(user => user.GroupId));
        }
    }
}
