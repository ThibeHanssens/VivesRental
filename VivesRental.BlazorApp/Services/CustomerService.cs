using System.Net.Http.Json; // Nodig voor het gebruik van GetFromJsonAsync.
using VivesRental.Services.Model.Results; // Importeert het model voor de resultaten.

namespace VivesRental.BlazorApp.Services
{
    // Deze service beheert alle API-aanroepen die betrekking hebben op Customers.
    public class CustomerService
    {
        private readonly HttpClient _httpClient;
        // De HttpClient wordt gebruikt om communicatie met de API te faciliteren.

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
}
