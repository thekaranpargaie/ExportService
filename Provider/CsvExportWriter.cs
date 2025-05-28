using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Exportify.Core.Configuration;

namespace Exportify.Core.Provider
{
    public class CsvExportWriter<T> : IExportWriter<T>
    {
        private readonly ExportSchema<T> _schema;

        public CsvExportWriter(ExportSchema<T> schema)
        {
            _schema = schema;
        }

        public async Task WriteAsync(IAsyncEnumerable<T> dataStream, Stream outputStream, CancellationToken token = default)
        {
            using var writer = new StreamWriter(outputStream, Encoding.UTF8, leaveOpen: true);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

            foreach (var col in _schema.Columns)
            {
                csv.WriteField(col.Header);
            }
            await csv.NextRecordAsync();

            await foreach (var record in dataStream.WithCancellation(token))
            {
                foreach (var col in _schema.Columns)
                {
                    csv.WriteField(col.ValueSelector(record));
                }
                await csv.NextRecordAsync();
            }
        }
    }
}
