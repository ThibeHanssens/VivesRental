using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization; // Voor autorisatie via AuthenticationStateProvider
using VivesRental.BlazorApp; // Hoofdapplicatie namespace
using VivesRental.Sdk.Extensions; // Voor SDK-integratie
using VivesRental.BlazorApp.Settings; // Voor API-instellingen
using Vives.Presentation.Authentication; // Voor authenticatiebeheer (TokenStore)
using Blazored.LocalStorage;
using VivesRental.BlazorApp.Stores;
using VivesRental.BlazorApp.Security; // Voor tokenopslag en andere opslag

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// **Root componenten**:
// Voeg de hoofdapplicatie (`App.razor`) en de head-outlet toe aan het project.
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// **API-instellingen configureren**:
// Laad de basis-URL van de API uit de configuratie en registreer de API in de DI-container.
var apiSettings = new ApiSettings();
builder.Configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);
builder.Services.AddApi(apiSettings.BaseUrl); // Verbindt de SDK met de opgegeven API-url.

// **Token-opslag en Blazored.LocalStorage**:
// Voeg ondersteuning toe voor lokale opslag, nodig voor tokenbeheer.
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IBearerTokenStore, TokenStore>(); // Voor veilige opslag van JWT-tokens.

// **JWT-authenticatie configureren**:
// Activeer autorisatie en configureer cascading authentication state.
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState(); // Voor toegang tot authenticatiestatus in child-componenten.

// **Bouw en start de applicatie**:
// Bouwt de applicatie en start deze op in de browser.
await builder.Build().RunAsync();
