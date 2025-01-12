using System.Net.Http.Json;
using VivesRental.Sdk;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.BlazorApp.Services;

// **Service for Articles**
public class ArticleService
{
    private readonly ArticlesSdk _sdk;

    // **Constructor**: Initializes the SDK dependency
    public ArticleService(ArticlesSdk sdk)
    {
        _sdk = sdk;
    }

    // **Get All Articles**: Retrieves a list of articles with optional filters
    public async Task<IList<ArticleResult>> GetAllAsync(ArticleFilter? filter = null)
    {
        return await _sdk.Find(filter);
    }

    // **Get Article by ID**: Fetches a specific article by ID
    public async Task<ArticleResult?> GetByIdAsync(Guid id)
    {
        return await _sdk.Get(id);
    }

    // **Create Article**: Adds a new article
    public async Task<ArticleResult?> CreateAsync(ArticleRequest request)
    {
        return await _sdk.Create(request);
    }

    // **Update Article Status**: Updates the status of an article
    public async Task<bool> UpdateStatusAsync(Guid id, string status)
    {
        return await _sdk.UpdateStatus(id, status);
    }

    // **Delete Article**: Removes an article by ID
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _sdk.Delete(id);
    }
}
