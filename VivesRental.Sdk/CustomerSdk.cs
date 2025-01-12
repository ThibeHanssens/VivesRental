using System.Net.Http.Json;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk;

// **SDK voor klanten**:
// Beheert alle HTTP-aanroepen naar de `Customers`-API.
public class CustomerSdk
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerSdk(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IList<CustomerResult>> Find(CustomerFilter? filter = null)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/customers";

        if (!string.IsNullOrEmpty(filter?.Search))
            route += $"?filter.Search={filter.Search}";

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IList<CustomerResult>>() ?? new List<CustomerResult>();
    }

    public async Task<CustomerResult?> Get(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/customers/{id}";

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CustomerResult>();
    }

    public async Task<CustomerResult?> Create(CustomerRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/customers";

        var response = await httpClient.PostAsJsonAsync(route, request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CustomerResult>();
    }

    public async Task<bool> Remove(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/customers/{id}";

        var response = await httpClient.DeleteAsync(route);
        return response.IsSuccessStatusCode;
    }
}
