using System.ComponentModel.DataAnnotations;

namespace MARO.Application.Aggregate.Models.DTOs
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Arg { get; set; } = null!;
        [Required]
        public string Code { get; set; } = null!;
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
