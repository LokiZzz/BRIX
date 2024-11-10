using BRIX.GameService.Options;
using System.Runtime.CompilerServices;

namespace BRIX.GameService.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseCors(this WebApplication app, ConfigurationManager config)
        {
            ClientOptions client = new();
            config.GetSection(ClientOptions.Client).Bind(client);

            app.UseCors(config => config.WithOrigins(client.ClientAddress)
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
        }

        public static void UseDevSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
