using MARO.Domain;

namespace MARO.Application.Aggregate.Models
{
    public class UserDetailsDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public IEnumerable<Criterion>? Criteria { get; set; }
    }
}
