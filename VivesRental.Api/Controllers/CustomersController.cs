using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers;

// **Controller voor klanten**:
// Beheert alle API-endpoints die gerelateerd zijn aan Klanten.
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    // **Dependency Injection**:
    // De CustomerService wordt via de constructor geïnjecteerd en beheert de logica voor klanten.
    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // **GET: api/customers**:
    // Haalt een lijst op van alle klanten.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResult>>> GetAll()
    {
        try
        {
            var customers = await _customerService.Find(null);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **GET: api/customers/{id}**:
    // Haalt een specifieke klant op op basis van ID.
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResult>> GetById(Guid id)
    {
        try
        {
            var customer = await _customerService.Get(id);
            if (customer == null)
            {
                return NotFound($"Klant met ID {id} niet gevonden.");
            }
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **POST: api/customers**:
    // Maakt een nieuwe klant aan op basis van een CustomerRequest.
    [HttpPost]
    public async Task<ActionResult<CustomerResult>> Create([FromBody] CustomerRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCustomer = await _customerService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **DELETE: api/customers/{id}**:
    // Verwijdert een klant op basis van ID.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _customerService.Remove(id);
            if (!result)
            {
                return NotFound($"Klant met ID {id} niet gevonden.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }
}
