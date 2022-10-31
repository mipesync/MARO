namespace MARO.Application.Aggregate.Models
{
    /// <summary>
    /// Получает параметры подключения к SMTP серверу и устанавливает их в <see cref="IEmailSender"/>
    /// </summary>
    public class EmailSenderOptions
    {
        /// <summary>
        /// Имя хоста от SMTP сервера
        /// </summary>
        public string Host { get; set; } = null!;
        /// <summary>
        /// Порт подключения к SMTP серверу
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Имя пользователя от аккаунта SMTP сервера
        /// </summary>
        public string Username { get; set; } = null!;
        /// <summary>
        /// Пароль от аккаунта SMTP сервера
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// Имя отправителя, отображаемое в письме
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Получает параметры подключения к SMTP серверу и устанавливает их в <see cref="IEmailSender"/>
        /// </summary>
        public EmailSenderOptions() { }
    }
}
