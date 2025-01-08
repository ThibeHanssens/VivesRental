using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers;

// **Controller voor producten**:
// Beheert alle API-endpoints die gerelateerd zijn aan Producten.
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    // **Dependency Injection**:
    // De ProductService wordt via de constructor geïnjecteerd en beheert de logica voor producten.
    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    // **GET: api/products**:
    // Haalt een lijst op van alle beschikbare producten.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResult>>> GetAll()
    {
        try
        {
            // Gebruik de ProductService om producten op te halen.
            var products = await _productService.Find(null);
            return Ok(products); // Retourneert een 200-status met de lijst van producten.
        }
        catch (Exception ex)
        {
            // Foutafhandeling bij onverwachte fouten.
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **GET: api/products/{id}**:
    // Haalt een specifiek product op op basis van ID.
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResult>> GetById(Guid id)
    {
        try
        {
            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound($"Product met ID {id} niet gevonden."); // Retourneert een 404-status.
            }
            return Ok(product); // Retourneert een 200-status met het product.
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **POST: api/products**:
    // Maakt een nieuw product aan op basis van een ProductRequest.
    [HttpPost]
    public async Task<ActionResult<ProductResult>> Create([FromBody] ProductRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Controleert of het request valide is.
            }

            var createdProduct = await _productService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **PUT: api/products/{id}**:
    // Wijzigt een bestaand product op basis van ID en een ProductRequest.
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResult>> Update(Guid id, [FromBody] ProductRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProduct = await _productService.Edit(id, request);
            if (updatedProduct == null)
            {
                return NotFound($"Product met ID {id} niet gevonden.");
            }
            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **DELETE: api/products/{id}**:
    // Verwijdert een product op basis van ID.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _productService.Remove(id);
            if (!result)
            {
                return NotFound($"Product met ID {id} niet gevonden.");
            }
            return NoContent(); // Retourneert een 204-status.
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }
}
