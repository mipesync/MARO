using System.Text.Json.Serialization;

namespace MARO.Domain
{
    public class UserItem
    {
        public string UserId { get; set; } = null!;
        public int CriterionItemId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;
        [JsonIgnore]
        public CriterionItem CriterionItem { get; set; } = null!;
    }
}
