using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;
using ExampleProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Infrastructure.Persistence.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly AppDbContext _db;

        public AlertRepository(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<Alert>> GetRecentAsync(int count, CancellationToken cancellationToken = default) =>
            await _db.Alerts
                .AsNoTracking()
                .OrderByDescending(e => e.Timestamp)
                .Take(count)
                .ToListAsync(cancellationToken);
    }
}
