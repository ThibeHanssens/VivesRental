using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Api.Controllers;

// **Controller voor artikelreserveringen**:
// Beheert alle API-endpoints gerelateerd aan `ArticleReservations`.
[ApiController]
[Route("api/[controller]")]
[Authorize] // Beveilig de hele controller.
public class ArticleReservationsController : ControllerBase
{
    private readonly ArticleReservationService _articleReservationService;

    // **Dependency Injection**:
    // Injecteert de `ArticleReservationService` voor logica van reserveringen.
    public ArticleReservationsController(ArticleReservationService articleReservationService)
    {
        _articleReservationService = articleReservationService;
    }

    // **GET: api/articlereservations**:
    // Haalt een lijst op van alle artikelreserveringen.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleReservationResult>>> GetAll()
    {
        try
        {
            // Haal alle reserveringen op.
            var reservations = await _articleReservationService.Find(null);
            return Ok(reservations); // Retourneert een 200-status met data.
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}"); // Algemene foutafhandeling.
        }
    }

    // **GET: api/articlereservations/{id}**:
    // Haalt een specifieke reservering op via ID.
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleReservationResult>> GetById(Guid id)
    {
        try
        {
            var reservation = await _articleReservationService.Get(id);
            if (reservation == null)
            {
                return NotFound($"Reservering met ID {id} niet gevonden."); // Specifieke foutmelding.
            }
            return Ok(reservation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **POST: api/articlereservations**:
    // Maakt een nieuwe reservering aan op basis van een request.
    [HttpPost]
    public async Task<ActionResult<ArticleReservationResult>> Create([FromBody] ArticleReservationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Controleer op invoervalidatie.
            }

            // Creëer de reservering.
            var createdReservation = await _articleReservationService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = createdReservation.Id }, createdReservation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **DELETE: api/articlereservations/{id}**:
    // Verwijdert een reservering op basis van ID.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _articleReservationService.Remove(id);
            if (!result)
            {
                return NotFound($"Reservering met ID {id} niet gevonden.");
            }
            return NoContent(); // Retourneert een 204-status bij succesvolle verwijdering.
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }
}
