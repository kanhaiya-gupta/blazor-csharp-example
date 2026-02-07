using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;
using ExampleProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject.Infrastructure.Persistence.Repositories
{
    public class FlexibilityOfferRepository : IFlexibilityOfferRepository
    {
        private readonly AppDbContext _db;

        public FlexibilityOfferRepository(AppDbContext db) => _db = db;

        public async Task<FlexibilityOffer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _db.FlexibilityOffers.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task<IReadOnlyList<FlexibilityOffer>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _db.FlexibilityOffers.AsNoTracking().OrderBy(e => e.CreatedAt).ToListAsync(cancellationToken);

        public async Task<FlexibilityOffer> AddAsync(FlexibilityOffer offer, CancellationToken cancellationToken = default)
        {
            _db.FlexibilityOffers.Add(offer);
            await _db.SaveChangesAsync(cancellationToken);
            return offer;
        }
    }
}
