using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization; // Voor autorisatie via AuthenticationStateProvider
using VivesRental.BlazorApp; // Hoofdapplicatie namespace
using VivesRental.BlazorApp.Services; // Zorgt voor services zoals artikelen, klanten en orders.
using Vives.Presentation.Authentication; // Voor TokenStore en TokenAuthenticationStateProvider
using Blazored.LocalStorage; // Voor tokenopslag en andere opslag

var builder = WebAssemblyHostBuilder.CreateDefault(args);
// **Hoofdingang van de applicatie**:
// Configureert een Blazor WebAssembly-applicatie.

builder.RootComponents.Add<App>("#app");
// Hoofdcomponent van de applicatie koppelen aan het element met id 'app'.

builder.RootComponents.Add<HeadOutlet>("head::after");
// Dynamische wijzigingen in de `<head>`-sectie, zoals metadata of CSS, ondersteunen.

// **Configureer API-instellingen**
var apiSettings = new ApiSettings();
builder.Configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);
// Bind de `ApiSettings` sectie uit `appsettings.json` aan de klasse.

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiSettings.BaseUrl) // Basis-URL voor alle API-aanroepen.
});
// Registreer `HttpClient` voor communicatie met de backend API.

// **Blazored.LocalStorage**
builder.Services.AddBlazoredLocalStorage();
// Ondersteunt lokale opslag voor tokens en andere gegevens.

// **JWT-authenticatie en autorisatie configureren**
builder.Services.AddAuthorizationCore(); // Voeg autorisatie-ondersteuning toe.
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
// Beheer authenticatiestatus op basis van JWT-tokens.
builder.Services.AddScoped<IBearerTokenStore, TokenStore>();
// Sla JWT-tokens veilig op met `TokenStore`.

// **Applicatiespecifieke services registreren**
builder.Services.AddScoped<ArticleService>(); // Service voor artikelbeheer.
builder.Services.AddScoped<CustomerService>(); // Service voor klantenbeheer.
builder.Services.AddScoped<OrderService>(); // Service voor orderbeheer.

// **Bouw en start de applicatie**
await builder.Build().RunAsync();
// Bouwt de Blazor WebAssembly-applicatie en start deze.
