using BRIX.GameService.Extensions;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddOptions(config);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithNewtonsoft();
builder.Services.AddCors();
builder.Services.AddDatabase(config);
builder.Services.AddAuth(config);
builder.Services.AddServices();
builder.Services.AddSwaggerWithJWT();

WebApplication app = builder.Build();

app.UseCors(config);
app.UseAuthentication();
app.UseAuthorization();
app.UseDevSwagger();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
