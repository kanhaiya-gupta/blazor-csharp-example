using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExampleProject.Core.Entities;

namespace ExampleProject.Core.Interfaces
{
    public interface IAlertRepository
    {
        Task<IReadOnlyList<Alert>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
    }
}
