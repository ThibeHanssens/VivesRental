using Microsoft.Extensions.DependencyInjection;
using VivesRental.Sdk.Handlers;

namespace VivesRental.Sdk.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services, string apiUrl)
        {
            // Registreert de AuthorizationHandler voor het afhandelen van Bearer tokens.
            services.AddScoped<AuthorizationHandler>();

            // Configuratie van de HttpClient met base URL en autorisatie.
            services.AddHttpClient("VivesRentalApi", options =>
            {
                options.BaseAddress = new Uri(apiUrl);
            }).AddHttpMessageHandler<AuthorizationHandler>();

            // Registratie van alle SDK-klassen.
            services.AddScoped<ArticleReservationSdk>();
            services.AddScoped<ArticlesSdk>();
            services.AddScoped<CustomerSdk>();
            services.AddScoped<OrderLineSdk>();
            services.AddScoped<OrderSdk>();
            services.AddScoped<ProductSdk>();

            return services;
        }
    }
}
