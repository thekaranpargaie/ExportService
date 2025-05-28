using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Exportify.Core.Configuration;

namespace Exportify.Core.Provider
{
    public class JsonExportWriter<T> : IExportWriter<T>
    {
        private readonly ExportSchema<T> _schema;

        public JsonExportWriter(ExportSchema<T> schema)
        {
            _schema = schema;
        }

        public async Task WriteAsync(IAsyncEnumerable<T> dataStream, Stream outputStream, CancellationToken token = default)
        {
            await using var writer = new Utf8JsonWriter(outputStream, new JsonWriterOptions { Indented = true });
            writer.WriteStartArray();

            await foreach (var record in dataStream.WithCancellation(token))
            {
                writer.WriteStartObject();
                foreach (var col in _schema.Columns)
                {
                    writer.WritePropertyName(col.Header);
                    JsonSerializer.Serialize(writer, col.ValueSelector(record));
                }
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}
