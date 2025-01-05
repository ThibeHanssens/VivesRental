using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Abstractions;

namespace VivesRental.Api.Controllers
{
    // Deze controller beheert alle endpoints die gerelateerd zijn aan Customers
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        // Dependency Injection van CustomerService via de constructor
        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService; // De service wordt gebruikt om logica te beheren
        }

        // GET: api/customers
        // Endpoint om alle klanten op te halen
        [HttpGet]
        public ActionResult<IEnumerable<CustomerResult>> GetAll()
        {
            try
            {
                var customers = _customerService.Find(null); // Haalt alle klanten op
                return Ok(customers); // Retourneert een 200-status met data
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // GET: api/customers/{id}
        // Endpoint om een specifieke klant op te halen via ID
        [HttpGet("{id}")]
        public ActionResult<CustomerResult> GetById(Guid id)
        {
            try
            {
                var customer = _customerService.Get(id); // Haalt een specifieke klant op
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

        // POST: api/customers
        // Endpoint om een nieuwe klant te creëren
        [HttpPost]
        public ActionResult<CustomerResult> Create([FromBody] CustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Controleert of het request valide is
                }

                var createdCustomer = _customerService.Create(request); // Maakt een nieuwe klant aan
                return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // DELETE: api/customers/{id}
        // Endpoint om een klant te verwijderen
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _customerService.Remove(id); // Await de Task<bool> en verwijdert de klant
                if (!result)
                {
                    return NotFound($"Klant met ID {id} niet gevonden.");
                }
                return NoContent(); // Retourneert een 204-status (geen inhoud)
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }
    }
}
