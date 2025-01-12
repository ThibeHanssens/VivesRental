using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens; // Voor parsing van JWT-tokens.
using Vives.Presentation.Authentication;

namespace VivesRental.BlazorApp.Security;

// **TokenAuthenticationStateProvider**: Beheert de authenticatiestatus van gebruikers.
// Gebruikt JWT-tokens om de identiteit te bepalen.
public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IBearerTokenStore _tokenStore;

    public TokenAuthenticationStateProvider(IBearerTokenStore tokenStore)
    {
        _tokenStore = tokenStore; // Injecteer tokenopslagservice.
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(GetAuthenticationStateFromToken());
        // Retourneert de huidige authenticatiestatus op basis van een opgeslagen token.
    }

    public void AuthenticateUser()
    {
        var authenticationState = GetAuthenticationStateFromToken();
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        // Verzend de nieuwe authenticatiestatus naar alle abonnees.
    }

    private AuthenticationState GetAuthenticationStateFromToken()
    {
        var token = _tokenStore.GetToken();
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var jwtToken = new JsonWebToken(token); // Parse het JWT-token.
        var claims = jwtToken.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }
}
