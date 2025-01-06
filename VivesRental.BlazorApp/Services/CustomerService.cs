namespace VivesRental.BlazorApp.Services;

using System.Net.Http.Json;
using VivesRental.Services.Model.Results;

public class CustomerService
{
    private readonly HttpClient _httpClient;
    // De HttpClient wordt gebruikt om API-aanroepen te doen.

    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient; // Dependency Injection van HttpClient.
    }

    public async Task<List<CustomerResult>> GetAllAsync()
    {
        // Methode om alle klanten op te halen via een GET-aanroep naar de API.
        return await _httpClient.GetFromJsonAsync<List<CustomerResult>>("customers");
        // De API-call haalt een JSON-array op en deserialiseert deze naar een lijst van CustomerResult-objecten.
    }
}
