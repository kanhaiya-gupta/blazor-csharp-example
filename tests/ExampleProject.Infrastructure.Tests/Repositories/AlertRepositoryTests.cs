using ExampleProject.Core.Entities;
using ExampleProject.Infrastructure.Persistence;
using ExampleProject.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Repositories;

public class AlertRepositoryTests
{
    private static AppDbContext CreateInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsEmpty_WhenNoAlerts()
    {
        await using var db = CreateInMemoryDb(nameof(GetRecentAsync_ReturnsEmpty_WhenNoAlerts));
        var repo = new AlertRepository(db);

        var list = await repo.GetRecentAsync(10);

        Assert.Empty(list);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsAlerts_OrderedByTimestampDescending()
    {
        await using var db = CreateInMemoryDb(nameof(GetRecentAsync_ReturnsAlerts_OrderedByTimestampDescending));
        var t1 = DateTimeOffset.UtcNow.AddHours(-2);
        var t2 = DateTimeOffset.UtcNow.AddHours(-1);
        db.Alerts.Add(new Alert { Id = Guid.NewGuid(), Type = "Spike", Severity = "High", Message = "First", Timestamp = t1 });
        db.Alerts.Add(new Alert { Id = Guid.NewGuid(), Type = "Threshold", Severity = "Medium", Message = "Second", Timestamp = t2 });
        await db.SaveChangesAsync();

        var repo = new AlertRepository(db);
        var list = await repo.GetRecentAsync(10);

        Assert.Equal(2, list.Count);
        Assert.True(list[0].Timestamp >= list[1].Timestamp);
        Assert.Equal("Second", list[0].Message);
        Assert.Equal("First", list[1].Message);
    }

    [Fact]
    public async Task GetRecentAsync_RespectsCountLimit()
    {
        await using var db = CreateInMemoryDb(nameof(GetRecentAsync_RespectsCountLimit));
        for (var i = 0; i < 5; i++)
            db.Alerts.Add(new Alert { Id = Guid.NewGuid(), Type = "Test", Severity = "Low", Message = $"Alert {i}", Timestamp = DateTimeOffset.UtcNow.AddMinutes(-i) });
        await db.SaveChangesAsync();

        var repo = new AlertRepository(db);
        var list = await repo.GetRecentAsync(2);

        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task GetRecentAsync_CanReturnAlert_WithResolvedAt()
    {
        await using var db = CreateInMemoryDb(nameof(GetRecentAsync_CanReturnAlert_WithResolvedAt));
        var resolvedAt = DateTimeOffset.UtcNow;
        db.Alerts.Add(new Alert
        {
            Id = Guid.NewGuid(),
            Type = "System",
            Severity = "Low",
            Message = "Resolved",
            Timestamp = resolvedAt.AddMinutes(-10),
            ResolvedAt = resolvedAt
        });
        await db.SaveChangesAsync();

        var repo = new AlertRepository(db);
        var list = await repo.GetRecentAsync(10);

        Assert.Single(list);
        Assert.NotNull(list[0].ResolvedAt);
        Assert.Equal(resolvedAt, list[0].ResolvedAt!.Value);
    }
}
