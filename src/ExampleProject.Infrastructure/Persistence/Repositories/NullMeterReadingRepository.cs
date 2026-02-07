using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;

namespace ExampleProject.Infrastructure.Persistence.Repositories
{
    public class NullMeterReadingRepository : IMeterReadingRepository
    {
        public Task<IReadOnlyList<MeterReading>> GetRecentAsync(int count, System.Threading.CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<MeterReading>>(Array.Empty<MeterReading>());
    }
}
