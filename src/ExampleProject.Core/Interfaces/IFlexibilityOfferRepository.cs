using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExampleProject.Core.Entities;

namespace ExampleProject.Core.Interfaces
{
    public interface IFlexibilityOfferRepository
    {
        Task<FlexibilityOffer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<FlexibilityOffer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<FlexibilityOffer> AddAsync(FlexibilityOffer offer, CancellationToken cancellationToken = default);
    }
}
