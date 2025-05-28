using System.IO;

namespace Exportify.Core.Models
{
    public class ExportFileResult
    {
        public Stream FileStream { get; }
        public string ContentType { get; }
        public string FileName { get; }

        public ExportFileResult(Stream fileStream, string contentType, string fileName)
        {
            FileStream = fileStream;
            ContentType = contentType;
            FileName = fileName;
        }
    }
}
