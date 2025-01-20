using Microsoft.AspNetCore.Components.Web; // Voor browsergebaseerde events.
using Microsoft.AspNetCore.Components.WebAssembly.Hosting; // Voor het hosten van de Blazor WebAssembly-app.
using Microsoft.AspNetCore.Components.Authorization; // Voor authenticatie en autorisatie.
using VivesRental.BlazorApp; // Basisnamespace van de applicatie.
using VivesRental.Sdk.Extensions; // Voor SDK-integratie.
using VivesRental.BlazorApp.Settings; // Voor API-configuratie.
using Vives.Presentation.Authentication; // Voor tokenbeheer.
using Blazored.LocalStorage; // Voor opslag in de lokale browser.
using VivesRental.BlazorApp.Stores; // Voor tokenopslag.
using VivesRental.BlazorApp.Security; // Voor authenticatiestatusbeheer.
using VivesRental.Sdk; // Voor SDK-functionaliteiten.
using VivesRental.BlazorApp.Services; // Voor service-klassen zoals CustomerService.

// **Startpunt van de applicatie**:
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// **Root componenten**:
// Bepaal de startcomponenten zoals de `App`-component en de `HeadOutlet`.
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// **Configuratie van API-instellingen**:
// Haal de basis-URL van de API op uit de configuratiebestanden (bijvoorbeeld appsettings.json).
var apiSettings = new ApiSettings();
builder.Configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);

// Controleer of de basis-URL correct is ingesteld.
if (string.IsNullOrWhiteSpace(apiSettings.BaseUrl))
{
    throw new InvalidOperationException("De API BaseUrl is niet geconfigureerd in appsettings.");
}

// **Registratie van de SDK**:
// Registreer de VIVES Rental SDK met de API BaseUrl.
builder.Services.AddApi(apiSettings.BaseUrl);

// **Toevoegen van lokale opslag en tokenbeheer**:
// Gebruik Blazored.LocalStorage voor veilige opslag van gegevens zoals tokens.
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IBearerTokenStore, TokenStore>();

// **JWT-authenticatie instellen**:
// Voeg autorisatieondersteuning toe aan de applicatie.
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();

// Voeg een cascading authenticatiestatus toe voor toegang in alle componenten.
builder.Services.AddCascadingAuthenticationState();

// **Registratie van services**:
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderLineService>();
builder.Services.AddScoped<ArticleReservationService>();
builder.Services.AddScoped<AuthSdk>(); // Voor toegang tot de `AuthController`.

// **HttpClient configureren**:
// Voeg de JWT-token automatisch toe aan alle API-verzoeken.
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri(apiSettings.BaseUrl) };
    var token = sp.GetRequiredService<IBearerTokenStore>().GetToken();

    Console.WriteLine($"API Base URL: {apiSettings.BaseUrl}");
    if (!string.IsNullOrWhiteSpace(token))
    {
        Console.WriteLine($"Token toegevoegd aan Authorization-header: {token}");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    return client;
});

// **Start de applicatie**:
await builder.Build().RunAsync();
