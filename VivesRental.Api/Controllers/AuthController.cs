using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VivesRental.Configuration;
using VivesRental.Repository.Core;
using VivesRental.Model;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers;

// **Controller voor authenticatie**
// Beheert login-functionaliteit en genereert JWT-tokens.
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous] // Deze controller vereist geen voorafgaande authenticatie.
public class AuthController : ControllerBase
{
    private readonly VivesRentalDbContext _context; // Databasecontext voor gebruikersverificatie.
    private readonly JwtSettings _jwtSettings; // JWT-instellingen zoals secret en expiratie.

    public AuthController(VivesRentalDbContext context, JwtSettings jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings;
    }

    // **POST: api/authenticate**
    // Verifieert inloggegevens en retourneert een JWT-token als de gegevens correct zijn.
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] LoginRequest request)
    {
        // **Zoek gebruiker in de database op basis van gebruikersnaam**:
        var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);

        // **Controleer of de gebruiker bestaat en het wachtwoord correct is**:
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized("Ongeldige gebruikersnaam of wachtwoord.");
        }

        // **Genereer JWT-token voor geauthenticeerde gebruiker**:
        var token = GenerateJwtToken(user);

        // Retourneer het token naar de client.
        return Ok(new { Token = token });
    }

    // **Hulpmethode: Genereer JWT-token**
    // Creëert een JWT-token op basis van gebruikersinformatie.
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = _jwtSettings.ValidIssuer, 
            Audience = _jwtSettings.ValidAudience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };



        var token = tokenHandler.CreateToken(tokenDescriptor); // Maak het token aan.
        return tokenHandler.WriteToken(token); // Retourneer het token als string.
    }
}
