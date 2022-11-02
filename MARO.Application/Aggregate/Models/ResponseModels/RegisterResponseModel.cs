namespace MARO.Application.Aggregate.Models.ResponseModels
{
    public class RegisterResponseModel
    {
        public string UserId { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
