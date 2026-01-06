using BRIX.GameService.Entities;
using BRIX.GameService.Entities.Users;
using BRIX.GameService.Options;
using BRIX.GameService.Services.Account;
using BRIX.GameService.Services.Characters;
using BRIX.GameService.Services.Mail;
using BRIX.GameService.Services.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace BRIX.GameService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IMailService, MailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();

            services.AddSingleton(services => new JsonSerializerSettings { 
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateParseHandling = DateParseHandling.None
            });
        }

        public static void AddOptions(this IServiceCollection services, ConfigurationManager config)
        {
            services.Configure<JWTOptions>(config.GetSection(JWTOptions.JWT));
            services.Configure<SMTPOptions>(config.GetSection(SMTPOptions.SMTP));
            services.Configure<ClientOptions>(config.GetSection(ClientOptions.Client));
        }

        public static void AddControllersWithNewtonsoft(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => 
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto
            );
        }

        public static void AddLogging(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddLogging();
        }

        public static void AddDatabase(this IServiceCollection services, ConfigurationManager config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        public static void AddAuth(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            JWTOptions jwt = new();
            config.GetSection(JWTOptions.JWT).Bind(jwt);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecurityKey
                        ?? throw new Exception("Не указан секретный ключ"))
                    )
                };
            }).AddIdentityCookies();
        }

        public static void AddExceptionHandling(this IServiceCollection services)
        {
            services.AddProblemDetails(options => 
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance =
                        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                    Activity? activity = context.HttpContext.Features
                        .GetRequiredFeature<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
                };
            });

            services.AddExceptionHandler<ProblemExceptionHandler>();
        }
    }
}
