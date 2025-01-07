using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Api.Controllers
{
    // Deze controller beheert alle endpoints die gerelateerd zijn aan producten
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        // Dependency Injection van ProductService via de constructor
        public ProductsController(ProductService productService)
        {
            _productService = productService; // De service wordt gebruikt om logica te beheren
        }

        // GET: api/products
        // Endpoint om alle producten op te halen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResult>>> GetAll()
        {
            try
            {
                // Haal alle producten op via de service
                var products = await _productService.Find(null);
                return Ok(products); // Retourneer een 200-status met data
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // GET: api/products/{id}
        // Endpoint om een specifiek product op te halen via ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResult>> GetById(Guid id)
        {
            try
            {
                var product = await _productService.Get(id);
                if (product == null)
                {
                    // Retourneer een 404-status als het product niet bestaat
                    return NotFound($"Product met ID {id} niet gevonden.");
                }
                return Ok(product); // Retourneer een 200-status met het product
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // POST: api/products
        // Endpoint om een nieuw product toe te voegen
        [HttpPost]
        public async Task<ActionResult<ProductResult>> Create([FromBody] ProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Controleer of het request voldoet aan de validatieregels
                    return BadRequest(ModelState);
                }

                // Creëer een nieuw product via de service
                var createdProduct = await _productService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // PUT: api/products/{id}
        // Endpoint om een bestaand product te bewerken
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResult>> Update(Guid id, [FromBody] ProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Controleer of het request voldoet aan de validatieregels
                    return BadRequest(ModelState);
                }

                // Bewerk een bestaand product via de service
                var updatedProduct = await _productService.Edit(id, request);
                if (updatedProduct == null)
                {
                    // Retourneer een 404-status als het product niet bestaat
                    return NotFound($"Product met ID {id} niet gevonden.");
                }
                return Ok(updatedProduct); // Retourneer een 200-status met het geüpdatete product
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // DELETE: api/products/{id}
        // Endpoint om een product te verwijderen
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                // Verwijder het product via de service
                var result = await _productService.Remove(id);
                if (!result)
                {
                    // Retourneer een 404-status als het product niet bestaat
                    return NotFound($"Product met ID {id} niet gevonden.");
                }
                return NoContent(); // Retourneer een 204-status (geen inhoud)
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }
    }
}
