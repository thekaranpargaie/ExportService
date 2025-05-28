using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Exportify.Core
{
    public interface IExportWriter<T>
    {
        Task WriteAsync(IAsyncEnumerable<T> dataStream, Stream outputStream, CancellationToken token = default);
    }
}
