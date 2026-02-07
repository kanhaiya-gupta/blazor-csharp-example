using ExampleProject.Core.Services;
using Xunit;

namespace ExampleProject.Core.Tests.Services;

public class FlexibilityOfferServiceTests
{
    [Fact]
    public void IFlexibilityOfferService_Is_Interface()
    {
        Assert.True(typeof(IFlexibilityOfferService).IsInterface);
    }
}
