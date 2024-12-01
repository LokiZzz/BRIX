using BRIX.GameService.Extensions;
using BRIX.GameService.Services.Utility;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

ConfigurationManager config = builder.Configuration;

builder.Services.AddOptions(config);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithNewtonsoft();
builder.Services.AddCors();
builder.Services.AddDatabase(config);
builder.Services.AddAuth(config);
builder.Services.AddServices();
builder.Services.AddSwaggerWithJWT();
builder.Services.AddLogging(config);
builder.Services.AddExceptionHandling();

WebApplication app = builder.Build();

app.UseCors(config);
app.UseAuthentication();
app.UseAuthorization();
app.UseDevSwagger();
app.UseHttpsRedirection();
app.MapControllers();
app.UseExceptionHandler(); // _ => { }

app.Run();
