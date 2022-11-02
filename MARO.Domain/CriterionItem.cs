using System.Text.Json.Serialization;

namespace MARO.Domain
{
    public class CriterionItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CriterionId { get; set; }

        [JsonIgnore]
        public Criterion? Criterion { get; set; }
        public IEnumerable<UserItem>? UserItems { get; set; }
    }
}
