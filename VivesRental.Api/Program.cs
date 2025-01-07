using Microsoft.EntityFrameworkCore;
using VivesRental.Repository.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Registreer controllers voor het verwerken van HTTP-verzoeken.

builder.Services.AddEndpointsApiExplorer(); // Activeert endpoint-documentatie.
builder.Services.AddSwaggerGen(); // Voeg Swagger toe voor API-documentatie.

// Voeg DbContext toe en configureer de verbinding met SQL Server via appsettings.json.
builder.Services.AddDbContext<VivesRentalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VivesRentalDatabase")));
// Gebruik de connection string uit de configuratie om verbinding te maken met de database.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Activeer Swagger-documentatie in ontwikkelmodus.
    app.UseSwaggerUI(); // Activeer de Swagger UI voor interactie met endpoints.
}

app.UseHttpsRedirection(); // Forceer HTTPS-verkeer.

app.UseAuthorization(); // Voeg middleware toe voor autorisatie.

app.MapControllers(); // Map controllers naar routes.

app.Run(); // Start de applicatie.
