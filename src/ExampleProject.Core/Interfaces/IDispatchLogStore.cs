using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExampleProject.Core.Entities;

namespace ExampleProject.Core.Interfaces
{
    public interface IDispatchLogStore
    {
        Task<IReadOnlyList<DispatchLogEntry>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
    }
}
