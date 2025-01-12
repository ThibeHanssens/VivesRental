using VivesRental.Sdk;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.BlazorApp.Services;

// **Service for Customers**
public class CustomerService
{
    private readonly CustomerSdk _sdk;

    // **Constructor**: Initializes the SDK dependency
    public CustomerService(CustomerSdk sdk)
    {
        _sdk = sdk;
    }

    // **Get All Customers**: Retrieves a list of customers with optional filters
    public async Task<IList<CustomerResult>> GetAllAsync(CustomerFilter? filter = null)
    {
        return await _sdk.Find(filter);
    }

    // **Get Customer by ID**: Fetches a specific customer by ID
    public async Task<CustomerResult?> GetByIdAsync(Guid id)
    {
        return await _sdk.Get(id);
    }

    // **Create Customer**: Adds a new customer
    public async Task<CustomerResult?> CreateAsync(CustomerRequest request)
    {
        return await _sdk.Create(request);
    }

    // **Delete Customer**: Removes a customer by ID
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _sdk.Remove(id);
    }
}
