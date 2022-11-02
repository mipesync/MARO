namespace MARO.Application.Aggregate.Models.ResponseModels
{
    public class RefreshTokenResponseModel
    {
        public string AccessToken { get; set; } = null!;
        public DateTime Expires { get; set; }
        public string? RefreshToken { get; set; }
    }
}
