using MARO.Application.Aggregate.Models;
using MARO.Application.Interfaces.CommonMessage;

namespace MARO.Application.Interfaces
{
    /// <summary>
    /// Отправляет сообщения на указанный Email
    /// </summary>
    public interface IEmailSender : ITextMessage
    {
        /// <summary>
        /// Тема отправляемого сообщения
        /// </summary>
        public string Subject { get; set; }
        public EmailSenderOptions Options { get; }
    }
}
