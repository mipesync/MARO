namespace MARO.Application.Aggregate.Models
{
    public class RegisterDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
