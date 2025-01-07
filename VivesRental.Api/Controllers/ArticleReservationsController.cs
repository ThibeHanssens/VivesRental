using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Api.Controllers
{
    // Deze controller beheert alle endpoints die gerelateerd zijn aan ArticleReservations
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleReservationsController : ControllerBase
    {
        private readonly ArticleReservationService _articleReservationService;

        // Dependency Injection van ArticleReservationService via de constructor
        public ArticleReservationsController(ArticleReservationService articleReservationService)
        {
            _articleReservationService = articleReservationService; // De service wordt gebruikt om logica te beheren
        }

        // GET: api/articlereservations
        // Endpoint om alle artikelreserveringen op te halen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleReservationResult>>> GetAll()
        {
            try
            {
                // Haal alle reserveringen op via de service
                var reservations = await _articleReservationService.Find(null);
                return Ok(reservations); // Retourneer een 200-status met data
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // GET: api/articlereservations/{id}
        // Endpoint om een specifieke reservering op te halen via ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleReservationResult>> GetById(Guid id)
        {
            try
            {
                var reservation = await _articleReservationService.Get(id);
                if (reservation == null)
                {
                    // Retourneer een 404-status als de reservering niet bestaat
                    return NotFound($"Reservering met ID {id} niet gevonden.");
                }
                return Ok(reservation); // Retourneer een 200-status met de reservering
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // POST: api/articlereservations
        // Endpoint om een nieuwe reservering te maken
        [HttpPost]
        public async Task<ActionResult<ArticleReservationResult>> Create([FromBody] ArticleReservationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Controleer of het request voldoet aan de validatieregels
                    return BadRequest(ModelState);
                }

                // Maak een nieuwe reservering via de service
                var createdReservation = await _articleReservationService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = createdReservation.Id }, createdReservation);
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // DELETE: api/articlereservations/{id}
        // Endpoint om een reservering te verwijderen
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                // Verwijder de reservering via de service
                var result = await _articleReservationService.Remove(id);
                if (!result)
                {
                    // Retourneer een 404-status als de reservering niet bestaat
                    return NotFound($"Reservering met ID {id} niet gevonden.");
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
