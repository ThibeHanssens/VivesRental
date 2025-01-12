namespace VivesRental.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!; // Gehashed wachtwoord
        public string Role { get; set; } = "User"; // Voor rollen als "Admin" of "User"
    }
}
