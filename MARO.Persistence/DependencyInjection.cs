using MARO.Application.Aggregate.Models;
using MARO.Application.Interfaces;
using MARO.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MARO.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection _services,
            EmailSenderOptions _emailSenderOptions, SmsSenderOptions _smsSenderOptions)
        {
            _services.AddScoped<IEmailSender>(u =>
            {
                return new EmailSender(_emailSenderOptions);
            });

            _services.AddScoped<ISmsSender>(u =>
            {
                return new SmsSender(_smsSenderOptions);
            });

            return _services;
        }
    }
}
