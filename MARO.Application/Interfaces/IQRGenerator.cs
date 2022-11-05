namespace MARO.Application.Interfaces
{
    public interface IQRGenerator
    {
        Task<string> GenerateAsync(string text, string webRootPath);
    }
}
