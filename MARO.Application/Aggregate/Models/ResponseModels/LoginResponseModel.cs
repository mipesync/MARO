namespace MARO.Application.Aggregate.Models.ResponseModels
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; } = null!;
        public DateTime Expires { get; set; }
        public string? RefreshToken { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
