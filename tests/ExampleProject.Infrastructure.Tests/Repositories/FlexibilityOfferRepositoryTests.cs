using ExampleProject.Infrastructure.Persistence.Repositories;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Repositories;

public class FlexibilityOfferRepositoryTests
{
    [Fact]
    public void FlexibilityOfferRepository_Type_Exists()
    {
        Assert.NotNull(typeof(FlexibilityOfferRepository));
    }
}
