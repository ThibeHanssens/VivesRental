namespace VivesRental.Model;

public class Product
{

    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Publisher { get; set; }
    public int RentalExpiresAfterDays { get; set; }

    // **Afbeelding URL**:
    // URL of pad naar een afbeelding van het product.
    // Deze kan worden gebruikt om een visuele representatie van het product te tonen in de UI.
    public string? ImageUrl { get; set; }

    public IList<Article> Articles { get; set; } = new List<Article>();
}