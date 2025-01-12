using System.Net.Http.Json;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk;

// **SDK voor artikelreserveringen**:
// Beheert alle HTTP-aanroepen naar de `ArticleReservations`-API.
public class ArticleReservationSdk
{
    private readonly IHttpClientFactory _httpClientFactory;

    // **Constructor**:
    // Ontvangt een `IHttpClientFactory` voor het maken van HTTP-clients.
    public ArticleReservationSdk(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // **Find**:
    // Haalt een lijst van artikelreserveringen op met optionele filters.
    public async Task<IList<ArticleReservationResult>> Find(ArticleReservationFilter? filter = null)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/articlereservations";

        if (filter?.CustomerId != null)
        {
            route += $"?filter.CustomerId={filter.CustomerId}";
        }

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IList<ArticleReservationResult>>() ?? new List<ArticleReservationResult>();
    }

    // **Get**:
    // Haalt een specifieke artikelreservering op via ID.
    public async Task<ArticleReservationResult?> Get(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/articlereservations/{id}";

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ArticleReservationResult>();
    }

    // **Create**:
    // Maakt een nieuwe artikelreservering aan.
    public async Task<ArticleReservationResult?> Create(ArticleReservationRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/articlereservations";

        var response = await httpClient.PostAsJsonAsync(route, request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ArticleReservationResult>();
    }

    // **Remove**:
    // Verwijdert een artikelreservering via ID.
    public async Task<bool> Remove(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/articlereservations/{id}";

        var response = await httpClient.DeleteAsync(route);
        return response.IsSuccessStatusCode;
    }
}
