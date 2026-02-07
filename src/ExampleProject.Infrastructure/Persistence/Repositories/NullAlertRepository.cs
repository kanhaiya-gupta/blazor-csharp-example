using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;

namespace ExampleProject.Infrastructure.Persistence.Repositories
{
    public class NullAlertRepository : IAlertRepository
    {
        public Task<IReadOnlyList<Alert>> GetRecentAsync(int count, System.Threading.CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Alert>>(Array.Empty<Alert>());
    }
}
