namespace VivesRental.BlazorApp.Settings
{
    // **Configuratieklasse voor API-instellingen**:
    // Deze klasse bevat de basis-URL voor de API. Dit kan in de toekomst uitgebreid worden met andere instellingen.
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty; // Zorgt ervoor dat de URL niet null is.
    }
}
