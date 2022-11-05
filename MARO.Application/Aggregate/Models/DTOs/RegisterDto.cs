using System.ComponentModel.DataAnnotations;

namespace MARO.Application.Aggregate.Models.DTOs
{
    /// <summary>
    /// Класс, содержащий информацию для совершения регистрации
    /// </summary>
    public class RegisterDto
    {
        private string arg = string.Empty;
        /// <summary>
        /// Здесь могут быть как Email адрес, так и номер телефона
        /// </summary>
        [Required]
        public string Arg
        {
            get
            {
                return arg;
            }
            set
            {
                if (value.Contains('+'))
                {
                    arg = value.Substring(1);
                }
                else arg = value;
            }
        }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;
        /// <summary>
        /// Хост клиента
        /// </summary>
        public string Host { get; set; } = null!;
        /// <summary>
        /// Адрес возврата пользователя после авторизации
        /// </summary>
        public string? ReturnUrl { get; set; }
    }
}
