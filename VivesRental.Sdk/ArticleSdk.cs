using System.Net.Http.Json;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk;

// **SDK voor artikelen**:
// Beheert alle HTTP-aanroepen naar de `Articles`-API.
public class ArticlesSdk
{
    private readonly IHttpClientFactory _httpClientFactory;

    // **Constructor**:
    // Ontvangt een `IHttpClientFactory` voor het maken van HTTP-clients.
    public ArticlesSdk(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // **Find**:
    // Haalt een lijst van artikelen op met optionele filters.
    public async Task<IList<ArticleResult>> Find(ArticleFilter? filter = null)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/articles";

        if (filter != null)
        {
            var parameters = new List<string>();
            if (filter.ProductId.HasValue)
            {
                parameters.Add($"filter.ProductId={filter.ProductId}");
            }
            if (parameters.Any())
            {
                route += "?" + string.Join("&", parameters);
            }
        }

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IList<ArticleResult>>() ?? new List<ArticleResult>();
    }

    // **Get**:
    // Haalt een specifiek artikel op op basis van ID.
    public async Task<ArticleResult?> Get(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/articles/{id}";

        var response = await httpClient.GetAsync(route);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ArticleResult>();
    }

    // **Create**:
    // Maakt een nieuw artikel op basis van een `ArticleRequest`.
    public async Task<ArticleResult> Create(ArticleRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = "api/articles";

        var response = await httpClient.PostAsJsonAsync(route, request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ArticleResult>();
    }

    // **UpdateStatus**:
    // Wijzigt de status van een artikel.
    public async Task<bool> UpdateStatus(Guid articleId, string status)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/articles/{articleId}/status";

        var response = await httpClient.PutAsJsonAsync(route, new { status });
        return response.IsSuccessStatusCode;
    }

    // **Delete**:
    // Verwijdert een artikel op basis van ID.
    public async Task<bool> Delete(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
        var route = $"api/articles/{id}";

        var response = await httpClient.DeleteAsync(route);
        return response.IsSuccessStatusCode;
    }
}
