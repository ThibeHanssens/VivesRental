using VivesRental.Sdk;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.BlazorApp.Services;

// **Service for Orders**
public class OrderService
{
    private readonly OrderSdk _sdk;

    // **Constructor**: Initializes the SDK dependency
    public OrderService(OrderSdk sdk)
    {
        _sdk = sdk;
    }

    // **Get All Orders**: Retrieves a list of orders with optional filters
    public async Task<IList<OrderResult>> GetAllAsync(OrderFilter? filter = null)
    {
        return await _sdk.Find(filter);
    }

    // **Get Order by ID**: Fetches a specific order by ID
    public async Task<OrderResult?> GetByIdAsync(Guid id)
    {
        return await _sdk.Get(id);
    }

    // **Create Order**: Adds a new order for a customer
    public async Task<OrderResult?> CreateAsync(Guid customerId)
    {
        return await _sdk.Create(customerId);
    }

    // **Return Order**: Marks an order as returned
    public async Task<bool> ReturnAsync(Guid id, DateTime returnedAt)
    {
        return await _sdk.Return(id, returnedAt);
    }
}
