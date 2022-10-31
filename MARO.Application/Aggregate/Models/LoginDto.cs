namespace MARO.Application.Aggregate.Models
{
    public class LoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
