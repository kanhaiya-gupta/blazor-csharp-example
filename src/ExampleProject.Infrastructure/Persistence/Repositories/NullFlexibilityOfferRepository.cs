using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;

namespace ExampleProject.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// No-op implementation when PostgreSQL is not configured.
    /// </summary>
    public class NullFlexibilityOfferRepository : IFlexibilityOfferRepository
    {
        public Task<FlexibilityOffer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult<FlexibilityOffer?>(null);

        public Task<IReadOnlyList<FlexibilityOffer>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<FlexibilityOffer>>(Array.Empty<FlexibilityOffer>());

        public Task<FlexibilityOffer> AddAsync(FlexibilityOffer offer, CancellationToken cancellationToken = default) =>
            Task.FromResult(offer);
    }
}
