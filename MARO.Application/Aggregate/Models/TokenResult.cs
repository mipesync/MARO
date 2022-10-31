namespace MARO.Application.Aggregate.Models
{
    public class TokenResult
    {
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
    }
}
