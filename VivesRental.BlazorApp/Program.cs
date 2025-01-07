using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VivesRental.BlazorApp;
using VivesRental.BlazorApp.Services; // Zorgt ervoor dat de services herkend worden.


var builder = WebAssemblyHostBuilder.CreateDefault(args);
// Start de configuratie van een Blazor WebAssembly-applicatie.

builder.RootComponents.Add<App>("#app");
// Hier wordt de hoofcomponent 'App' aan de HTML DOM gekoppeld op het element met id 'app'.

builder.RootComponents.Add<HeadOutlet>("head::after");
// Hier worden eventuele dynamische wijzigingen in de <head>-sectie toegevoegd, zoals metadata en styles.

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
// Registreer de `HttpClient` service die wordt gebruikt voor API-aanroepen.
// De BaseAddress wordt ingesteld op de huidige URL van de applicatie (HostEnvironment).

builder.Services.AddScoped<ArticleService>(); // Voeg de ArticleService toe aan dependency injection.
builder.Services.AddScoped<CustomerService>(); // Voeg de CustomerService toe aan dependency injection.
builder.Services.AddScoped<OrderService>(); // Voeg de OrderService toe aan dependency injection.

await builder.Build().RunAsync();
// Bouw de applicatie en start deze asynchroon.
