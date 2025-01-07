using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Extensions;

namespace VivesRental.Repository.Core;

// Deze klasse beheert de interactie met de database via Entity Framework Core.
public class VivesRentalDbContext : DbContext
{
    // Constructor waarbij opties voor DbContext worden doorgegeven, zoals de connection string.
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

    // Configuratie van de models tijdens het aanmaken van de database.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Verwijdert de standaard meervoudige naamgeving van tabellen.
        modelBuilder.RemovePluralizingTableNameConvention();
        base.OnModelCreating(modelBuilder);
    }
}
