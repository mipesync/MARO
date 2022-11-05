using MARO.Application.Common.Mappings;
using MARO.Application.Common.Services;
using MARO.Application.Interfaces;
using MARO.Application.Repository.AuthRepo;
using MARO.Application.Repository.GroupRepo;
using MARO.Application.Repository.RouteRepo;
using MARO.Application.Repository.UserRepo;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MARO.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection _services)
        {
            _services.AddAutoMapper(u => u.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())));
            _services.AddTransient<IAuthRepository, AuthRepository>();
            _services.AddTransient<IUserRepository, UserRepository>();
            _services.AddTransient<IRouteRepository, RouteRepository>();
            _services.AddTransient<IQRGenerator, QRGenerator>();
            _services.AddTransient<IGroupRepository, GroupRepository>();

            return _services;
        }
    }
}
