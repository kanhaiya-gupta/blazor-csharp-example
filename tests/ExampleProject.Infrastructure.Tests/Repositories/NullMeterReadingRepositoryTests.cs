using ExampleProject.Infrastructure.Persistence.Repositories;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Repositories;

public class NullMeterReadingRepositoryTests
{
    private readonly NullMeterReadingRepository _repo = new();

    [Fact]
    public async Task GetRecentAsync_ReturnsEmptyList()
    {
        var result = await _repo.GetRecentAsync(10);
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsEmpty_RegardlessOfCount()
    {
        var result = await _repo.GetRecentAsync(100);
        Assert.Empty(result);
    }
}
