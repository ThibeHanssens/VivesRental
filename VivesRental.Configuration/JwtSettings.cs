namespace VivesRental.Configuration
{
    public class JwtSettings
    {
        public string Secret { get; set; } = null!;
        public TimeSpan ExpirationTimeSpan { get; set; }
        public string ValidIssuer { get; set; } = null!; // Voeg ValidIssuer toe
        public string ValidAudience { get; set; } = null!; // Voeg ValidAudience toe
    }
}
