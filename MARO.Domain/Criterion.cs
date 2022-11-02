using System.Text.Json.Serialization;

namespace MARO.Domain
{
    public class Criterion
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<CriterionItem>? Children { get; set; }
        [JsonIgnore]
        public IEnumerable<User>? Users { get; set; }
        [JsonIgnore]
        public IEnumerable<UserCriteria>? UserCriteria { get; set; }
    }
}
