using MARO.Application.Aggregate.Models;
using MARO.Application.Interfaces.CommonMessage;

namespace MARO.Application.Interfaces
{
    public interface ISmsSender : ITextMessage
    {
        public SmsSenderOptions Options { get; }
    }
}
