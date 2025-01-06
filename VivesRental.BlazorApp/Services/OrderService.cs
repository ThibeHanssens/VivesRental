using System.Net.Http.Json; // Nodig voor het gebruik van GetFromJsonAsync.
using VivesRental.Services.Model.Results; // Importeert het model voor de resultaten.

namespace VivesRental.BlazorApp.Services
{
    // Deze service beheert alle API-aanroepen die betrekking hebben op Orders.
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        // De HttpClient wordt gebruikt om communicatie met de API te faciliteren.

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient; // Dependency Injection van HttpClient.
        }

        // Methode om alle orders op te halen via een GET-aanroep naar de API.
        public async Task<List<OrderResult>> GetAllAsync()
        {
            // Haalt een lijst van OrderResult-objecten op uit de API.
            return await _httpClient.GetFromJsonAsync<List<OrderResult>>("orders");
        }

        // Methode om een specifiek order op te halen via een GET-aanroep naar de API.
        public async Task<OrderResult?> GetByIdAsync(Guid id)
        {
            // Haalt een specifiek order op via het opgegeven ID.
            return await _httpClient.GetFromJsonAsync<OrderResult>($"orders/{id}");
        }

        // Methode om een nieuw order aan te maken via een POST-aanroep naar de API.
        public async Task<OrderResult?> CreateAsync(Guid customerId)
        {
            // Stuurt een POST-aanroep met het klant-ID om een nieuw order te creëren.
            var response = await _httpClient.PostAsJsonAsync("orders", customerId);

            if (response.IsSuccessStatusCode)
            {
                // Retourneert het gecreëerde order als de aanroep succesvol is.
                return await response.Content.ReadFromJsonAsync<OrderResult>();
            }

            // Retourneert null als de aanroep niet succesvol is.
            return null;
        }

        // Methode om een order terug te brengen via een PUT-aanroep naar de API.
        public async Task<bool> ReturnOrderAsync(Guid orderId, DateTime returnedAt)
        {
            // Stuurt een PUT-aanroep met de retourdatum om het order terug te brengen.
            var response = await _httpClient.PutAsJsonAsync($"orders/{orderId}/return", returnedAt);

            // Retourneert true als de aanroep succesvol is; anders false.
            return response.IsSuccessStatusCode;
        }
    }
}
