using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VivesRental.Configuration;
using VivesRental.Repository.Core;
using VivesRental.Services;

var builder = WebApplication.CreateBuilder(args);

// **CORS configureren**:
// Sta de Blazor-app toe om verbinding te maken met de API.
var corsPolicyName = "AllowBlazorClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.WithOrigins("https://localhost:5001") // Pas aan naar de URL van je Blazor-app.
            .AllowCredentials();
    });
});

// **Controllers registreren**:
// Configureer JSON-opties.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// **Swagger configureren**:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Voeg JWT-beveiliging toe aan Swagger
    var securityDefinition = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Voeg een geldig JWT-token toe in het volgende formaat: 'Bearer {token}'.",
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
            new string[] { }
        }
    };

    options.AddSecurityRequirement(securityRequirement);
});

// **Database Context registreren**:
builder.Services.AddDbContext<VivesRentalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VivesRentalDatabase")));

// **JWT Configuratie**:
// Lees de JWT-instellingen uit `appsettings.json`.
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

// Voeg authenticatie toe aan de services.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "VivesRentalApi",
        ValidAudience = "VivesRentalClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

// **Services registreren**:
// Registreer services voor Dependency Injection.
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ArticleReservationService>();
builder.Services.AddScoped<OrderLineService>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// **Swagger activeren in ontwikkelmodus**:
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// **CORS gebruiken**:
app.UseCors(corsPolicyName);

// **Middleware voor veilige communicatie**:
app.UseHttpsRedirection();

// **Authenticatie en autorisatie middleware**:
app.UseAuthentication();
app.UseAuthorization();

// **Controllers koppelen aan routes**:
app.MapControllers();

app.Run();
