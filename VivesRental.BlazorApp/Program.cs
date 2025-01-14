using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization; // Voor autorisatie via AuthenticationStateProvider
using VivesRental.BlazorApp;
using VivesRental.Sdk.Extensions;
using VivesRental.BlazorApp.Settings; // Voor configuratie van de API-instellingen
using Vives.Presentation.Authentication; // Voor authenticatiebeheer zoals het opslaan van tokens
using Blazored.LocalStorage; // Voor opslag van JWT-tokens in de lokale browseropslag
using VivesRental.BlazorApp.Stores; // Voor opslag zoals tokenbeheer
using VivesRental.BlazorApp.Security;
using VivesRental.Sdk;
using VivesRental.BlazorApp.Services; // Voor authenticatie en autorisatie in de applicatie

// **Startpunt van de applicatie**:
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// **Root componenten**:
// App.razor als de hoofdingang van de applicatie.
// `HeadOutlet`-component toe voor dynamische `<head>` inhoud zoals pagina-titels.
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// **Configuratie van API-instellingen**:
// Laad de basis-URL van de API uit de configuratiebestanden (appsettings.json of appsettings.Development.json).
var apiSettings = new ApiSettings();
builder.Configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);

// Controleer of de API BaseUrl correct is ingesteld, anders gooi een foutmelding.
// Dit zorgt ervoor dat de applicatie niet onverwachts faalt tijdens runtime.
if (string.IsNullOrWhiteSpace(apiSettings.BaseUrl))
{
    throw new InvalidOperationException("De API BaseUrl is niet geconfigureerd in appsettings.");
}

// **Registratie van de SDK**:
// Verbindt de VIVES Rental SDK met de opgegeven API BaseUrl. 
builder.Services.AddApi(apiSettings.BaseUrl);

// **Toevoegen van lokale opslag en tokenbeheer**:
// Blazored.LocalStorage wordt gebruikt om gegevens op te slaan in de browser (bijvoorbeeld JWT-tokens).
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IBearerTokenStore, TokenStore>(); // Token-opslag voor veilige authenticatie

// **JWT-authenticatie instellen**:
// Voeg ondersteuning toe voor autorisatie in Blazor met `AuthorizationCore`.
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();

// Voeg een cascading authenticatiestatus toe. Dit zorgt ervoor dat de authenticatiestatus beschikbaar is
// voor alle child-componenten in de applicatie.
builder.Services.AddCascadingAuthenticationState();

// **Registratie van BlazorApp-services**:
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderLineService>();
builder.Services.AddScoped<ArticleReservationService>();

// **Registratie van AuthSdk**:
builder.Services.AddScoped<AuthSdk>(); // Voor communicatie met de `AuthController`

// **Registratie van SDK-services**:
builder.Services.AddScoped<CustomerSdk>();
builder.Services.AddScoped<ProductSdk>();
builder.Services.AddScoped<ArticlesSdk>();
builder.Services.AddScoped<OrderSdk>();
builder.Services.AddScoped<OrderLineSdk>();
builder.Services.AddScoped<ArticleReservationSdk>();

// **HttpClient met ondersteuning voor tokens**:
// Stel de HttpClient in zodat alle verzoeken automatisch een JWT-token bevatten als de gebruiker is ingelogd.
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri(apiSettings.BaseUrl) };
    var token = sp.GetRequiredService<IBearerTokenStore>().GetToken();
    if (!string.IsNullOrWhiteSpace(token))
    {
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
    return client;
});

// **Applicatie bouwen en starten**:
await builder.Build().RunAsync();
