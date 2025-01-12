using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Vives.Presentation.Authentication;

namespace VivesRental.BlazorApp.Security;

public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IBearerTokenStore _tokenStore;

    public TokenAuthenticationStateProvider(IBearerTokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(GetAuthenticationStateFromToken());
    }

    public void AuthenticateUser()
    {
        var authenticationState = GetAuthenticationStateFromToken();
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
    }

    private AuthenticationState GetAuthenticationStateFromToken()
    {
        var token = _tokenStore.GetToken();
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        try
        {
            var jwtToken = new JsonWebToken(token);
            var claims = jwtToken.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch
        {
            // Foutafhandeling voor corrupte tokens.
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}
