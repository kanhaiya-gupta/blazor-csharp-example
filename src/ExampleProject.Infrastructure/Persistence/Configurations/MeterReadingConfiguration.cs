using ExampleProject.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleProject.Infrastructure.Persistence.Configurations
{
    public class MeterReadingConfiguration : IEntityTypeConfiguration<MeterReading>
    {
        public void Configure(EntityTypeBuilder<MeterReading> builder)
        {
            builder.ToTable("MeterReadings");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Timestamp).IsRequired();
            builder.Property(e => e.Value).HasPrecision(10, 2).IsRequired();
        }
    }
}
