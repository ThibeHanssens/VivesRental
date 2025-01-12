using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Api.Controllers;

// **Controller voor orderlijnen**:
// Beheert alle API-endpoints gerelateerd aan `OrderLines`.
[ApiController]
[Route("api/[controller]")]
[Authorize] // Beveilig de hele controller.
public class OrderLinesController : ControllerBase
{
    private readonly OrderLineService _orderLineService;

    // **Dependency Injection**:
    // Injecteert de `OrderLineService` voor logica van orderlijnen.
    public OrderLinesController(OrderLineService orderLineService)
    {
        _orderLineService = orderLineService; // Initialiseer de service.
    }

    // **GET: api/orderlines**:
    // Haalt een lijst op van alle orderlijnen.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderLineResult>>> GetAll()
    {
        try
        {
            // Haal alle orderlijnen op via de service.
            var orderLines = await _orderLineService.Find(null);
            return Ok(orderLines); // Retourneer een lijst met orderlijnen in een HTTP 200 (OK) response.
        }
        catch (Exception ex)
        {
            // Foutafhandeling voor onverwachte problemen.
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **GET: api/orderlines/{id}**:
    // Haalt een specifieke orderlijn op via ID.
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderLineResult>> GetById(Guid id)
    {
        try
        {
            // Haal een specifieke orderlijn op via ID.
            var orderLine = await _orderLineService.Get(id);
            if (orderLine == null)
            {
                // Retourneer een 404-status als de orderlijn niet bestaat.
                return NotFound($"Orderlijn met ID {id} niet gevonden.");
            }
            return Ok(orderLine); // Retourneer de gevonden orderlijn.
        }
        catch (Exception ex)
        {
            // Foutafhandeling voor onverwachte problemen.
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **POST: api/orderlines/rent**:
    // Verhuurt een artikel via een orderlijn.
    [HttpPost("rent")]
    public async Task<IActionResult> Rent([FromQuery] Guid orderId, [FromQuery] Guid articleId)
    {
        try
        {
            // Controleer of de GUID's geldig zijn.
            if (orderId == Guid.Empty || articleId == Guid.Empty)
            {
                return BadRequest("OrderId of ArticleId is ongeldig.");
            }

            // Probeer het artikel te huren via de service.
            var success = await _orderLineService.Rent(orderId, articleId);
            if (!success)
            {
                // Retourneer een 400-status als de actie mislukt.
                return BadRequest("Het artikel is niet beschikbaar of er is een fout opgetreden.");
            }
            return NoContent(); // Retourneer een 204-status (geen inhoud).
        }
        catch (Exception ex)
        {
            // Foutafhandeling voor onverwachte problemen.
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **PUT: api/orderlines/{id}/return**:
    // Retourneert een artikel via een orderlijn.
    [HttpPut("{id}/return")]
    public async Task<IActionResult> Return(Guid id, [FromBody] DateTime returnedAt)
    {
        try
        {
            // Controleer of de geretourneerde datum geldig is.
            if (returnedAt == DateTime.MinValue)
            {
                return BadRequest("De geretourneerde datum is ongeldig.");
            }

            // Probeer het artikel te retourneren via de service.
            var success = await _orderLineService.Return(id, returnedAt);
            if (!success)
            {
                // Retourneer een 400-status als de return mislukt.
                return BadRequest("De orderlijn kon niet worden geretourneerd.");
            }
            return NoContent(); // Retourneer een 204-status (geen inhoud).
        }
        catch (Exception ex)
        {
            // Foutafhandeling voor onverwachte problemen.
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }
}
