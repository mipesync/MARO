namespace MARO.Domain
{
    public class UserCriteria
    {
        public string UserId { get; set; } = null!;
        public int CriterionId { get; set; }

        public User User { get; set; } = null!;
        public Criterion Criterion { get; set; } = null!;
    }
}
