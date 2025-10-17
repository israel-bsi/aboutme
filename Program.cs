using AboutMe;
using AboutMe.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Globalization;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddSingleton<ThemeService>();

var host = builder.Build();

var js = host.Services.GetRequiredService<IJSRuntime>();
var cultureString = await js.InvokeAsync<string>("localStorage.getItem", "culture");

CultureInfo culture;

if (!string.IsNullOrEmpty(cultureString))
{
    culture = new CultureInfo(cultureString);
}
else
{
    var browserLanguage = await js.InvokeAsync<string>("eval", "navigator.language || navigator.userLanguage");
    
    if (!string.IsNullOrEmpty(browserLanguage) && browserLanguage.StartsWith("en", StringComparison.OrdinalIgnoreCase))
    {
        culture = new CultureInfo("en-US");
    }
    else
    {
        culture = new CultureInfo("pt-BR");
    }
    
    await js.InvokeVoidAsync("localStorage.setItem", "culture", culture.Name);
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
