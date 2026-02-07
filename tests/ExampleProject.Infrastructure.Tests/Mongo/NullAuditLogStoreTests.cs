using ExampleProject.Core.Entities;
using ExampleProject.Infrastructure.Persistence.Mongo;
using Xunit;

namespace ExampleProject.Infrastructure.Tests.Mongo;

public class NullAuditLogStoreTests
{
    private readonly NullAuditLogStore _store = new();

    [Fact]
    public async Task AppendAsync_DoesNotThrow()
    {
        var entry = new AuditEntry
        {
            Id = "1",
            Action = "Test",
            UserId = "user-1",
            Timestamp = DateTimeOffset.UtcNow,
            Details = "Details"
        };
        await _store.AppendAsync(entry);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsEmptyList()
    {
        var result = await _store.GetRecentAsync(10);
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
