using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers
{
    // Deze controller beheert alle endpoints die gerelateerd zijn aan Orders
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        // Dependency Injection van OrderService via de constructor
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService; // De service wordt gebruikt om logica te beheren
        }

        // GET: api/orders
        // Endpoint om alle orders op te halen
        [HttpGet]
        public ActionResult<IEnumerable<OrderResult>> GetAll()
        {
            try
            {
                var orders = _orderService.Find(null); // Haalt alle orders op
                return Ok(orders); // Retourneert een 200-status met data
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // GET: api/orders/{id}
        // Endpoint om een specifiek order op te halen via ID
        [HttpGet("{id}")]
        public ActionResult<OrderResult> GetById(Guid id)
        {
            try
            {
                var order = _orderService.Get(id); // Haalt een specifiek order op
                if (order == null)
                {
                    return NotFound($"Order met ID {id} niet gevonden.");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // POST: api/orders
        // Endpoint om een nieuw order te creëren
        [HttpPost]
        public ActionResult<OrderResult> Create([FromBody] OrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Controleert of het request valide is
                }

                var createdOrder = _orderService.Create(request); // Maakt een nieuw order aan
                return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // DELETE: api/orders/{id}
        // Endpoint om een order te verwijderen
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var result = _orderService.Remove(id); // Verwijdert het order
                if (!result)
                {
                    return NotFound($"Order met ID {id} niet gevonden.");
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
