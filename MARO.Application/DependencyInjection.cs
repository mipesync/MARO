using MARO.Application.Common.Mappings;
using MARO.Application.Common.Services;
using MARO.Application.Interfaces;
using MARO.Application.Repository.AuthRepo;
using MARO.Application.Repository.GroupRepo;
using MARO.Application.Repository.RateRepo;
using MARO.Application.Repository.RouteRepo;
using MARO.Application.Repository.UserRepo;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MARO.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection _services, string QR_Access_Token)
        {
            _services.AddAutoMapper(u => u.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())));
            _services.AddTransient<IAuthRepository, AuthRepository>();
            _services.AddTransient<IUserRepository, UserRepository>();
            _services.AddTransient<IRouteRepository, RouteRepository>();
            _services.AddTransient<IQRGenerator>(u =>
            {
                return new QRGenerator(QR_Access_Token);
            });
            _services.AddTransient<IGroupRepository, GroupRepository>();
            _services.AddTransient<IRateRepository, RateRepository>();

            return _services;
        }
    }
}
