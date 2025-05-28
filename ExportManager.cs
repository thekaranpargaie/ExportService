using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Exportify.Core.Configuration;
using Exportify.Core.Enums;
using Exportify.Core.Models;

namespace Exportify.Core
{
    public class ExportManager<T>
    {
        private readonly IDataStreamProvider<T> _dataProvider;

        public ExportManager(IDataStreamProvider<T> dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task ExportToFileAsync(ExportFormat format, ExportSchema<T> schema, string destinationPath, CancellationToken token = default)
        {
            await using var stream = File.Create(destinationPath);
            var writer = GetWriter(format, schema);
            var dataStream = _dataProvider.StreamAsync(token);
            await writer.WriteAsync(dataStream, stream, token);
        }

        public async Task<ExportFileResult> ExportToStreamAsync(ExportFormat format, ExportSchema<T> schema, string fileName, CancellationToken token = default)
        {
            var stream = new MemoryStream();
            var writer = GetWriter(format, schema);
            var dataStream = _dataProvider.StreamAsync(token);
            await writer.WriteAsync(dataStream, stream, token);
            stream.Seek(0, SeekOrigin.Begin);

            var contentType = format switch
            {
                ExportFormat.Csv => "text/csv",
                ExportFormat.Json => "application/json",
                _ => "application/octet-stream"
            };

            return new ExportFileResult(stream, contentType, fileName);
        }

        private IExportWriter<T> GetWriter(ExportFormat format, ExportSchema<T> schema) => format switch
        {
            ExportFormat.Csv => new CsvExportWriter<T>(schema),
            ExportFormat.Json => new JsonExportWriter<T>(schema),
            _ => throw new NotSupportedException($"Export format {format} is not supported.")
        };
    }
}
