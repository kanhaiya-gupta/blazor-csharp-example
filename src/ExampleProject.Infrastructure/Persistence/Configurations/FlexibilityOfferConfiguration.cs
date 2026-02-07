using ExampleProject.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleProject.Infrastructure.Persistence.Configurations
{
    public class FlexibilityOfferConfiguration : IEntityTypeConfiguration<FlexibilityOffer>
    {
        public void Configure(EntityTypeBuilder<FlexibilityOffer> builder)
        {
            builder.ToTable("FlexibilityOffers");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();
        }
    }
}
