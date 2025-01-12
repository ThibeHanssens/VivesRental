using VivesRental.Sdk;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.BlazorApp.Services;

// **Service for Order Lines**
public class OrderLineService
{
    private readonly OrderLineSdk _sdk;

    // **Constructor**: Initializes the SDK dependency
    public OrderLineService(OrderLineSdk sdk)
    {
        _sdk = sdk;
    }

    // **Get All Order Lines**: Retrieves a list of order lines with optional filters
    public async Task<IList<OrderLineResult>> GetAllAsync(OrderLineFilter? filter = null)
    {
        return await _sdk.Find(filter);
    }

    // **Get Order Line by ID**: Fetches a specific order line by ID
    public async Task<OrderLineResult?> GetByIdAsync(Guid id)
    {
        return await _sdk.Get(id);
    }

    // **Rent Article**: Associates an article with an order
    public async Task<bool> RentAsync(Guid orderId, Guid articleId)
    {
        return await _sdk.Rent(orderId, articleId);
    }

    // **Return Article**: Marks an article in an order line as returned
    public async Task<bool> ReturnAsync(Guid id, DateTime returnedAt)
    {
        return await _sdk.Return(id, returnedAt);
    }
}
