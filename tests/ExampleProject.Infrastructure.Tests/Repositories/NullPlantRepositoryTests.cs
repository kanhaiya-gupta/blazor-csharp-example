using ExampleProject.Core.Entities;
using ExampleProject.Infrastructure.Persistence.Repositories;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Repositories;

public class NullPlantRepositoryTests
{
    private readonly NullPlantRepository _repo = new();

    [Fact]
    public async Task GetByIdAsync_ReturnsNull()
    {
        var result = await _repo.GetByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList()
    {
        var result = await _repo.GetAllAsync();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSamePlant_WithoutPersisting()
    {
        var plant = new Plant
        {
            Id = Guid.NewGuid(),
            Name = "No-op",
            AssetType = "CHP",
            CapacityMw = 2.0m,
            Status = "Pending",
            RegisteredAt = DateTimeOffset.UtcNow
        };
        var result = await _repo.AddAsync(plant);
        Assert.Same(plant, result);

        var found = await _repo.GetByIdAsync(plant.Id);
        Assert.Null(found);
    }
}
