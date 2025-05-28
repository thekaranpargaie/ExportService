using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exportify.Core
{
    public interface IDataStreamProvider<T>
    {
        IAsyncEnumerable<T> StreamAsync(CancellationToken token = default);
    }
}
