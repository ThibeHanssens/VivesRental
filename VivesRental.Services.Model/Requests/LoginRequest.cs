namespace VivesRental.Services.Model.Requests
{
    // **Model voor inloggen**
    // Dit model wordt gebruikt om inloggegevens van de medewerker te ontvangen.
    public class LoginRequest
    {
        public string Username { get; set; } // De gebruikersnaam van de medewerker.
        public string Password { get; set; } // Het wachtwoord van de medewerker.
    }
}
