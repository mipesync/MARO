namespace MARO.Application.Models
{
    public class TokenResult
    {
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
    }
}
