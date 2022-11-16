using MARO.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace MARO.Application.Common.Services
{
    public class QRGenerator : IQRGenerator
    {
        private readonly string _token;

        public QRGenerator(string access_token)
        {
            _token = access_token;
        }

        public async Task<string> GenerateAsync(string text, string webRootPath)
        {
            using (var httpClient = new HttpClient()) 
            {
                var url = $"https://api.qr-code-generator.com/v1/create?access-token={_token}";

                HttpContent content = new StringContent(JsonSerializer.Serialize(new
                {
                    frame_name = "no-frame",
                    qr_code_text = text,
                    image_format = "SVG",
                    qr_code_logo = "scan-me-square"
                }), Encoding.UTF8, "application/json");
                
                var response = await httpClient.PostAsync(url, content);

                var path = $"/qr-codes/{Guid.NewGuid()}.svg";

                using (var fileStream = new FileStream(webRootPath + path, FileMode.Create))
                {
                    await response.Content.CopyToAsync(fileStream);
                }

                return path;
            }
        }
    }
}
