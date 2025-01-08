using Microsoft.EntityFrameworkCore;
using VivesRental.Repository.Core;
using VivesRental.Services; // Zorg ervoor dat de services worden herkend door Dependency Injection.
using System.Text.Json.Serialization;
using System.Text.Json; // Nodig voor JSON-instellingen.

var builder = WebApplication.CreateBuilder(args); // Maakt en configureert de applicatie.

// **Controllers registreren**:
// Hiermee worden alle controllers in het project geregistreerd. JSON-opties worden ingesteld om potentiële problemen zoals cyclische referenties te voorkomen.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Negeer cyclische referenties in JSON-output.
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // Zorg ervoor dat JSON-eigenschappen in camelCase worden geschreven.
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

        // Negeer velden met een null-waarde om data te vereenvoudigen.
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// **Swagger configureren**:
// Hiermee wordt Swagger gebruikt voor documentatie van de API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// **Database Context registreren**:
// De VivesRentalDbContext wordt geregistreerd met een SQL Server-connectiestring uit `appsettings.json`.
builder.Services.AddDbContext<VivesRentalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VivesRentalDatabase")));

// **Services registreren**:
// Dit zijn de services die gebruikt worden in de applicatie. Dependency Injection zorgt ervoor dat deze beschikbaar zijn in de controllers.
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ArticleReservationService>();
builder.Services.AddScoped<OrderLineService>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build(); // Bouw de applicatie op.

// **Swagger activeren in ontwikkelmodus**:
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// **Middleware voor veilige communicatie**:
app.UseHttpsRedirection(); // Verplicht het gebruik van HTTPS.

// **Autorisatie instellen**:
app.UseAuthorization(); // Middleware voor eventuele toekomstige autorisatie.

// **Controllers koppelen aan routes**:
app.MapControllers(); // Maakt alle API-endpoints bereikbaar.

app.Run(); // Start de applicatie.
