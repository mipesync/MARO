namespace MARO.Application.Aggregate.Models.DTOs
{
    public class ConfirmDto
    {
        public Guid UserId { get; set; }
        public string Code { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
