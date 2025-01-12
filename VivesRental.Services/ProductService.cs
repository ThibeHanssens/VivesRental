using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services;

public class ProductService : IProductService
{
    private readonly VivesRentalDbContext _context;

    // **Constructor**:
    // De databasecontext wordt geïnjecteerd via Dependency Injection.
    public ProductService(VivesRentalDbContext context)
    {
        _context = context;
    }

    // **Haalt een specifiek product op**:
    public Task<ProductResult?> Get(Guid id)
    {
        return _context.Products
            .Where(p => p.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();
    }

    // **Zoekt producten met een filter**:
    public Task<List<ProductResult>> Find(ProductFilter? filter = null)
    {
        return _context.Products
            .ApplyFilter(filter)
            .MapToResults(filter)
            .ToListAsync();
    }

    // **Voegt een nieuw product toe**:
    public async Task<ProductResult?> Create(ProductRequest entity)
    {
        // Controleert of de URL geldig is. Als de URL ongeldig is, wordt een standaard URL ingesteld.
        if (!IsValidUrl(entity.ImageUrl))
        {
            entity.ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fnl.wikipedia.org%2Fwiki%2FHogeschool_VIVES&psig=AOvVaw2p0j1SFap9Ok9Ynu1ZbgP0&ust=1736754822854000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCMj0iYXa74oDFQAAAAAdAAAAABAE";
        }

        var product = new Product
        {
            Name = entity.Name,
            Description = entity.Description,
            Manufacturer = entity.Manufacturer,
            Publisher = entity.Publisher,
            RentalExpiresAfterDays = entity.RentalExpiresAfterDays,
            ImageUrl = entity.ImageUrl
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return await Get(product.Id);
    }

    // **Wijzigt een bestaand product**:
    public async Task<ProductResult?> Edit(Guid id, ProductRequest entity)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return null; // Retourneert null als het product niet bestaat.
        }

        // Controleert of de URL geldig is. Als de URL ongeldig is, wordt een standaard URL ingesteld.
        if (!IsValidUrl(entity.ImageUrl))
        {
            entity.ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fnl.wikipedia.org%2Fwiki%2FHogeschool_VIVES&psig=AOvVaw2p0j1SFap9Ok9Ynu1ZbgP0&ust=1736754822854000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCMj0iYXa74oDFQAAAAAdAAAAABAE";
        }

        // Update de eigenschappen van het product.
        product.Name = entity.Name;
        product.Description = entity.Description;
        product.Manufacturer = entity.Manufacturer;
        product.Publisher = entity.Publisher;
        product.RentalExpiresAfterDays = entity.RentalExpiresAfterDays;
        product.ImageUrl = entity.ImageUrl;

        await _context.SaveChangesAsync();
        return await Get(product.Id);
    }

    // **Valideert een URL**:
    private bool IsValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    /// <summary>
    /// Removes one Product, removes ArticleReservations, removes all linked articles and disconnects OrderLines from articles
    /// </summary>
    /// <param name="id">The id of the Product</param>
    /// <returns>True if the product was deleted</returns>
    public async Task<bool> Remove(Guid id)
    {
        if (_context.Database.IsInMemory())
        {
            await RemoveInternal(id);
            return true;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await RemoveInternal(id);
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task RemoveInternal(Guid id)
    {
        await ClearArticleByProductId(id);
        _context.ArticleReservations.RemoveRange(
            _context.ArticleReservations.Where(a => a.Article.ProductId == id));
        _context.Articles.RemoveRange(_context.Articles.Where(a => a.ProductId == id));

        //Remove product
        var product = new Product { Id = id };
        _context.Products.Attach(product);
        _context.Products.Remove(product);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a number of articles with Normal status to the Product.
    /// This is limited to maximum 10.000
    /// </summary>
    /// <returns>True if articles are added</returns>
    public async Task<bool> GenerateArticles(Guid productId, int amount)
    {
        if (amount <= 0 || amount > 10000) //Set a limit to 10K
        {
            return false;
        }

        for (int i = 0; i < amount; i++)
        {
            var article = new Article
            {
                ProductId = productId
            };
            _context.Articles.Add(article);
        }

        var numberOfObjectsUpdated = await _context.SaveChangesAsync();
        return numberOfObjectsUpdated > 0;
    }
        
    private async Task ClearArticleByProductId(Guid productId)
    {
        if (_context.Database.IsInMemory())
        {
            var orderLines = await _context.OrderLines
                .Where(ol => ol.Article.ProductId == productId)
                .ToListAsync();
            foreach (var orderLine in orderLines)
            {
                orderLine.ArticleId = null;
            }

            return;
        }

        var commandText =
            "UPDATE OrderLine SET ArticleId = null from OrderLine inner join Article on Article.ProductId = @ProductId";
        var articleIdParameter = new SqlParameter("@ProductId", productId);

        await _context.Database.ExecuteSqlRawAsync(commandText, articleIdParameter);
    }
}