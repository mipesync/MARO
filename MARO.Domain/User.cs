using Microsoft.AspNetCore.Identity;

namespace MARO.Domain
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? PhoneConfirmationCode { get; set; }

        public IEnumerable<Criterion>? Criteria { get; set; }
        public IEnumerable<UserCriteria>? UserCriteria { get; set; }
        public IEnumerable<UserItem>? UserItems { get; set; }
        public IEnumerable<CriterionItem>? CriterionItems { get; set; }
    }
}
