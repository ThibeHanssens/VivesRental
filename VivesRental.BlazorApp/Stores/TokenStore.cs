using Blazored.LocalStorage;
using Vives.Presentation.Authentication;

namespace VivesRental.BlazorApp.Stores;

// **TokenStore**: Beheert de opslag van JWT-tokens in LocalStorage.
// Implementeert `IBearerTokenStore` om consistentie te garanderen.
public class TokenStore : IBearerTokenStore
{
    private readonly ISyncLocalStorageService _localStorageService;
    private const string TokenName = "BearerToken"; // Sleutelnaam voor JWT-token.

    public TokenStore(ISyncLocalStorageService localStorageService)
    {
        _localStorageService = localStorageService; // Injecteer LocalStorage-service.
    }

    // **GetToken**: Haalt het opgeslagen JWT-token op.
    public string GetToken()
    {
        return _localStorageService.GetItem<string>(TokenName) ?? string.Empty;
        // Retourneer een lege string als er geen token is opgeslagen.
    }

    // **SetToken**: Slaat een nieuw JWT-token op.
    public void SetToken(string token)
    {
        _localStorageService.SetItem(TokenName, token);
    }
}
