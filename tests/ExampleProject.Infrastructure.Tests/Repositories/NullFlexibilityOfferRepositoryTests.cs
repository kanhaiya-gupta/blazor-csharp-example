using ExampleProject.Core.Entities;
using ExampleProject.Infrastructure.Persistence.Repositories;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Repositories;

public class NullFlexibilityOfferRepositoryTests
{
    private readonly NullFlexibilityOfferRepository _repo = new();

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
    public async Task AddAsync_ReturnsSameOffer_WithoutPersisting()
    {
        var offer = new FlexibilityOffer
        {
            Id = Guid.NewGuid(),
            Name = "No-op",
            Status = "Pending",
            CreatedAt = DateTimeOffset.UtcNow
        };
        var result = await _repo.AddAsync(offer);
        Assert.Same(offer, result);

        var found = await _repo.GetByIdAsync(offer.Id);
        Assert.Null(found);
    }
}
