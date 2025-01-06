namespace VivesRental.BlazorApp.Services;

using System.Net.Http.Json;
using VivesRental.Services.Model.Results;

public class ArticleService
{
    private readonly HttpClient _httpClient;
    // De HttpClient wordt gebruikt om API-aanroepen te doen.

    public ArticleService(HttpClient httpClient)
    {
        _httpClient = httpClient; // Dependency Injection van HttpClient.
    }

    public async Task<List<ArticleResult>> GetAllAsync()
    {
        // Methode om alle artikelen op te halen via een GET-aanroep naar de API.
        return await _httpClient.GetFromJsonAsync<List<ArticleResult>>("articles");
        // De API-call haalt een JSON-array op en deserialiseert deze naar een lijst van ArticleResult-objecten.
    }
}
