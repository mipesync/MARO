using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MARO.API
{
    public class JwtOptions
    {
        public static string ISSUER = "https://localhost:7269";
        public static string AUDIENCE = "MAROAPI";
        const string KEY = "375ff8a55aca72cf3a11e318d1592d2f0d3995ae";
        public DateTime EXPIRES = DateTime.UtcNow.Add(TimeSpan.FromMinutes(30));
        public DateTime RefreshTokenExpires = DateTime.UtcNow.Add(TimeSpan.FromDays(14));
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
