using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MARO.Application.Aggregate.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [DisplayName("")]
        public string Arg
        {
            get
            {
                return Arg;
            }
            set
            {
                if (value.Contains('+'))
                {
                    Arg = value.Substring(0, value.IndexOf('+'));
                }

                return;
            }
        }
        [Required]
        public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
