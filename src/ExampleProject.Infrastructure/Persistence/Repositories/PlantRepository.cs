using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;
using ExampleProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Infrastructure.Persistence.Repositories
{
    public class PlantRepository : IPlantRepository
    {
        private readonly AppDbContext _db;

        public PlantRepository(AppDbContext db) => _db = db;

        public async Task<Plant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _db.Plants.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task<IReadOnlyList<Plant>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _db.Plants.AsNoTracking().OrderBy(e => e.RegisteredAt).ToListAsync(cancellationToken);

        public async Task<Plant> AddAsync(Plant plant, CancellationToken cancellationToken = default)
        {
            _db.Plants.Add(plant);
            await _db.SaveChangesAsync(cancellationToken);
            return plant;
        }
    }
}
