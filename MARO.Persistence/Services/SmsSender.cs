using MARO.Application.Aggregate.Models;
using MARO.Application.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace MARO.Persistence.Services
{
    public class SmsSender : ISmsSender
    {
        public SmsSenderOptions Options { get; }
        private readonly HttpClient httpClient;

        public string Message { get; set; } = null!;
        public string From { get; set; } = "SMS Aero";
        public string To { get; set; } = null!;
        private readonly string APIUrl = "https://gate.smsaero.ru/v2/sms/send/";

        public SmsSender(SmsSenderOptions options)
        {
            Options = options;
            this.httpClient = new HttpClient();
        }

        public async Task<string> SendAsync()
        {
            var data = DataBuilder();

            HttpRequestMessage? request = null;

            request = new HttpRequestMessage(HttpMethod.Get, APIUrl + "?" + data);

            if (request == null)
                throw new Exception($"{nameof(HttpMethod.Get)} value exception");

            // асинхронно отправляем запрос
            var response = await httpClient.SendAsync(request);

            // проверяем статус код ответа, если не 200 бросаем исключение
            response.EnsureSuccessStatusCode();

            // асинхронно читаем тело ответа сервера
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        private string DataBuilder()
        {
            var from = HttpUtility.UrlEncode(From);

            var basicAuthData =
                Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes(Options.Email + ":" + Options.APIKey));

            // Авторизация по умолчанию для HttpClient
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", basicAuthData);

            var data = $"sign={from}&text={HttpUtility.UrlEncode(Message)}&number={To}";

            return data;
        }
    }
}
