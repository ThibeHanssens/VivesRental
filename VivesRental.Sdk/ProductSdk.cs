using System.Net.Http.Json;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk;

// **SDK voor producten**:
// Beheert alle HTTP-aanroepen naar de `Products`-API.
public class ProductSdk
{
    private readonly IHttpClientFactory _httpClientFactory;

    // **Constructor**:
    // Ontvangt een `IHttpClientFactory` voor het maken van HTTP-clients.
    public ProductSdk(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // **Find**:
    // Haalt een lijst van producten op met optionele filters.
    public async Task<IList<ProductResult>> Find(ProductFilter? filter = null)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/products";

        if (filter?.AvailableFromDateTime != null)
        {
            route += $"?filter.AvailableFromDateTime={filter.AvailableFromDateTime}";
        }

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IList<ProductResult>>() ?? new List<ProductResult>();
    }

    // **Get**:
    // Haalt een specifiek product op via ID.
    public async Task<ProductResult?> Get(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/products/{id}";

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ProductResult>();
    }

    // **Create**:
    // Maakt een nieuw product aan op basis van een `ProductRequest`.
    public async Task<ProductResult?> Create(ProductRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/products";

        var response = await httpClient.PostAsJsonAsync(route, request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ProductResult>();
    }

    // **Edit**:
    // Wijzigt een bestaand product op basis van een `ProductRequest`.
    public async Task<ProductResult?> Edit(Guid id, ProductRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/products/{id}";

        var response = await httpClient.PutAsJsonAsync(route, request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ProductResult>();
    }

    // **Remove**:
    // Verwijdert een product via ID.
    public async Task<bool> Remove(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/products/{id}";

        var response = await httpClient.DeleteAsync(route);
        return response.IsSuccessStatusCode;
    }
}
