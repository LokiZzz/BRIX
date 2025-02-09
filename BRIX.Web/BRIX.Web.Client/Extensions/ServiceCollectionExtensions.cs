using BRIX.Web.Client.Options;
using BRIX.Web.Client.Services.Auth;
using BRIX.Web.Client.Services.Characters;
using BRIX.Web.Client.Services.Http;
using BRIX.Web.Client.Services.UI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BRIX.Web.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuth(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>();
            services.AddScoped<IAuthService, AuthService>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<CharacterManager>();
            services.AddSingleton<ModalService>();
        }

        public static void AddOptions(this IServiceCollection services, WebAssemblyHostConfiguration config)
        {
            services.Configure<GameServiceOptions>(config.GetSection(GameServiceOptions.GameService));
        }

        public static void AddHttpClient(this IServiceCollection services, WebAssemblyHostConfiguration config)
        {
            services.AddScoped<HttpClientBuilder>();
        }
    }
}
