using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;

namespace ExampleProject.Infrastructure.Persistence.Mongo
{
    public class NullMarketSignalStore : IMarketSignalStore
    {
        public Task<IReadOnlyList<MarketSignal>> GetRecentAsync(int count, System.Threading.CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<MarketSignal>>(Array.Empty<MarketSignal>());
    }
}
