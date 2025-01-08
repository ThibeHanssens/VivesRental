using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Results;

namespace VivesRental.Api.Controllers;

// **Controller voor orders**:
// Beheert alle API-endpoints die gerelateerd zijn aan Orders.
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    // **GET: api/orders**:
    // Haalt een lijst op van alle orders.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResult>>> GetAll()
    {
        try
        {
            var orders = await _orderService.Find(null);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **GET: api/orders/{id}**:
    // Haalt een specifiek order op op basis van ID.
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResult>> GetById(Guid id)
    {
        try
        {
            var order = await _orderService.Get(id);
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

    // **POST: api/orders**:
    // Maakt een nieuw order aan op basis van een CustomerId.
    [HttpPost]
    public async Task<ActionResult<OrderResult>> Create([FromBody] Guid customerId)
    {
        try
        {
            var createdOrder = await _orderService.Create(customerId);
            if (createdOrder == null)
            {
                return BadRequest($"Klant met ID {customerId} niet gevonden.");
            }
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **PUT: api/orders/{id}/return**:
    // Retourneert een order door alle orderlijnen te markeren als geretourneerd.
    [HttpPut("{id}/return")]
    public async Task<IActionResult> Return(Guid id, [FromBody] DateTime returnedAt)
    {
        try
        {
            var result = await _orderService.Return(id, returnedAt);
            if (!result)
            {
                return NotFound($"Order met ID {id} niet gevonden of geen actieve orderlijnen.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }
}
