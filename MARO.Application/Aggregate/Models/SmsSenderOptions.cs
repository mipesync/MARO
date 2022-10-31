using MARO.Application.Interfaces;

namespace MARO.Application.Aggregate.Models
{
    /// <summary>
    /// Получает параметры подключения к SMTP серверу и устанавливает их в <see cref="ISmsSender"/>
    /// </summary>
    public class SmsSenderOptions
    {
        /// <summary>
        /// Email от аккаунта SMTP сервера
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// API ключ от аккаунта SMTP сервера
        /// </summary>
        public string APIKey { get; set; } = null!;


        /// <summary>
        /// Получает параметры подключения к SMTP серверу и устанавливает их в <see cref="ISmsSender"/>
        /// </summary>
        public SmsSenderOptions() { }
    }
}
