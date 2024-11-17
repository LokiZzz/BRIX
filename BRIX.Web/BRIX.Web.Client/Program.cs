using Blazored.LocalStorage;
using BRIX.Web.Client.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
WebAssemblyHostConfiguration config = builder.Configuration;

builder.Services.AddOptions(config);
builder.Services.AddHttpClient(config);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuth();
builder.Services.AddServices();

await builder.Build().RunAsync();