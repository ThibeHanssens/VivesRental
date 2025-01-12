using System.Net.Http.Json;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk;

// **SDK voor orderlijnen**:
// Beheert alle HTTP-aanroepen naar de `OrderLines`-API.
public class OrderLineSdk
{
    private readonly IHttpClientFactory _httpClientFactory;

    // **Constructor**:
    // Ontvangt een `IHttpClientFactory` voor het maken van HTTP-clients.
    public OrderLineSdk(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // **Find**:
    // Haalt een lijst van orderlijnen op met optionele filters.
    public async Task<IList<OrderLineResult>> Find(OrderLineFilter? filter = null)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/orderlines";

        if (filter?.OrderId != null)
        {
            route += $"?filter.OrderId={filter.OrderId}";
        }

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IList<OrderLineResult>>() ?? new List<OrderLineResult>();
    }

    // **Get**:
    // Haalt een specifieke orderlijn op via ID.
    public async Task<OrderLineResult?> Get(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/orderlines/{id}";

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OrderLineResult>();
    }

    // **Rent**:
    // Verhuurt een artikel via een orderlijn.
    public async Task<bool> Rent(Guid orderId, Guid articleId)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/orderlines/rent?orderId={orderId}&articleId={articleId}";

        var response = await httpClient.PostAsync(route, null);
        return response.IsSuccessStatusCode;
    }

    // **Return**:
    // Retourneert een artikel via een orderlijn.
    public async Task<bool> Return(Guid orderLineId, DateTime returnedAt)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/orderlines/{orderLineId}/return";

        var response = await httpClient.PutAsJsonAsync(route, returnedAt);
        return response.IsSuccessStatusCode;
    }
}
