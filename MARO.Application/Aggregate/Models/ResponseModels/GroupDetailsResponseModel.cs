using AutoMapper;
using MARO.Application.Common.Mappings;
using MARO.Domain;

namespace MARO.Application.Aggregate.Models.ResponseModels
{
    public class GroupDetailsResponseModel : IMapWith<Group>
    {
        public string Id { get; set; } = null!;
        public string QRLink { get; set; } = null!;
        public IEnumerable<string> Members { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupDetailsResponseModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(group => group.Id))
                .ForMember(model => model.QRLink, opt => opt.MapFrom(group => group.QRLink))
                .ForMember(model => model.Members, opt => opt.MapFrom(group => group.Users.Select(u => u.Id)));
        }
    }
}
