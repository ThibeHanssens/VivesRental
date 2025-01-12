using VivesRental.Sdk;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.BlazorApp.Services;

// **Service for Article Reservations**
public class ArticleReservationService
{
    private readonly ArticleReservationSdk _sdk;

    // **Constructor**: Initializes the SDK dependency
    public ArticleReservationService(ArticleReservationSdk sdk)
    {
        _sdk = sdk;
    }

    // **Get All Reservations**: Retrieves a list of article reservations with optional filters
    public async Task<IList<ArticleReservationResult>> GetAllAsync(ArticleReservationFilter? filter = null)
    {
        return await _sdk.Find(filter);
    }

    // **Get Reservation by ID**: Fetches a specific article reservation by ID
    public async Task<ArticleReservationResult?> GetByIdAsync(Guid id)
    {
        return await _sdk.Get(id);
    }

    // **Create Reservation**: Adds a new article reservation
    public async Task<ArticleReservationResult?> CreateAsync(ArticleReservationRequest request)
    {
        return await _sdk.Create(request);
    }

    // **Delete Reservation**: Removes an article reservation by ID
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _sdk.Remove(id);
    }
}
