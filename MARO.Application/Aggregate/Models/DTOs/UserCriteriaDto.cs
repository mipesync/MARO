using MARO.Domain;

namespace MARO.Application.Aggregate.Models.DTOs
{
    public class UserCriteriaDto
    {
        public IEnumerable<Criterion>? Criteria { get; set; }
    }
}
