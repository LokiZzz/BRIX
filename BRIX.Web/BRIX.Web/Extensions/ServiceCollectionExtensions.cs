using BRIX.Web.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BRIX.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuth(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>();
            services.AddScoped<IAuthService, AuthService>();
        }

        public static void AddServices(this IServiceCollection services)
        {

        }
    }
}
