using ExampleProject.Core.Entities;
using ExampleProject.Infrastructure.Persistence;
using ExampleProject.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Repositories;

public class MeterReadingRepositoryTests
{
    private static AppDbContext CreateInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsEmpty_WhenNoReadings()
    {
        await using var db = CreateInMemoryDb(nameof(GetRecentAsync_ReturnsEmpty_WhenNoReadings));
        var repo = new MeterReadingRepository(db);

        var list = await repo.GetRecentAsync(10);

        Assert.Empty(list);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsReadings_OrderedByTimestampDescending()
    {
        await using var db = CreateInMemoryDb(nameof(GetRecentAsync_ReturnsReadings_OrderedByTimestampDescending));
        var t1 = DateTimeOffset.UtcNow.AddHours(-2);
        var t2 = DateTimeOffset.UtcNow.AddHours(-1);
        db.MeterReadings.Add(new MeterReading { Id = Guid.NewGuid(), Timestamp = t1, Value = 1.0m, MetricType = "MW" });
        db.MeterReadings.Add(new MeterReading { Id = Guid.NewGuid(), Timestamp = t2, Value = 2.0m, MetricType = "MW" });
        await db.SaveChangesAsync();

        var repo = new MeterReadingRepository(db);
        var list = await repo.GetRecentAsync(10);

        Assert.Equal(2, list.Count);
        Assert.True(list[0].Timestamp >= list[1].Timestamp);
        Assert.Equal(2.0m, list[0].Value);
        Assert.Equal(1.0m, list[1].Value);
    }

    [Fact]
    public async Task GetRecentAsync_RespectsCountLimit()
    {
        await using var db = CreateInMemoryDb(nameof(GetRecentAsync_RespectsCountLimit));
        for (var i = 0; i < 5; i++)
            db.MeterReadings.Add(new MeterReading { Id = Guid.NewGuid(), Timestamp = DateTimeOffset.UtcNow.AddMinutes(-i), Value = i, MetricType = "MW" });
        await db.SaveChangesAsync();

        var repo = new MeterReadingRepository(db);
        var list = await repo.GetRecentAsync(3);

        Assert.Equal(3, list.Count);
    }

    [Fact]
    public async Task GetRecentAsync_CanReturnReading_WithPlantId()
    {
        await using var db = CreateInMemoryDb(nameof(GetRecentAsync_CanReturnReading_WithPlantId));
        var plantId = Guid.NewGuid();
        db.MeterReadings.Add(new MeterReading { Id = Guid.NewGuid(), PlantId = plantId, Timestamp = DateTimeOffset.UtcNow, Value = 4.5m, MetricType = "MW" });
        await db.SaveChangesAsync();

        var repo = new MeterReadingRepository(db);
        var list = await repo.GetRecentAsync(10);

        Assert.Single(list);
        Assert.Equal(plantId, list[0].PlantId);
        Assert.Equal(4.5m, list[0].Value);
    }
}
