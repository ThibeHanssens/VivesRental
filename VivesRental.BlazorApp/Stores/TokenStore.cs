using Blazored.LocalStorage;
using Vives.Presentation.Authentication;

namespace VivesRental.BlazorApp.Stores;

public class TokenStore : IBearerTokenStore
{
    private readonly ISyncLocalStorageService _localStorageService;
    private const string TokenName = "BearerToken";

    public TokenStore(ISyncLocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public string GetToken()
    {
        return _localStorageService.GetItem<string>(TokenName) ?? string.Empty;
    }

    public void SetToken(string token)
    {
        _localStorageService.SetItem(TokenName, token);
    }

    public void RemoveToken()
    {
        _localStorageService.RemoveItem(TokenName);
    }
}
