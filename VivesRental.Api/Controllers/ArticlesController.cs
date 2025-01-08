using Microsoft.AspNetCore.Mvc;
using VivesRental.Services;
using VivesRental.Services.Model.Results;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers;

// **Controller voor artikelen**:
// Beheert alle API-endpoints die gerelateerd zijn aan Artikelen.
[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ArticleService _articleService;

    // **Dependency Injection**:
    // De ArticleService wordt via de constructor geïnjecteerd en beheert de logica voor artikelen.
    public ArticlesController(ArticleService articleService)
    {
        _articleService = articleService;
    }

    // **GET: api/articles**:
    // Haalt een lijst op van alle beschikbare artikelen.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleResult>>> GetAll()
    {
        try
        {
            // Gebruik de ArticleService om artikelen op te halen.
            var articles = await _articleService.Find(null);
            return Ok(articles); // Retourneert een 200-status met de lijst van artikelen.
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **GET: api/articles/{id}**:
    // Haalt een specifiek artikel op op basis van ID.
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleResult>> GetById(Guid id)
    {
        try
        {
            var article = await _articleService.Get(id);
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

    // **POST: api/articles**:
    // Maakt een nieuw artikel aan op basis van een ArticleRequest.
    [HttpPost]
    public async Task<ActionResult<ArticleResult>> Create([FromBody] ArticleRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdArticle = await _articleService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = createdArticle.Id }, createdArticle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }

    // **DELETE: api/articles/{id}**:
    // Verwijdert een artikel op basis van ID.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _articleService.Remove(id);
            if (!result)
            {
                return NotFound($"Artikel met ID {id} niet gevonden.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne fout: {ex.Message}");
        }
    }
}
