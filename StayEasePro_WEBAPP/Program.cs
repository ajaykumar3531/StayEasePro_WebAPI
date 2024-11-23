using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StayEasePro_WEBAPP;
using StayEasePro_WEBAPP.Data.DTO_s;
using StayEasePro_WEBAPP.Services.Contracts;
using StayEasePro_WEBAPP.Services.Implementations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredToast();

builder.Configuration.AddJsonFile("/appsettings.json", optional: false, reloadOnChange: true);

var apiUrls = builder.Configuration.GetSection("ApiUrls").Get<ApiUrlsDto>();
if (apiUrls == null)
{
    throw new InvalidOperationException("ApiUrls section is missing or malformed in appsettings.json.");
}

builder.Services.AddSingleton(apiUrls);


// Register services
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrls.Testing) });

builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IUserService, UserService>();

await builder.Build().RunAsync();
