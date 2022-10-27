namespace MARO.Domain
{
    public class Criterion
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ParentId { get; set; }

        public Criterion? Parent { get; set; }
        public IEnumerable<Criterion>? Children { get; set; }
        public IEnumerable<User>? Users { get; set; }
        public IEnumerable<UserCriteria>? UserCriteria { get; set; }
    }
}
