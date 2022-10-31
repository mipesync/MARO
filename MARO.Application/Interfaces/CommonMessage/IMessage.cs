namespace MARO.Application.Interfaces.CommonMessage
{
    public interface IMessage
    {
        /// <summary>
        /// Содержит отправителя сообщения
        /// </summary>
        public string From { get; set; }
        /// Содержит получателя сообщения
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Метод, совершающий отправление сообщения на указанный Email
        /// </summary>
        Task<string> SendAsync();
    }
}
