using System.Net.Http.Json;
using VivesRental.Sdk;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.BlazorApp.Services;

// **Service for Products**
public class ProductService
{
    private readonly ProductSdk _sdk;

    // **Constructor**: Initializes the SDK dependency
    public ProductService(ProductSdk sdk)
    {
        _sdk = sdk;
    }

    // **Get All Products**: Retrieves a list of products with optional filters
    public async Task<IList<ProductResult>> GetAllAsync(ProductFilter? filter = null)
    {
        return await _sdk.Find(filter);
    }

    // **Get Product by ID**: Fetches a specific product by ID
    public async Task<ProductResult?> GetByIdAsync(Guid id)
    {
        return await _sdk.Get(id);
    }

    // **Create Product**: Adds a new product
    public async Task<ProductResult?> CreateAsync(ProductRequest request)
    {
        return await _sdk.Create(request);
    }

    // **Edit Product**: Updates an existing product
    public async Task<ProductResult?> EditAsync(Guid id, ProductRequest request)
    {
        return await _sdk.Edit(id, request);
    }

    // **Delete Product**: Removes a product by ID
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _sdk.Remove(id);
    }
}
