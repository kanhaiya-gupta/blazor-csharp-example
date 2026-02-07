using ExampleProject.Core.Entities;
using Xunit;

namespace ExampleProject.Core.Tests.Entities;

public class FlexibilityOfferTests
{
    [Fact]
    public void Inherits_BaseEntity()
    {
        var offer = new FlexibilityOffer();
        Assert.IsAssignableFrom<BaseEntity>(offer);
    }

    [Fact]
    public void Id_Can_Be_Set()
    {
        var id = Guid.NewGuid();
        var offer = new FlexibilityOffer { Id = id };
        Assert.Equal(id, offer.Id);
    }
}
