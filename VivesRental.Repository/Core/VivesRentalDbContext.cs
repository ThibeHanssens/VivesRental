using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VivesRental.Model;
using VivesRental.Repository.Extensions;

namespace VivesRental.Repository.Core;

// Deze klasse beheert de interactie met de database via Entity Framework Core.
public class VivesRentalDbContext : DbContext
{
    public VivesRentalDbContext(DbContextOptions<VivesRentalDbContext> options) : base(options)
    {
    }

    // Een parameterloze constructor voor design-time
    public VivesRentalDbContext() : base(new DbContextOptionsBuilder<VivesRentalDbContext>()
        .UseSqlServer("Server=localhost\\VIVES_RENTAL;Database=VivesRentalDb;User Id=sa;Password=Vives2023!;TrustServerCertificate=True;MultipleActiveResultSets=true")
        .Options)
    {
    }

    // Definieert DbSets voor de verschillende entiteiten in de applicatie.
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleReservation> ArticleReservations => Set<ArticleReservation>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();

    // Configuratie van de models tijdens het aanmaken van de database.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Verwijdert de standaard meervoudige naamgeving van tabellen.
        modelBuilder.RemovePluralizingTableNameConvention();
        base.OnModelCreating(modelBuilder);

        // **Seed een standaardgebruiker**:
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.NewGuid(),
            Username = "medewerker",
            Password = BCrypt.Net.BCrypt.HashPassword("1234"), // Gebruik BCrypt voor hashing
            Role = "Admin"
        });
    }

    // **Suppress PendingModelChangesWarning**:
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Onderdruk waarschuwingen voor niet-verwerkte modelwijzigingen
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}
