using ExampleProject.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleProject.Infrastructure.Persistence.Configurations
{
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.ToTable("Alerts");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Type).IsRequired();
            builder.Property(e => e.Severity).IsRequired();
            builder.Property(e => e.Message).IsRequired();
            builder.Property(e => e.Timestamp).IsRequired();
        }
    }
}
