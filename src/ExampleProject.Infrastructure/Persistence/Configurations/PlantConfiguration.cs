using ExampleProject.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleProject.Infrastructure.Persistence.Configurations
{
    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder.ToTable("Plants");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.AssetType).IsRequired();
            builder.Property(e => e.CapacityMw).HasPrecision(10, 2);
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.RegisteredAt).IsRequired();
        }
    }
}
