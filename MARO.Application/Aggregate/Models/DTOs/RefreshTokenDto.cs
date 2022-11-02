using System.ComponentModel.DataAnnotations;

namespace MARO.Application.Aggregate.Models.DTOs
{
    public class RefreshTokenDto
    {
        [Required]
        public string AccessToken { get; set; } = null!;

        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
