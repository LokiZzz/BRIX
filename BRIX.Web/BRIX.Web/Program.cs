using Blazored.LocalStorage;
using BRIX.Web.Host.Components;
using BRIX.Web.Host.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuth();
builder.Services.AddServices();
builder.Services.AddHttpClientBuilder();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BRIX.Web.Client.Components._Imports).Assembly);

app.Run();
