using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExampleProject.Infrastructure.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_Returns_Same_ServiceCollection()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder().Build();
        var result = ExampleProject.Infrastructure.DependencyInjection.AddInfrastructure(services, config);
        Assert.Same(services, result);
    }
}
