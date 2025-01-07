using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Api.Controllers
{
    // Deze controller beheert alle endpoints die gerelateerd zijn aan OrderLines
    [ApiController]
    [Route("api/[controller]")]
    public class OrderLinesController : ControllerBase
    {
        private readonly OrderLineService _orderLineService;

        // Dependency Injection van OrderLineService via de constructor
        public OrderLinesController(OrderLineService orderLineService)
        {
            _orderLineService = orderLineService; // De service wordt gebruikt om logica te beheren
        }

        // GET: api/orderlines
        // Endpoint om alle orderlijnen op te halen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderLineResult>>> GetAll()
        {
            try
            {
                // Haal alle orderlijnen op via de service
                var orderLines = await _orderLineService.Find(null);
                return Ok(orderLines); // Retourneer een 200-status met data
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // GET: api/orderlines/{id}
        // Endpoint om een specifieke orderlijn op te halen via ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderLineResult>> GetById(Guid id)
        {
            try
            {
                var orderLine = await _orderLineService.Get(id);
                if (orderLine == null)
                {
                    // Retourneer een 404-status als de orderlijn niet bestaat
                    return NotFound($"Orderlijn met ID {id} niet gevonden.");
                }
                return Ok(orderLine); // Retourneer een 200-status met de orderlijn
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // POST: api/orderlines/rent
        // Endpoint om een artikel te huren
        [HttpPost("rent")]
        public async Task<IActionResult> Rent(Guid orderId, Guid articleId)
        {
            try
            {
                // Probeer een artikel te huren via de service
                var success = await _orderLineService.Rent(orderId, articleId);
                if (!success)
                {
                    // Retourneer een 400-status als het artikel niet beschikbaar is
                    return BadRequest("Het artikel is niet beschikbaar of er is een fout opgetreden.");
                }
                return NoContent(); // Retourneer een 204-status (geen inhoud)
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // PUT: api/orderlines/{id}/return
        // Endpoint om een orderlijn te retourneren
        [HttpPut("{id}/return")]
        public async Task<IActionResult> Return(Guid id, [FromBody] DateTime returnedAt)
        {
            try
            {
                // Retourneer een artikel via de service
                var success = await _orderLineService.Return(id, returnedAt);
                if (!success)
                {
                    // Retourneer een 400-status als de return niet succesvol was
                    return BadRequest("De orderlijn kon niet worden geretourneerd.");
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
