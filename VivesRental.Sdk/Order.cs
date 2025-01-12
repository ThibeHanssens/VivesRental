using System.Net.Http.Json;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk;

// **SDK voor orders**:
// Beheert alle HTTP-aanroepen naar de `Orders`-API.
public class OrderSdk
{
    private readonly IHttpClientFactory _httpClientFactory;

    // **Constructor**:
    // Ontvangt een `IHttpClientFactory` voor het maken van HTTP-clients.
    public OrderSdk(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // **Find**:
    // Haalt een lijst van orders op met optionele filters.
    public async Task<IList<OrderResult>> Find(OrderFilter? filter = null)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/orders";

        if (filter?.CustomerId != null)
        {
            route += $"?filter.CustomerId={filter.CustomerId}";
        }

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IList<OrderResult>>() ?? new List<OrderResult>();
    }

    // **Get**:
    // Haalt een specifiek order op via ID.
    public async Task<OrderResult?> Get(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/orders/{id}";

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OrderResult>();
    }

    // **Create**:
    // Maakt een nieuw order aan voor een specifieke klant.
    public async Task<OrderResult?> Create(Guid customerId)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/orders";

        var response = await httpClient.PostAsJsonAsync(route, customerId);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OrderResult>();
    }

    // **Return**:
    // Retourneert een order door alle bijbehorende artikelen te markeren als geretourneerd.
    public async Task<bool> Return(Guid orderId, DateTime returnedAt)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/orders/{orderId}/return";

        var response = await httpClient.PutAsJsonAsync(route, returnedAt);
        return response.IsSuccessStatusCode;
    }
}
