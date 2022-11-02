using MARO.Domain;

namespace MARO.Application.Aggregate.Models.DTOs
{
    public class UserDetailsDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
    }
}
