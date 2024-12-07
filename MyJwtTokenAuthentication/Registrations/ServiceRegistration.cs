using MyJwtTokenAuthentication.Models;
using Microsoft.Extensions.DependencyInjection;
using MyJwtTokenAuthentication.Interfaces;

namespace MyJwtTokenAuthentication.Registrations
{
    public static class ServiceRegistration
    {
        public static void AddMyLibraryServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
            services.AddSingleton<IJwtSecretKeyProvider, JwtSecretKeyProvider>();
        }
    }
}
