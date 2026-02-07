using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;

namespace ExampleProject.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// No-op implementation when PostgreSQL is not configured.
    /// </summary>
    public class NullPlantRepository : IPlantRepository
    {
        public Task<Plant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult<Plant?>(null);

        public Task<IReadOnlyList<Plant>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Plant>>(Array.Empty<Plant>());

        public Task<Plant> AddAsync(Plant plant, CancellationToken cancellationToken = default) =>
            Task.FromResult(plant);
    }
}
