var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Registers controllers to handle incoming HTTP requests.

builder.Services.AddEndpointsApiExplorer();
// Enables endpoint documentation.

builder.Services.AddSwaggerGen();
// Adds Swagger generation for API documentation.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Serves the Swagger-generated OpenAPI document.
    app.UseSwaggerUI(); // Enables the Swagger UI for testing endpoints.
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS.

app.UseAuthorization(); // Adds middleware for authorization.

app.MapControllers(); // Maps controller endpoints.

app.Run(); // Runs the application.
