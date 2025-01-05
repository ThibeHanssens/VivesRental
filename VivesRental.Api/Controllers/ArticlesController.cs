using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers
{
    // Deze controller beheert alle endpoints die gerelateerd zijn aan Articles
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _articleService;

        // Dependency Injection van ArticleService via de constructor
        public ArticlesController(ArticleService articleService)
        {
            _articleService = articleService; // De service wordt gebruikt om logica te beheren
        }

        // GET: api/articles
        // Endpoint om alle artikelen op te halen
        [HttpGet]
        public ActionResult<IEnumerable<ArticleResult>> GetAll()
        {
            try
            {
                // Gebruik van de service om data op te halen
                var articles = _articleService.Find(null);
                return Ok(articles); // Retourneert een 200-status met data
            }
            catch (Exception ex)
            {
                // Foutafhandeling voor onverwachte problemen
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // GET: api/articles/{id}
        // Endpoint om een specifiek artikel op te halen via ID
        [HttpGet("{id}")]
        public ActionResult<ArticleResult> GetById(Guid id)
        {
            try
            {
                var article = _articleService.Get(id);
                if (article == null)
                {
                    return NotFound($"Artikel met ID {id} niet gevonden.");
                }
                return Ok(article);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // POST: api/articles
        // Endpoint om een nieuw artikel toe te voegen
        [HttpPost]
        public ActionResult<ArticleResult> Create([FromBody] ArticleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Validatie van het request
                }

                var createdArticle = _articleService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = createdArticle.Id }, createdArticle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne fout: {ex.Message}");
            }
        }

        // DELETE: api/articles/{id}
        // Endpoint om een artikel te verwijderen
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var result = _articleService.Remove(id);
                if (!result)
                {
                    return NotFound($"Artikel met ID {id} niet gevonden.");
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
