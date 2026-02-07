using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExampleProject.Core.Entities;

namespace ExampleProject.Core.Interfaces
{
    public interface IAuditLogStore
    {
        Task AppendAsync(AuditEntry entry, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AuditEntry>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
    }
}
