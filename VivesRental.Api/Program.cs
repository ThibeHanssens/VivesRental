using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // Voor tokenvalidatie in de JWT-configuratie.
using Microsoft.OpenApi.Models; // Voor het configureren van Swagger-documentatie.
using VivesRental.Configuration; // Bevat JWT-configuratieklasse.
using VivesRental.Repository.Core; // Bevat de database context klasse.
using VivesRental.Services; // Voor service-klassen zoals CustomerService.

var builder = WebApplication.CreateBuilder(args); // Initialiseert en configureert de webapplicatie.

// **CORS configureren**:
// Hier wordt een CORS-beleid (Cross-Origin Resource Sharing) geconfigureerd.
// Dit zorgt ervoor dat de Blazor WebAssembly-client toegang heeft tot deze API, 
// terwijl andere domeinen niet automatisch toegang krijgen.
var corsPolicyName = "AllowBlazorClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, builder =>
    {
        builder.AllowAnyHeader() // Alle headers toestaan (zoals Authorization).
            .AllowAnyMethod() // Alle HTTP-methoden toestaan (GET, POST, DELETE, enz.).
            .WithOrigins("https://localhost:7164") // URL van de Blazor WebAssembly-client (pas aan indien nodig).
            .AllowCredentials(); // Sta cookies en andere credenties toe.
    });
});

// **Controllers registreren**:
// Hier worden de controllers (API-endpoints) geconfigureerd.
// JSON-opties worden ingesteld om cyclische referenties te vermijden en eigenschappen in camelCase te schrijven.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Voorkomt cyclische referenties.
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Zet JSON-output in camelCase.
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Sla null-waarden over in de JSON-output.
    });

// **Swagger configureren**:
// Hier wordt Swagger geconfigureerd om de API te documenteren. 
// JWT-authenticatie wordt geïntegreerd in de Swagger UI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securityDefinition = new OpenApiSecurityScheme
    {
        Name = "Authorization", // Naam van de header.
        In = ParameterLocation.Header, // Locatie van de header.
        Type = SecuritySchemeType.ApiKey, // Type beveiliging (API-sleutel).
        Scheme = "Bearer", // Gebruik Bearer-tokens.
        BearerFormat = "JWT", // Specificatie voor JWT-tokens.
        Description = "Voeg een geldig JWT-token toe in het formaat: 'Bearer {token}'.", // Uitleg in Swagger UI.
    };

    options.AddSecurityDefinition("Bearer", securityDefinition);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { } // Geen specifieke scopes nodig.
        }
    };

    options.AddSecurityRequirement(securityRequirement);
});

// **Database Context registreren**:
// Registreer de `VivesRentalDbContext` met de connectiestring uit `appsettings.json`.
// Dit verbindt de applicatie met de SQL Server-database.
builder.Services.AddDbContext<VivesRentalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VivesRentalDatabase")));

// **JWT Configuratie**:
// Lees de JWT-instellingen zoals `Secret` en `ExpirationTimeSpan` uit `appsettings.json`.
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings); // Bind de instellingen aan de `JwtSettings`-klasse.
builder.Services.AddSingleton(jwtSettings); // Registreer de JWT-configuratie als singleton.

// **JWT-authenticatie toevoegen**:
// Hier wordt de applicatie geconfigureerd om JWT-tokens te gebruiken voor beveiliging.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Standaard authenticatieschema.
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Standaard schema voor uitdagingen.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Standaard schema voor uitdagingen.
}).AddJwtBearer(options =>
{

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Controleer de uitgever van het token.
        ValidateAudience = true, // Controleer de doelgroep van het token.
        ValidateLifetime = true, // Controleer of het token nog geldig is.
        ValidateIssuerSigningKey = true, // Controleer de handtekening van het token.
        ValidIssuer = jwtSettings.ValidIssuer, // Haalt de issuer op uit de instellingen
        ValidAudience = jwtSettings.ValidAudience, // Haalt de audience op uit de instellingen
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };

});

// **Services registreren**:
// Registreer alle services (zoals `ArticleService`, `CustomerService`, enz.) voor Dependency Injection.
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ArticleReservationService>();
builder.Services.AddScoped<OrderLineService>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build(); // Bouw de applicatie.

// **Swagger activeren in ontwikkelmodus**:
// Zorg ervoor dat Swagger alleen beschikbaar is in de ontwikkelmodus.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Activeer Swagger.
    app.UseSwaggerUI(); // Activeer de Swagger UI.
}

// **CORS gebruiken**:
// Activeer het eerder geconfigureerde CORS-beleid.
app.UseCors(corsPolicyName);

// **Middleware voor veilige communicatie**:
// Zorg ervoor dat alle communicatie via HTTPS verloopt.
app.UseHttpsRedirection();

// **Authenticatie en autorisatie middleware**:
// Activeer authenticatie en autorisatie.
app.UseAuthentication(); // Controleer of verzoeken zijn geauthenticeerd.
app.UseAuthorization(); // Controleer of geauthenticeerde gebruikers toegang hebben.

// **Controllers koppelen aan routes**:
// Zorg ervoor dat de API endpoints bereikbaar zijn.
app.MapControllers();

app.Run(); // Start de applicatie.
