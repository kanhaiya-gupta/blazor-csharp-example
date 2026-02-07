using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;

namespace ExampleProject.Infrastructure.Persistence.Mongo
{
    /// <summary>
    /// No-op implementation when MongoDB is not configured.
    /// </summary>
    public class NullAuditLogStore : IAuditLogStore
    {
        public Task AppendAsync(AuditEntry entry, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<IReadOnlyList<AuditEntry>> GetRecentAsync(int count, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<AuditEntry>>(Array.Empty<AuditEntry>());
    }
}
