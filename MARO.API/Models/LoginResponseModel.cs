namespace MARO.API.Models
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; } = null!;
        public DateTime Expires { get; set; }
        public string? RefreshToken { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
