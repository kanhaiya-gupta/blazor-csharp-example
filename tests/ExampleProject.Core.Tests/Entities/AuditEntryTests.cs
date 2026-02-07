using ExampleProject.Core.Entities;
using Xunit;

namespace ExampleProject.Core.Tests.Entities;

public class AuditEntryTests
{
    [Fact]
    public void Properties_Can_Be_Set_And_Read()
    {
        var id = "audit-1";
        var action = "Login";
        var userId = "user-001";
        var timestamp = DateTimeOffset.UtcNow;
        var details = "User logged in";

        var entry = new AuditEntry
        {
            Id = id,
            Action = action,
            UserId = userId,
            Timestamp = timestamp,
            Details = details
        };

        Assert.Equal(id, entry.Id);
        Assert.Equal(action, entry.Action);
        Assert.Equal(userId, entry.UserId);
        Assert.Equal(timestamp, entry.Timestamp);
        Assert.Equal(details, entry.Details);
    }
}
