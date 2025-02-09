using BRIX.Web.Client.Services.Auth;
using BRIX.Web.Client.Services.Http;
using BRIX.Web.Client.Services.UI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BRIX.Web.Host.Extensions
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
            services.AddSingleton<ModalService>();
        }

        public static void AddHttpClientBuilder(this IServiceCollection services)
        {
            services.AddScoped<HttpClientBuilder>();
        }
    }
}
