using BRIX.GameService.Entities;
using BRIX.GameService.Entities.Users;
using BRIX.GameService.Options;
using BRIX.GameService.Services.Account;
using BRIX.GameService.Services.Characters;
using BRIX.GameService.Services.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
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

        public static void AddSwaggerWithJWT(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTToken_Auth_API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() 
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. " +
                        "\r\nEnter 'Bearer' [space] and then your token in the text input below." +
                        "\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {{
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }});
            });
        }
    }
}
