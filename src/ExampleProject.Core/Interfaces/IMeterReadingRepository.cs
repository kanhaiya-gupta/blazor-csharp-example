using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExampleProject.Core.Entities;

namespace ExampleProject.Core.Interfaces
{
    public interface IMeterReadingRepository
    {
        Task<IReadOnlyList<MeterReading>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
    }
}
