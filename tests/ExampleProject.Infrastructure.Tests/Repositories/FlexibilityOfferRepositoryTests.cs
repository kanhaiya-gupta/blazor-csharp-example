using ExampleProject.Core.Entities;
using ExampleProject.Infrastructure.Persistence;
using ExampleProject.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Repositories;

public class FlexibilityOfferRepositoryTests
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
        var repo = new FlexibilityOfferRepository(db);

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoOffers()
    {
        await using var db = CreateInMemoryDb(nameof(GetAllAsync_ReturnsEmpty_WhenNoOffers));
        var repo = new FlexibilityOfferRepository(db);

        var list = await repo.GetAllAsync();

        Assert.Empty(list);
    }

    [Fact]
    public async Task AddAsync_Persists_AndGetByIdAsync_ReturnsIt()
    {
        await using var db = CreateInMemoryDb(nameof(AddAsync_Persists_AndGetByIdAsync_ReturnsIt));
        var repo = new FlexibilityOfferRepository(db);
        var id = Guid.NewGuid();
        var offer = new FlexibilityOffer
        {
            Id = id,
            Name = "Test Offer",
            Status = "Active",
            CreatedAt = DateTimeOffset.UtcNow
        };

        var added = await repo.AddAsync(offer);

        Assert.Same(offer, added);
        var found = await repo.GetByIdAsync(id);
        Assert.NotNull(found);
        Assert.Equal(id, found.Id);
        Assert.Equal("Test Offer", found.Name);
        Assert.Equal("Active", found.Status);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOffers_OrderedByCreatedAt()
    {
        await using var db = CreateInMemoryDb(nameof(GetAllAsync_ReturnsOffers_OrderedByCreatedAt));
        var repo = new FlexibilityOfferRepository(db);
        var t1 = DateTimeOffset.UtcNow.AddMinutes(-2);
        var t2 = DateTimeOffset.UtcNow.AddMinutes(-1);
        await repo.AddAsync(new FlexibilityOffer { Id = Guid.NewGuid(), Name = "Second", Status = "Active", CreatedAt = t2 });
        await repo.AddAsync(new FlexibilityOffer { Id = Guid.NewGuid(), Name = "First", Status = "Pending", CreatedAt = t1 });

        var list = await repo.GetAllAsync();

        Assert.Equal(2, list.Count);
        Assert.Equal("First", list[0].Name);
        Assert.Equal("Second", list[1].Name);
    }
}
