using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExampleProject.Core.Entities;

namespace ExampleProject.Core.Interfaces
{
    public interface IPlantRepository
    {
        Task<Plant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Plant>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Plant> AddAsync(Plant plant, CancellationToken cancellationToken = default);
    }
}
