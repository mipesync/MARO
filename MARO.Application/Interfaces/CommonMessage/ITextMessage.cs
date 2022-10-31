namespace MARO.Application.Interfaces.CommonMessage
{
    public interface ITextMessage : IMessage
    {
        /// <summary>
        /// Текст отправляемого сообщения
        /// </summary>
        public string Message { get; set; }
    }
}
