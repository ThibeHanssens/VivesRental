using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace VivesRental.Sdk
{
    // **AuthSdk**:
    // Deze klasse is verantwoordelijk voor communicatie met de `AuthController` in de API.
    // Het biedt methoden om gebruikers te authenticeren en een JWT-token op te halen.
    public class AuthSdk
    {
        private readonly HttpClient _httpClient;

        // **Constructor**:
        // De HttpClient wordt via Dependency Injection geleverd en gebruikt voor API-aanroepen.
        public AuthSdk(HttpClient httpClient)
        {
            _httpClient = httpClient; // Verbindt de HttpClient aan deze klasse
        }

        // **AuthenticateAsync**:
        // Verifieert gebruikerscredentials door een POST-verzoek naar de `AuthController` te sturen.
        // Bij succes retourneert het de JWT-token als string.
        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            // Maak een loginverzoek met de opgegeven gebruikersnaam en wachtwoord.
            var response = await _httpClient.PostAsJsonAsync("api/authenticate", new
            {
                Username = username,
                Password = password
            });

            // Controleer of de API-aanroep succesvol was.
            if (!response.IsSuccessStatusCode)
            {
                // Retourneer `null` als de inloggegevens ongeldig zijn.
                return null;
            }

            // Lees de responsinhoud en converteer deze naar een `TokenResponse`-object.
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            // Retourneer de JWT-token (of `null` als er geen token aanwezig is).
            return tokenResponse?.Token;
        }

        // **TokenResponse**:
        // Deze interne klasse wordt gebruikt om de API-respons met de JWT-token te deserialiseren.
        private class TokenResponse
        {
            public string Token { get; set; } = string.Empty; // De ontvangen JWT-token
        }
    }
}
