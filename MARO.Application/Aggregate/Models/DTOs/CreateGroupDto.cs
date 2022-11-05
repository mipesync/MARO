namespace MARO.Application.Aggregate.Models.DTOs
{
    public class CreateGroupDto
    {
        public Guid UserId { get; set; }
        public string Host { get; set; } = null!;
    }
}
