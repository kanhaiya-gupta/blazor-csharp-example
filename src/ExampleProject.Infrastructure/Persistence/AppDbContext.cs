using ExampleProject.Core.Entities;
using Microsoft.EntityFrameworkCore;
using ExampleProject.Infrastructure.Persistence.Configurations;

namespace ExampleProject.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<FlexibilityOffer> FlexibilityOffers => Set<FlexibilityOffer>();
        public DbSet<Plant> Plants => Set<Plant>();
        public DbSet<MeterReading> MeterReadings => Set<MeterReading>();
        public DbSet<Alert> Alerts => Set<Alert>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FlexibilityOfferConfiguration());
            modelBuilder.ApplyConfiguration(new PlantConfiguration());
            modelBuilder.ApplyConfiguration(new MeterReadingConfiguration());
            modelBuilder.ApplyConfiguration(new AlertConfiguration());
        }
    }
}
