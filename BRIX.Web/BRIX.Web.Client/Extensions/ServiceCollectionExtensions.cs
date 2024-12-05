using BRIX.Library.Characters;
using BRIX.Web.Client.Options;
using BRIX.Web.Client.Services.Auth;
using BRIX.Web.Client.Services.Characters;
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
        }

        public static void AddOptions(this IServiceCollection services, WebAssemblyHostConfiguration config)
        {
            services.Configure<GameServiceOptions>(config.GetSection(GameServiceOptions.GameService));
        }

        public static void AddHttpClient(this IServiceCollection services, WebAssemblyHostConfiguration config)
        {
            services.AddScoped(x => new HttpClient());
        }
    }
}
