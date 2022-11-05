namespace MARO.Application.Aggregate.Models.DTOs
{
    /// <summary>
    /// Класс, содержащий информацию для совершения аутентификации
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Здесь могут быть как Email адрес, так и номер телефона
        /// </summary>
        public string Arg { get; set; } = null!;
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// Запомнить вход для последующих операций
        /// </summary>
        public bool RememberMe { get; set; }
        /// <summary>
        /// Адрес возврата пользователя после авторизации
        /// </summary>
        public string? ReturnUrl { get; set; }

    }
}
