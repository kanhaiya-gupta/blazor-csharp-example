using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;

namespace ExampleProject.Infrastructure.Persistence.Mongo
{
    public class NullDispatchLogStore : IDispatchLogStore
    {
        public Task<IReadOnlyList<DispatchLogEntry>> GetRecentAsync(int count, System.Threading.CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<DispatchLogEntry>>(Array.Empty<DispatchLogEntry>());
    }
}
