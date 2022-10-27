namespace MARO.API.Models
{
    public class RegisterResponseModel
    {
        public string UserId { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
