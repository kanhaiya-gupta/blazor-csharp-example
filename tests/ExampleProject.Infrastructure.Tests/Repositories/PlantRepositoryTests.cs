using ExampleProject.Core.Entities;
using ExampleProject.Infrastructure.Persistence;
using ExampleProject.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Repositories;

public class PlantRepositoryTests
{
    private static AppDbContext CreateInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        await using var db = CreateInMemoryDb(nameof(GetByIdAsync_ReturnsNull_WhenNotFound));
        var repo = new PlantRepository(db);

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoPlants()
    {
        await using var db = CreateInMemoryDb(nameof(GetAllAsync_ReturnsEmpty_WhenNoPlants));
        var repo = new PlantRepository(db);

        var list = await repo.GetAllAsync();

        Assert.Empty(list);
    }

    [Fact]
    public async Task AddAsync_Persists_AndGetByIdAsync_ReturnsIt()
    {
        await using var db = CreateInMemoryDb(nameof(AddAsync_Persists_AndGetByIdAsync_ReturnsIt));
        var repo = new PlantRepository(db);
        var id = Guid.NewGuid();
        var plant = new Plant
        {
            Id = id,
            Name = "CHP Test",
            AssetType = "CHP",
            CapacityMw = 4.5m,
            Status = "Active",
            RegisteredAt = DateTimeOffset.UtcNow
        };

        var added = await repo.AddAsync(plant);

        Assert.Same(plant, added);
        var found = await repo.GetByIdAsync(id);
        Assert.NotNull(found);
        Assert.Equal(id, found.Id);
        Assert.Equal("CHP Test", found.Name);
        Assert.Equal("CHP", found.AssetType);
        Assert.Equal(4.5m, found.CapacityMw);
        Assert.Equal("Active", found.Status);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPlants_OrderedByRegisteredAt()
    {
        await using var db = CreateInMemoryDb(nameof(GetAllAsync_ReturnsPlants_OrderedByRegisteredAt));
        var repo = new PlantRepository(db);
        var t1 = DateTimeOffset.UtcNow.AddMinutes(-2);
        var t2 = DateTimeOffset.UtcNow.AddMinutes(-1);
        await repo.AddAsync(new Plant { Id = Guid.NewGuid(), Name = "Second", AssetType = "Battery", Status = "Active", RegisteredAt = t2 });
        await repo.AddAsync(new Plant { Id = Guid.NewGuid(), Name = "First", AssetType = "CHP", Status = "Pending", RegisteredAt = t1 });

        var list = await repo.GetAllAsync();

        Assert.Equal(2, list.Count);
        Assert.Equal("First", list[0].Name);
        Assert.Equal("Second", list[1].Name);
    }

    [Fact]
    public async Task AddAsync_WithNullCapacityMw_PersistsCorrectly()
    {
        await using var db = CreateInMemoryDb(nameof(AddAsync_WithNullCapacityMw_PersistsCorrectly));
        var repo = new PlantRepository(db);
        var id = Guid.NewGuid();
        var plant = new Plant
        {
            Id = id,
            Name = "VPP Portfolio",
            AssetType = "VPP",
            CapacityMw = null,
            Status = "Active",
            RegisteredAt = DateTimeOffset.UtcNow
        };

        await repo.AddAsync(plant);
        var found = await repo.GetByIdAsync(id);

        Assert.NotNull(found);
        Assert.Null(found.CapacityMw);
    }
}
