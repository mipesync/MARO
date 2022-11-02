using MARO.Application.Common.Mappings;
using MARO.Application.Repository;
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

            return _services;
        }
    }
}
