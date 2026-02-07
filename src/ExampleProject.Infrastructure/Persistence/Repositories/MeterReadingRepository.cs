using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;
using ExampleProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Infrastructure.Persistence.Repositories
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly AppDbContext _db;

        public MeterReadingRepository(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<MeterReading>> GetRecentAsync(int count, CancellationToken cancellationToken = default) =>
            await _db.MeterReadings
                .AsNoTracking()
                .OrderByDescending(e => e.Timestamp)
                .Take(count)
                .ToListAsync(cancellationToken);
    }
}
