using ExampleProject.Infrastructure.Persistence.Mongo;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Mongo;

public class NullMarketSignalStoreTests
{
    private readonly NullMarketSignalStore _store = new();

    [Fact]
    public async Task GetRecentAsync_ReturnsEmptyList()
    {
        var result = await _store.GetRecentAsync(20);
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsEmpty_RegardlessOfCount()
    {
        var result = await _store.GetRecentAsync(100);
        Assert.Empty(result);
    }
}
