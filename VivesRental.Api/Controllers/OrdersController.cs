using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Filters;

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
            _orderService = orderService;
        }

        // GET: api/orders
        // Endpoint om alle orders op te halen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetAll()
        {
            try
            {
                var orders = await _orderService.Find(null); // Haalt alle orders op
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
        public async Task<ActionResult<OrderResult>> GetById(Guid id)
        {
            try
            {
                var order = await _orderService.Get(id); // Haalt een specifiek order op
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
        public async Task<ActionResult<OrderResult>> Create([FromBody] Guid customerId)
        {
            try
            {
                // Geen complex requestmodel nodig, alleen CustomerId wordt verwacht
                var createdOrder = await _orderService.Create(customerId); // Maakt een nieuw order aan
                if (createdOrder == null)
                {
                    return BadRequest($"Klant met ID {customerId} niet gevonden."); // Valideert of de klant bestaat
                }
                return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // PUT: api/orders/{id}/return
        // Endpoint om een order terug te brengen
        [HttpPut("{id}/return")]
        public async Task<IActionResult> Return(Guid id, [FromBody] DateTime returnedAt)
        {
            try
            {
                var result = await _orderService.Return(id, returnedAt); // Retourneert het order
                if (!result)
                {
                    return NotFound($"Order met ID {id} niet gevonden of er zijn geen actieve orderlijnen.");
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
